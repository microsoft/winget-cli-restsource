# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function New-ARMParameterObjects
{
    <#
    .SYNOPSIS
    Creates the parameter files, and an object which points to both the created parameter and template files.

    .DESCRIPTION
    Creates a new PowerShell object that contains the Azure Resource type, name, and parameter values. Once created it'll
    output the parameter files into a *.json file that can be used in combination with with template files to build Azure
    resources required for hosting a Windows Package Manager REST source. Returns the PowerShell object.

    .PARAMETER ParameterFolderPath
    Path to the directory where the Parameter files will be created.

    .PARAMETER TemplateFolderPath
    Path to the directory containing the Template files.

    .PARAMETER Name
    The name of the objects to be created.

    .PARAMETER Region
    The Azure location where objects will be created in.

    .PARAMETER ImplementationPerformance
    ["Developer", "Basic", "Enhanced"] specifies the performance of the resources to be created for the Windows Package Manager REST source.

    .PARAMETER PublisherName
    [Optional] The WinGet rest source publisher name

    .PARAMETER PublisherEmail
    [Optional] The WinGet rest source publisher email

    .PARAMETER RestSourceAuthentication
    [Optional] ["None", "MicrosoftEntraId"] The WinGet rest source authentication type. (Default: None)

    .PARAMETER MicrosoftEntraIdResource
    [Optional] Microsoft Entra Id authentication resource

    .PARAMETER MicrosoftEntraIdResourceScope
    [Optional] Microsoft Entra Id authentication resource scope

    .PARAMETER ForUpdate
    [Optional] The operation is for update. (Default: False)

    .EXAMPLE
    New-ARMParameterObjects -ParameterFolderPath "C:\WinGet\Parameters" -TemplateFolderPath "C:\WinGet\Templates" -Name "contosorestsource" -AzLocation "westus" -ImplementationPerformance "Developer"

    Creates the Parameter files required for the creation of the ARM objects.

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)] [string]$ParameterFolderPath,
        [Parameter(Position=1, Mandatory=$true)] [string]$TemplateFolderPath,
        [Parameter(Position=2, Mandatory=$true)] [string]$Name,
        [Parameter(Position=3, Mandatory=$true)] [string]$Region,
        [Parameter(Position=4, Mandatory=$true)] [string]$ImplementationPerformance,
        [Parameter(Mandatory=$false)] [string]$PublisherName = "",
        [Parameter(Mandatory=$false)] [string]$PublisherEmail = "",
        [ValidateSet("None", "MicrosoftEntraId")]
        [Parameter(Mandatory=$false)] [string]$RestSourceAuthentication = "None",
        [Parameter(Mandatory=$false)] [string]$MicrosoftEntraIdResource = "",
        [Parameter(Mandatory=$false)] [string]$MicrosoftEntraIdResourceScope = "",
        [Parameter()] [switch]$ForUpdate
    )

    $ARMObjects = @()

    ## The Names that are to be assigned to each resource.
    $LogAnalyticsName   = "log-" + $Name -replace "[^a-zA-Z0-9-]", ""
    $AppInsightsName    = "appi-" + $Name -replace "[^a-zA-Z0-9-]", ""
    $KeyVaultName       = "kv-" + $Name -replace "[^a-zA-Z0-9-]", ""
    $StorageAccountName = "st" + $Name.ToLower() -replace "[^a-z0-9]", ""
    $AspName            = "asp-" + $Name -replace "[^a-zA-Z0-9-]", ""
    $CDBAccountName     = "cosmos-" + $Name.ToLower() -replace "[^a-z0-9-]", ""
    $FunctionName       = "func-" + $Name -replace "[^a-zA-Z0-9-]", ""
    $AppConfigName      = "appcs-" + $Name -replace "[^a-zA-Z0-9-]", ""
    $ApiManagementName  = "apim-" + $Name -replace "[^a-zA-Z0-9-]", ""
    $ServerIdentifier   = "WinGetRestSource-" + $Name -replace "[^a-zA-Z0-9-]", ""

    ## Not supported in deployment script
    ## $FrontDoorName   = ""
    ## $AspGenevaName   = ""

    ## The names of the Azure Cosmos Database and Container - Do not change (Must match with the values in the compiled
    ## Windows Package Manager Functions [WinGet.RestSource.Functions.zip])
    $CDBDatabaseName    = "WinGet"
    $CDBContainerName   = "Manifests"

    if ($RestSourceAuthentication -eq "MicrosoftEntraId") {
        $ServerAuthenticationType = "microsoftEntraId"
        $QueryApiValidationEnabled = $true
    }
    else {
        $ServerAuthenticationType = "none"
        $MicrosoftEntraIdResource = ""
        $MicrosoftEntraIdResourceScope = ""
        $QueryApiValidationEnabled = $false
    }

    ## The values required for Function ARM Template. But not supported in deployment script.
    $ManifestCacheEndpoint      = ""
    $MonitoringTenant           = ""
    $MonitoringRole             = ""
    $MonitoringMetricsAccount   = ""
    $RunFromPackageUrl          = ""

    ## Relative Path from the Working Directory to the Azure ARM Template Files
    $TemplateLogAnalyticsPath   = "$TemplateFolderPath\loganalytics.json"
    $TemplateAppInsightsPath    = "$TemplateFolderPath\applicationinsights.json"
    $TemplateKeyVaultPath       = "$TemplateFolderPath\keyvault.json"
    $TemplateStorageAccountPath = "$TemplateFolderPath\storageaccount.json"
    $TemplateASPPath            = "$TemplateFolderPath\asp.json"
    $TemplateCDBAccountPath     = "$TemplateFolderPath\cosmosdb.json"
    $TemplateCDBPath            = "$TemplateFolderPath\cosmosdb-sql.json"
    $TemplateCDBContainerPath   = "$TemplateFolderPath\cosmosdb-sql-container.json"
    $TemplateFunctionPath       = "$TemplateFolderPath\azurefunction.json"
    $TemplateAppConfigPath      = "$TemplateFolderPath\appconfig.json"
    $TemplateApiManagementPath  = "$TemplateFolderPath\apimanagement.json"

    $ParameterLogAnalyticsPath   = "$ParameterFolderPath\loganalytics.json"
    $ParameterAppInsightsPath    = "$ParameterFolderPath\applicationinsights.json"
    $ParameterKeyVaultPath       = "$ParameterFolderPath\keyvault.json"
    $ParameterStorageAccountPath = "$ParameterFolderPath\storageaccount.json"
    $ParameterASPPath            = "$ParameterFolderPath\asp.json"
    $ParameterCDBAccountPath     = "$ParameterFolderPath\cosmosdb.json"
    $ParameterCDBPath            = "$ParameterFolderPath\cosmosdb-sql.json"
    $ParameterCDBContainerPath   = "$ParameterFolderPath\cosmosdb-sql-container.json"
    $ParameterFunctionPath       = "$ParameterFolderPath\azurefunction.json"
    $ParameterAppConfigPath      = "$ParameterFolderPath\appconfig.json"
    $ParameterApiManagementPath  = "$ParameterFolderPath\apimanagement.json"

    Write-Verbose -Message "ARM Parameter Resource performance is based on the: $ImplementationPerformance."

    $PrimaryRegion       = $(Get-AzLocation).Where({$_.Location -eq $Region})
    $PrimaryRegionName   = $PrimaryRegion.DisplayName
    if ($PrimaryRegion.PairedRegion.Length -gt 0) {
        $SecondaryRegionName = $(Get-AzLocation).Where({$_.Location -eq $PrimaryRegion.PairedRegion[0].Name}).DisplayName
    }

    Write-Verbose -Message "Retrieving the Azure Tenant and User Information"
    $AzContext = Get-AzContext
    $AzTenantID = $AzContext.Tenant.Id
    $AzTenantDomain = $AzContext.Tenant.Domains[0]

    if ($AzContext.Account.Type -eq "User") {
        $LocalDeployment = $true
        $AzObjectID = $(Get-AzADUser -SignedIn).Id
        if (!$PublisherEmail) {
            $PublisherEmail = $AzContext.Account.Id
        }
        if (!$PublisherName) {
            $PublisherName = $AzContext.Account.Id
        }
    }
    else {
        $LocalDeployment = $false
        $AzObjectID = $(Get-AzADServicePrincipal -ApplicationId $AzContext.Account.ID).Id
        if (!$PublisherEmail) {
            $PublisherEmail = "WinGetRestSource@$AzTenantDomain"
        }
        if (!$PublisherName) {
            $PublisherName = "WinGetRestSource@$AzTenantDomain"
        }
    }
    Write-Verbose -Message "Retrieved the Azure Object Id: $AzObjectID"

    if ($AzContext.Environment.Name -eq "AzureUSGovernment") {
        $blobStorageServiceUri = "https://" + $StorageAccountName + ".blob.core.usgovcloudapi.net"
        $queueStorageServiceUri = "https://" + $StorageAccountName + ".queue.core.usgovcloudapi.net"
        $tableStorageServiceUri = "https://" + $StorageAccountName + ".table.core.usgovcloudapi.net"
    }
    elseif ($AzContext.Environment.Name -eq "AzureChinaCloud") {
        $blobStorageServiceUri = "https://" + $StorageAccountName + ".blob.core.chinacloudapi.net"
        $queueStorageServiceUri = "https://" + $StorageAccountName + ".queue.core.chinacloudapi.net"
        $tableStorageServiceUri = "https://" + $StorageAccountName + ".table.core.chinacloudapi.net"
    }
    else { ## AzureCloud as default
        $blobStorageServiceUri = "https://" + $StorageAccountName + ".blob.core.windows.net"
        $queueStorageServiceUri = "https://" + $StorageAccountName + ".queue.core.windows.net"
        $tableStorageServiceUri = "https://" + $StorageAccountName + ".table.core.windows.net"
    }

    switch ($ImplementationPerformance) {
        "Developer" {
            $AppConfigSku = "Free"
            $KeyVaultSKU  = "Standard"
            $StorageAccountPerformance = "Standard_LRS"
            $AspSku = "B1"
            $CosmosDBAEnableFreeTier   = $true
            ## To enable Serverless then set CosmosDBACapatilities to "[{"name": ""EnableServerless""}]"
            $CosmosDBACapabilities     = "[]"
            $CosmosDBAConsistency      = "ConsistentPrefix"
            $CosmosDBALocations = @(
                            @{
                                locationName     = $PrimaryRegionName
                                failoverPriority = 0
                                isZoneRedundant  = $false
                            }
                        )
            $ApiManagementSku = "Developer"
        }
        "Basic" {
            $AppConfigSku = "Standard"
            $KeyVaultSKU  = "Standard"
            $StorageAccountPerformance = "Standard_GRS"
            $AspSku = "S1"
            $CosmosDBAEnableFreeTier   = $false
            ## To enable Serverless then set CosmosDBACapatilities to "[{"name": ""EnableServerless""}]"
            $CosmosDBACapabilities     = "[]"
            $CosmosDBAConsistency      = "Session"
            $CosmosDBALocations = @(
                            @{
                                locationName     = $PrimaryRegionName
                                failoverPriority = 0
                                isZoneRedundant  = $false
                            }
                        )
            if ($SecondaryRegionName) {
                $CosmosDBALocations += @{
                                locationName     = $SecondaryRegionName
                                failoverPriority = 1
                                isZoneRedundant  = $false
                            }
            }
            $ApiManagementSku = "Basic"
        }
        "Enhanced" {
            $AppConfigSku = "Standard"
            $KeyVaultSKU  = "Standard"
            $StorageAccountPerformance = "Standard_GRS"
            $AspSku = "P1V2"
            $CosmosDBAEnableFreeTier   = $false
            ## To enable Serverless then set CosmosDBACapatilities to "[{"name": ""EnableServerless""}]"
            $CosmosDBACapabilities     = "[]"
            $CosmosDBAConsistency      = "Session"
            $CosmosDBALocations = @(
                            @{
                                locationName     = $PrimaryRegionName
                                failoverPriority = 0
                                isZoneRedundant  = $false
                            }
                        )
            if ($SecondaryRegionName) {
                $CosmosDBALocations += @{
                                locationName     = $SecondaryRegionName
                                failoverPriority = 1
                                isZoneRedundant  = $false
                            }
            }
            $ApiManagementSku = "Standard"
        }
    }

    ## This is specific to the JSON file creation
    $JSONContentVersion = "1.0.0.0"
    $JSONSchema         = "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#"

    ## Creates a PowerShell object array to contain the details of the Parameter files.
    $ARMObjects = @(
        @{  ObjectType = "LogAnalytics"
            ObjectName = $LogAnalyticsName
            ParameterPath  = "$ParameterLogAnalyticsPath"
            TemplatePath   = "$TemplateLogAnalyticsPath"
            Error      = ""
            Parameters = @{
                '$Schema' = $JSONSchema
                contentVersion = $JSONContentVersion
                Parameters = @{
                    name       = @{ value = $LogAnalyticsName }
                }
            }
        },
        @{  ObjectType = "AppInsight"
            ObjectName = $AppInsightsName
            ParameterPath  = "$ParameterAppInsightsPath"
            TemplatePath   = "$TemplateAppInsightsPath"
            Error      = ""
            Parameters = @{
                '$Schema' = $JSONSchema
                contentVersion = $JSONContentVersion
                Parameters = @{
                    name       = @{ value = $AppInsightsName }
                    location   = @{ value = $Region }
                    workspaceResourceName = @{ value = $LogAnalyticsName }
                }
            }
        },
        @{  ObjectType = "Keyvault"
            ObjectName = $KeyVaultName
            ParameterPath  = "$ParameterKeyVaultPath"
            TemplatePath   = "$TemplateKeyVaultPath"
            Error      = ""
            Parameters = @{
                '$Schema' = $JSONSchema
                contentVersion = $JSONContentVersion
                Parameters = @{
                    name            = @{ value = $KeyVaultName }
                    location        = @{ value = $Region }
                    sku             = @{ value = $KeyVaultSKU }
                    accessPolicies  = @{
                        value = @(
                            @{
                                tenantId = $AzTenantID
                                objectID = $AzObjectID
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
        @{  ObjectType = "AppConfig"
            ObjectName = $FunctionName
            ParameterPath  = "$ParameterAppConfigPath"
            TemplatePath   = "$TemplateAppConfigPath"
            Error      = ""
            Parameters = @{
                '$Schema' = $JSONSchema
                contentVersion = $JSONContentVersion
                Parameters = @{
                    appConfigName           = @{ value = $appConfigName         }   # Name used to contain the Storage Account connection string in the Key Value
                    location                = @{ value = $Region                }   # Azure hosting location
                    sku                     = @{ value = $AppConfigSku          }
                    localDeployment         = @{ value = $LocalDeployment       }
                }
            }
        },
        @{  ObjectType = "StorageAccount"
            ObjectName = $StorageAccountName
            ParameterPath  = "$ParameterStorageAccountPath"
            TemplatePath   = "$TemplateStorageAccountPath"
            Error      = ""
            Parameters = @{
                '$Schema' = $JSONSchema
                contentVersion = $JSONContentVersion
                Parameters = @{
                    location           = @{ value = $Region }
                    storageAccountName = @{ value = $StorageAccountName }
                    accountType        = @{ value = $StorageAccountPerformance }
                }
            }
        },
        @{  ObjectType = "asp"
            ObjectName = $aspName
            ParameterPath  = "$ParameterASPPath"
            TemplatePath   = "$TemplateASPPath"
            Error      = ""
            Parameters = @{
                '$Schema' = $JSONSchema
                contentVersion = $JSONContentVersion
                Parameters = @{
                    aspName         = @{ value = $aspName }
                    location        = @{ value = $Region }
                    skuCode         = @{ value = $AspSku }
                }
            }
        },
        @{  ObjectType = "CosmosDBAccount"
            ObjectName = $CDBAccountName
            ParameterPath  = "$ParameterCDBAccountPath"
            TemplatePath   = "$TemplateCDBAccountPath"
            Error      = ""
            Parameters = @{
                '$Schema' = $JSONSchema
                contentVersion = $JSONContentVersion
                Parameters = @{
                    name = @{ value = $CDBAccountName }
                    location = @{ value = $Region }
                    enableFreeTier = @{ value = $CosmosDBAEnableFreeTier }
                    consistencyPolicy = @{
                        value = @{
                            defaultConsistencyLevel = $CosmosDBAConsistency
                            maxIntervalInSeconds    = 5
                            maxStalenessPrefix      = 100
                        }
                    }
                    locations = @{ value = $CosmosDBALocations }
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
            ParameterPath  = "$ParameterCDBPath"
            TemplatePath   = "$TemplateCDBPath"
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
            ParameterPath  = "$ParameterCDBContainerPath"
            TemplatePath   = "$TemplateCDBContainerPath"
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
            ParameterPath  = "$ParameterFunctionPath"
            TemplatePath   = "$TemplateFunctionPath"
            Error      = ""
            Parameters = @{
                '$Schema' = $JSONSchema
                contentVersion = $JSONContentVersion
                Parameters = @{
                    location                 = @{ value = $Region                   }   # Azure hosting location
                    cosmosDatabase           = @{ value = $CDBDatabaseName          }   # Cosmos Database Name
                    cosmosContainer          = @{ value = $CDBContainerName         }   # Cosmos Container Name
                    serverIdentifier         = @{ value = $ServerIdentifier         }   # Azure Function Server Identifier
                    functionName             = @{ value = $FunctionName             }   # Azure Function Name
                    appServiceName           = @{ value = $aspName                  }   # Azure App Service Name
                    keyVaultName             = @{ value = $KeyVaultName             }   # Azure Keyvault Name
                    blobStorageServiceUri    = @{ value = $blobStorageServiceUri    }   # Azure Blob Storage Uri
                    queueStorageServiceUri   = @{ value = $queueStorageServiceUri   }   # Azure Queue Storage Uri
                    tableStorageServiceUri   = @{ value = $tableStorageServiceUri   }   # Azure Table Storage Uri
                    appInsightName           = @{ value = $AppInsightsName          }   # Azure App Insights Name
                    manifestCacheEndpoint    = @{ value = $manifestCacheEndpoint    }   # Not suported
                    monitoringTenant         = @{ value = $monitoringTenant         }   # Not suported
                    monitoringRole           = @{ value = $monitoringRole           }   # Not suported
                    monitoringMetricsAccount = @{ value = $monitoringMetricsAccount }   # Not suported
                    runFromPackageUrl        = @{ value = $RunFromPackageUrl        }   # Not suported
                    serverAuthenticationType = @{ value = $ServerAuthenticationType }   # Server authentication type
                    microsoftEntraIdResource = @{ value = $MicrosoftEntraIdResource }   # Microsoft Entra Id Resource
                    microsoftEntraIdResourceScope = @{ value = $MicrosoftEntraIdResourceScope }  # Microsoft Entra Id Resource Scope
                }
            }
        },
        @{  ObjectType = "ApiManagement"
            ObjectName = $ApiManagementName
            ParameterPath  = "$ParameterApiManagementPath"
            TemplatePath   = "$TemplateApiManagementPath"
            Error      = ""
            Parameters = @{
                '$Schema' = $JSONSchema
                contentVersion = $JSONContentVersion
                Parameters = @{
                    serviceName = @{ value = $ApiManagementName }
                    publisherEmail = @{ value = $PublisherEmail }
                    publisherName = @{ value = $PublisherName }
                    sku = @{ value = $ApiManagementSku }
                    location = @{ value = $Region }
                    keyVaultName = @{ value = $KeyVaultName }
                    backendUrls = @{ value = @() }  # Value to be populated after Azure Function is created
                    queryApiValidationEnabled = @{ value = $QueryApiValidationEnabled }
                    microsoftEntraIdResource = @{ value = $MicrosoftEntraIdResource }
                }
            }
        }    
    )

    ## Uses the newly created ARMObjects[#].Parameters to create new JSON Parameter files.
    Write-Verbose -Message "Creating JSON Parameter files for Azure Object Creation:"

    ## Creates each JSON Parameter file inside of a Parameter folder in the working directory
    foreach ($object in $ARMObjects) {
        ## Converts the structure of the variable to a JSON file.
        Write-Verbose -Message "Creating the Parameter file for $($Object.ObjectType) in the following location: $($Object.ParameterPath)"

        if ($ForUpdate -and (Test-Path -Path $Object.ParameterPath)) {
            $Object.Parameters = Get-Content -Path $Object.ParameterPath -Raw | ConvertFrom-Json
        }
        else {
            $ParameterFileContent = $Object.Parameters | ConvertTo-Json -Depth 8
            $ParameterFileContent | Out-File -FilePath $Object.ParameterPath -Force
        }
    }

    ## Returns the completed object.
    return $ARMObjects
}
