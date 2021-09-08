<#
    .SYNOPSIS
    Sets up a Windows Package Manager private repository.
    
    .DESCRIPTION
    This script will generate new parameter files, that can be used to create the necessary Azure resources to host the Windows Package Manager private repository. Uploads the compiled rest apis to the newly created Azure function.

    This script will:
        1. Connect to your Azure Subscription (prompt for authentication).
        2. Create Parameter files to be used with the ARM Templates.
        3. Use the Parameter files and ARM Template files to create the Azure resources.
        4. Creates secure keys in the Azure Keyvault.
        5. Publish the Windows Package Manager private repository functions to the Azure Function.

    The following Azure Modules are used by this script:
        Az.Resources --> Get-AzResourceGroup, New-AzResourceGroup, Get-AzADUser, Test-AzResourceGroupDeployment, New-AzResourceGroupDeployment
        Az.Accounts  --> Connect-AzAccount, Get-AzContext, Set-AzContext
        Az.KeyVault  --> Set-AzKeyVaultSecret
        Az.Websites  --> Publish-AzWebapp
        Az.Functions --> Get-AzFunctionApp
    
    .PARAMETER ResourcePrefix
    Azure resources will be created with the "ResourcePrefix" prefixed to the name.
    
    .PARAMETER Index
    Azure resources will be created with the "Index" sufixed to the name.
    
    .PARAMETER AzResourceGroup
    Azure Resource Group that will be created or used to group the Azure resources being created by this script.
    
    .PARAMETER AzSubscriptionName
    Azure Subscription that will have the Azure Resource Group and Azure resources created in (optional).
    
    .PARAMETER AzLocation
    Azure location that will be used for the created resources. (Default: westus)
    
    .PARAMETER WorkingDirectory
    Local file repository that will be referenced by this script. Used to locate the Template files, and identifies where the Parameters will be created (Default: Script Location).
    
    .EXAMPLE
    .\automation.ps1 -ResourcePrefix "contoso-" -Index "Demo" -AzResourceGroup "WinGet_PrivateRepo_Demo"

    .EXAMPLE
    .\automation.ps1 -ResourcePrefix "contoso-" -Index "001" -AzResourceGroup "WinGet_PrivateRepo" -AzLocation "westus"

    .EXAMPLE
    .\automation.ps1 -ResourcePrefix "contoso-" -Index "Prod" -AzResourceGroup "WinGet_PrivateRepo_Prod" -AzSubscription "Contoso Global"    

    .NOTES
    Something
    #>

param(
    [Parameter(Position=0, Mandatory=$true)]  [string]$ResourcePrefix,
    [Parameter(Position=1, Mandatory=$true)]  [string]$Index,
    [Parameter(Position=2, Mandatory=$true)]  [string]$AzResourceGroup,
    [Parameter(Position=3, Mandatory=$false)] [string]$AzSubscriptionName,
    [Parameter(Position=4, Mandatory=$false)] [string]$AzLocation         = "westus",
    [Parameter(Position=5, Mandatory=$false)] [string]$WorkingDirectory   = $PSScriptRoot
)

Function Test-RequiredModules
{
    Param(
        [Parameter(Position=0, Mandatory=$true)] [string]$RequiredModule
    )
    Begin
    {}
    Process
    {
        ## Determinds if the PowerShell Module is missing, If missing Returns the name of the missing module
        IF(!$(Get-Module -ListAvailable -Name $RequiredModule) )
            { $Result = $RequiredModule }
    }
    End
    {
        ## Returns a value only if the module is missing
        Return $Result
    }
}

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
    Begin
    {
        $ParameterFolderPath = "$WorkingDirectory\Parameters"       # Path that will be used to target the Parameter files.
        $TemplateFolderPath  = "$WorkingDirectory\Templates"        # Path that will be used to target the ARM Template files.
        $RequiredModules = @("Az.Resources", "Az.Accounts", "Az.KeyVault","Az.Websites", "Az.Functions")
    }
    Process
    {
        ## Test that the Required Azure Modules are installed
        $Result = @()
        foreach( $RequiredModule in $RequiredModules )
            { $Result += Test-RequiredModules -RequiredModule $RequiredModule }
        
        IF( $Result )
        {
            ## Modules have been identified as missing
            $ErrorMessage = "`n`nMissing required PowerShell modules`n"
            ##$Result | Foreach { $ErrorMessage += "`n  - $_ "}
            $ErrorMessage += "    Run the following command to install the missing modules: Import-Module Az`n"
            
            Write-Host $ErrorMessage -ForegroundColor Yellow
            
            Throw "Unable to run script, missing required PowerShell modules..."
        }

        ## Create Folders for the Parameter and Template folder paths
        $Result = New-Item -ItemType Directory -Path $ParameterFolderPath -ErrorAction SilentlyContinue -InformationAction SilentlyContinue
        If($Result)
            { Write-Host "Created Directory to contain the ARM Parameter files ($($Result.FullName))." }

        #### Connect to Azure ####
        $Result = Connect-Azure -AzSubscriptionName $AzSubscriptionName

        ## If the connection to azure attempt fails.. then exit.
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
    }
    End
    {
        Return $ARMObjects
    }
}

Function Connect-Azure
{
    Param(
        [Parameter(Position=0, Mandatory=$false)] [string]$AzSubscriptionName
    )
    Begin
    {
        If($null -eq $AzSubscriptionName)
            { $AzSubscriptionName = "" }
    }
    Process
    {
        ## Connect to Azure Environment
        Write-Host "`n`nConnecting to Azure Environment"
        IF($AzSubscriptionName.Equals(""))
        {
            ## Connects to Azure without a pre-defined Subscription. Uses the default Subscription.
            $Result = Connect-AzAccount -ErrorVariable Azerror -WarningAction SilentlyContinue
        }
        else 
        {
            ## Connects to Azure with a pre-defined Subscription.
            $Result = Connect-AzAccount -SubscriptionName $AzSubscriptionName -ErrorVariable Azerror -WarningAction SilentlyContinue
        }
    }
    End
    {
        ## Verifies connection to Azure was successful.
        If($Azerror)
        { 
            ## Connection to Azure failed.
            Write-Host "  Failed to connect to Azure Environment...`n  $Azerror" -ForegroundColor Red
            Return $false
        }

        ## Retrieves the connected Azure Subscription
        $AzSubscriptionName = $(Get-AzContext).Subscription.Name
        
        ## Connection to Azure was successful.
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
        IF(!$RGerror)
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
        $AppInsightsName    = $($ResourcePrefix + "appinsight" + $Index)            # Name that will be assigned to the Azure AppInsights.
        $KeyVaultName       = $($ResourcePrefix + "keyvault" + $Index)              # Name that will be assigned to the Azure Keyvault.
        $StorageAccountName = $($ResourcePrefix + "sa" + $Index).Replace("-", "")   # Name that will be assigned to the Azure Storage Account. Doesn't support special characters.
        $aspName            = $($ResourcePrefix + "asp" + $Index)                   # Name that will be assigned to the Azure ASP.
        $CDBAccountName     = $($ResourcePrefix + "cdba" + $Index)                  # Name that will be assigned to the Azure Cosmos Database Account.
        $CDBDatabaseName    = $("WinGet")                                           # Name of the Cosmos Database - Value must match the name found in the Compiled Windows Package Manager Functions (CompiledFunctions.zip).
        $FunctionName       = $($ResourcePrefix + "function" + $Index)              # Name that will be assigned to the Azure Function.
        $FrontDoorName      = $($ResourcePrefix + "frontdoor" + $Index)             # Name that will be assigned to the Azure Front Door (Currently not created by this script, and not required to use a single instance of the Windows Package Manager private repository).

        ## This is the Azure Key Vault Key used to store the Connection String to the Storage Account
        $AzKVStorageSecretName = "AzStorageAccountKey"                              # The name that will be used to specify the Azure Keyvault Connection string. Do not change.
        $AzTenantID            = $(Get-AzContext).Tenant.Id                         # This is the Azure Tenant ID
        $AzDirectoryID         = $(Get-AzADUser).Where({$_.UserPrincipalName -like "$($(Get-AzContext).Account.ID.Split("@")[0])*"}).ID     # This is your User Account ID. Permissions are being set to your account to update the KeyVault.
        
        ## This is specific to the JSON file creation
        $JSONContentVersion = "1.0.0.0"
        $JSONSchema         = "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#"
    }
    Process
    {    
        ## Creates a PowerShell object array to contain the details of the Parameter files.
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

        ## Uses the newly created ARMObjects[#].Parameters to create new JSON Parameter files.
        Write-Host "`n`nCreating JSON Parameter files for Azure Object Creation:"

        ## Creates each JSON Parameter file inside of a Parameter folder in the working directory
        foreach ($object in $ARMObjects)
        {
            ## Converts the structure of the variable to a JSON file.
            Write-Host "  Creating the Parameter file for $($Object.ObjectType) in the following location:`n    $($Object.ParameterPath)"
            $Object.Parameters | ConvertTo-Json -Depth 7 | Out-File -FilePath $Object.ParameterPath -Force
        }
    }
    End
    {
        ## Returns the completed object.
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

        ## Parses through all ARM Parameter objects to validate they are properly configured.
        Foreach($Object in $ARMObjects)
        {
            ## Validates that each ARM object will work.
            Write-Host "  Validation testing on ARM Resource ($($Object.ObjectType))..."
            $Result = Test-AzResourceGroupDeployment -ResourceGroupName $AzResourceGroup -Mode Complete -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath

            ## If the ARM object fails validation, report error to screen.
            IF($Result -ne "")
            { 
                ## Testing fails.
                Write-Host "    ERROR:  $Result" -ForegroundColor Red
                $TestResults += "$($Object.ObjectType):`n$Result`n`n"
            }
        }
    }
    End
    {
        ## Returns the TestResults.
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
        
        ## Imports the contents of the Parameter Files for reference and logging purposes:
        $jsonStorageAccount = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "StorageAccount"}).ParameterPath) | ConvertFrom-Json
        $jsonKeyVault       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Keyvault"}).ParameterPath) | ConvertFrom-Json
        $jsonFunction       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Function"}).ParameterPath) | ConvertFrom-Json
        $jsoncdba           = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "CosmosDBAccount"}).ParameterPath) | ConvertFrom-Json

        $AzKeyVaultName       = $jsonKeyVault.parameters.name.value                         # Name of the Azure Keyvault
        $AzStorageAccountName = $jsonStorageAccount.parameters.storageAccountName.value     # Name of the Azure Storage Account
        $CosmosAccountName    = $jsoncdba.Parameters.Name.Value                             # Name of the Azure Cosmos Database Account

        ## Azure Keyvault Secret Names
        $AzStorageAccountKeyName      = "AzStorageAccountKey"       # Do not change
        $CosmosAccountEndpointKeyName = "CosmosAccountEndpoint"     # Do not change
        $CosmosAccountKeyKeyName      = "CosmosAccountKey"          # Do not change

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
    
            ## If the object to be created is an Azure Function, then complete these pre-required steps before creating the Azure Function.
            If($Object.ObjectType -eq "Function")
            {
                Write-Host "    Creating KeyVault Secrets:"

                ## Creates a reference to the Azure Storage Account Connection String as a Secret in the Azure Keyvault.
                $AzStorageAccountKey  = $(Get-AzStorageAccountKey -ResourceGroupName $AzResourceGroup -Name $AzStorageAccountName)[0].Value

                ## Retrieves the required information from the previously created Azure objects. Values will be used to generate required information for the Azure Keyvault.
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
    
            ## Sets a sleep of 10 seconds after object creation to allow Azure to update creation status, and mark as "running"
            Start-Sleep -Seconds 10
    
            ## Verifies that no error occured when creating the Azure resource
            If($objerror)
            {
                ## Creating the object following the ARM template failed.
                Write-Host "      Error: Object failed to create.`n      $objerror" -ForegroundColor Red
                Return
            }
            else 
            {
                ## Creating the object was successful
                Write-Host "      $($Object.ObjectType) was successfully created."
            }
    
            ## Copy GitHub Functions to newly created Azure Function
            If($Object.ObjectType -eq "Function")
            {
                ## Gets the Azure Function Name from the Parameter JSON file contents.
                $AzFunctionName = $jsonFunction.parameters.functionName.value
    
                ## Verifies the presence of the "CompiledFunctions.zip" file.
                Write-Host "    Confirming Compiled Azure Functions is present"
                IF(Test-Path $ArchiveFunctionZip)
                {
                    ## The "CompiledFunctions.zip" was found in the working directory
                    Write-Host "      File Path Found: $ArchiveFunctionZip"

                    ## Uploads the Windows Package Manager functions to the Azure Function.
                    Write-Host "    Copying function files to the Azure Function."
                    $Result = Publish-AzWebApp -ArchivePath $ArchiveFunctionZip -ResourceGroupName $AzResourceGroup -Name $AzFunctionName -Force
                }
                else 
                {
                    ## The "CompiledFunctions.zip" was not found. Unable to uploaded to the Azure Function.
                    Write-Host "      File Path not found: $ArchiveFunctionZip" -ForegroundColor Red
                }
            }
        }
    }
    End
    {}
}

## Incorporates the indexing into the Azure Resource Group Name. Not required.
$AzResourceGroup = $("$AzResourceGroup$Index").Replace("-","")
$GenerateString  = $true

## Script Begins
$Result = New-WinGetRepo -ResourcePrefix $ResourcePrefix -Index $Index -AzResourceGroup $AzResourceGroup -AzSubscriptionName $AzSubscriptionName -AzLocation $AzLocation -WorkingDirectory $WorkingDirectory

#Creates a spacing between the last step and the next
Write-Host "`n"

$AzSubscriptionName = $(Get-AzContext).Subscription.Name
$jsonFunction       = Get-Content -Path $($Result.Where({$_.ObjectType -eq "Function"}).ParameterPath) | ConvertFrom-Json
$AzFunctionName     = $jsonFunction.Parameters.FunctionName.Value
$AzFunctionURL      = $(Get-AzFunctionApp -Name $AzFunctionName -ResourceGroupName $AzResourceGroup).DefaultHostName

## Post script Run Informational:
#### Instructions on how to add the repository to your Windows Package Manager Client
Write-Host "Use the following command to register the new private repository with your Windows Package Manager Client:" -ForegroundColor Yellow
Write-Host "  winget source add -n ""PrivateRepo"" -a ""https://$AzFunctionURL/api/"" -t ""Microsoft.Rest""" -ForegroundColor Yellow

#### For more information about how to use the solution, visit the aka.ms link.
Write-Host "`n  For more information on the Windows Package Manager Client, go to: https://aka.ms/winget-command-help`n"