# writes a secret into a specified key vault.

Param(
    [Parameter(Mandatory=$true)]
    [String]
    $azureKeyVault,

    [Parameter(Mandatory=$true)]
    [String]
    $azureKeyVaultSecret,

    [Parameter(Mandatory=$true)]
    [String]
    $azureKeyVaultSecretValue,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, [int]::MaxValue)]
    [Int]
    $maxAttempts = 15,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, [int]::MaxValue)]
    [Int]
    $secondsToWait = 15
)

# Function to Wait
function CheckAndWait($value, $threshold, $message, $waitTime) {
    if($value -gt $threshold)
    {
        Write-Host $message
        Start-Sleep -s $waitTime
    }
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
PrintHeader -message "Setting secret for following Parameters"
Write-Host "azureKeyVault: $azureKeyVault"
Write-Host "azureKeyVaultSecret: $azureKeyVaultSecret"
Write-Host "maxAttempts: $maxAttempts"
Write-Host "secondsToWait: $secondsToWait"

# Update Key Vault
PrintHeader -message "Updating Key-Vault: $azureKeyVault"
$attempt = 0
do {
    CheckAndWait -value $attempt -threshold 0 -message "Waiting before next attempt...." -waitTime $secondsToWait
    $attempt++
    Write-Host "Attempting to update key-vault:" $attempt
    az keyvault secret set --vault-name $azureKeyVault --name $azureKeyVaultSecret --value $azureKeyVaultSecretValue
    $result = $?
} while(!$result -and $attempt -lt $maxAttempts)
if(!$result) {
    Write-Host "Failed to update keyvault."
    exit 1
}

PrintHeader -message "Secret Updated"
