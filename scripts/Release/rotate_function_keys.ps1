# This script attempts to read the current key value for a given function key (or host key).
# It then stores that key as an alternate (to allow for a rolling two key window.)
# It then generates a new key, and adds it to the key-vault.

Param(
    [Parameter(Mandatory=$true)]
    [String]
    $subscriptionId,

    [Parameter(Mandatory=$true)]
    [String]
    $resourceGroup,

    [Parameter(Mandatory=$true)]
    [String[]]
    $webAppNames,

    [Parameter(Mandatory=$true)]
    [String]
    $webAppKeyName,

    [Parameter(Mandatory=$true)]
    [hashtable]
    $azureKeyVaultSecretPair
)

# Function to Create Random Strings
function Create-AppKey()
{
    $private:characters = 'abcdefghiklmnoprstuvwxyzABCDEFGHIJKLMENOPTSTUVWXYZ'
    $private:randomChars = 1..64 | ForEach-Object { Get-Random -Maximum $characters.length }

    # Set the output field separator to empty instead of space
    $private:ofs=""
    return [String]$characters[$randomChars]
}

$local:newAltKeyValue = ""

Write-Host "Verifying keys of web apps"
foreach ($webApp in $webAppNames)
{
    Write-Host "Getting keys of" $webApp
    $private:keysJson = az functionapp keys list -g $resourceGroup -n $webApp
    $private:keys = $keysJson | ConvertFrom-Json -AsHashtable

    if ($keys.functionKeys.ContainsKey($webAppKeyName))
    {
        if ([string]::IsNullOrEmpty($newAltKeyValue))
        {
            $newAltKeyValue = $keys.functionKeys[$webAppKeyName]
        }
        elseif ($newAltKeyValue -ne $keys.functionKeys[$webAppKeyName])
        {
            # Maybe eventually have a switch to overwrite, but for now let the dev figure it out manually.
            throw "The value of $webAppKeyName is not the same in all web apps."
        }
    }
}

Write-Host "Creating new app key"
$local:newKeyValue = Create-AppKey

if ([string]::IsNullOrEmpty($newAltKeyValue))
{
    Write-Warning "$webAppKeyName doesn't exist in any of the web apps."
    $newAltKeyValue = Create-AppKey
}

$local:webAppKeyNameAlt = $webAppKeyName + "Alt"

foreach ($webApp in $webAppNames)
{
    Write-Host "Setting keys for" $webApp

    # Always do alt first.
    az functionapp keys set --key-name $webAppKeyNameAlt --key-type functionKeys -n $webApp -g $resourceGroup --key-value $newAltKeyValue | Out-Null
    az functionapp keys set --key-name $webAppKeyName --key-type functionKeys -n $webApp -g $resourceGroup --key-value $newKeyValue | Out-Null
}

Write-Host "Setting new app key in keyvaults"
foreach ($keyVaultName in $azureKeyVaultSecretPair.keys)
{
    Write-Host "Setting new app key value to $($azureKeyVaultSecretPair[$keyVaultName]) in $keyVaultName"
    az keyvault secret set --vault-name $keyVaultName --name $azureKeyVaultSecretPair[$keyVaultName] --value $newKeyValue | Out-Null
}
