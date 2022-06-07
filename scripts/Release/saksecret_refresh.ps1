<#
.SYNOPSIS
    Refreshes storage account keyvault secrets on scheduled interval.
.DESCRIPTION
    The storage account connection strings in the keyvault need to be updated after the keys are autorotated
    by keyvault. This script fetches the current active key from managed storage account and updates the corresponding
    keyvault secrets. It doesn't update the secrets if they are the same.
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

# Write Parameters to Host
Write-Host "*********************************************************************"
Write-Host "Running KeyVault managed Storage Account Key secret refresh:"
Write-Host "*********************************************************************"

Write-Host "subscriptionId: $subscriptionId"
Write-Host "resourceGroup: $resourceGroup"
Write-Host "storageAccountName: $storageAccountName"
Write-Host "keyVaultName: $keyVaultName"
Write-Host "azureKeyVaultSecret: $azureKeyVaultSecret"

# Get the active key
# NOTE: After the key is regenerated at the specified time interval, it take some time to reflect the new active key using the below command (atlease 10 mins from observation).
# So, this script for updating the key vault secret should run atleast after an hour of key regeneration to be on the safer side.
Write-Host "---------------------------------------------------------"
Write-Host "Fetching active key"
Write-Host "---------------------------------------------------------"

$activeKeyName = az keyvault storage show -n $storageAccountName --subscription $subscriptionId --vault-name $keyVaultName --query "activeKeyName" --output tsv
if (!$?)
{
    Write-Host "Error occurred while retrieving the current active key. Ensure the storage account is set as a managed account under keyvault"
    return
}

Write-Host "Active key is $activeKeyName"

# Get the storage account key corresponding to the active key
$activeKey = "primary"

if ($activeKeyName -eq "key2")
{
    $activeKey = "secondary"
}

Write-Host "---------------------------------------------------------"
Write-Host "Retrieving connection string corresponding to active key"
Write-Host "---------------------------------------------------------"

$storageAccountConnectionString = az storage account show-connection-string -g $resourceGroup -n $storageAccountName --key $activeKey --output tsv
if (!$?)
{
    Write-Host "Error occurred while retrieving the active key connection string."
    return
}

# Add the rotated key to KeyVault if its value is not the same as the secret's current value
Write-Host "---------------------------------------------------------"
Write-Host "Verifying if keyvault secret $azureKeyVaultSecret exists"
Write-Host "---------------------------------------------------------"
$currentSecretValue = az keyvault secret show --vault-name $keyVaultName --name $azureKeyVaultSecret --query 'value' --output tsv

# If the secret doesn't exists or is not the same as the rotated new key, update it
if (!$? -or $currentSecretValue -cne $storageAccountConnectionString)
{
    Write-Host "Adding keyvault secret to contain the new connection string"

    $_ = az keyvault secret set --vault-name $keyVaultName --name $azureKeyVaultSecret --value $storageAccountConnectionString
    if (!$?)
    {
        Write-Host "Error occurred while updating the keyvault secret."
        return
    }

    Write-Host "Updated keyvault secret $azureKeyVaultSecret."
}
else
{
    Write-Host "Keyvault secret $azureKeyVaultSecret is already up to date."
}