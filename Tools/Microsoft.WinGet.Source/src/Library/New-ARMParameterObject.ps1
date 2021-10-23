Function New-ARMParameterObject
{
    <#
    Description:
    Creates a new PowerShell object that contains the Azure Resource type, name, and parameter values. Once
    created it'll output the parameter files into a *.json file that can be used in combination with with 
    template files to build Azure resources required for hosting a Windows Package Manager private source.
    Returns the PowerShell object.
    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)] [string]$ParameterFolderPath,
        [Parameter(Position=1, Mandatory=$true)] [string]$TemplateFolderPath,
        [Parameter(Position=2, Mandatory=$false)][string]$Index,
        [Parameter(Position=3, Mandatory=$true)] [string]$ResourcePrefix,
        [Parameter(Position=4, Mandatory=$true)] [string]$AzLocation,
        $ImplementationPerformance
    )
    BEGIN
    {
        ## This can be used to create a random and unique string name for a resource: "defaultValue": "[concat('kv', uniquestring(resourceGroup().id))]",

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
        

        ## Relative Path from the Working Directory to the Azure ARM Template Files
        $TemplateFolderPath         = "$PSScriptRoot\ARMTemplate"
        $TemplateAppInsightsPath    = "$TemplateFolderPath\applicationinsights.json"
        $TemplateKeyVaultPath       = "$TemplateFolderPath\keyvault.json"
        $TemplateStorageAccountPath = "$TemplateFolderPath\storageaccount.json"
        $TemplateASPPath            = "$TemplateFolderPath\asp.json"
        $TemplateCDBAccountPath     = "$TemplateFolderPath\cosmosdb.json"
        $TemplateCDBPath            = "$TemplateFolderPath\cosmosdb-sql.json"
        $TemplateCDBContainerPath   = "$TemplateFolderPath\cosmosdb-sql-container.json"
        $TemplateFunctionPath       = "$TemplateFolderPath\azurefunction.json"
        $TemplateFrontDoorPath      = "$TemplateFolderPath\frontdoor.json"

        ## Relative Path from the Working Directory to the Azure ARM Parameter Files
        if($null -ne $Index -or $Index -ne "") {
            ## Prefixes the "." to the beginning of the $Index value.
            $NameEntryIndex = ".$Index"
        }

        $ParameterAppInsightsPath    = "$ParameterFolderPath\applicationinsights$NameEntryIndex.json"
        $ParameterKeyVaultPath       = "$ParameterFolderPath\keyvault$NameEntryIndex.json"
        $ParameterStorageAccountPath = "$ParameterFolderPath\storageaccount$NameEntryIndex.json"
        $ParameterASPPath            = "$ParameterFolderPath\asp$NameEntryIndex.json"
        $ParameterCDBAccountPath     = "$ParameterFolderPath\cosmosdb$NameEntryIndex.json"
        $ParameterCDBPath            = "$ParameterFolderPath\cosmosdb-sql$NameEntryIndex.json"
        $ParameterCDBContainerPath   = "$ParameterFolderPath\cosmosdb-sql-container$NameEntryIndex.json"
        $ParameterFunctionPath       = "$ParameterFolderPath\azurefunction$NameEntryIndex.json"
        $ParameterFrontDoorPath      = "$ParameterFolderPath\frontdoor$NameEntryIndex.json"

        $aspSKU             = "P1V2"
        $aspNumberOfWorkers = "1"

        $cosmosDBAAccountType   = "Production"
        $CosmosDBACapacityMode  = ""  ## Serverless or Provisioned Throughput??
        $CosmosDBAFreeTier      = ""
        $CosmosDBAGeoRedundancy = ""
        $CosmosDBABackupRedundancy = ""  ## Geo-Redundant backup storage, or Locally-Redundant backup storage


        switch ($ImplementationPerformance) {
            "Demo" {
                $KeyVaultSKU  = "Standard"
                $StorageAccountPerformance = "Standard_LRS"
#                $StorageAccountRedundancy = "LRS" ## Locally Redundant Storage (LRS)
                $ASPSKU = "B1"  ## 41.11 CAD/Month (Just above shared instance - no time limits)
                $CosmosDBAEnableFreeTier   = $true
                $CosmosDBACapabilities     = "[]"  ## To enable Serverless then set this to "[{"name"; ""EnableServerless""}]"
            }
            "Basic" { 
                $KeyVaultSKU  = "Standard"
                $StorageAccountPerformance = "Standard_GRS"
#                $StorageAccountRedundancy = "GRS" ## Zone Redundant Storage (GRS)
                $ASPSKU = "S1"  ## 56.06 CAD/Month (no time limits)
                $CosmosDBAEnableFreeTier   = $false
                $CosmosDBACapabilities     = "[]"  ## To enable Serverless then set this to "[{"name"; ""EnableServerless""}]"
            }
            "Enhanced" {
                $KeyVaultSKU  = "Standard"  ## Need to find the SKU for Premium
                $StorageAccountPerformance = "Standard_GZRS" ## ?? Maybe.. set as "Premium" though that doesn't work with the below value though on Redundancy (File Shares, and Zone Redundant Storage (ZRS))
#                $StorageAccountRedundancy = "GZRS" ## Geo Zone Redundant Storage (GZRS)
                $ASPSKU = "P1V2"  ## 65.41 CAD/Month (no time limits)
                $CosmosDBAEnableFreeTier   = $false
                $CosmosDBACapabilities     = "[]"  ## To enable Serverless then set this to "[{"name"; ""EnableServerless""}]"
            }
        }
        
        ## The name of the Secret that will be created in the Azure Keyvault - Do not change
        $AzKVStorageSecretName = "AzStorageAccountKey"
        
        ## This is the Azure Key Vault Key used to store the Connection String to the Storage Account
        $AzTenantID            = $(Get-AzContext).Tenant.Id
        $AzDirectoryID         = $(Get-AzADUser).Where({$_.UserPrincipalName -like "$($(Get-AzContext).Account.ID.Split("@")[0])*"}).ID
        
        ## This is specific to the JSON file creation
        $JSONContentVersion = "1.0.0.0"
        $JSONSchema         = "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#"
    }
    PROCESS
    {    
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
                ParameterPath  = "$ParameterStorageAccountPath"
                TemplatePath   = "$TemplateStorageAccountPath"
                Error      = ""
                Parameters = @{
                    '$Schema' = $JSONSchema
                    contentVersion = $JSONContentVersion
                    Parameters = @{
                        location           = @{ value = $AzLocation }
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
                        location        = @{ value = $AzLocation }
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
            #     ParameterPath  = "$ParameterFrontDoorPath"
            #     TemplatePath   = "$TemplateFrontDoorPath"
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
        Write-Verbose -Message "Creating JSON Parameter files for Azure Object Creation:"

        ## Creates each JSON Parameter file inside of a Parameter folder in the working directory
        foreach ($object in $ARMObjects) {
            ## Converts the structure of the variable to a JSON file.
            Write-Verbose -Message "  Creating the Parameter file for $($Object.ObjectType) in the following location:`n    $($Object.ParameterPath)"
            $Object.Parameters | ConvertTo-Json -Depth 7 | Out-File -FilePath $Object.ParameterPath -Force
        }
    }
    END
    {
        ## Returns the completed object.
        Return $ARMObjects
    }
}