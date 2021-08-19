param(
    [Parameter(Position=0)] $ResourcePrefix     = "rpm-",
    [Parameter(Position=1)] $Index              = "02",
    [Parameter(Position=2)] $AzResourceGroup    = "WinGet_PrivateRepo_",
    [Parameter(Position=3)] $AzSubscriptionName = "1. Visual Studio Enterprise Subscription",
    [Parameter(Position=4)] $AzLocation         = "westus",
    [Parameter(Position=5)] $WorkingDirectory   = $PSScriptRoot
)

## Script Begins
$Result = New-WinGetRepo -ResourcePrefix $ResourcePrefix -Index $Index -AzResourceGroup $AzResourceGroup -AzSubscriptionName $AzSubscriptionName -AzLocation $AzLocation -WorkingDirectory $WorkingDirectory

Function New-WinGetRepo
{
    param(
        [Parameter(Position=0)] [string]$ResourcePrefix,
        [Parameter(Position=1)] [string]$Index,
        [Parameter(Position=2)] [string]$AzResourceGroup,
        [Parameter(Position=3)] [string]$AzSubscriptionName,
        [Parameter(Position=4)] [string]$AzLocation,
        [Parameter(Position=5)] [string]$WorkingDirectory = $PSScriptRoot
    )

    $ParameterFolderPath = "$WorkingDirectory\Parameters"
    $TemplateFolderPath  = "$WorkingDirectory\Templates"
    $AzResourceGroup     = $("$AzResourceGroup$Index").Replace("-","")

    ## Create Folders for the Parameter and Template folder paths
    $Result = New-Item -ItemType Directory -Path $ParameterFolderPath -ErrorAction SilentlyContinue -InformationAction SilentlyContinue
    If($Result)
        { Write-Host "Created Directory to contain the ARM Parameter files ($($Result.FullName))." }

    #### Connect to Azure ####
    $Result = Connect-Azure -AzSubscriptionName $AzSubscriptionName

    ## If the attempt fails.. then exit.
    If(!$($Result))
        { Throw "Failed to connect to Azure" }

    #### Create Resource Group ####
    New-AzureResourceGroup -AzResourceGroupName $AzResourceGroup

    #### Creates the ARM Parameter files ####
    $ARMObjects = New-ARMParameterObject -ParameterFolderPath $ParameterFolderPath -TemplateFolderPath $TemplateFolderPath -Index $Index -ResourcePrefix $ResourcePrefix -AzLocation $AzLocation

    #### Verifies ARM Parameters are correct ####
    $Result = Test-ARMTemplate -ARMObjects $ARMObjects

    ## If the attempt fails.. then exit.
    If($($Result))
        { Throw "ARM Template and Parameter testing failed" }

    #### Creates Azure Objects with ARM Templates and Parameters ####
    New-ARMObjects -ARMObjects $ARMObjects -WorkingDirectory $WorkingDirectory

    Return $ARMObjects
}

Function Connect-Azure
{
    Param(
        [Parameter(Position=0, Mandatory=$true)] [string]$AzSubscriptionName
    )
    Begin
    {}
    Process
    {
        ## Connect to Azure Environment
        Write-Host "`n`nConnecting to Azure Environment"
        $Result = Connect-AzAccount -SubscriptionName $AzSubscriptionName -ErrorVariable Azerror -WarningAction SilentlyContinue
    }
    End
    {
        ## Verifies connection to Azure was successful.
        If($Azerror)
        { 
            Write-Host "  Failed to connect to Azure Environment...`n  $Azerror" -ForegroundColor Red
            Return $false
        }
        
        Write-Host "  Connected to Azure Environment: `n    Subscription ID: $AzSubscriptionName"
        Return $true
    }
}

Function New-AzureResourceGroup
{
    Param(
        [Parameter(Position=1, Mandatory=$true)] [string]$AzResourceGroupName
    )
    Begin
    {
        ## Creating a line break from previous steps
        Write-Host "`n"
    }
    Process
    {
        ## Determines if the Resource Group already exists
        $Result = Get-AzResourceGroup -Name $AzResourceGroupName -ErrorAction SilentlyContinue -ErrorVariable RGerror -InformationAction SilentlyContinue -WarningAction SilentlyContinue

        IF($RGerror)
        { 
            ## Resource Group does not already exist, creating a new Resource Group
            Write-Host "Creating Resource Group ($AzResourceGroupName) in the $AzLocation region..."
            $Result = New-AzResourceGroup -Name $AzResourceGroupName -Location $AzLocation 
        }
    }
    End
    {
        IF(!$RGError)
        {
            ## Resource Group already exists, do nothing
            Write-Host "Resource Group $AzResourceGroupName already exists, will not recreate..." -ForegroundColor Yellow
            Return $true
        }
        ElseIF(Get-AzResourceGroup -Name $AzResourceGroupName -ErrorAction SilentlyContinue -WarningAction SilentlyContinue -InformationAction SilentlyContinue)
        {
            ## Resource Group was created Successfully 
            Write-Host "  Resource Group ($AzResourceGroupName) in the $AzLocation region was created successfully"
            Return $true
        }
        else 
        {
            ## Resource Group failed to be created 
            Write-Host "  Resource Group ($AzResourceGroupName) in the $AzLocation region failed to be created" -ForegroundColor Red 
            Return $false
        }
    }
}

Function New-ARMParameterObject
{
    param(
        [Parameter(Position=0, Mandatory=$true)] [string]$ParameterFolderPath,
        [Parameter(Position=1, Mandatory=$true)] [string]$TemplateFolderPath,
        [Parameter(Position=2, Mandatory=$false)][string]$Index,
        [Parameter(Position=3, Mandatory=$true)] [string]$ResourcePrefix,
        [Parameter(Position=4, Mandatory=$true)] [string]$AzLocation
    )
    Begin
    {
        ## The Names that are to be assigned to each resource.
        $AppInsightsName    = $($ResourcePrefix + "appinsight" + $Index)
        $KeyVaultName       = $($ResourcePrefix + "keyvault" + $Index)
        $StorageAccountName = $($ResourcePrefix + "sa" + $Index).Replace("-", "")   # Doesn't support special characters
        $aspName            = $($ResourcePrefix + "asp" + $Index)
        $CDBAccountName     = $($ResourcePrefix + "cdba" + $Index)
        $CDBDatabaseName    = $("WinGet")
        $FunctionName       = $($ResourcePrefix + "function" + $Index)
        $FrontDoorName      = $($ResourcePrefix + "frontdoor" + $Index)

        ## This is the Azure Key Vault Key used to store the Connection String to the Storage Account
        $AzKVStorageSecretName = "AzStorageAccountKey"
        $AzTenantID            = $(Get-AzContext).Tenant.Id     # This is the Azure Tenant ID
        $AzDirectoryID         = $(Get-AzADUser).Where({$_.UserPrincipalName -like "$($(Get-AzContext).Account.ID.Split("@")[0])*"}).ID     # This is your User Account ID. Permissions are being set to your account to update the KeyVault.
        
        ## This is specific to the JSON file creation
        $JSONContentVersion = "1.0.0.0"
        $JSONSchema         = "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#"
    }
    Process
    {    
        $ARMObjects = @(
            @{  ObjectType = "AppInsight"
                AzName     = "ContosoAppInsight"
                ParameterPath  = "$ParameterFolderPath\applicationinsights.$Index.json"
                TemplatePath   = "$TemplateFolderPath\ApplicationInsights\applicationinsights.json"
                Error      = ""
                Parameters = @{
                    '$Schema' = $JSONSchema
                    contentVersion = $JSONContentVersion
                    Parameters = @{
                        Name       = @{ value = $AppInsightsName }
                    }
                }
            },
            @{  ObjectType = "Keyvault"
                AzName     = "ContosoKV"
                ParameterPath  = "$ParameterFolderPath\keyvault.$Index.json"
                TemplatePath   = "$TemplateFolderPath\KeyVault\keyvault.json"
                Error      = ""
                Parameters = @{
                    '$Schema' = $JSONSchema
                    contentVersion = $JSONContentVersion
                    Parameters = @{
                        name            = @{ 
                            value = $KeyVaultName
                            type  = "string"
                        }
                        accessPolicies  = @{
                            value = @(
                                @{
                                    tenantId = $AzTenantID
                                    objectID = $AzDirectoryID
                                    permissions = @{
                                        keys         = @()
                                        secrets      = @( "Get", "Set" )
                                        certificates = @()
                                    }
                                }
                            )
                        }
                    }
                }
            },
            @{  ObjectType = "StorageAccount"
                ParameterPath  = "$ParameterFolderPath\storageaccount.$Index.json"
                TemplatePath   = "$TemplateFolderPath\StorageAccount\storageaccount.json"
                Error      = ""
                Parameters = @{
                    '$Schema' = $JSONSchema
                    contentVersion = $JSONContentVersion
                    Parameters = @{
                        location           = @{ value = $AzLocation }
                        storageAccountName = @{ value = $StorageAccountName }
                    }
                }
            },
            @{  ObjectType = "asp"
                ParameterPath  = "$ParameterFolderPath\asp.$Index.json"
                TemplatePath   = "$TemplateFolderPath\AppServicePlan\asp.json"
                Error      = ""
                Parameters = @{
                    '$Schema' = $JSONSchema
                    contentVersion = $JSONContentVersion
                    Parameters = @{
                        aspName         = @{ value = $aspName }
                        location        = @{ value = $AzLocation }
                        skuCode         = @{ value = "P1V2" }
                        numberOfWorkers = @{ value = "1" }
                    }
                }
            },
            @{  ObjectType = "CosmosDBAccount"
                ParameterPath  = "$ParameterFolderPath\cosmosdb.$Index.json"
                TemplatePath   = "$TemplateFolderPath\CosmosDB\cosmosdb.json"
                Error      = ""
                Parameters = @{
                    '$Schema' = $JSONSchema
                    contentVersion = $JSONContentVersion
                    Parameters = @{
                        name = @{ value = $CDBAccountName }
                        tags = @{ 
                            value = @{
                                defaultExperience = "Core (SQL)"
                                CosmosAccountType = "Production"
                            } 
                        }
                        consistencyPolicy = @{
                            value = @{
                                defaultConsistencyLevel = "ConsistentPrefix"
                                maxIntervalInSeconds    = 5
                                maxStalenessPrefix      = 100
                            }
                        }
                        locations = @{
                            value = @(
                                @{
                                    locationName     = "West US"
                                    failoverPriority = 0
                                    isZoneRedundant  = $false
                                }
                                @{
                                    locationName     = "East US"
                                    failoverPriority = 1
                                    isZoneRedundant  = $false
                                }
                            )
                        }
                        ipRules =@{
                            value = @(
                                @{
                                    ipAddressOrRange = "104.42.195.92"
                                }
                                @{
                                    ipAddressOrRange = "40.76.54.131"
                                }
                                @{
                                    ipAddressOrRange = "52.176.6.30"
                                }
                                @{
                                    ipAddressOrRange = "52.169.50.45"
                                }
                                @{
                                    ipAddressOrRange = "52.187.184.26"
                                }
                                @{
                                    ipAddressOrRange = "0.0.0.0"
                                }
                            )
                        }
                        backupPolicy = @{
                            value = @{
                                type = "Periodic"
                                periodicModeProperties = @{
                                        backupIntervalInMinutes        = 240
                                        backupRetentionIntervalInHours = 720
                                }
                            }
                        }
                    }
                }
            },
            @{  ObjectType = "CosmosDBDatabase"
                ParameterPath  = "$ParameterFolderPath\cosmosdb-sql.$Index.json"
                TemplatePath   = "$TemplateFolderPath\CosmosDB\cosmosdb-sql.json"
                Error      = ""
                Parameters = @{
                    '$Schema' = $JSONSchema
                    contentVersion = $JSONContentVersion
                    Parameters = @{
                        cosmosName = @{ value = $CDBAccountName }
                        sqlname    = @{ value = $CDBDatabaseName }
                        options    = @{
                            Value  = @{
                                autoscaleSettings = @{
                                    maxThroughput = 4000
                                }
                            }
                        }
                    }
                }
            },
            @{  ObjectType = "CosmosDBContainer"
                ParameterPath  = "$ParameterFolderPath\cosmosdb-sql-container.$Index.json"
                TemplatePath   = "$TemplateFolderPath\CosmosDB\cosmosdb-sql-container.json"
                Error      = ""
                Parameters = @{
                    '$Schema' = $JSONSchema
                    contentVersion = $JSONContentVersion
                    Parameters = @{
                        cosmosName     = @{ Value = $CDBAccountName }
                        sqlname        = @{ Value = $CDBDatabaseName }
                        containerName  = @{ value = "Manifests"}
                        indexingPolicy = @{ Value = @{
                            IndexingMode  = "consistent"
                            automatic     = $true
                            includedPaths = @(@{
                                path = "/*"
                            })
                            excludePaths = @(@{
                                path = '/"_etag"/?'
                            })
                        }}
                        partitionKey = @{
                            value = @{
                                paths = @("/id")
                                kind = "Hash"
                            }
                        }
                        conflictResolutionPolicy = @{
                            value = @{
                                mode = "LastWriterWins"
                                conflictResolutionPath = "/_ts"
                            }
                        }
                    }
                }
            },
            @{  ObjectType = "Function"
                ParameterPath  = "$ParameterFolderPath\azurefunction.$Index.json"
                TemplatePath   = "$TemplateFolderPath\AzureFunction\azurefunction.json"
                Error      = ""
                Parameters = @{
                    '$Schema' = $JSONSchema
                    contentVersion = $JSONContentVersion
                    Parameters = @{
                        storageSecretName = @{ value = $AzKVStorageSecretName }     # Name used to contain the Storage Account connection string in the Key Value
                        location          = @{ value = $AzLocation      }           # Azure hosting location
                        serverIdentifier  = @{ value = $aspName         }           # 
                        functionName      = @{ value = $FunctionName    }           # Azure Function Name
                        appServiceName    = @{ value = $aspName         }           # Azure App Service Name
                        keyVaultName      = @{ value = $KeyVaultName    }           # Azure Keyvault Name
                        appInsightName    = @{ value = $AppInsightsName }           # Azure App Insights Name
                    }
                }
            }#,
            # @{  ObjectType = "FrontDoor"      ## Requires a CName entry be created for the frontend endpoint.
            #     ParameterPath  = "$ParameterFolderPath\frontdoor.$Index.json"
            #     TemplatePath   = "$TemplateFolderPath\FrontDoor\frontdoor.json"
            #     Error      = ""
            #     Parameters = @{
            #         '$Schema' = $JSONSchema
            #         contentVersion = $JSONContentVersion
            #         Parameters = @{
            #             name              = @{ value = $FrontDoorName }
            #             frontendEndpoints = @{
            #                 value = @(
            #                     @{
            #                         name = "$($ResourcePrefix + "azurefd" + $Index)"
            #                         properties = @{
            #                             hostName = "$($ResourcePrefix + "azurefd" + $Index).azurefd.net"
            #                             sessionAffinityEnabledState = "Disabled"
            #                             sessionAffinityTtlSeconds   = 0
            #                             resourceState               = "Enabled"
            #                         }
            #                     }
            #                 )
            #             }
            #             healthProbeSettings = @{
            #                 value = @(
            #                     @{
            #                         name = "healthProbe-fiveSecond"
            #                         properties = @{
            #                             intervalInSeconds   = 5
            #                             path                = "/"
            #                             protocol            = "Https"
            #                             resourceState       = "Enabled"
            #                             enabledState        = "Enabled"
            #                             healthProbeMethod   = "Head"
            #                         }
            #                     }
            #                 )
            #             }
            #             loadBalancingSettings = @{
            #                 value = @(
            #                     @{
            #                         name = "loadBalancing-tenSample"
            #                         properties = @{
            #                             additionalLatencyMilliseconds = 1000
            #                             sampleSize                    = 10
            #                             successfulSamplesRequired     = 5
            #                             resourceState                 = "Enabled"
            #                         }
            #                     }
            #                 )
            #             }
            #             backendPools = @{
            #                 value = @(
            #                     @{
            #                         name     = "api"
            #                         backEnds = @(
            #                             @{
            #                                 address           = "$FunctionName.azurewebsites.net"
            #                                 httpPort          = 80
            #                                 httpsPort         = 443
            #                                 priority          = 1
            #                                 weight            = 50
            #                                 backendHostHeader = "$FunctionName.azurewebsites.net"
            #                                 enabledState      = "Enabled"
            #                             }
            #                         )
            #                         HealthProbeSettingsName   = "healthProbe-fiveSecond"
            #                         loadBalancingSettingsName = "loadBalancing-tenSample"
            #                     }
            #                 )
            #             }
            #             routingRules = @{
            #                 value = @(
            #                     @{
            #                         name                = "api-rule"
            #                         frontendEndpoint    = "$($ResourcePrefix + "azurefd" + $Index)"
            #                         acceptedProtocols   = @( "Https" )
            #                         patternsToMatch     = @( "/api/*" )
            #                         enabledState        = "Enabled"
            #                         routeConfiguration  = @{
            #                             odataType           = "#Microsoft.Azure.FrontDoor.Models.FrontdoorForwardingConfiguration"
            #                             forwardingProtocol  = "HttpsOnly"
            #                             backendPoolName     = "api"
            #                         }
            #                     }    
            #                 )
            #             }
            #             backendPoolsSettings = @{
            #                 value = @{
            #                     enforceCertificateNameCheck = "Enabled"
            #                     sendRecvTimeoutSeconds      = 30
            #                 }
            #             }
            #             enabledState = @{
            #                 value = "Enabled"
            #             }
            #         }
            #     }
            # }
        )

        Write-Host "`n`nCreating JSON Parameter files for Azure Object Creation:"

        ## Creates each JSON Parameter file inside of a Parameter folder in the working directory
        foreach ($object in $ARMObjects) 
        {
            Write-Host "  Creating the Parameter file for $($Object.ObjectType) in the following location:`n    $($Object.ParameterPath)"
            $Object.Parameters | ConvertTo-Json -Depth 7 | Out-File -FilePath $Object.ParameterPath -Force
        }
    }
    End
    {
        Return $ARMObjects
    }
}

Function Test-ARMTemplate
{
    param(
        [Parameter(Position=0)] $ARMObjects
    )
    Begin
    {
        Write-Host "`n"
    }
    Process
    {
        Write-Host "Verifying the ARM Resource Templates and Parameters are valid:"
        $TestResults = ""
        Foreach($Object in $ARMObjects)
        {
            ## Validates that each ARM object will work.
            Write-Host "  Validation testing on ARM Resource ($($Object.ObjectType))..."
            $Result = Test-AzResourceGroupDeployment -ResourceGroupName $AzResourceGroup -Mode Complete -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath

            ## If the ARM object fails validation, report error to screen.
            IF($Result -ne "")
            { 
                Write-Host "    ERROR:  $Result" -ForegroundColor Red
                $TestResults += "$($Object.ObjectType):`n$Result`n`n"
            }
        }
    }
    End
    {
        Return $TestResults
    }
}

Function New-ARMObjects
{
    param(
        [Parameter(Position=0)] $ARMObjects,
        [Parameter(Position=1)] [string] $WorkingDirectory
    )
    Begin
    {
        Write-Host "`n"
        ## Path to the compiled Functions compressed into a Zip file for upload to Azure Function
        $ArchiveFunctionZip = "$WorkingDirectory\CompiledFunctions.zip"
        
        ## Imports the Parameter Files
        $jsonStorageAccount = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "StorageAccount"}).ParameterPath) | ConvertFrom-Json
        $jsonKeyVault       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Keyvault"}).ParameterPath) | ConvertFrom-Json
        #$jsonASP            = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "asp"}).ParameterPath) | ConvertFrom-Json
        $jsonFunction       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Function"}).ParameterPath) | ConvertFrom-Json
        $jsoncdba           = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "CosmosDBAccount"}).ParameterPath) | ConvertFrom-Json

        ## Name of target Keyvault
        $AzKeyVaultName       = $jsonKeyVault.parameters.name.value
        $AzStorageAccountName = $jsonStorageAccount.parameters.storageAccountName.value
        $CosmosAccountName    = $jsoncdba.Parameters.Name.Value

        ## Azure Keyvault Secret Names
        $AzStorageAccountKeyName      = "AzStorageAccountKey"
        $CosmosAccountEndpointKeyName = "CosmosAccountEndpoint"
        $CosmosAccountKeyKeyName      = "CosmosAccountKey"

        ## Azure Storage Account Connection String Endpoint Suffix
        $AzEndpointSuffix     = "core.windows.net"
    }
    Process
    {
        ## Creates the Azure Resources following the ARM template / parameters
        Write-Host "Creating Azure Resources following ARM Templates..."
        
        foreach ($Object in $ARMObjects)
        {
            Write-Host "  Creating the Azure Object - $($Object.ObjectType)"
    
            If($Object.ObjectType -eq "Function")
            {
                #$ServerIdentifier = $jsonASP.parameters.aspName.value
                Write-Host "    Creating KeyVault Secrets:"

                ## Creates the Azure Storage Account Connection String
                $AzStorageAccountKey  = $(Get-AzStorageAccountKey -ResourceGroupName $AzResourceGroup -Name $AzStorageAccountName)[0].Value

                ## Azure Keyvault Secrets
                $CosmosAccountEndpointValue       = ConvertTo-SecureString -String $($(Get-AzCosmosDBAccount -ResourceGroupName $AzResourceGroup).DocumentEndpoint) -AsPlainText -Force
                $CosmosAccountKeyValue            = ConvertTo-SecureString -String $($(Get-AzCosmosDBAccountKey -ResourceGroupName $AzResourceGroup -Name $CosmosAccountName).PrimaryMasterKey) -AsPlainText -Force
                $AzStorageAccountConnectionString = ConvertTo-SecureString -String "DefaultEndpointsProtocol=https;AccountName=$AzStorageAccountName;AccountKey=$AzStorageAccountKey;EndpointSuffix=$AzEndpointSuffix" -AsPlainText -Force

                ## Adds the Azure Storage Account Connection String to the Keyvault
                Write-Host "      Creating Keyvault Secret for Azure Storage Account Connection String."
                $Result = Set-AzKeyVaultSecret -VaultName $AzKeyVaultName -Name $AzStorageAccountKeyName -SecretValue $AzStorageAccountConnectionString
    
                ## Adds the Azure Cosmos Account Endpoint URL to the Keyvault
                Write-Host "      Creating Keyvault Secret for Azure CosmosDB Connection String."
                $Result = Set-AzKeyVaultSecret -VaultName $AzKeyVaultName -Name $CosmosAccountEndpointKeyName -SecretValue $CosmosAccountEndpointValue
    
                ## Adds the Azure Cosmos Primary Account Key to the Keyvault
                Write-Host "      Creating Keyvault Secret for Azure CosmosDB Primary Key."
                $Result = Set-AzKeyVaultSecret -VaultName $AzKeyVaultName -Name $CosmosAccountKeyKeyName -SecretValue $CosmosAccountKeyValue
    
                ## Create base object of the Azure Function, generating reference object ID for Keyvault
                Write-Host "    Creating base Azure Function object."
                $Result = New-AzResourceGroupDeployment -ResourceGroupName $AzResourceGroup -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath -Mode Incremental -ErrorAction SilentlyContinue
            }
    
            ## Creates the Azure Resource
            Write-Host "    Creating $($Object.ObjectType) following the ARM Parameter File..."
            $Result = New-AzResourceGroupDeployment -ResourceGroupName $AzResourceGroup -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath -Mode Incremental -ErrorAction SilentlyContinue -ErrorVariable objerror
    
            ## Sets a sleep of 5 seconds after object creation to allow Azure to update creation status.
            Start-Sleep -Seconds 5
    
            If($objerror)
            {
                ## Creating the object following the ARM template failed.
                Write-Host "      Error: Object failed to create.`n      $objerror" -ForegroundColor Red
                Return
            }
            else 
                { Write-Host "      $($Object.ObjectType) was successfully created." }
    
            ## Copy GitHub Functions to newly created Azure Function
            If($Object.ObjectType -eq "Function")
            {
                $AzFunctionName = $jsonFunction.parameters.functionName.value
    
                Write-Host "    Copying function files to the Azure Function."
                $Result = Publish-AzWebApp -ArchivePath $ArchiveFunctionZip -ResourceGroupName $AzResourceGroup -Name $AzFunctionName -Force
            }
        }
    }
    End
    {}
}