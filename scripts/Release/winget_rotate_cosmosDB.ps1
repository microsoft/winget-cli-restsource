# This script attempts to rotate the CosmosDB access Keys. Because Cosmos can take hours to propogate a key change
# we swap to the alternate key (that has been active since last rotation), and rotate out the old primary to be used in the next rotation.
Param(
    [Parameter(Mandatory=$true)]
    [String]
    $subscriptionId,

    [Parameter(Mandatory=$true)]
    [String]
    $resourceGroup,

    [Parameter(Mandatory=$true)]
    [String[]]
    $cosmosDB,

    [Parameter(Mandatory=$true)]
    [String[]]
    [ValidateSet('write','read')]
    $cosmosDbKeySet,

    [Parameter(Mandatory=$true)]
    [String]
    $keyVault,

    [Parameter(Mandatory=$true)]
    [String]
    $keyVaultSecretName,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, [int]::MaxValue)]
    [Int]
    $maxAttempts = 15,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, [int]::MaxValue)]
    [Int]
    $secondsToWait = 15
)

function Get-CosmosKey($cosmosDB, $resourceGroup, $cosmosDBQuery, $maxAttempts, $secondsToWait) {
    Write-Host "Executing Get-CosmosKey"
    $secret = [string]::Empty
    $attempt = 0
    do {
        $attempt++
        try {
            $secret = az cosmosdb keys list --name $cosmosDB --resource-group $resourceGroup --query $cosmosDBQuery --output tsv
            
            if(!$?)
            {
                throw "Failed to execute az cosmosdb keys list properly"
            }

            return $secret
        }
        catch {
            Write-Host "Failed Get-CosmosKey Attempt: $($attempt)"
            Start-Sleep -s $secondsToWait
        }
    } while($attempt -lt $maxAttempts)

    Write-Host "Failed Get-CosmosKey: $($cosmosDB), $($resourceGroup), $($cosmosDBQuery), $($maxAttempts), $($secondsToWait)"
    exit 1
}

function Get-CosmosKeys($cosmosDB, $resourceGroup, $cosmosDbKeySet, $maxAttempts, $secondsToWait)
{
    Write-Host "Executing Get-CosmosKeys"

    # Validate KeySet
    if(($cosmosDbKeySet -ne "read") -And ($cosmosDbKeySet -ne "write")) {
        Write-Host "Invalid CosmosDB KeySet"
        exit
    }

    # Fetch Keys
    $secretPrimary = [string]::Empty
    $secretSecondary = [string]::Empty

    if($cosmosDbKeySet -eq "read")
    {
        $secretPrimary = Get-CosmosKey -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDBQuery "primaryReadonlyMasterKey" -maxAttempts $maxAttempts -secondsToWait $secondsToWait
        $secretSecondary = Get-CosmosKey -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDBQuery "secondaryReadonlyMasterKey" -maxAttempts $maxAttempts -secondsToWait $secondsToWait
    }

    if($cosmosDbKeySet -eq "write")
    {
        $secretPrimary = Get-CosmosKey -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDBQuery "primaryMasterKey" -maxAttempts $maxAttempts -secondsToWait $secondsToWait
        $secretSecondary = Get-CosmosKey -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDBQuery "secondaryMasterKey" -maxAttempts $maxAttempts -secondsToWait $secondsToWait
    }

    # Set Parameters
    [hashtable] $CosmosKeys = @{}
    $CosmosKeys.add("primary", $secretPrimary)
    $CosmosKeys.add("secondary", $secretSecondary)

    return $CosmosKeys
}

function Regenerate-CosmosKey($cosmosDB, $resourceGroup, $cosmosDBKey, $maxAttempts, $secondsToWait) {
    Write-Host "Executing Regenerate-CosmosKey"

    $attempt = 0
    do {
        $attempt++
        try {
            $_ = az cosmosdb keys regenerate --name $cosmosDB --resource-group $resourceGroup --key-kind $cosmosDBKey
            
            if(!$?)
            {
                throw "Failed to execute az cosmosdb keys regenerate properly"
            }

            return
        }
        catch {
            Write-Host "Failed Regenerate-CosmosKey Attempt: $($attempt)"
            Start-Sleep -s $secondsToWait
        }
    } while($attempt -lt $maxAttempts)

    Write-Host "Failed Regenerate-CosmosKey: $($cosmosDB), $($resourceGroup), $($cosmosDBKey), $($maxAttempts), $($secondsToWait)"
    exit 1
}


function Regenerate-CosmosKeys($cosmosDB, $resourceGroup, $cosmosDbKeySet, $activeKey, $maxAttempts, $secondsToWait)
{
    Write-Host "Executing Regenerate-CosmosKeys"
    # Validate KeySet
    if(($cosmosDbKeySet -ne "read") -And ($cosmosDbKeySet -ne "write")) {
        Write-Host "Invalid CosmosDB KeySet"
        exit
    }

    # Validate Active Key
    if(($activeKey -ne "Primary") -And ($activeKey -ne "Secondary") -And ($activeKey -ne "None")) {
        Write-Host "Invalid Active Key"
        exit
    }

    if($cosmosDbKeySet -eq "read")
    {
        if($activeKey -eq "Primary")
        {
            Regenerate-CosmosKey -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDBKey "primaryReadonly" -maxAttempts $maxAttempts -secondsToWait $secondsToWait
        }
        if($activeKey -eq "Secondary")
        {
            Regenerate-CosmosKey -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDBKey "secondaryReadonly" -maxAttempts $maxAttempts -secondsToWait $secondsToWait
        }
        if($activeKey -eq "None")
        {
            Regenerate-CosmosKey -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDBKey "secondaryReadonly" -maxAttempts $maxAttempts -secondsToWait $secondsToWait
        }
    }

    if($cosmosDbKeySet -eq "write")
    {
        if($activeKey -eq "Primary")
        {
            Regenerate-CosmosKey -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDBKey "primary" -maxAttempts $maxAttempts -secondsToWait $secondsToWait
        }
        if($activeKey -eq "Secondary")
        {
            Regenerate-CosmosKey -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDBKey "secondary" -maxAttempts $maxAttempts -secondsToWait $secondsToWait
        }
        if($activeKey -eq "None")
        {
            Regenerate-CosmosKey -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDBKey "secondary" -maxAttempts $maxAttempts -secondsToWait $secondsToWait
        }
    }
}

function Get-KeyVaultSecret($keyVault, $keyVaultSecretName, $maxAttempts, $secondsToWait) {
    Write-Host "Executing Get-KeyVaultSecret"
    $secret = [string]::Empty
    $attempt = 0
    do {
        $attempt++
        try {
            $secret = az keyvault secret show --vault-name $keyVault --name $keyVaultSecretName --query 'value' --output tsv

            if(!$?)
            {
                throw "Failed to execute az keyvault secret show properly"
            }

            return $secret
        }
        catch {
            Write-Host "Failed Get-KeyVaultSecret Attempt: $($attempt)"
            Start-Sleep -s $secondsToWait
        }
    } while($attempt -lt $maxAttempts)

    Write-Host "Failed Get-KeyVaultSecret Attempt: $($cosmosDB), $($keyVaultSecretName), $($maxAttempts), $($secondsToWait)"
    exit 1
}

function Set-KeyVaultSecret($keyVault, $keyVaultSecretName, $secret, $maxAttempts, $secondsToWait) {
    Write-Host "Executing Set-KeyVaultSecret"
    $attempt = 0
    do {
        $attempt++
        try {
            $_ = az keyvault secret set --vault-name $keyVault --name $keyVaultSecretName --value $secret

            if(!$?)
            {
                throw "Failed to execute az keyvault secret set properly"
            }

            return
        }
        catch {
            Write-Host "Failed Set-KeyVaultSecret Attempt: $($attempt)"
            Start-Sleep -s $secondsToWait
        }
    } while($attempt -lt $maxAttempts)

    Write-Host "Failed Set-KeyVaultSecret Attempt: $($cosmosDB), $($keyVaultSecretName), $($maxAttempts), $($secondsToWait)"
    exit 1
}

function Get-ActiveKey($keyvaultKey, $cosmosDBKeys)
{
    Write-Host "Executing Get-ActiveKey"
    $activeKey = [string]::Empty
    
    switch ($keyvaultKey) {
        $cosmosDBKeys['primary'] { $activeKey = "Primary"  }
        $cosmosDBKeys['secondary'] { $activeKey = "Secondary"  }
        Default { $activeKey = "None" }
    }

    return $activeKey
}

# Write Parameters to Host
Write-Host "Rotating Keys with following Parameters"
Write-Host "subscriptionId: $subscriptionId"
Write-Host "resourceGroup: $resourceGroup"
Write-Host "cosmosDB: $cosmosDB"
Write-Host "cosmosDbKeySet: $cosmosDbKeySet"
Write-Host "keyVault: $keyVault"
Write-Host "keyVaultSecretName: $keyVaultSecretName"
Write-Host "maxAttempts: $maxAttempts"
Write-Host "secondsToWait: $secondsToWait"
Write-Host

# Fetch Cosmos DB Keys
[hashtable] $cosmosDBKeys = @{}
$cosmosDBKeys = Get-CosmosKeys -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDbKeySet $cosmosDbKeySet -maxAttempts $maxAttempts -secondsToWait $secondsToWait

# Fetch KV Key
$keyVaultKey = [string]::Empty
$keyVaultKey = Get-KeyVaultSecret -keyVault $keyVault -keyVaultSecretName $keyVaultSecretName -maxAttempts $maxAttempts -secondsToWait $secondsToWait

# Determine Current Key
$activeKey = [string]::Empty
$activeKey = Get-ActiveKey -keyvaultKey $keyvaultKey -cosmosDBKeys $cosmosDBKeys

# Set Alternative Key
switch($activeKey)
{
    "Primary" { 
        Set-KeyVaultSecret -keyVault $keyVault -keyVaultSecretName $keyVaultSecretName -secret $cosmosDBKeys['secondary'] -maxAttempts $maxAttempts -secondsToWait $secondsToWait
    }
    "Secondary" { 
        Set-KeyVaultSecret -keyVault $keyVault -keyVaultSecretName $keyVaultSecretName -secret $cosmosDBKeys['primary'] -maxAttempts $maxAttempts -secondsToWait $secondsToWait
    }
    Default { 
        Set-KeyVaultSecret -keyVault $keyVault -keyVaultSecretName $keyVaultSecretName -secret $cosmosDBKeys['primary'] -maxAttempts $maxAttempts -secondsToWait $secondsToWait
    }
}

# Rotate Alternative Key
Regenerate-CosmosKeys -cosmosDB $cosmosDB -resourceGroup $resourceGroup -cosmosDbKeySet $cosmosDbKeySet -activeKey $activeKey -maxAttempts $maxAttempts -secondsToWait $secondsToWait

Write-Host "Rotation Complete"
