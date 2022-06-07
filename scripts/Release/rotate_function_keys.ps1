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
    $webAppName,

    [Parameter(Mandatory=$true)]
    [String]
    $webAppKeyName,

    [Parameter(Mandatory=$true)]
    [String]
    $azureKeyVault,

    [Parameter(Mandatory=$true)]
    [String]
    $azureKeyVaultSecret,

    [Parameter(Mandatory=$false)]
    [String]
    $functionName,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, [int]::MaxValue)]
    [Int]
    $keyLength = 64,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, [int]::MaxValue)]
    [Int]
    $maxAttempts = 15,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, [int]::MaxValue)]
    [Int]
    $secondsToWait = 15
)

# Function to Create Random Strings
function Get-RandomCharacters($length, $characters) { 
    $random = 1..$length | ForEach-Object { Get-Random -Maximum $characters.length } 
    $private:ofs="" 
    return [String]$characters[$random]
}

# Function to Wait
function CheckAndWait($value, $threshold, $message, $waitTime) {
    if($value -gt $threshold)
    {
        Write-Host $message
        Start-Sleep -s $waitTime
    }
}

# Function to set Resource Strings
function SetResourceStrings($subscriptionId, $resourceGroup, $webAppName, $webAppKeyName, $functionName)
{
    [hashtable] $ResourceStrings = @{}

    # Set Parameters
    $ResourceStrings += @{subscriptionId = $subscriptionId}
    $ResourceStrings += @{resourceGroup = $resourceGroup}
    $ResourceStrings += @{webAppName = $webAppName}
    $ResourceStrings += @{webAppKeyName = $webAppKeyName}
    $ResourceStrings += @{functionName = $functionName}

    # Set Azure Function Resource String, and Alternate Key Name
    $ResourceStrings += @{resourceString = "https://management.azure.com/subscriptions/$($ResourceStrings['subscriptionId'])/resourceGroups/$($ResourceStrings['resourceGroup'])/providers/Microsoft.Web/sites/$($ResourceStrings['webAppName'])"}
    $ResourceStrings += @{webAppKeyNameAlt = $webAppKeyName + "Alt"}

    # Set Azure Function Resource String, and Alternate Key Name
    if([string]::IsNullOrEmpty($($ResourceStrings['functionName']))){
        # Set URIs
        $ResourceStrings += @{primaryUri = "$($ResourceStrings['resourceString'])/host/default/functionkeys/$($($ResourceStrings['webAppKeyName']))?api-version=2018-11-01"}
        $ResourceStrings += @{secondaryUri = "$($ResourceStrings['resourceString'])/host/default/functionkeys/$($($ResourceStrings['webAppKeyNameAlt']))?api-version=2018-11-01"}
        $ResourceStrings += @{keyRequestUri = "$($ResourceStrings['resourceString'])/host/default/listKeys?api-version=2018-11-01"}
    } else {
        # Set URIs
        $ResourceStrings += @{primaryUri = "$($ResourceStrings['resourceString'])/functions/$($ResourceStrings['functionName'])/keys/$($($ResourceStrings['webAppKeyName']))?api-version=2018-11-01"}
        $ResourceStrings += @{secondaryUri = "$($ResourceStrings['resourceString'])/functions/$($ResourceStrings['functionName'])/keys/$($($ResourceStrings['webAppKeyNameAlt']))?api-version=2018-11-01"}
        $ResourceStrings += @{keyRequestUri = "$($ResourceStrings['resourceString'])/functions/$($ResourceStrings['functionName'])/listKeys?api-version=2018-02-01"}
    }

    return $ResourceStrings
}

Function PrintHeader($message)
{
    Write-Host
    Write-Host
    Write-Host "*******************************************************"
    Write-Host $message
    Write-Host "*******************************************************"
}

# Write Parameters to Host
PrintHeader -message "Rotating Keys for following Parameters"
Write-Host "subscriptionId: $subscriptionId"
Write-Host "resourceGroup: $resourceGroup"
Write-Host "webAppName: $webAppName"
Write-Host "webAppKeyName: $webAppKeyName"
Write-Host "azureKeyVault: $azureKeyVault"
Write-Host "azureKeyVaultSecret: $azureKeyVaultSecret"
Write-Host "functionName: $functionName"
Write-Host "keyLength: $keyLength"
Write-Host "maxAttempts: $maxAttempts"
Write-Host "secondsToWait: $secondsToWait"

# Fetch Token and Set Headers
PrintHeader -message "Fetching Access Token"
$token=$(az account get-access-token --query accessToken --output tsv)
if(!$?) {
    Write-Host "Failed to get access token."
    exit 1
}
Write-Host "Creating Headers"
$header = @{Authorization = "Bearer " + $token; Accept = 'application/json'}

# Create Initial Resource Strings
[hashtable] $ResourceStrings = @{}
$ResourceStrings = SetResourceStrings -subscriptionId $subscriptionId -resourceGroup $resourceGroup -webAppName $webAppName[0] -webAppKeyName $webAppKeyName -functionName $functionName

# Read current key
PrintHeader -message "Reading Keys from First App: $($ResourceStrings['webAppName'])"
$attempt = 0
do {
    CheckAndWait -value $attempt -threshold 0 -message "Waiting before next attempt...." -waitTime $secondsToWait
    $attempt++
    Write-Host "Attempting to read current key:" $attempt
    $secondaryKeyResponse = Invoke-WebRequest -Method Post -Uri $ResourceStrings['keyRequestUri'] -Headers $header
    Start-Sleep -s 5
} while($secondaryKeyResponse.StatusCode -ne 200 -or !$? -and $attempt -lt $maxAttempts)

# Fail if we failed to read key
if($secondaryKeyResponse.StatusCode -ne 200) {
    Write-Host "Failed to fetch current key."
    exit 1
}

# Parse Key
$secondaryKeyParse = $secondaryKeyResponse.Content | ConvertFrom-Json
if([string]::IsNullOrEmpty($ResourceStrings['functionName'])){
    $secondaryKey = $secondaryKeyParse.functionKeys."$webAppKeyName"
} else {
    $secondaryKey =$secondaryKeyParse."$webAppKeyName"
}

# Create new Function Keys
$primaryKey = Get-RandomCharacters -length $keyLength -characters 'abcdefghiklmnoprstuvwxyzABCDEFGHIJKLMENOPTSTUVWXYZ'
if([string]::IsNullOrEmpty($secondaryKey)){
    $secondaryKey = Get-RandomCharacters -length $keyLength -characters 'abcdefghiklmnoprstuvwxyzABCDEFGHIJKLMENOPTSTUVWXYZ'
}


# Process all Functions
foreach ($app in $webAppName)  
{ 
    $ResourceStrings = SetResourceStrings -subscriptionId $subscriptionId -resourceGroup $resourceGroup -webAppName $app -webAppKeyName $webAppKeyName -functionName $functionName
    PrintHeader -message "Processing: $($ResourceStrings['webAppName'])"

    # Create Function Key Payloads
    $primaryPayload = (@{ properties=@{ name=$($ResourceStrings['webAppKeyName']); value=$primaryKey } } | ConvertTo-Json -Compress)
    $secondaryPayload = (@{ properties=@{ name=$($ResourceStrings['$webAppKeyNameAlt']); value=$secondaryKey } } | ConvertTo-Json -Compress)

    ## Rotate Keys
    # Set Alternate Key First
    $attempt = 0
    do {
        CheckAndWait -value $attempt -threshold 0 -message "Waiting before next attempt...." -waitTime $secondsToWait
        $attempt++
        Write-Host "Attempting to set alternate key:" $attempt
        $alternateKeyResponse = Invoke-WebRequest -Method Put -Uri $ResourceStrings['secondaryUri'] -Body "$secondaryPayload" -Headers $header -ContentType "application/json"
    } while ($alternateKeyResponse.StatusCode -ne 200 -or !$? -and $attempt -lt $maxAttempts)
    if($alternateKeyResponse.StatusCode -ne 200) {
        Write-Host "Failed to set alternate key."
        exit 1
    }
    
    # Set Primary Key
    $attempt = 0
    do {
        CheckAndWait -value $attempt -threshold 0 -message "Waiting before next attempt...." -waitTime $secondsToWait
        $attempt++
        Write-Host "Attempting to set primary key:" $attempt
        $primaryKeyResponse = Invoke-WebRequest -Method Put -Uri $ResourceStrings['primaryUri'] -Body "$primaryPayload" -Headers $header -ContentType "application/json"
    } while ($primaryKeyResponse.StatusCode -ne 200 -or !$? -and $attempt -lt $maxAttempts)
    if($primaryKeyResponse.StatusCode -ne 200) {
        Write-Host "Failed to set primary key."
        exit 1
    }
}

# Update Key Vault
PrintHeader -message "Updating Key-Vault: $azureKeyVault"
$attempt = 0
do {
    CheckAndWait -value $attempt -threshold 0 -message "Waiting before next attempt...." -waitTime $secondsToWait
    $attempt++
    Write-Host "Attempting to update key-vault:" $attempt
    $_ = az keyvault secret set --vault-name $azureKeyVault --name $azureKeyVaultSecret --value $primaryKey
} while(!$? -and $attempt -lt $maxAttempts)
if(!$?) {
    Write-Host "Failed to update keyvault."
    exit 1
}

PrintHeader -message "Keys Updated"
