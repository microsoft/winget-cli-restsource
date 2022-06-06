<#
.SYNOPSIS
    Updates an existing storage account as a keyvault managed storage account.
.DESCRIPTION
    Storage account keys need to be autorotated to decrease the risk of having them compromised.
    Adding a storage account as a keyvault managed storage account lets the keyvault auto rotate
    the account keys on specified intervals.
    Since user principal is needed to add a managed storage account, this script needs to be manually run.
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
    [string]
    $upn,

    [Parameter(Mandatory=$false)]
    [string]
    $regenerationPeriod = "P60D",

    #The default here is for the msft tenant.
    #For production/PME, pass in id: e6045dae-1e30-48d4-9502-f77712ec84c8
    [Parameter(Mandatory=$false)]
    [string]
    $keyVaultObjectId = "93c27d83-f79b-4cb2-8dd4-4aa716542e74"
)

# Write Parameters to Host
Write-Host "*********************************************************************"
Write-Host "Running KeyVault managed Storage Account onetime setup:"
Write-Host "*********************************************************************"

Write-Host "subscriptionId: $subscriptionId"
Write-Host "resourceGroup: $resourceGroup"
Write-Host "storageAccountName: $storageAccountName"
Write-Host "keyVaultName: $keyVaultName"
Write-Host "upn: $upn"
Write-Host "regenerationPeriod: $regenerationPeriod"
Write-Host "keyVaultObjectId: $keyVaultObjectId"

# Set Storage Account Key Operator Service Role
$storageAccountResourceid = "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$storageAccountName"

Write-Host "---------------------------------------------------------"
Write-Host 'Adding "Storage Account Key Operator Service Role"'
Write-Host "---------------------------------------------------------"
az account set --subscription $subscriptionId
az role assignment create --role "Storage Account Key Operator Service Role" --assignee-object-id $keyVaultObjectId --scope $storageAccountResourceid
if (!$?)
{
    Write-Host "Error occurred while creating role assignment."
    return
}

# Give your user principal access to all storage account permissions, on your Key Vault instance. Service principal will not work.
Write-Host "---------------------------------------------------------"
Write-Host "Setting policy for the upn"
Write-Host "---------------------------------------------------------"

az keyvault set-policy --name $keyVaultName --upn $upn --storage-permissions set get
Write-Host "---------------------------------------------------------"
Write-Host "Adding storage account to keyvault"
Write-Host "---------------------------------------------------------"

az keyvault storage add --vault-name $keyVaultName -n $storageAccountName --active-key-name key1 --auto-regenerate-key --regeneration-period $regenerationPeriod --resource-id $storageAccountResourceid

if (!$?)
{
    Write-Host "Error occurred while adding managed storage account."
    return
}

Write-Host "Managed Storage account is added."

