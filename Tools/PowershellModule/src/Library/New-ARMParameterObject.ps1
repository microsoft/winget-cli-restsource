# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function New-ARMParameterObject
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
    ["Demo", "Basic", "Enhanced"] specifies the performance of the resources to be created for the Windows Package Manager REST source.

    .EXAMPLE
    New-ARMParameterObject -ParameterFolderPath "C:\WinGet\Parameters" -TemplateFolderPath "C:\WinGet\Templates" -Name "contosorestsource" -AzLocation "westus" -ImplementationPerformance "Demo"

    Creates the Parameter files required for the creation of the ARM objects.

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)] [string]$ParameterFolderPath,
        [Parameter(Position=1, Mandatory=$false)][string]$TemplateFolderPath = "$PSScriptRoot\ARMTemplate",
        [Parameter(Position=2, Mandatory=$true)] [string]$Name,
        [Parameter(Position=3, Mandatory=$true)] [string]$Region,
        [Parameter(Position=4, Mandatory=$true)] [string]$ImplementationPerformance
    )
    BEGIN
    {
        ## The Names that are to be assigned to each resource.
        $AppInsightsName    = $Name
        $KeyVaultName       = $Name
        $StorageAccountName = $Name.Replace("-", "")
        $aspName            = $Name
        $CDBAccountName     = $Name
        $FunctionName       = $Name
        $FrontDoorName      = $Name

        ## The names of the Azure Cosmos Database and Container - Do not change (Must match with the values in the compiled
        ## Windows Package Manager Functions [WinGet.RestSource.Functions.zip])
        $CDBDatabaseName    = "WinGet"
        $CDBContainerName   = "Manifests"

        ## The values required for Function ARM Template
        $manifestCacheEndpoint      = ""
        $monitoringTenant           = ""
        $monitoringRole             = ""
        $monitoringMetricsAccount   = ""


        ## Relative Path from the Working Directory to the Azure ARM Template Files
        $TemplateAppInsightsPath    = "$TemplateFolderPath\applicationinsights.json"
        $TemplateKeyVaultPath       = "$TemplateFolderPath\keyvault.json"
        $TemplateStorageAccountPath = "$TemplateFolderPath\storageaccount.json"
        #$TemplateASPPath            = "$TemplateFolderPath\asp.json"
        $TemplateCDBAccountPath     = "$TemplateFolderPath\cosmosdb.json"
        $TemplateCDBPath            = "$TemplateFolderPath\cosmosdb-sql.json"
        $TemplateCDBContainerPath   = "$TemplateFolderPath\cosmosdb-sql-container.json"
        $TemplateFunctionPath       = "$TemplateFolderPath\azurefunction.json"
        $TemplateFrontDoorPath      = "$TemplateFolderPath\frontdoor.json"
        $TemplateAppConfigPath      = "$TemplateFolderPath\appconfig.json"
        $TemplateASPGenevaPath      = "$TemplateFolderPath\asp_geneva.json"

        $ParameterAppInsightsPath    = "$ParameterFolderPath\applicationinsights.json"
        $ParameterKeyVaultPath       = "$ParameterFolderPath\keyvault.json"
        $ParameterStorageAccountPath = "$ParameterFolderPath\storageaccount.json"
        #$ParameterASPPath            = "$ParameterFolderPath\asp.json"
        $ParameterCDBAccountPath     = "$ParameterFolderPath\cosmosdb.json"
        $ParameterCDBPath            = "$ParameterFolderPath\cosmosdb-sql.json"
        $ParameterCDBContainerPath   = "$ParameterFolderPath\cosmosdb-sql-container.json"
        $ParameterFunctionPath       = "$ParameterFolderPath\azurefunction.json"
        $ParameterFrontDoorPath      = "$ParameterFolderPath\frontdoor.json"
        $ParameterAppConfigPath      = "$ParameterFolderPath\appconfig.json"
        $ParameterASPGenevaPath      = "$ParameterFolderPath\aspgeneva.json"

        Write-Verbose -Message "ARM Parameter Resource performance is based on the: $ImplementationPerformance."

        switch ($ImplementationPerformance) {
            "Demo" {
                $KeyVaultSKU  = "Standard"
                $StorageAccountPerformance = "Standard_LRS"
                $ASPSKU = "B1"
                $CosmosDBAEnableFreeTier   = $true
                ## To enable Serverless then set CosmosDBACapatilities to "[{"name"; ""EnableServerless""}]"
                $CosmosDBACapabilities     = "[]"
            }
            "Basic" {
                $KeyVaultSKU  = "Standard"
                $StorageAccountPerformance = "Standard_GRS"
                $ASPSKU = "S1"
                $CosmosDBAEnableFreeTier   = $false
                ## To enable Serverless then set CosmosDBACapatilities to "[{"name"; ""EnableServerless""}]"
                $CosmosDBACapabilities     = "[]"
            }
            "Enhanced" {
                $KeyVaultSKU  = "Standard"
                $StorageAccountPerformance = "Standard_GZRS"
                $ASPSKU = "P1V2"
                $CosmosDBAEnableFreeTier   = $false
                ## To enable Serverless then set CosmosDBACapatilities to "[{"name"; ""EnableServerless""}]"
                $CosmosDBACapabilities     = "[]"
            }
        }
        $PrimaryRegionName   = $(Get-AzLocation).Where({$_.Location -eq $Region}).DisplayName
        $SecondaryRegion     = Get-PairedAzureRegion -Region $Region
        $SecondaryRegionName = $(Get-AzLocation).Where({$_.Location -eq $SecondaryRegion}).DisplayName

        ## The name of the Secret that will be created in the Azure Keyvault - Do not change
        $AzKVStorageSecretName = "AzStorageAccountKey"

        ## This is the Azure Key Vault Key used to store the Connection String to the Storage Account
        Write-Verbose -Message "Retrieving the Azure Tenant and User Id Information"
        $AzContext = $(Get-AzContext)
        $AzTenantID = $AzContext.Tenant.Id
        Write-Verbose -Message "Retrieved the Azure Tenant Id: $AzTenantID"

        if ($AzContext.Account.type -eq "User")
        {
            $AzObjectID = $(Get-AzADUser -SignedIn).Id
        }
        else
        {
            $AzObjectID = $(Get-AzADServicePrincipal -ApplicationId $AzContext.Account.ID).Id
        }

        Write-Verbose -Message "Retrieved the Azure Object Id: $AzObjectID"

        ## This is specific to the JSON file creation
        $JSONContentVersion = "1.0.0.0"
        $JSONSchema         = "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#"
    }
    PROCESS
    {
        Write-Verbose -Message "Validating that the inputs for the AppInsights template are not null."
        if(!($AppInsightsName -and $ParameterAppInsightsPath -and $TemplateAppInsightsPath -and $JSONSchema -and $JSONContentVersion))
        {
            Write-Verbose -Message "    Required values are null"
            Write-Error -Message "    Required values are null..."
        }
        else
        {
            Write-Verbose -Message "    inputs are not null."
        }

        Write-Verbose -Message "Validating that the inputs for the Keyvault template are not null."
        if(!($KeyVaultName -and $ParameterKeyVaultPath -and $TemplateAppInsightsPath -and $JSONSchema -and $JSONContentVersion -and $KeyVaultName -and $KeyVaultSKU -and $AzObjectID -and $AzTenantID))
        {
            Write-Verbose -Message "    Required values are null"
            Write-Error -Message "    Required values are null..."
        }
        else
        {
            Write-Verbose -Message "    inputs are not null."
        }

        Write-Verbose -Message "Validating that the inputs for the StorageAccount template are not null."
        if(!($StorageAccountName -and $ParameterStorageAccountPath -and $TemplateStorageAccountPath -and $JSONSchema -and $JSONContentVersion -and $Region -and $StorageAccountName -and $StorageAccountPerformance))
        {
            Write-Verbose -Message "    Required values are null"
            Write-Error -Message "    Required values are null..."
        }
        else
        {
            Write-Verbose -Message "    inputs are not null."
        }

        Write-Verbose -Message "Validating that the inputs for the asp template are not null."
        if(!($aspName -and $ParameterASPPath -and $TemplateASPPath -and $JSONSchema -and $JSONContentVersion -and $aspName -and $Region -and $ASPSKU))
        {
            Write-Verbose -Message "    Required values are null"
            Write-Error -Message "    Required values are null..."
        }
        else
        {
            Write-Verbose -Message "    inputs are not null."
        }

        Write-Verbose -Message "Validating that the inputs for the CosmosDBAccount template are not null."
        if(!($CDBAccountName -and $ParameterCDBAccountPath -and $TemplateCDBAccountPath -and $JSONSchema -and $JSONContentVersion -and $CDBAccountName -and $null -ne $CosmosDBAEnableFreeTier -and $PrimaryRegionName -and $SecondaryRegionName))
        {
            Write-Verbose -Message "    Required values are null"
            Write-Error -Message "    Required values are null..."
        }
        else
        {
            Write-Verbose -Message "    inputs are not null."
        }

        Write-Verbose -Message "Validating that the inputs for the CosmosDBDatabase template are not null."
        if(!($CDBDatabaseName -and $ParameterCDBPath -and $TemplateCDBPath -and $JSONSchema -and $JSONContentVersion -and $CDBAccountName -and $CDBDatabaseName))
        {
            Write-Verbose -Message "    Required values are null"
            Write-Error -Message "    Required values are null..."
        }
        else
        {
            Write-Verbose -Message "    inputs are not null."
        }

        Write-Verbose -Message "Validating that the inputs for the CosmosDBContainer template are not null."
        if(!($CDBContainerName -and $ParameterCDBContainerPath -and $TemplateCDBContainerPath -and $JSONSchema -and $JSONContentVersion -and $CDBAccountName -and $CDBDatabaseName -and $CDBContainerName))
        {
            Write-Verbose -Message "    Required values are null"
            Write-Error -Message "    Required values are null..."
        }
        else
        {
            Write-Verbose -Message "    inputs are not null."
        }

        Write-Verbose -Message "Validating that the inputs for the Function template are not null."
        if(!($FunctionName -and $ParameterFunctionPath -and $TemplateFunctionPath -and $JSONSchema -and $JSONContentVersion -and $AzKVStorageSecretName -and $Region -and $CDBDatabaseName -and $CDBContainerName -and $aspName -and $FunctionName -and $KeyVaultName -and $AppInsightsName))
        {
            Write-Verbose -Message "    Required values are null"
            Write-Error -Message "    Required values are null..."
        }
        else
        {
            Write-Verbose -Message "    inputs are not null."
        }

        ## Creates a PowerShell object array to contain the details of the Parameter files.
        $ARMObjects = @(
            @{  ObjectType = "AppInsight"
                ObjectName = $AppInsightsName
                ParameterPath  = "$ParameterAppInsightsPath"
                TemplatePath   = "$TemplateAppInsightsPath"
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
                ParameterPath  = "$ParameterKeyVaultPath"
                TemplatePath   = "$TemplateKeyVaultPath"
                Error      = ""
                Parameters = @{
                    '$Schema' = $JSONSchema
                    contentVersion = $JSONContentVersion
                    Parameters = @{
                        name            = @{
                            value = $KeyVaultName
                            type  = "string"
                        }
                        sku             = @{ value = $KeyVaultSKU}
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
                        skuCode         = @{ value = $ASPSKU }
                        numberOfWorkers = @{ value = "1" }
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
                        enableFreeTier = @{ value = $CosmosDBAEnableFreeTier }
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
                                    locationName     = $PrimaryRegionName
                                    failoverPriority = 0
                                    isZoneRedundant  = $false
                                }
                                @{
                                    locationName     = $SecondaryRegionName
                                    failoverPriority = 1
                                    isZoneRedundant  = $false
                                }
                            )
                        }
                        # Allows requests from azure portal and Azure datacenter ip range (0.0.0.0)
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
                        storageSecretName        = @{ value = $AzKVStorageSecretName    }   # Name used to contain the Storage Account connection string in the Key Value
                        location                 = @{ value = $Region                   }   # Azure hosting location
                        cosmosDatabase           = @{ value = $CDBDatabaseName          }   # Cosmos Database Name
                        cosmosContainer          = @{ value = $CDBContainerName         }   # Cosmos Container Name
                        serverIdentifier         = @{ value = $aspName                  }   # Azure Function Name
                        functionName             = @{ value = $FunctionName             }   # Azure Function Name
                        appServiceName           = @{ value = $aspName                  }   # Azure App Service Name
                        keyVaultName             = @{ value = $KeyVaultName             }   # Azure Keyvault Name
                        appInsightName           = @{ value = $AppInsightsName          }   # Azure App Insights Name
                        manifestCacheEndpoint    = @{ value = $manifestCacheEndpoint    }   # unknown
                        monitoringTenant         = @{ value = $monitoringTenant         }   # unknown
                        monitoringRole           = @{ value = $monitoringRole           }   # unknown
                        monitoringMetricsAccount = @{ value = $monitoringMetricsAccount }   # unknown
                    }
                }
            }
        )

        ## Uses the newly created ARMObjects[#].Parameters to create new JSON Parameter files.
        Write-Verbose -Message "Creating JSON Parameter files for Azure Object Creation:"

        ## Creates each JSON Parameter file inside of a Parameter folder in the working directory
        foreach ($object in $ARMObjects) {
            ## Converts the structure of the variable to a JSON file.
            Write-Verbose -Message "  Creating the Parameter file for $($Object.ObjectType) in the following location:`n    $($Object.ParameterPath)"
            $parameterFile = $Object.Parameters | ConvertTo-Json -Depth 7
            $parameterFile| Out-File -FilePath $Object.ParameterPath -Force
            Write-Verbose -Message "Parameter file: ($parameterFile)"
        }
    }
    END
    {
        ## Returns the completed object.
        Return $ARMObjects
    }
}