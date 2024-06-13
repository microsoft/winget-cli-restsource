<#
.SYNOPSIS
    Refreshes storage account keyvault secrets on scheduled interval.
.DESCRIPTION
    The storage account connection strings in the keyvault need to be updated after the keys are autorotated
    by keyvault. This script fetches the current active key from managed storage account and updates the corresponding
    keyvault secrets. It doesn't update the secrets if they are the same.
    NOTE: Key Vault Managed Storage Account Keys is supported as-is with no more updates planned, so in the
    future we might not be able to do this.
#>

Param(
    [Parameter(Mandatory=$true)]
    [String]
    $subscriptionId,

    [Parameter(Mandatory=$true)]
    [String]
    $resourceGroup,

    [Parameter(Mandatory=$true)]
    [String]
    $keyVaultName,

    [Parameter(Mandatory=$true)]
    [String]
    $storageAccountName,

    [Parameter(Mandatory=$true)]
    [String]
    $azureKeyVaultSecret
)

# Get keys
Write-Host "Getting storage keys"
$keys = & az storage account keys list --account-name $storageAccountName -g $resourceGroup --subscription $subscriptionId
if (!$?)
{
    Write-Error "Error occurred while retrieving the storage account keys."
    return
}
$keys = $keys | ConvertFrom-Json

# 'az storage account keys list' used to tell us which one is the active key
# but they like making things harder. Figure out which one is newer and set that as the secret.
# This allow us to be functional if the Az func takes a while to pick up the new key vault secret.
$newKey = $keys[0]
if ($keys[0].creationTime -lt $keys[1].creationTime)
{
    $newKey = $keys[1]
}

Write-Host "$($newKey.keyName) is newer"

Write-Host "Getting storage $($newKey.keyName) connection string"
$cstr = & az storage account show-connection-string -g $resourceGroup -n $storageAccountName --key $newKey.keyName --subscription $subscriptionId
if (!$?)
{
    Write-Error "Error occurred while retrieving the $($newKey.keyName) connection string."
    return
}
$cstr = $cstr | ConvertFrom-Json

Write-Host "Getting secret in key vault"
$currentSecret = & az keyvault secret show --vault-name $keyVaultName --name $azureKeyVaultSecret --subscription $subscriptionId
if (!$?)
{
    Write-Error "$azureKeyVaultSecret in $keyVaultName doesn't exist."
    return
}
$currentSecret = $currentSecret | ConvertFrom-Json

# If not the same update it.
if ($currentSecret.value -cne $cstr.connectionString)
{
    Write-Host "Adding keyvault secret to contain the new connection string"
    & az keyvault secret set --vault-name $keyVaultName --name $azureKeyVaultSecret --value $cstr.connectionString | Out-Null
    if (!$?)
    {
        Write-Error "Error occurred while updating the keyvault secret."
        return
    }

    Write-Host "Updated keyvault secret $azureKeyVaultSecret."
}
else
{
    Write-Host "Keyvault secret $azureKeyVaultSecret is already up to date."
}
