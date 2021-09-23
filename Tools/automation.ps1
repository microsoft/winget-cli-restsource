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
        - If not specified, the default Subscription will be used. (https://docs.microsoft.com/en-us/azure/azure-portal/set-preferences)
        - If specified, then the specified Azure Subscription will be used.
    
    .PARAMETER AzLocation
    Azure location that will be used for the created resources. (Default: westus)
    
    .PARAMETER WorkingDirectory
    Local file repository that will be referenced by this script. Used to locate the Template files, and identifies where the Parameters will be created (Default: Script Location).
    
    .PARAMETER ShowConnectionInstructions
    Outputs to the screen after all Azure objects have been created, the command to add the new private repository to your Windows Package Manager client.

    .EXAMPLE
    .\automation.ps1 -ResourcePrefix "contoso-" -Index "Demo" -AzResourceGroup "WinGet_PrivateRepo_Demo"

    .EXAMPLE
    .\automation.ps1 -ResourcePrefix "contoso-" -Index "001" -AzResourceGroup "WinGet_PrivateRepo" -AzLocation "westus"

    .EXAMPLE
    .\automation.ps1 -ResourcePrefix "contoso-" -Index "Prod" -AzResourceGroup "WinGet_PrivateRepo_Prod" -AzSubscription "Contoso Global" -ShowConnectionInstructions

    #>

param(
    [Parameter(Position=0, Mandatory=$true)]  [string]$ResourcePrefix,
    [Parameter(Position=1, Mandatory=$false)][AllowNull()][AllowEmptyString()]  [string]$Index,
    [Parameter(Position=2, Mandatory=$true)]  [string]$AzResourceGroup,
    [Parameter(Position=3, Mandatory=$false)] [string]$AzSubscriptionName,
    [Parameter(Position=4, Mandatory=$false)] [string]$AzLocation         = "westus",
    [Parameter(Position=5, Mandatory=$false)] [string]$WorkingDirectory   = "$($(Get-Item $PSScriptRoot).fullname)\..\src\WinGet.RestSource.Infrastructure",
    [Parameter(Position=6, Mandatory=$false)] [string]$ArchiveFunctionZip = "CompiledFunctions.zip"
)

Function Test-RequiredModules
{
    <#
        Description:
        This function will validate that the PowerShell session has the required PowerShell modules installed. Returning a boolean. 
            - True if the PowerShell session has all required modules installed,
            - False if the PowerShell session does not have all required modules installed.
    #>
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
    <#
    Description:
    This function will leverage all other functions in this script to:
        - Initiate a connection to Azure
        - Create the Azure Resource Group
        - Create Parameter files for all required Azure Resources
        - Validate the Names and parameter files meet Azure Requirements
        - Create the Azure resources
    #>
    param(
        [Parameter(Position=0)] [string]$ResourcePrefix,
        [Parameter(Position=1)] [string]$Index,
        [Parameter(Position=2)] [string]$AzResourceGroup,
        [Parameter(Position=3)] [string]$AzSubscriptionName,
        [Parameter(Position=4)] [string]$AzLocation,
        [Parameter(Position=5)] [string]$WorkingDirectory = $PSScriptRoot,
        [Parameter(Position=5)] [string]$ArchiveFunctionZip,
        [Parameter(Position=6)] [switch]$ShowConnectionInstructions
    )
    Begin
    {
        ## Paths to the Parameter and Template folders and the location of the Function Zip
        $ParameterFolderPath = "$WorkingDirectory\Parameters"
        $TemplateFolderPath  = "$WorkingDirectory\Templates"
        $ArchiveFunctionZip  = "$WorkingDirectory\$ArchiveFunctionZip"

        ## Outlines the Azure Modules that are required for this Function to work.
        $RequiredModules     = @("Az.Resources", "Az.Accounts", "Az.KeyVault","Az.Websites", "Az.Functions")
    }
    Process
    {
        ## Test that the Required Azure Modules are installed
        $Result = @()
        ForEach( $RequiredModule in $RequiredModules )
            { $Result += Test-RequiredModules -RequiredModule $RequiredModule }
        
        IF( $Result )
        {
            ## Modules have been identified as missing
            $ErrorMessage = "`n`nMissing required PowerShell modules`n"
            $ErrorMessage += "    Run the following command to install the missing modules: Import-Module Az`n"
            
            Write-Host $ErrorMessage -ForegroundColor Yellow
            Throw "Unable to run script, missing required PowerShell modules..."
        }

        ## Create Folders for the Parameter and Template folder paths
        $Result = New-Item -ItemType Directory -Path $ParameterFolderPath -ErrorAction SilentlyContinue -InformationAction SilentlyContinue
        IF($Result)
            { Write-Host "Created Directory to contain the ARM Parameter files ($($Result.FullName))." }

        #### Connect to Azure ####
        $Result = Connect-Azure -AzSubscriptionName $AzSubscriptionName

        ## If the connection to azure attempt fails.. then exit.
        IF(!$($Result))
            { Throw "Failed to connect to Azure" }

        #### Create Resource Group ####
        New-AzureResourceGroup -AzResourceGroupName $AzResourceGroup

        #### Creates the ARM Parameter files ####
        $ARMObjects = New-ARMParameterObject -ParameterFolderPath $ParameterFolderPath -TemplateFolderPath $TemplateFolderPath -Index $Index -ResourcePrefix $ResourcePrefix -AzLocation $AzLocation

        #### Verifies ARM Parameters are correct ####
        $Result = Test-ARMTemplate -ARMObjects $ARMObjects

        ## If the attempt fails.. then exit.
        IF($($Result))
        { 
            Write-Host "  ERROR: ARM Template and Parameter testing failed`n" -ForegroundColor Red
            Return $ARMObjects
        }

        #### Creates Azure Objects with ARM Templates and Parameters ####
        New-ARMObjects -ARMObjects $ARMObjects -ArchiveFunctionZip "$WorkingDirectory\CompiledFunctions.zip" -AzResourceGroup $AzResourceGroup

        #Creates a spacing between the last step and the next
        Write-Host "`n"

        ## Shows how to connect local Windows Package Manager Client to newly created private repository
        IF($ShowConnectionInstructions)
        {
            $AzSubscriptionName = $(Get-AzContext).Subscription.Name
            $jsonFunction       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Function"}).ParameterPath) | ConvertFrom-Json
            $AzFunctionName     = $jsonFunction.Parameters.FunctionName.Value
            $AzFunctionURL      = $(Get-AzFunctionApp -Name $AzFunctionName -ResourceGroupName $AzResourceGroup).DefaultHostName

            ## Post script Run Informational:
            #### Instructions on how to add the repository to your Windows Package Manager Client
            Write-Host "Use the following command to register the new private repository with your Windows Package Manager Client:" -ForegroundColor Yellow
            Write-Host "  winget source add -n ""PrivateRepo"" -a ""https://$AzFunctionURL/api/"" -t ""Microsoft.Rest""" -ForegroundColor Yellow

            #### For more information about how to use the solution, visit the aka.ms link.
            Write-Host "`n  For more information on the Windows Package Manager Client, go to: https://aka.ms/winget-command-help`n"
        }
    }
    End
    {
        Return $ARMObjects
    }
}

Function Connect-Azure
{
    <#
    Description:
    Initiates a connection to Azure, and returns a Boolean.
        - True if the connection was successful
        - False if the connection failed
    #>
    Param(
        [Parameter(Position=0, Mandatory=$false)] [string]$AzSubscriptionName
    )
    Begin
    {
        IF($null -eq $AzSubscriptionName)
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
        Else 
        {
            ## Connects to Azure with a pre-defined Subscription.
            $Result = Connect-AzAccount -SubscriptionName $AzSubscriptionName -ErrorVariable Azerror -WarningAction SilentlyContinue
        }
    }
    End
    {
        ## Verifies connection to Azure was successful.
        IF($Azerror)
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
    <#
    Description:
    Checks to see if the designated Resource Group name already exists, and if exists will not re-create. If
    the Resource Group doesn't exist, it will create a new Resource Group in Azure. If successful, returns a boolean.
        - True if the group was pre-existing or created successfully.
        - False if the group failed to be created.
    #>
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
        Else 
        {
            ## Resource Group failed to be created 
            Write-Host "  Resource Group ($AzResourceGroupName) in the $AzLocation region failed to be created" -ForegroundColor Red 
            Return $false
        }
    }
}

Function New-ARMParameterObject
{
    <#
    Description:
    Creates a new PowerShell object that contains the Azure Resource type, name, and parameter values. Once
    created it'll output the parameter files into a *.json file that can be used in combination with with 
    template files to build Azure resources required for hosting a Windows Package Manager private source.
    Returns the PowerShell object.
    #>
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
        $AppInsightsName    = $($ResourcePrefix + $Index)
        $KeyVaultName       = $($ResourcePrefix + $Index)
        $StorageAccountName = $($ResourcePrefix + $Index).Replace("-", "")
        $aspName            = $($ResourcePrefix + $Index)
        $CDBAccountName     = $($ResourcePrefix + $Index)
        $FunctionName       = $($ResourcePrefix + $Index)
        $FrontDoorName      = $($ResourcePrefix + $Index)

        ## The names of the Azure Cosmos Database and Container - Do not change (Must match with the values in the compiled Windows Package Manager Functions [CompiledFunctions.zip])
        $CDBDatabaseName    = $("WinGet")
        $CDBContainerName   = $("Manifests")
        
        ## The name of the Secret that will be created in the Azure Keyvault - Do not change
        $AzKVStorageSecretName = "AzStorageAccountKey"
        
        ## This is the Azure Key Vault Key used to store the Connection String to the Storage Account
        $AzTenantID            = $(Get-AzContext).Tenant.Id
        $AzDirectoryID         = $(Get-AzADUser).Where({$_.UserPrincipalName -like "$($(Get-AzContext).Account.ID.Split("@")[0])*"}).ID
        
        ## This is specific to the JSON file creation
        $JSONContentVersion = "1.0.0.0"
        $JSONSchema         = "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#"
    }
    Process
    {    
        ## Creates a PowerShell object array to contain the details of the Parameter files.
        $ARMObjects = @(
            @{  ObjectType = "AppInsight"
                ObjectName = $AppInsightsName
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
                ObjectName = $KeyVaultName
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
                ObjectName = $StorageAccountName
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
                ObjectName = $aspName
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
                ObjectName = $CDBAccountName
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
                ObjectName = $CDBDatabaseName
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
                ObjectName = $CDBContainerName
                ParameterPath  = "$ParameterFolderPath\cosmosdb-sql-container.$Index.json"
                TemplatePath   = "$TemplateFolderPath\CosmosDB\cosmosdb-sql-container.json"
                Error      = ""
                Parameters = @{
                    '$Schema' = $JSONSchema
                    contentVersion = $JSONContentVersion
                    Parameters = @{
                        cosmosName     = @{ Value = $CDBAccountName }
                        sqlname        = @{ Value = $CDBDatabaseName }
                        containerName  = @{ value = $CDBContainerName}
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
                ObjectName = $FunctionName
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
            #     ObjectName = $FrontDoorName
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
        ForEach ($object in $ARMObjects)
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
    <#
    Description:
    Validates that the parameter files have been build correctly, matches to the template files, and
    can be used to build Azure resources. Will also validate that the naming used for the resources
    is available, and meets requirements. Returns boolean.
        - True, if the validation testing passes
        - False, if the validation testing fails.
    #>
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
        $TestResults = @()

        ## Parses through all ARM Parameter objects to validate they are properly configured.
        ForEach($Object in $ARMObjects)
        {
            ## Validates that each ARM object will work.
            Write-Host "  Validation testing on ARM Resource ($($Object.ObjectType))..."
            $AzResourceResult = Test-AzResourceGroupDeployment -ResourceGroupName $AzResourceGroup -Mode Complete -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath
            $AzNameResult     = Test-ARMResourceName -ARMObject $Object

            ## If the ARM object fails validation, report error to screen.
            IF($AzResourceResult -ne "" -or !$AzNameResult)
            { 
                ## Testing fails.
                IF($AzResourceResult -ne "")
                {
                    Write-Host "    ERROR:  $AzResourceResult" -ForegroundColor Red
                    IF($AzNameResult)
                        { Write-Host "    ERROR:  $($Object.ObjectType) Name is already in use, or there is an error with the Parameter file" -ForegroundColor Red }
                    Else
                        { Write-Host "    ERROR:  $($Object.ObjectType) Name does not meet the requirements" -ForegroundColor Red }
                }
                ElseIF(!$AzNameResult)
                {
                    Write-Host "    ERROR:  $($Object.ObjectType) Name does not meet the requirements." -ForegroundColor Red
                }

                $TestResult = @{
                    ObjectType = $Object.ObjectType
                    ObjectName = $Object.Parameters.Parameters.Name
                    Result     = $Result
                }
                $TestResults += $TestResult
            }
        }
    }
    End
    {
        ## Returns the TestResults.
        Return $TestResults
    }
}

Function Test-ARMResourceName
{
    <#
    Description:
    Validates that the name of the Azure Resource meets the Azure specified requirements
    #>
    Param(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Targetted")]
        [ValidateSet("AppInsight", "KeyVault", "StorageAccount", "asp", "CosmosDBAccount", "CosmosDBDatabase", "CosmosDBContainer", "Function", "FrontDoor")][String] $ResourceType,
        [Parameter(Position=1, Mandatory=$true, ParameterSetName="Targetted")][String] $ResourceName,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="SingleObject")] $ARMObject,
        [Parameter(Position=2, Mandatory=$false)][Switch] $VerboseLogging)

    Begin
    {
        ## Allows for a single instance of the ARM Object to be passed in
        IF($PSCmdlet.ParameterSetName -eq "SingleObject")
        {
            ## Sets the required variables based on the ARM Object
            $ResourceType = $ARMObject.ObjectType
            $ResourceName = $ARMObject.ObjectName
        }

        ## Preset output experience
        $TextColorIfTrue      = "Green"
        $TextColorIfFalse     = "Red"
        $TextPaddingRight     = 24
        $TextPaddingRightChar = " "

        ## Creates an array of values to be compared against
        $LowerAlphabet  = @("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z")
        $UpperAlphabet  = @("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z")
        $Numbers        = @("1", "2", "3", "4", "5", "6", "7", "8", "9", "0")
        $SpecialChar    = @("!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "+", "=", "{", "}", "[", "]", ":", ";", """", "'", "<", ",", ">", ".", "?", "/", "|", "\")

        ## Pre-Sets Values to False until proven otherwise
        $NameContainsHyphen                 = $false
        $NameContainsUnderscore             = $false
        $NameContainsConsecutiveHyphen      = $false
        $NameContainsConsecutiveunderscore  = $false
        $NameStartsWithLetter               = $false
        $NameStartsWithNumber               = $false
        $NameEndsWithLetter                 = $false
        $NameEndsWithNumber                 = $false
        $NameContainsLowerCase              = $false
        $NameContainsUpperCase              = $false
        $NameContainsNumber                 = $false
        $NameContainsSpecialChar            = $false
        $NameLengthInRange                  = $false
        $NameContainsSpaces                 = $false

        ## Sets the Values accordingly
        $Result = $False
        $NameLength = $ResourceName.Length
        $NameContainsHyphen     = $ResourceName.Contains("-")
        $NameContainsUnderscore = $ResourceName.Contains("_")
        $NameContainsSpaces     = $ResourceName.Contains(" ")
        $NameContainsConsecutiveHyphen     = $ResourceName.Contains("--")
        $NameContainsConsecutiveunderscore = $ResourceName.Contains("__")

        ## Validates if the name starts with, ends with or contains a number
        ForEach($Number in $Numbers){IF($ResourceName.StartsWith($Number)){$NameStartsWithNumber = $True}}
        ForEach($Number in $Numbers){IF($ResourceName.EndsWith($Number))  {$NameEndsWithNumber   = $True}}
        ForEach($Number in $Numbers){IF($ResourceName.Contains($Number))  {$NameContainsNumber   = $True}}

        ## Validates if the name starts with or ends with a letter
        ForEach($Letter in $LowerAlphabet){IF($ResourceName.ToLower().StartsWith($Letter)){$NameStartsWithLetter = $True}}
        ForEach($Letter in $LowerAlphabet){IF($ResourceName.ToLower().EndsWith($Letter))  {$NameEndsWithLetter   = $True}}

        ## Validates that the name contains an upper, lower or special characters
        ForEach($Letter in $LowerAlphabet){IF($ResourceName.Contains($Letter)){$NameContainsLowerCase   = $True}}
        ForEach($Letter in $LowerAlphabet){IF($ResourceName.Contains($Letter)){$NameContainsLowerCase   = $True}}
        ForEach($Char   in $SpecialChar)  {IF($ResourceName.Contains($Char))  {$NameContainsSpecialChar = $True}}

    }
    Process
    {
        IF($VerboseLogging)
        {
            ## Writes the value of each test to the screen if VerboseLogging is enabled.
            ## If the value is true, it is displayed in the color Green
            Write-Host "`n`n    The $ResourceType named ""$ResourceName"" meets the follow specifications:"
            Write-Host "      Name Length:            $NameLength"
            Write-Host "      Hypen:                  " -NoNewline; IF($NameContainsHyphen)               { Write-Host "$NameContainsHyphen" -ForegroundColor $TextColorIfTrue }                Else { Write-Host "$NameContainsHyphen" -ForegroundColor $TextColorIfFalse }
            Write-Host "      Underscore:             " -NoNewline; IF($NameContainsUnderscore)           { Write-Host "$NameContainsUnderscore" -ForegroundColor $TextColorIfTrue }            Else { Write-Host "$NameContainsUnderscore" -ForegroundColor $TextColorIfFalse }
            Write-Host "      2-Hypens:               " -NoNewline; IF($NameContainsConsecutiveHyphen)    { Write-Host "$NameContainsConsecutiveHyphen" -ForegroundColor $TextColorIfTrue }     Else { Write-Host "$NameContainsConsecutiveHyphen" -ForegroundColor $TextColorIfFalse }
            Write-Host "      2-Underscore:           " -NoNewline; IF($NameContainsConsecutiveunderscore){ Write-Host "$NameContainsConsecutiveunderscore" -ForegroundColor $TextColorIfTrue } Else { Write-Host "$NameContainsConsecutiveunderscore" -ForegroundColor $TextColorIfFalse }
            Write-Host "      Special Char:           " -NoNewline; IF($NameContainsSpecialChar)          { Write-Host "$NameContainsSpecialChar" -ForegroundColor $TextColorIfTrue }           Else { Write-Host "$NameContainsSpecialChar" -ForegroundColor $TextColorIfFalse }
            Write-Host "      Starts with letter:     " -NoNewline; IF($NameStartsWithLetter)             { Write-Host "$NameStartsWithLetter" -ForegroundColor $TextColorIfTrue }              Else { Write-Host "$NameStartsWithLetter" -ForegroundColor $TextColorIfFalse }
            Write-Host "      Starts with number:     " -NoNewline; IF($NameStartsWithNumber)             { Write-Host "$NameStartsWithNumber" -ForegroundColor $TextColorIfTrue }              Else { Write-Host "$NameStartsWithNumber" -ForegroundColor $TextColorIfFalse }
            Write-Host "      Ends with letter:       " -NoNewline; IF($NameEndsWithLetter)               { Write-Host "$NameEndsWithLetter" -ForegroundColor $TextColorIfTrue }                Else { Write-Host "$NameEndsWithLetter" -ForegroundColor $TextColorIfFalse }
            Write-Host "      Ends with number:       " -NoNewline; IF($NameEndsWithNumber)               { Write-Host "$NameEndsWithNumber" -ForegroundColor $TextColorIfTrue }                Else { Write-Host "$NameEndsWithNumber" -ForegroundColor $TextColorIfFalse }
            Write-Host "      Contains lower case:    " -NoNewline; IF($NameContainsLowerCase)            { Write-Host "$NameContainsLowerCase" -ForegroundColor $TextColorIfTrue }             Else { Write-Host "$NameContainsLowerCase" -ForegroundColor $TextColorIfFalse }
            Write-Host "      Contains upper case:    " -NoNewline; IF($NameContainsUpperCase)            { Write-Host "$NameContainsUpperCase" -ForegroundColor $TextColorIfTrue }             Else { Write-Host "$NameContainsUpperCase" -ForegroundColor $TextColorIfFalse }
            Write-Host "      Contains numbers:       " -NoNewline; IF($NameContainsNumber)               { Write-Host "$NameContainsNumber" -ForegroundColor $TextColorIfTrue }                Else { Write-Host "$NameContainsNumber" -ForegroundColor $TextColorIfFalse }
            Write-Host "      Contains spaces:        " -NoNewline; IF($NameContainsSpaces)               { Write-Host "$NameContainsSpaces" -ForegroundColor $TextColorIfTrue }                Else { Write-Host "$NameContainsSpaces" -ForegroundColor $TextColorIfFalse }
            Write-Host "`n"
        }

        Switch ($ResourceType)
        {
            "KeyVault" 
            { 
                ## Alphanumerics, and Hyphens, starts with letter, ends with letter or number. Con't contain consecutive hyphens. Length: 3-24
                IF($($NameLength -ge 3) -and $($NameLength -le 24))
                    { $NameLengthInRange = $true}

                IF($NameLengthInRange -and !$NameContainsSpecialChar -and !$NameStartsWithNumber -and !$NameContainsConsecutiveHyphen)
                    { $Result = $true }

                ## Outputs the tests to the screen and their status
                Write-Host "    Testing the ""$ResourceName"" name meets the follow requirements:"
                Write-Host "      $("Name within Length:".PadRight($TextPaddingRight, $TextPaddingRightChar))"     -NoNewline; IF($NameLengthInRange)              { Write-Host "$NameLengthInRange"                 -ForegroundColor $TextColorIfTrue }Else { Write-Host "$NameLengthInRange" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("No Special Chars:".PadRight($TextPaddingRight, $TextPaddingRightChar))"       -NoNewline; IF(!$NameContainsSpecialChar)       { Write-Host "$(!$NameContainsSpecialChar)"       -ForegroundColor $TextColorIfTrue }Else { Write-Host "$(!$NameContainsSpecialChar)" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("Doesn't start with Num:".PadRight($TextPaddingRight, $TextPaddingRightChar))" -NoNewline; IF(!$NameStartsWithNumber)          { Write-Host "$(!$NameStartsWithNumber)"          -ForegroundColor $TextColorIfTrue }Else { Write-Host "$(!$NameStartsWithNumber)" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("No consecutive hyphens:".PadRight($TextPaddingRight, $TextPaddingRightChar))" -NoNewline; IF(!$NameContainsConsecutiveHyphen) { Write-Host "$(!$NameContainsConsecutiveHyphen)" -ForegroundColor $TextColorIfTrue }Else { Write-Host "$(!$NameContainsConsecutiveHyphen)" -ForegroundColor $TextColorIfFalse }
                Write-Host "      ------------------------------"
                Write-Host "      $("Validation Result:".PadRight($TextPaddingRight, $TextPaddingRightChar))"      -NoNewline; IF($Result)                         { Write-Host "$Result"                            -ForegroundColor $TextColorIfTrue }Else { Write-Host "$Result" -ForegroundColor $TextColorIfFalse }
                Write-Host ""
            }
            "StorageAccount" 
            { 
                ## Alphanumerics, Hyphens, and underscores, Length: 3-60
                IF($($NameLength -ge 3) -and $($NameLength -le 60))
                    { $NameLengthInRange = $true}

                IF($NameLengthInRange -and !$NameContainsSpecialChar -and !$NameContainsUpperCase)
                    { $Result = $true }
                
                ## Outputs the tests to the screen and their status
                Write-Host "    Testing the ""$ResourceName"" name meets the follow requirements:"
                Write-Host "      $("Name within length:".PadRight($TextPaddingRight, $TextPaddingRightChar))"  -NoNewline; IF($NameLengthInRange)        { Write-Host "$NameLengthInRange"           -ForegroundColor $TextColorIfTrue }Else { Write-Host "$NameLengthInRange" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("No special chars:".PadRight($TextPaddingRight, $TextPaddingRightChar))"    -NoNewline; IF(!$NameContainsSpecialChar) { Write-Host "$(!$NameContainsSpecialChar)" -ForegroundColor $TextColorIfTrue }Else { Write-Host "$(!$NameContainsSpecialChar)" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("No upper case chars:".PadRight($TextPaddingRight, $TextPaddingRightChar))" -NoNewline; IF(!$NameContainsUpperCase)   { Write-Host "$(!$NameContainsUpperCase)"   -ForegroundColor $TextColorIfTrue }Else { Write-Host "$(!$NameContainsUpperCase)" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("No spaces:".PadRight($TextPaddingRight, $TextPaddingRightChar))"           -NoNewline; IF(!$NameContainsSpaces)      { Write-Host "$(!$NameContainsSpaces)"      -ForegroundColor $TextColorIfTrue }Else { Write-Host "$(!$NameContainsSpaces)" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("-".PadRight($TextPaddingRight+6, "-"))"
                Write-Host "      $("Validation Result:".PadRight($TextPaddingRight, $TextPaddingRightChar))"   -NoNewline; IF($Result)                   { Write-Host "$Result"                      -ForegroundColor $TextColorIfTrue }Else { Write-Host "$Result" -ForegroundColor $TextColorIfFalse }
                Write-Host ""
            }
            "asp"
            {
                $NameLengthInRange = $true

                IF($NameLengthInRange -and !$NameContainsSpaces)
                    { $Result = $true }

                ## Outputs the tests to the screen and their status
                Write-Host "    Testing the ""$ResourceName"" name meets the follow requirements:"
                Write-Host "      $("No spaces:".PadRight($TextPaddingRight, $TextPaddingRightChar))"         -NoNewline; IF(!$NameContainsSpaces){ Write-Host "$(!$NameContainsSpaces)" -ForegroundColor $TextColorIfTrue }Else { Write-Host "$(!$NameContainsSpaces)" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("-".PadRight($TextPaddingRight+6, "-"))"
                Write-Host "      $("Validation Result:".PadRight($TextPaddingRight, $TextPaddingRightChar))" -NoNewline; IF($Result)             { Write-Host "$Result"                 -ForegroundColor $TextColorIfTrue }Else { Write-Host "$Result" -ForegroundColor $TextColorIfFalse }
                Write-Host ""
            }
            "Function"
            { 
                ## Alphanumerics, Hyphens, and underscores, Length: 3-60
                IF($($NameLength -ge 3) -and $($NameLength -le 63))
                    { $NameLengthInRange = $true}

                IF($NameLengthInRange -and !$NameContainsSpecialChar)
                    { $Result = $true }

                ## Outputs the tests to the screen and their status
                Write-Host "    Testing the ""$ResourceName"" name meets the follow requirements:"
                Write-Host "      $("Name within Length:".PadRight($TextPaddingRight, $TextPaddingRightChar))" -NoNewline; IF($NameLengthInRange)        { Write-Host "$NameLengthInRange"           -ForegroundColor $TextColorIfTrue }Else { Write-Host "$NameLengthInRange" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("No Special Chars:".PadRight($TextPaddingRight, $TextPaddingRightChar))"   -NoNewline; IF(!$NameContainsSpecialChar) { Write-Host "$(!$NameContainsSpecialChar)" -ForegroundColor $TextColorIfTrue }Else { Write-Host "$(!$NameContainsSpecialChar)" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("-".PadRight($TextPaddingRight+6, "-"))"
                Write-Host "      $("Validation Result:".PadRight($TextPaddingRight, $TextPaddingRightChar))"  -NoNewline; IF($Result)                   { Write-Host "$Result"                      -ForegroundColor $TextColorIfTrue }Else { Write-Host "$Result" -ForegroundColor $TextColorIfFalse }
                Write-Host ""
            }
            "FrontDoor"
            { 
                ## Alphanumerics and hyphens. Start and end with alphanumeric. Length: 5-64
                IF($($NameLength -ge 5) -and $($NameLength -le 64))
                    { $NameLengthInRange = $true}

                IF($NameLengthInRange -and !$NameContainsSpecialChar)
                    { $Result = $true }

                ## Outputs the tests to the screen and their status
                Write-Host "    Testing the ""$ResourceName"" name meets the follow requirements:"
                Write-Host "      $("Name within Length:".PadRight($TextPaddingRight, $TextPaddingRightChar))" -NoNewline; IF($NameLengthInRange)        { Write-Host "$NameLengthInRange"           -ForegroundColor $TextColorIfTrue }Else { Write-Host "$NameLengthInRange" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("No Special Chars:".PadRight($TextPaddingRight, $TextPaddingRightChar))"   -NoNewline; IF(!$NameContainsSpecialChar) { Write-Host "$(!$NameContainsSpecialChar)" -ForegroundColor $TextColorIfTrue }Else { Write-Host "$(!$NameContainsSpecialChar)" -ForegroundColor $TextColorIfFalse }
                Write-Host "      $("-".PadRight($TextPaddingRight+6, "-"))"
                Write-Host "      $("Validation Result:".PadRight($TextPaddingRight, $TextPaddingRightChar))"  -NoNewline; IF($Result)                   { Write-Host "$Result"                      -ForegroundColor $TextColorIfTrue }Else { Write-Host "$Result" -ForegroundColor $TextColorIfFalse }
                Write-Host ""
            }
            Default 
            { 
                $Result = $true
            }
        }
    }
    End
    {
        Return $Result
    }
}

Function New-ARMObjects
{
    <#
    Description:
    Uses the custom PowerShell object provided by the "New-ARMParameterObject" cmdlet to create
    Azure resources, and will create the the Key Vault secrets and publish the Windows Package
    Manager private source rest apis to the Azure Function.
    #>
    param(
        [Parameter(Position=0)] $ARMObjects,
        [Parameter(Position=1)] [string] $ArchiveFunctionZip,
        [Parameter(Position=2)] [string] $AzResourceGroup
    )
    Begin
    {
        Write-Host "`n"

        ## Path to the compiled Functions compressed into a Zip file for upload to Azure Function
        #$ArchiveFunctionZip = "$WorkingDirectory\CompiledFunctions.zip"
        
        ## Imports the contents of the Parameter Files for reference and logging purposes:
        $jsonStorageAccount = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "StorageAccount" }).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json
        $jsoncdba           = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "CosmosDBAccount"}).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json
        $jsonKeyVault       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Keyvault"}).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json
        $jsonFunction       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Function"}).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json

        ## Azure resource names retrieved from the Parameter files.
        $AzKeyVaultName       = $jsonKeyVault.parameters.name.value
        $AzStorageAccountName = $jsonStorageAccount.parameters.storageAccountName.value
        $CosmosAccountName    = $jsoncdba.Parameters.Name.Value

        ## Azure Keyvault Secret Names - Do not change values
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
        
        ForEach ($Object in $ARMObjects)
        {
            Write-Host "  Creating the Azure Object - $($Object.ObjectType)"
    
            ## If the object to be created is an Azure Function, then complete these pre-required steps before creating the Azure Function.
            IF($Object.ObjectType -eq "Function")
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
            IF($objerror)
            {
                ## Creating the object following the ARM template failed.
                Write-Host "      Error: Object failed to create.`n      $objerror" -ForegroundColor Red
                Return
            }
            Else 
            {
                ## Creating the object was successful
                Write-Host "      $($Object.ObjectType) was successfully created."
            }
    
            ## Publish GitHub Functions to newly created Azure Function
            IF($Object.ObjectType -eq "Function")
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
                Else 
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

## Removes unsupported characters from the Resource Group Name
Write-Host "Removing hyphens (""-"") from the Azure Resource Group Name."
$AzResourceGroup = $("$AzResourceGroup$Index").Replace("-","")

## Script Begins
$Result = New-WinGetRepo -ResourcePrefix $ResourcePrefix -Index $Index -AzResourceGroup $AzResourceGroup -AzSubscriptionName $AzSubscriptionName -AzLocation $AzLocation -WorkingDirectory $WorkingDirectory -ArchiveFunctionZip $ArchiveFunctionZip -ShowConnectionInstructions
