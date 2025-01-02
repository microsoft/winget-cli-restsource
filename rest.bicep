{
    "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
    "contentVersion": "1.0.0.0",
    "parameters": {
        "subscriptions_677358889a937c006d070001_displayName": {
            "type": "SecureString"
        },
        "subscriptions_677358889a937c006d070002_displayName": {
            "type": "SecureString"
        },
        "users_1_lastName": {
            "type": "SecureString"
        },
        "sites_azfun_rest_name": {
            "defaultValue": "azfun-rest",
            "type": "String"
        },
        "vaults_kv_rest_name": {
            "defaultValue": "kv-rest",
            "type": "String"
        },
        "serverfarms_asp_rest_name": {
            "defaultValue": "asp-rest",
            "type": "String"
        },
        "components_appin_rest_name": {
            "defaultValue": "appin-rest",
            "type": "String"
        },
        "storageAccounts_strest_name": {
            "defaultValue": "strest",
            "type": "String"
        },
        "service_apim_rest_name": {
            "defaultValue": "apim-rest",
            "type": "String"
        },
        "databaseAccounts_cosmos_rest_name": {
            "defaultValue": "cosmos-rest",
            "type": "String"
        },
        "configurationStores_appconfig_rest_name": {
            "defaultValue": "appconfig-rest",
            "type": "String"
        },
        "systemTopics_strest_bc39d05f_e52c_407d_a7f8_1c2d85e68c85_name": {
            "defaultValue": "strest-bc39d05f-e52c-407d-a7f8-1c2d85e68c85",
            "type": "String"
        },
        "smartdetectoralertrules_failure_anomalies___appin_rest_name": {
            "defaultValue": "failure anomalies - appin-rest",
            "type": "String"
        },
        "actiongroups_application_insights_smart_detection_externalid": {
            "defaultValue": "/subscriptions/a4d8af3d-9def-4893-b250-0140eae8bac6/resourceGroups/rg-almit-automation-dev/providers/microsoft.insights/actiongroups/application insights smart detection",
            "type": "String"
        }
    },
    "variables": {},
    "resources": [
        {
            "type": "Microsoft.ApiManagement/service",
            "apiVersion": "2023-09-01-preview",
            "name": "[parameters('service_apim_rest_name')]",
            "location": "West US",
            "sku": {
                "name": "Basic",
                "capacity": 1
            },
            "identity": {
                "type": "SystemAssigned"
            },
            "properties": {
                "publisherEmail": "Gijs.Reijn@rabobank.com",
                "publisherName": "Gijs.Reijn@rabobank.com",
                "notificationSenderEmail": "apimgmt-noreply@mail.windowsazure.com",
                "hostnameConfigurations": [
                    {
                        "type": "Proxy",
                        "hostName": "[concat(parameters('service_apim_rest_name'), '.azure-api.net')]",
                        "negotiateClientCertificate": false,
                        "defaultSslBinding": true,
                        "certificateSource": "BuiltIn"
                    }
                ],
                "customProperties": {
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Tls10": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Tls11": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Ssl30": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TripleDes168": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Tls10": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Tls11": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Ssl30": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Protocols.Server.Http2": "False"
                },
                "virtualNetworkType": "None",
                "disableGateway": false,
                "natGatewayState": "Unsupported",
                "apiVersionConstraint": {},
                "publicNetworkAccess": "Enabled",
                "legacyPortalStatus": "Disabled",
                "developerPortalStatus": "Enabled"
            }
        },
        {
            "type": "Microsoft.AppConfiguration/configurationStores",
            "apiVersion": "2023-09-01-preview",
            "name": "[parameters('configurationStores_appconfig_rest_name')]",
            "location": "westus",
            "sku": {
                "name": "standard"
            },
            "properties": {
                "encryption": {},
                "disableLocalAuth": false,
                "softDeleteRetentionInDays": 7,
                "enablePurgeProtection": false,
                "dataPlaneProxy": {
                    "authenticationMode": "Local",
                    "privateLinkDelegation": "Disabled"
                },
                "telemetry": {},
                "experimentation": {}
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts",
            "apiVersion": "2024-05-15",
            "name": "[parameters('databaseAccounts_cosmos_rest_name')]",
            "location": "West US",
            "kind": "GlobalDocumentDB",
            "identity": {
                "type": "None"
            },
            "properties": {
                "publicNetworkAccess": "Enabled",
                "enableAutomaticFailover": true,
                "enableMultipleWriteLocations": true,
                "isVirtualNetworkFilterEnabled": false,
                "virtualNetworkRules": [],
                "disableKeyBasedMetadataWriteAccess": false,
                "enableFreeTier": false,
                "enableAnalyticalStorage": false,
                "analyticalStorageConfiguration": {
                    "schemaType": "WellDefined"
                },
                "databaseAccountOfferType": "Standard",
                "defaultIdentity": "FirstPartyIdentity",
                "networkAclBypass": "None",
                "disableLocalAuth": true,
                "enablePartitionMerge": false,
                "enableBurstCapacity": false,
                "minimalTlsVersion": "Tls",
                "consistencyPolicy": {
                    "defaultConsistencyLevel": "Session",
                    "maxIntervalInSeconds": 5,
                    "maxStalenessPrefix": 100
                },
                "locations": [
                    {
                        "locationName": "West US",
                        "failoverPriority": 0,
                        "isZoneRedundant": false
                    },
                    {
                        "locationName": "Central US",
                        "failoverPriority": 1,
                        "isZoneRedundant": false
                    }
                ],
                "cors": [],
                "capabilities": [],
                "ipRules": [
                    {
                        "ipAddressOrRange": "13.91.105.215"
                    },
                    {
                        "ipAddressOrRange": "4.210.172.107"
                    },
                    {
                        "ipAddressOrRange": "13.88.56.148"
                    },
                    {
                        "ipAddressOrRange": "40.91.218.243"
                    },
                    {
                        "ipAddressOrRange": "0.0.0.0"
                    }
                ],
                "backupPolicy": {
                    "type": "Periodic",
                    "periodicModeProperties": {
                        "backupIntervalInMinutes": 240,
                        "backupRetentionIntervalInHours": 720,
                        "backupStorageRedundancy": "Geo"
                    }
                },
                "networkAclBypassResourceIds": []
            }
        },
        {
            "type": "microsoft.insights/components",
            "apiVersion": "2020-02-02",
            "name": "[parameters('components_appin_rest_name')]",
            "location": "westus",
            "kind": "web",
            "properties": {
                "Application_Type": "web",
                "RetentionInDays": 90,
                "IngestionMode": "ApplicationInsights",
                "publicNetworkAccessForIngestion": "Enabled",
                "publicNetworkAccessForQuery": "Enabled"
            }
        },
        {
            "type": "Microsoft.KeyVault/vaults",
            "apiVersion": "2024-04-01-preview",
            "name": "[parameters('vaults_kv_rest_name')]",
            "location": "westus",
            "properties": {
                "sku": {
                    "family": "A",
                    "name": "standard"
                },
                "tenantId": "6e93a626-8aca-4dc1-9191-ce291b4b75a1",
                "accessPolicies": [
                    {
                        "tenantId": "6e93a626-8aca-4dc1-9191-ce291b4b75a1",
                        "objectId": "15505982-c643-486a-a871-b10ad76101c2",
                        "permissions": {
                            "certificates": [],
                            "keys": [],
                            "secrets": [
                                "get",
                                "set",
                                "list"
                            ],
                            "storage": []
                        }
                    },
                    {
                        "tenantId": "6e93a626-8aca-4dc1-9191-ce291b4b75a1",
                        "objectId": "5bc4dc27-79fd-4a44-909e-631525d129fe",
                        "permissions": {
                            "keys": [],
                            "secrets": [
                                "Get"
                            ],
                            "certificates": [],
                            "storage": []
                        }
                    },
                    {
                        "tenantId": "6e93a626-8aca-4dc1-9191-ce291b4b75a1",
                        "objectId": "8e2f6f9a-6c3c-4a42-b706-2c9e5c2767d6",
                        "permissions": {
                            "keys": [],
                            "secrets": [
                                "Get"
                            ],
                            "certificates": [],
                            "storage": []
                        }
                    }
                ],
                "enabledForDeployment": true,
                "enabledForDiskEncryption": true,
                "enabledForTemplateDeployment": true,
                "enableSoftDelete": true,
                "softDeleteRetentionInDays": 90,
                "enableRbacAuthorization": false,
                "enablePurgeProtection": true,
                "vaultUri": "[concat('https://', parameters('vaults_kv_rest_name'), '.vault.azure.net/')]",
                "provisioningState": "Succeeded",
                "publicNetworkAccess": "Enabled"
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts",
            "apiVersion": "2023-05-01",
            "name": "[parameters('storageAccounts_strest_name')]",
            "location": "westus",
            "sku": {
                "name": "Standard_GRS",
                "tier": "Standard"
            },
            "kind": "StorageV2",
            "properties": {
                "allowCrossTenantReplication": false,
                "minimumTlsVersion": "TLS1_2",
                "allowBlobPublicAccess": false,
                "allowSharedKeyAccess": false,
                "networkAcls": {
                    "resourceAccessRules": [
                        {
                            "tenantId": "6e93a626-8aca-4dc1-9191-ce291b4b75a1",
                            "resourceId": "/subscriptions/a4d8af3d-9def-4893-b250-0140eae8bac6/providers/Microsoft.Security/datascanners/storageDataScanner"
                        }
                    ],
                    "bypass": "AzureServices",
                    "virtualNetworkRules": [],
                    "ipRules": [],
                    "defaultAction": "Allow"
                },
                "supportsHttpsTrafficOnly": true,
                "encryption": {
                    "services": {
                        "file": {
                            "keyType": "Account",
                            "enabled": true
                        },
                        "blob": {
                            "keyType": "Account",
                            "enabled": true
                        }
                    },
                    "keySource": "Microsoft.Storage"
                },
                "accessTier": "Hot"
            }
        },
        {
            "type": "Microsoft.Web/serverfarms",
            "apiVersion": "2023-12-01",
            "name": "[parameters('serverfarms_asp_rest_name')]",
            "location": "West US",
            "sku": {
                "name": "S1",
                "tier": "Standard",
                "size": "S1",
                "family": "S",
                "capacity": 1
            },
            "kind": "app",
            "properties": {
                "perSiteScaling": false,
                "elasticScaleEnabled": false,
                "maximumElasticWorkerCount": 1,
                "isSpot": false,
                "reserved": false,
                "isXenon": false,
                "hyperV": false,
                "targetWorkerCount": 0,
                "targetWorkerSizeId": 0,
                "zoneRedundant": false
            }
        },
        {
            "type": "microsoft.alertsmanagement/smartdetectoralertrules",
            "apiVersion": "2021-04-01",
            "name": "[parameters('smartdetectoralertrules_failure_anomalies___appin_rest_name')]",
            "location": "global",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "description": "Failure Anomalies notifies you of an unusual rise in the rate of failed HTTP requests or dependency calls.",
                "state": "Enabled",
                "severity": "Sev3",
                "frequency": "PT1M",
                "detector": {
                    "id": "FailureAnomaliesDetector"
                },
                "scope": [
                    "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
                ],
                "actionGroups": {
                    "groupIds": [
                        "[parameters('actiongroups_application_insights_smart_detection_externalid')]"
                    ]
                }
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/echo-api')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Echo API",
                "apiRevision": "1",
                "subscriptionRequired": true,
                "serviceUrl": "http://echoapi.cloudapp.net/api",
                "path": "echo",
                "protocols": [
                    "https"
                ],
                "authenticationSettings": {
                    "oAuth2AuthenticationSettings": [],
                    "openidAuthenticationSettings": []
                },
                "subscriptionKeyParameterNames": {
                    "header": "Ocp-Apim-Subscription-Key",
                    "query": "subscription-key"
                },
                "isCurrent": true
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "winget rest source api",
                "apiRevision": "1",
                "description": "winget rest source api",
                "subscriptionRequired": false,
                "path": "winget",
                "protocols": [
                    "https"
                ],
                "authenticationSettings": {
                    "oAuth2AuthenticationSettings": [],
                    "openidAuthenticationSettings": []
                },
                "subscriptionKeyParameterNames": {
                    "header": "Ocp-Apim-Subscription-Key",
                    "query": "subscription-key"
                },
                "isCurrent": true
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/backends",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget-backend0')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "description": "winget rest source backend function",
                "url": "https://azfun-rest.azurewebsites.net/api",
                "protocol": "http",
                "credentials": {
                    "header": {
                        "x-functions-key": [
                            "{{winget-functions-access-key}}"
                        ]
                    }
                }
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/administrators')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Administrators",
                "description": "Administrators is a built-in group containing the admin email account provided at the time of service creation. Its membership is managed by the system.",
                "type": "system"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/developers')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Developers",
                "description": "Developers is a built-in group. Its membership is managed by the system. Signed-in users fall into this group.",
                "type": "system"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/guests')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Guests",
                "description": "Guests is a built-in group. Its membership is managed by the system. Unauthenticated users visiting the developer portal fall into this group.",
                "type": "system"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/namedValues",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget-functions-access-key')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "winget-functions-access-key",
                "keyVault": {
                    "secretIdentifier": "https://kv-rest.vault.azure.net/secrets/AzureFunctionHostKey"
                },
                "secret": true
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/AccountClosedPublisher')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/BCC')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/NewApplicationNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/NewIssuePublisherNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/PurchasePublisherNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/QuotaLimitApproachingPublisherNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/RequestPublisherNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<!--\r\n    IMPORTANT:\r\n    - Policy elements can appear only within the <inbound>, <outbound>, <backend> section elements.\r\n    - Only the <forward-request> policy element can appear within the <backend> section element.\r\n    - To apply a policy to the incoming request (before it is forwarded to the backend service), place a corresponding policy element within the <inbound> section element.\r\n    - To apply a policy to the outgoing response (before it is sent back to the caller), place a corresponding policy element within the <outbound> section element.\r\n    - To add a policy position the cursor at the desired insertion point and click on the round button associated with the policy.\r\n    - To remove a policy, delete the corresponding policy statement from the policy document.\r\n    - Policies are applied in the order of their appearance, from the top down.\r\n-->\r\n<policies>\r\n  <inbound />\r\n  <backend>\r\n    <forward-request />\r\n  </backend>\r\n  <outbound />\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/portalconfigs",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "enableBasicAuth": true,
                "signin": {
                    "require": false
                },
                "signup": {
                    "termsOfService": {
                        "requireConsent": false
                    }
                },
                "delegation": {
                    "delegateRegistration": false,
                    "delegateSubscription": false
                },
                "cors": {
                    "allowedOrigins": []
                },
                "csp": {
                    "mode": "disabled",
                    "reportUri": [],
                    "allowedSources": []
                }
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/portalsettings",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/delegation')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subscriptions": {
                    "enabled": false
                },
                "userRegistration": {
                    "enabled": false
                }
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/portalsettings",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/signin')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "enabled": false
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/portalsettings",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/signup')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "enabled": true,
                "termsOfService": {
                    "enabled": false,
                    "consentRequired": false
                }
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/starter')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Starter",
                "description": "Subscribers will be able to run 5 calls/minute up to a maximum of 100 calls/week.",
                "subscriptionRequired": true,
                "approvalRequired": false,
                "subscriptionsLimit": 1,
                "state": "published"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/unlimited')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Unlimited",
                "description": "Subscribers have completely unlimited access to the API. Administrator approval is required.",
                "subscriptionRequired": true,
                "approvalRequired": true,
                "subscriptionsLimit": 1,
                "state": "published"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/subscriptions",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/master')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "scope": "[concat(resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name')), '/')]",
                "displayName": "Built-in all-access subscription",
                "state": "active",
                "allowTracing": false
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/AccountClosedDeveloper')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "Thank you for using the $OrganizationName API!",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          On behalf of $OrganizationName and our customers we thank you for giving us a try. Your $OrganizationName API account is now closed.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Your $OrganizationName Team</p>\r\n    <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n    <p />\r\n  </body>\r\n</html>",
                "title": "Developer farewell letter",
                "description": "Developers receive this farewell email after they close their account.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/ApplicationApprovedNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "Your application $AppName is published in the application gallery",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          We are happy to let you know that your request to publish the $AppName application in the application gallery has been approved. Your application has been published and can be viewed <a href=\"http://$DevPortalUrl/Applications/Details/$AppId\">here</a>.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Best,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n  </body>\r\n</html>",
                "title": "Application gallery submission approved (deprecated)",
                "description": "Developers who submitted their application for publication in the application gallery on the developer portal receive this email after their submission is approved.",
                "parameters": [
                    {
                        "name": "AppId",
                        "title": "Application id"
                    },
                    {
                        "name": "AppName",
                        "title": "Application name"
                    },
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/ConfirmSignUpIdentityDefault')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "Please confirm your new $OrganizationName API account",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <title>Letter</title>\r\n  </head>\r\n  <body>\r\n    <table width=\"100%\">\r\n      <tr>\r\n        <td>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\"></p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you for joining the $OrganizationName API program! We host a growing number of cool APIs and strive to provide an awesome experience for API developers.</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">First order of business is to activate your account and get you going. To that end, please click on the following link:</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a id=\"confirmUrl\" href=\"$ConfirmUrl\" style=\"text-decoration:none\">\r\n              <strong>$ConfirmUrl</strong>\r\n            </a>\r\n          </p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">If clicking the link does not work, please copy-and-paste or re-type it into your browser's address bar and hit \"Enter\".</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">$OrganizationName API Team</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n          </p>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>",
                "title": "New developer account confirmation",
                "description": "Developers receive this email to confirm their e-mail address after they sign up for a new account.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    },
                    {
                        "name": "ConfirmUrl",
                        "title": "Developer activation URL"
                    },
                    {
                        "name": "DevPortalHost",
                        "title": "Developer portal hostname"
                    },
                    {
                        "name": "ConfirmQuery",
                        "title": "Query string part of the activation URL"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/EmailChangeIdentityDefault')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "Please confirm the new email associated with your $OrganizationName API account",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <title>Letter</title>\r\n  </head>\r\n  <body>\r\n    <table width=\"100%\">\r\n      <tr>\r\n        <td>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\"></p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">You are receiving this email because you made a change to the email address on your $OrganizationName API account.</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Please click on the following link to confirm the change:</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a id=\"confirmUrl\" href=\"$ConfirmUrl\" style=\"text-decoration:none\">\r\n              <strong>$ConfirmUrl</strong>\r\n            </a>\r\n          </p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">If clicking the link does not work, please copy-and-paste or re-type it into your browser's address bar and hit \"Enter\".</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">$OrganizationName API Team</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n          </p>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>",
                "title": "Email change confirmation",
                "description": "Developers receive this email to confirm a new e-mail address after they change their existing one associated with their account.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    },
                    {
                        "name": "ConfirmUrl",
                        "title": "Developer confirmation URL"
                    },
                    {
                        "name": "DevPortalHost",
                        "title": "Developer portal hostname"
                    },
                    {
                        "name": "ConfirmQuery",
                        "title": "Query string part of the confirmation URL"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/InviteUserNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "You are invited to join the $OrganizationName developer network",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          Your account has been created. Please follow the link below to visit the $OrganizationName developer portal and claim it:\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <a href=\"$ConfirmUrl\">$ConfirmUrl</a>\r\n    </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Best,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n  </body>\r\n</html>",
                "title": "Invite user",
                "description": "An e-mail invitation to create an account, sent on request by API publishers.",
                "parameters": [
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "ConfirmUrl",
                        "title": "Confirmation link"
                    },
                    {
                        "name": "DevPortalHost",
                        "title": "Developer portal hostname"
                    },
                    {
                        "name": "ConfirmQuery",
                        "title": "Query string part of the confirmation link"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/NewCommentNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "$IssueName issue has a new comment",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">This is a brief note to let you know that $CommenterFirstName $CommenterLastName made the following comment on the issue $IssueName you created:</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">$CommentText</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          To view the issue on the developer portal click <a href=\"http://$DevPortalUrl/issues/$IssueId\">here</a>.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Best,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n  </body>\r\n</html>",
                "title": "New comment added to an issue (deprecated)",
                "description": "Developers receive this email when someone comments on the issue they created on the Issues page of the developer portal.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "CommenterFirstName",
                        "title": "Commenter first name"
                    },
                    {
                        "name": "CommenterLastName",
                        "title": "Commenter last name"
                    },
                    {
                        "name": "IssueId",
                        "title": "Issue id"
                    },
                    {
                        "name": "IssueName",
                        "title": "Issue name"
                    },
                    {
                        "name": "CommentText",
                        "title": "Comment text"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/NewDeveloperNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "Welcome to the $OrganizationName API!",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <title>Letter</title>\r\n  </head>\r\n  <body>\r\n    <h1 style=\"color:#000505;font-size:18pt;font-family:'Segoe UI'\">\r\n          Welcome to <span style=\"color:#003363\">$OrganizationName API!</span></h1>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Your $OrganizationName API program registration is completed and we are thrilled to have you as a customer. Here are a few important bits of information for your reference:</p>\r\n    <table width=\"100%\" style=\"margin:20px 0\">\r\n      <tr>\r\n            #if ($IdentityProvider == \"Basic\")\r\n            <td width=\"50%\" style=\"height:40px;vertical-align:top;font-family:'Segoe UI';font-size:12pt\">\r\n              Please use the following <strong>username</strong> when signing into any of the ${OrganizationName}-hosted developer portals:\r\n            </td><td style=\"vertical-align:top;font-family:'Segoe UI';font-size:12pt\"><strong>$DevUsername</strong></td>\r\n            #else\r\n            <td width=\"50%\" style=\"height:40px;vertical-align:top;font-family:'Segoe UI';font-size:12pt\">\r\n              Please use the following <strong>$IdentityProvider account</strong> when signing into any of the ${OrganizationName}-hosted developer portals:\r\n            </td><td style=\"vertical-align:top;font-family:'Segoe UI';font-size:12pt\"><strong>$DevUsername</strong></td>            \r\n            #end\r\n          </tr>\r\n      <tr>\r\n        <td style=\"height:40px;vertical-align:top;font-family:'Segoe UI';font-size:12pt\">\r\n              We will direct all communications to the following <strong>email address</strong>:\r\n            </td>\r\n        <td style=\"vertical-align:top;font-family:'Segoe UI';font-size:12pt\">\r\n          <a href=\"mailto:$DevEmail\" style=\"text-decoration:none\">\r\n            <strong>$DevEmail</strong>\r\n          </a>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Best of luck in your API pursuits!</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">$OrganizationName API Team</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <a href=\"http://$DevPortalUrl\">$DevPortalUrl</a>\r\n    </p>\r\n  </body>\r\n</html>",
                "title": "Developer welcome letter",
                "description": "Developers receive this “welcome” email after they confirm their new account.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "DevUsername",
                        "title": "Developer user name"
                    },
                    {
                        "name": "DevEmail",
                        "title": "Developer email"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    },
                    {
                        "name": "IdentityProvider",
                        "title": "Identity Provider selected by Organization"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/NewIssueNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "Your request $IssueName was received",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you for contacting us. Our API team will review your issue and get back to you soon.</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          Click this <a href=\"http://$DevPortalUrl/issues/$IssueId\">link</a> to view or edit your request.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Best,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n  </body>\r\n</html>",
                "title": "New issue received (deprecated)",
                "description": "This email is sent to developers after they create a new topic on the Issues page of the developer portal.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "IssueId",
                        "title": "Issue id"
                    },
                    {
                        "name": "IssueName",
                        "title": "Issue name"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/PasswordResetByAdminNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "Your password was reset",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <table width=\"100%\">\r\n      <tr>\r\n        <td>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\"></p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">The password of your $OrganizationName API account has been reset, per your request.</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n                Your new password is: <strong>$DevPassword</strong></p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Please make sure to change it next time you sign in.</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">$OrganizationName API Team</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n          </p>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>",
                "title": "Password reset by publisher notification (Password reset by admin)",
                "description": "Developers receive this email when the publisher resets their password.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "DevPassword",
                        "title": "New Developer password"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/PasswordResetIdentityDefault')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "Your password change request",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <title>Letter</title>\r\n  </head>\r\n  <body>\r\n    <table width=\"100%\">\r\n      <tr>\r\n        <td>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\"></p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">You are receiving this email because you requested to change the password on your $OrganizationName API account.</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Please click on the link below and follow instructions to create your new password:</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a id=\"resetUrl\" href=\"$ConfirmUrl\" style=\"text-decoration:none\">\r\n              <strong>$ConfirmUrl</strong>\r\n            </a>\r\n          </p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">If clicking the link does not work, please copy-and-paste or re-type it into your browser's address bar and hit \"Enter\".</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">$OrganizationName API Team</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n          </p>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>",
                "title": "Password change confirmation",
                "description": "Developers receive this email when they request a password change of their account. The purpose of the email is to verify that the account owner made the request and to provide a one-time perishable URL for changing the password.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    },
                    {
                        "name": "ConfirmUrl",
                        "title": "Developer new password instruction URL"
                    },
                    {
                        "name": "DevPortalHost",
                        "title": "Developer portal hostname"
                    },
                    {
                        "name": "ConfirmQuery",
                        "title": "Query string part of the instruction URL"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/PurchaseDeveloperNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "Your subscription to the $ProdName",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Greetings $DevFirstName $DevLastName!</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          Thank you for subscribing to the <a href=\"http://$DevPortalUrl/products/$ProdId\"><strong>$ProdName</strong></a> and welcome to the $OrganizationName developer community. We are delighted to have you as part of the team and are looking forward to the amazing applications you will build using our API!\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Below are a few subscription details for your reference:</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <ul>\r\n            #if ($SubStartDate != \"\")\r\n            <li style=\"font-size:12pt;font-family:'Segoe UI'\">Start date: $SubStartDate</li>\r\n            #end\r\n            \r\n            #if ($SubTerm != \"\")\r\n            <li style=\"font-size:12pt;font-family:'Segoe UI'\">Subscription term: $SubTerm</li>\r\n            #end\r\n          </ul>\r\n    </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            Visit the developer <a href=\"http://$DevPortalUrl/developer\">profile area</a> to manage your subscription and subscription keys\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">A couple of pointers to help get you started:</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <strong>\r\n        <a href=\"http://$DevPortalUrl/docs/services?product=$ProdId\">Learn about the API</a>\r\n      </strong>\r\n    </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The API documentation provides all information necessary to make a request and to process a response. Code samples are provided per API operation in a variety of languages. Moreover, an interactive console allows making API calls directly from the developer portal without writing any code.</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <strong>\r\n        <a href=\"http://$DevPortalUrl/applications\">Feature your app in the app gallery</a>\r\n      </strong>\r\n    </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">You can publish your application on our gallery for increased visibility to potential new users.</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <strong>\r\n        <a href=\"http://$DevPortalUrl/issues\">Stay in touch</a>\r\n      </strong>\r\n    </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          If you have an issue, a question, a suggestion, a request, or if you just want to tell us something, go to the <a href=\"http://$DevPortalUrl/issues\">Issues</a> page on the developer portal and create a new topic.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Happy hacking,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n    <a style=\"font-size:12pt;font-family:'Segoe UI'\" href=\"http://$DevPortalUrl\">$DevPortalUrl</a>\r\n  </body>\r\n</html>",
                "title": "New subscription activated",
                "description": "Developers receive this acknowledgement email after subscribing to a product.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "ProdId",
                        "title": "Product ID"
                    },
                    {
                        "name": "ProdName",
                        "title": "Product name"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "SubStartDate",
                        "title": "Subscription start date"
                    },
                    {
                        "name": "SubTerm",
                        "title": "Subscription term"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/QuotaLimitApproachingDeveloperNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "You are approaching an API quota limit",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head>\r\n    <style>\r\n          body {font-size:12pt; font-family:\"Segoe UI\",\"Segoe WP\",\"Tahoma\",\"Arial\",\"sans-serif\";}\r\n          .alert { color: red; }\r\n          .child1 { padding-left: 20px; }\r\n          .child2 { padding-left: 40px; }\r\n          .number { text-align: right; }\r\n          .text { text-align: left; }\r\n          th, td { padding: 4px 10px; min-width: 100px; }\r\n          th { background-color: #DDDDDD;}\r\n        </style>\r\n  </head>\r\n  <body>\r\n    <p>Greetings $DevFirstName $DevLastName!</p>\r\n    <p>\r\n          You are approaching the quota limit on you subscription to the <strong>$ProdName</strong> product (primary key $SubPrimaryKey).\r\n          #if ($QuotaResetDate != \"\")\r\n          This quota will be renewed on $QuotaResetDate.\r\n          #else\r\n          This quota will not be renewed.\r\n          #end\r\n        </p>\r\n    <p>Below are details on quota usage for the subscription:</p>\r\n    <p>\r\n      <table>\r\n        <thead>\r\n          <th class=\"text\">Quota Scope</th>\r\n          <th class=\"number\">Calls</th>\r\n          <th class=\"number\">Call Quota</th>\r\n          <th class=\"number\">Bandwidth</th>\r\n          <th class=\"number\">Bandwidth Quota</th>\r\n        </thead>\r\n        <tbody>\r\n          <tr>\r\n            <td class=\"text\">Subscription</td>\r\n            <td class=\"number\">\r\n                  #if ($CallsAlert == true)\r\n                  <span class=\"alert\">$Calls</span>\r\n                  #else\r\n                  $Calls\r\n                  #end\r\n                </td>\r\n            <td class=\"number\">$CallQuota</td>\r\n            <td class=\"number\">\r\n                  #if ($BandwidthAlert == true)\r\n                  <span class=\"alert\">$Bandwidth</span>\r\n                  #else\r\n                  $Bandwidth\r\n                  #end\r\n                </td>\r\n            <td class=\"number\">$BandwidthQuota</td>\r\n          </tr>\r\n              #foreach ($api in $Apis)\r\n              <tr><td class=\"child1 text\">API: $api.Name</td><td class=\"number\">\r\n                  #if ($api.CallsAlert == true)\r\n                  <span class=\"alert\">$api.Calls</span>\r\n                  #else\r\n                  $api.Calls\r\n                  #end\r\n                </td><td class=\"number\">$api.CallQuota</td><td class=\"number\">\r\n                  #if ($api.BandwidthAlert == true)\r\n                  <span class=\"alert\">$api.Bandwidth</span>\r\n                  #else\r\n                  $api.Bandwidth\r\n                  #end\r\n                </td><td class=\"number\">$api.BandwidthQuota</td></tr>\r\n              #foreach ($operation in $api.Operations)\r\n              <tr><td class=\"child2 text\">Operation: $operation.Name</td><td class=\"number\">\r\n                  #if ($operation.CallsAlert == true)\r\n                  <span class=\"alert\">$operation.Calls</span>\r\n                  #else\r\n                  $operation.Calls\r\n                  #end\r\n                </td><td class=\"number\">$operation.CallQuota</td><td class=\"number\">\r\n                  #if ($operation.BandwidthAlert == true)\r\n                  <span class=\"alert\">$operation.Bandwidth</span>\r\n                  #else\r\n                  $operation.Bandwidth\r\n                  #end\r\n                </td><td class=\"number\">$operation.BandwidthQuota</td></tr>\r\n              #end\r\n              #end\r\n            </tbody>\r\n      </table>\r\n    </p>\r\n    <p>Thank you,</p>\r\n    <p>$OrganizationName API Team</p>\r\n    <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n    <p />\r\n  </body>\r\n</html>",
                "title": "Developer quota limit approaching notification",
                "description": "Developers receive this email to alert them when they are approaching a quota limit.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "ProdName",
                        "title": "Product name"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "SubPrimaryKey",
                        "title": "Primary Subscription key"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    },
                    {
                        "name": "QuotaResetDate",
                        "title": "Quota reset date"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/RejectDeveloperNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "Your subscription request for the $ProdName",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          We would like to inform you that we reviewed your subscription request for the <strong>$ProdName</strong>.\r\n        </p>\r\n        #if ($SubDeclineReason == \"\")\r\n        <p style=\"font-size:12pt;font-family:'Segoe UI'\">Regretfully, we were unable to approve it, as subscriptions are temporarily suspended at this time.</p>\r\n        #else\r\n        <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          Regretfully, we were unable to approve it at this time for the following reason:\r\n          <div style=\"margin-left: 1.5em;\"> $SubDeclineReason </div></p>\r\n        #end\r\n        <p style=\"font-size:12pt;font-family:'Segoe UI'\"> We truly appreciate your interest. </p><p style=\"font-size:12pt;font-family:'Segoe UI'\">All the best,</p><p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p><a style=\"font-size:12pt;font-family:'Segoe UI'\" href=\"http://$DevPortalUrl\">$DevPortalUrl</a></body>\r\n</html>",
                "title": "Subscription request declined",
                "description": "This email is sent to developers when their subscription requests for products requiring publisher approval is declined.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "SubDeclineReason",
                        "title": "Reason for declining subscription"
                    },
                    {
                        "name": "ProdName",
                        "title": "Product name"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/RequestDeveloperNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "subject": "Your subscription request for the $ProdName",
                "body": "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          Thank you for your interest in our <strong>$ProdName</strong> API product!\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          We were delighted to receive your subscription request. We will promptly review it and get back to you at <strong>$DevEmail</strong>.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n    <a style=\"font-size:12pt;font-family:'Segoe UI'\" href=\"http://$DevPortalUrl\">$DevPortalUrl</a>\r\n  </body>\r\n</html>",
                "title": "Subscription request received",
                "description": "This email is sent to developers to acknowledge receipt of their subscription requests for products requiring publisher approval.",
                "parameters": [
                    {
                        "name": "DevFirstName",
                        "title": "Developer first name"
                    },
                    {
                        "name": "DevLastName",
                        "title": "Developer last name"
                    },
                    {
                        "name": "DevEmail",
                        "title": "Developer email"
                    },
                    {
                        "name": "ProdName",
                        "title": "Product name"
                    },
                    {
                        "name": "OrganizationName",
                        "title": "Organization name"
                    },
                    {
                        "name": "DevPortalUrl",
                        "title": "Developer portal URL"
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/users",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/1')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "firstName": "Administrator",
                "email": "Gijs.Reijn@rabobank.com",
                "state": "active",
                "identities": [
                    {
                        "provider": "Azure",
                        "id": "Gijs.Reijn@rabobank.com"
                    }
                ],
                "lastName": "[parameters('users_1_lastName')]"
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlDatabases",
            "apiVersion": "2024-05-15",
            "name": "[concat(parameters('databaseAccounts_cosmos_rest_name'), '/WinGet')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmos_rest_name'))]"
            ],
            "properties": {
                "resource": {
                    "id": "WinGet"
                }
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlRoleDefinitions",
            "apiVersion": "2024-05-15",
            "name": "[concat(parameters('databaseAccounts_cosmos_rest_name'), '/00000000-0000-0000-0000-000000000001')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmos_rest_name'))]"
            ],
            "properties": {
                "roleName": "Cosmos DB Built-in Data Reader",
                "type": "BuiltInRole",
                "assignableScopes": [
                    "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmos_rest_name'))]"
                ],
                "permissions": [
                    {
                        "dataActions": [
                            "Microsoft.DocumentDB/databaseAccounts/readMetadata",
                            "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/executeQuery",
                            "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/readChangeFeed",
                            "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/items/read"
                        ],
                        "notDataActions": []
                    }
                ]
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlRoleDefinitions",
            "apiVersion": "2024-05-15",
            "name": "[concat(parameters('databaseAccounts_cosmos_rest_name'), '/00000000-0000-0000-0000-000000000002')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmos_rest_name'))]"
            ],
            "properties": {
                "roleName": "Cosmos DB Built-in Data Contributor",
                "type": "BuiltInRole",
                "assignableScopes": [
                    "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmos_rest_name'))]"
                ],
                "permissions": [
                    {
                        "dataActions": [
                            "Microsoft.DocumentDB/databaseAccounts/readMetadata",
                            "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/*",
                            "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/items/*"
                        ],
                        "notDataActions": []
                    }
                ]
            }
        },
        {
            "type": "Microsoft.EventGrid/systemTopics",
            "apiVersion": "2024-06-01-preview",
            "name": "[parameters('systemTopics_strest_bc39d05f_e52c_407d_a7f8_1c2d85e68c85_name')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "source": "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]",
                "topicType": "microsoft.storage.storageaccounts"
            }
        },
        {
            "type": "Microsoft.EventGrid/systemTopics/eventSubscriptions",
            "apiVersion": "2024-06-01-preview",
            "name": "[concat(parameters('systemTopics_strest_bc39d05f_e52c_407d_a7f8_1c2d85e68c85_name'), '/StorageAntimalwareSubscription')]",
            "dependsOn": [
                "[resourceId('Microsoft.EventGrid/systemTopics', parameters('systemTopics_strest_bc39d05f_e52c_407d_a7f8_1c2d85e68c85_name'))]"
            ],
            "properties": {
                "destination": {
                    "properties": {
                        "maxEventsPerBatch": 1,
                        "preferredBatchSizeInKilobytes": 64,
                        "azureActiveDirectoryTenantId": "33e01921-4d64-4f8c-a055-5bdaffd5e33d",
                        "azureActiveDirectoryApplicationIdOrUri": "f1f8da5f-609a-401d-85b2-d498116b7265"
                    },
                    "endpointType": "WebHook"
                },
                "filter": {
                    "includedEventTypes": [
                        "Microsoft.Storage.BlobCreated"
                    ],
                    "advancedFilters": [
                        {
                            "values": [
                                "BlockBlob"
                            ],
                            "operatorType": "StringContains",
                            "key": "data.blobType"
                        }
                    ]
                },
                "eventDeliverySchema": "EventGridSchema",
                "retryPolicy": {
                    "maxDeliveryAttempts": 30,
                    "eventTimeToLiveInMinutes": 1440
                }
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/degradationindependencyduration')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "degradationindependencyduration",
                    "DisplayName": "Degradation in dependency duration",
                    "Description": "Smart Detection rules notify you of performance anomaly issues.",
                    "HelpUrl": "https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics",
                    "IsHidden": false,
                    "IsEnabledByDefault": true,
                    "IsInPreview": false,
                    "SupportsEmailNotifications": true
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/degradationinserverresponsetime')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "degradationinserverresponsetime",
                    "DisplayName": "Degradation in server response time",
                    "Description": "Smart Detection rules notify you of performance anomaly issues.",
                    "HelpUrl": "https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics",
                    "IsHidden": false,
                    "IsEnabledByDefault": true,
                    "IsInPreview": false,
                    "SupportsEmailNotifications": true
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/digestMailConfiguration')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "digestMailConfiguration",
                    "DisplayName": "Digest Mail Configuration",
                    "Description": "This rule describes the digest mail preferences",
                    "HelpUrl": "www.homail.com",
                    "IsHidden": true,
                    "IsEnabledByDefault": true,
                    "IsInPreview": false,
                    "SupportsEmailNotifications": true
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/extension_billingdatavolumedailyspikeextension')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "extension_billingdatavolumedailyspikeextension",
                    "DisplayName": "Abnormal rise in daily data volume (preview)",
                    "Description": "This detection rule automatically analyzes the billing data generated by your application, and can warn you about an unusual increase in your application's billing costs",
                    "HelpUrl": "https://github.com/Microsoft/ApplicationInsights-Home/tree/master/SmartDetection/billing-data-volume-daily-spike.md",
                    "IsHidden": false,
                    "IsEnabledByDefault": true,
                    "IsInPreview": true,
                    "SupportsEmailNotifications": false
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/extension_canaryextension')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "extension_canaryextension",
                    "DisplayName": "Canary extension",
                    "Description": "Canary extension",
                    "HelpUrl": "https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/",
                    "IsHidden": true,
                    "IsEnabledByDefault": true,
                    "IsInPreview": true,
                    "SupportsEmailNotifications": false
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/extension_exceptionchangeextension')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "extension_exceptionchangeextension",
                    "DisplayName": "Abnormal rise in exception volume (preview)",
                    "Description": "This detection rule automatically analyzes the exceptions thrown in your application, and can warn you about unusual patterns in your exception telemetry.",
                    "HelpUrl": "https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/abnormal-rise-in-exception-volume.md",
                    "IsHidden": false,
                    "IsEnabledByDefault": true,
                    "IsInPreview": true,
                    "SupportsEmailNotifications": false
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/extension_memoryleakextension')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "extension_memoryleakextension",
                    "DisplayName": "Potential memory leak detected (preview)",
                    "Description": "This detection rule automatically analyzes the memory consumption of each process in your application, and can warn you about potential memory leaks or increased memory consumption.",
                    "HelpUrl": "https://github.com/Microsoft/ApplicationInsights-Home/tree/master/SmartDetection/memory-leak.md",
                    "IsHidden": false,
                    "IsEnabledByDefault": true,
                    "IsInPreview": true,
                    "SupportsEmailNotifications": false
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/extension_securityextensionspackage')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "extension_securityextensionspackage",
                    "DisplayName": "Potential security issue detected (preview)",
                    "Description": "This detection rule automatically analyzes the telemetry generated by your application and detects potential security issues.",
                    "HelpUrl": "https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/application-security-detection-pack.md",
                    "IsHidden": false,
                    "IsEnabledByDefault": true,
                    "IsInPreview": true,
                    "SupportsEmailNotifications": false
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/extension_traceseveritydetector')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "extension_traceseveritydetector",
                    "DisplayName": "Degradation in trace severity ratio (preview)",
                    "Description": "This detection rule automatically analyzes the trace logs emitted from your application, and can warn you about unusual patterns in the severity of your trace telemetry.",
                    "HelpUrl": "https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/degradation-in-trace-severity-ratio.md",
                    "IsHidden": false,
                    "IsEnabledByDefault": true,
                    "IsInPreview": true,
                    "SupportsEmailNotifications": false
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/longdependencyduration')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "longdependencyduration",
                    "DisplayName": "Long dependency duration",
                    "Description": "Smart Detection rules notify you of performance anomaly issues.",
                    "HelpUrl": "https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics",
                    "IsHidden": false,
                    "IsEnabledByDefault": true,
                    "IsInPreview": false,
                    "SupportsEmailNotifications": true
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/migrationToAlertRulesCompleted')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "migrationToAlertRulesCompleted",
                    "DisplayName": "Migration To Alert Rules Completed",
                    "Description": "A configuration that controls the migration state of Smart Detection to Smart Alerts",
                    "HelpUrl": "https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics",
                    "IsHidden": true,
                    "IsEnabledByDefault": false,
                    "IsInPreview": true,
                    "SupportsEmailNotifications": false
                },
                "enabled": false,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/slowpageloadtime')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "slowpageloadtime",
                    "DisplayName": "Slow page load time",
                    "Description": "Smart Detection rules notify you of performance anomaly issues.",
                    "HelpUrl": "https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics",
                    "IsHidden": false,
                    "IsEnabledByDefault": true,
                    "IsInPreview": false,
                    "SupportsEmailNotifications": true
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appin_rest_name'), '/slowserverresponsetime')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appin_rest_name'))]"
            ],
            "properties": {
                "ruleDefinitions": {
                    "Name": "slowserverresponsetime",
                    "DisplayName": "Slow server response time",
                    "Description": "Smart Detection rules notify you of performance anomaly issues.",
                    "HelpUrl": "https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics",
                    "IsHidden": false,
                    "IsEnabledByDefault": true,
                    "IsInPreview": false,
                    "SupportsEmailNotifications": true
                },
                "enabled": true,
                "sendEmailsToSubscriptionOwners": true,
                "customEmails": []
            }
        },
        {
            "type": "Microsoft.KeyVault/vaults/secrets",
            "apiVersion": "2024-04-01-preview",
            "name": "[concat(parameters('vaults_kv_rest_name'), '/AppConfigPrimaryEndpoint')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('Microsoft.KeyVault/vaults', parameters('vaults_kv_rest_name'))]"
            ],
            "properties": {
                "attributes": {
                    "enabled": true
                }
            }
        },
        {
            "type": "Microsoft.KeyVault/vaults/secrets",
            "apiVersion": "2024-04-01-preview",
            "name": "[concat(parameters('vaults_kv_rest_name'), '/AppConfigSecondaryEndpoint')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('Microsoft.KeyVault/vaults', parameters('vaults_kv_rest_name'))]"
            ],
            "properties": {
                "attributes": {
                    "enabled": true
                }
            }
        },
        {
            "type": "Microsoft.KeyVault/vaults/secrets",
            "apiVersion": "2024-04-01-preview",
            "name": "[concat(parameters('vaults_kv_rest_name'), '/AzureFunctionHostKey')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('Microsoft.KeyVault/vaults', parameters('vaults_kv_rest_name'))]"
            ],
            "properties": {
                "attributes": {
                    "enabled": true
                }
            }
        },
        {
            "type": "Microsoft.KeyVault/vaults/secrets",
            "apiVersion": "2024-04-01-preview",
            "name": "[concat(parameters('vaults_kv_rest_name'), '/CosmosAccountEndpoint')]",
            "location": "westus",
            "dependsOn": [
                "[resourceId('Microsoft.KeyVault/vaults', parameters('vaults_kv_rest_name'))]"
            ],
            "properties": {
                "attributes": {
                    "enabled": true
                }
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/blobServices",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "sku": {
                "name": "Standard_GRS",
                "tier": "Standard"
            },
            "properties": {
                "cors": {
                    "corsRules": []
                },
                "deleteRetentionPolicy": {
                    "allowPermanentDelete": false,
                    "enabled": false
                }
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/fileServices",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "sku": {
                "name": "Standard_GRS",
                "tier": "Standard"
            },
            "properties": {
                "protocolSettings": {
                    "smb": {}
                },
                "cors": {
                    "corsRules": []
                },
                "shareDeleteRetentionPolicy": {
                    "enabled": true,
                    "days": 7
                }
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/queueServices",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "cors": {
                    "corsRules": []
                }
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/tableServices",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "cors": {
                    "corsRules": []
                }
            }
        },
        {
            "type": "Microsoft.Web/sites",
            "apiVersion": "2023-12-01",
            "name": "[parameters('sites_azfun_rest_name')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/serverfarms', parameters('serverfarms_asp_rest_name'))]"
            ],
            "kind": "functionapp",
            "identity": {
                "type": "SystemAssigned"
            },
            "properties": {
                "enabled": true,
                "hostNameSslStates": [
                    {
                        "name": "[concat(parameters('sites_azfun_rest_name'), '.azurewebsites.net')]",
                        "sslState": "Disabled",
                        "hostType": "Standard"
                    },
                    {
                        "name": "[concat(parameters('sites_azfun_rest_name'), '.scm.azurewebsites.net')]",
                        "sslState": "Disabled",
                        "hostType": "Repository"
                    }
                ],
                "serverFarmId": "[resourceId('Microsoft.Web/serverfarms', parameters('serverfarms_asp_rest_name'))]",
                "reserved": false,
                "isXenon": false,
                "hyperV": false,
                "dnsConfiguration": {},
                "vnetRouteAllEnabled": false,
                "vnetImagePullEnabled": false,
                "vnetContentShareEnabled": false,
                "siteConfig": {
                    "numberOfWorkers": 1,
                    "acrUseManagedIdentityCreds": false,
                    "alwaysOn": true,
                    "http20Enabled": false,
                    "functionAppScaleLimit": 0,
                    "minimumElasticInstanceCount": 0
                },
                "scmSiteAlsoStopped": false,
                "clientAffinityEnabled": false,
                "clientCertEnabled": false,
                "clientCertMode": "Required",
                "hostNamesDisabled": false,
                "vnetBackupRestoreEnabled": false,
                "customDomainVerificationId": "3AE4B0D96270F9FD6245978C3E9070E9480B095802237E88AE0DBC7F011F6387",
                "containerSize": 1536,
                "dailyMemoryTimeQuota": 0,
                "httpsOnly": true,
                "redundancyMode": "None",
                "storageAccountRequired": false,
                "keyVaultReferenceIdentity": "SystemAssigned"
            }
        },
        {
            "type": "Microsoft.Web/sites/basicPublishingCredentialsPolicies",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/ftp')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "allow": true
            }
        },
        {
            "type": "Microsoft.Web/sites/basicPublishingCredentialsPolicies",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/scm')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "allow": true
            }
        },
        {
            "type": "Microsoft.Web/sites/config",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/web')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "numberOfWorkers": 1,
                "defaultDocuments": [
                    "Default.htm",
                    "Default.html",
                    "Default.asp",
                    "index.htm",
                    "index.html",
                    "iisstart.htm",
                    "default.aspx",
                    "index.php"
                ],
                "netFrameworkVersion": "v4.0",
                "requestTracingEnabled": false,
                "remoteDebuggingEnabled": false,
                "httpLoggingEnabled": false,
                "acrUseManagedIdentityCreds": false,
                "logsDirectorySizeLimit": 35,
                "detailedErrorLoggingEnabled": false,
                "publishingUsername": "$azfun-rest",
                "scmType": "None",
                "use32BitWorkerProcess": true,
                "webSocketsEnabled": false,
                "alwaysOn": true,
                "managedPipelineMode": "Integrated",
                "virtualApplications": [
                    {
                        "virtualPath": "/",
                        "physicalPath": "site\\wwwroot",
                        "preloadEnabled": true
                    }
                ],
                "loadBalancing": "LeastRequests",
                "experiments": {
                    "rampUpRules": []
                },
                "autoHealEnabled": false,
                "vnetRouteAllEnabled": false,
                "vnetPrivatePortsCount": 0,
                "cors": {
                    "allowedOrigins": [
                        "https://functions.azure.com",
                        "https://functions-staging.azure.com",
                        "https://functions-next.azure.com"
                    ],
                    "supportCredentials": false
                },
                "localMySqlEnabled": false,
                "managedServiceIdentityId": 1507,
                "ipSecurityRestrictions": [
                    {
                        "ipAddress": "Any",
                        "action": "Allow",
                        "priority": 2147483647,
                        "name": "Allow all",
                        "description": "Allow all access"
                    }
                ],
                "scmIpSecurityRestrictions": [
                    {
                        "ipAddress": "Any",
                        "action": "Allow",
                        "priority": 2147483647,
                        "name": "Allow all",
                        "description": "Allow all access"
                    }
                ],
                "scmIpSecurityRestrictionsUseMain": false,
                "http20Enabled": false,
                "minTlsVersion": "1.2",
                "scmMinTlsVersion": "1.2",
                "ftpsState": "FtpsOnly",
                "preWarmedInstanceCount": 0,
                "functionAppScaleLimit": 0,
                "functionsRuntimeScaleMonitoringEnabled": false,
                "minimumElasticInstanceCount": 0,
                "azureStorageAccounts": {}
            }
        },
        {
            "type": "Microsoft.Web/sites/deployments",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/9d4d9455ac6d41b1bdbc7879ba083d73')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "status": 4,
                "author_email": "N/A",
                "author": "N/A",
                "deployer": "OneDeploy",
                "message": "OneDeploy",
                "start_time": "2024-12-31T02:28:13.430233Z",
                "end_time": "2024-12-31T02:28:26.4008686Z",
                "active": true
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/InformationGet')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/InformationGet/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/InformationGet/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/InformationGet.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/InformationGet",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "information",
                            "methods": [
                                "get"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.ServerFunctions.InformationGetAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/information",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/InstallerDelete')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/InstallerDelete/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/InstallerDelete/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/InstallerDelete.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/InstallerDelete",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier}",
                            "methods": [
                                "delete"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.InstallerFunctions.InstallerDeleteAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions/{packageversion}/installers/{installeridentifier}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/InstallerGet')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/InstallerGet/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/InstallerGet/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/InstallerGet.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/InstallerGet",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier?}",
                            "methods": [
                                "get"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.InstallerFunctions.InstallerGetAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions/{packageversion}/installers/{installeridentifier?}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/InstallerPost')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/InstallerPost/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/InstallerPost/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/InstallerPost.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/InstallerPost",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions/{packageVersion}/installers",
                            "methods": [
                                "post"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.InstallerFunctions.InstallerPostAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions/{packageversion}/installers",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/InstallerPut')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/InstallerPut/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/InstallerPut/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/InstallerPut.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/InstallerPut",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier}",
                            "methods": [
                                "put"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.InstallerFunctions.InstallerPutAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions/{packageversion}/installers/{installeridentifier}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/LocaleDelete')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/LocaleDelete/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/LocaleDelete/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/LocaleDelete.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/LocaleDelete",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}",
                            "methods": [
                                "delete"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.LocaleFunctions.LocaleDeleteAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions/{packageversion}/locales/{packagelocale}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/LocaleGet')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/LocaleGet/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/LocaleGet/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/LocaleGet.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/LocaleGet",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale?}",
                            "methods": [
                                "get"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.LocaleFunctions.LocaleGetAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions/{packageversion}/locales/{packagelocale?}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/LocalePost')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/LocalePost/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/LocalePost/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/LocalePost.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/LocalePost",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions/{packageVersion}/locales",
                            "methods": [
                                "post"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.LocaleFunctions.LocalePostAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions/{packageversion}/locales",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/LocalePut')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/LocalePut/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/LocalePut/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/LocalePut.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/LocalePut",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}",
                            "methods": [
                                "put"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.LocaleFunctions.LocalePutAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions/{packageversion}/locales/{packagelocale}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/ManifestDelete')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/ManifestDelete/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/ManifestDelete/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/ManifestDelete.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/ManifestDelete",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packageManifests/{packageIdentifier}",
                            "methods": [
                                "delete"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.PackageManifestFunctions.ManifestDeleteAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packagemanifests/{packageidentifier}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/ManifestGet')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/ManifestGet/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/ManifestGet/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/ManifestGet.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/ManifestGet",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packageManifests/{packageIdentifier?}",
                            "methods": [
                                "get"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.PackageManifestFunctions.ManifestGetAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packagemanifests/{packageidentifier?}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/ManifestPost')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/ManifestPost/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/ManifestPost/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/ManifestPost.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/ManifestPost",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packageManifests",
                            "methods": [
                                "post"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.PackageManifestFunctions.ManifestPostAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packagemanifests",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/ManifestPut')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/ManifestPut/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/ManifestPut/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/ManifestPut.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/ManifestPut",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packageManifests/{packageIdentifier}",
                            "methods": [
                                "put"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.PackageManifestFunctions.ManifestPutAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packagemanifests/{packageidentifier}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/ManifestSearchPost')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/ManifestSearchPost/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/ManifestSearchPost/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/ManifestSearchPost.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/ManifestSearchPost",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "manifestSearch",
                            "methods": [
                                "post"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.ManifestSearchFunctions.ManifestSearchPostAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/manifestsearch",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/PackageDelete')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/PackageDelete/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/PackageDelete/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/PackageDelete.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/PackageDelete",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}",
                            "methods": [
                                "delete"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.PackageFunctions.PackageDeleteAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/PackageGet')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/PackageGet/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/PackageGet/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/PackageGet.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/PackageGet",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier?}",
                            "methods": [
                                "get"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.PackageFunctions.PackagesGetAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier?}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/PackagePost')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/PackagePost/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/PackagePost/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/PackagePost.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/PackagePost",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages",
                            "methods": [
                                "post"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.PackageFunctions.PackagesPostAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/PackagePut')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/PackagePut/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/PackagePut/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/PackagePut.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/PackagePut",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}",
                            "methods": [
                                "put"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.PackageFunctions.PackagesPutAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/RebuildActivity')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/RebuildActivity/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/RebuildActivity/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/RebuildActivity.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/RebuildActivity",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "activityTrigger",
                            "name": "durableContext"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.SourceFunctions.RebuildActivityAsync"
                },
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/RebuildOrchestrator')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/RebuildOrchestrator/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/RebuildOrchestrator/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/RebuildOrchestrator.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/RebuildOrchestrator",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "orchestrationTrigger",
                            "name": "durableContext"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.SourceFunctions.RebuildOrchestratorAsync"
                },
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/RebuildPost')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/RebuildPost/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/RebuildPost/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/RebuildPost.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/RebuildPost",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "rebuild",
                            "methods": [
                                "post"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        },
                        {
                            "type": "durableClient",
                            "externalClient": false,
                            "name": "durableClient"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.SourceFunctions.RebuildPostAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/rebuild",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/UpdateActivity')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/UpdateActivity/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/UpdateActivity/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/UpdateActivity.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/UpdateActivity",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "activityTrigger",
                            "name": "durableContext"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.SourceFunctions.UpdateActivityAsync"
                },
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/UpdateOrchestrator')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/UpdateOrchestrator/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/UpdateOrchestrator/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/UpdateOrchestrator.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/UpdateOrchestrator",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "orchestrationTrigger",
                            "name": "durableContext"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.SourceFunctions.UpdateOrchestratorAsync"
                },
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/UpdatePost')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/UpdatePost/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/UpdatePost/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/UpdatePost.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/UpdatePost",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "update",
                            "methods": [
                                "post"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        },
                        {
                            "type": "durableClient",
                            "externalClient": false,
                            "name": "durableClient"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.SourceFunctions.UpdatePostAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/update",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/VersionDelete')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/VersionDelete/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/VersionDelete/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/VersionDelete.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/VersionDelete",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions/{packageVersion}",
                            "methods": [
                                "delete"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.VersionFunctions.VersionsDeleteAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions/{packageversion}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/VersionGet')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/VersionGet/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/VersionGet/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/VersionGet.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/VersionGet",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions/{packageVersion?}",
                            "methods": [
                                "get"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.VersionFunctions.VersionsGetAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions/{packageversion?}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/VersionPost')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/VersionPost/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/VersionPost/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/VersionPost.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/VersionPost",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions",
                            "methods": [
                                "post"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.VersionFunctions.VersionsPostAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/functions",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/VersionPut')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "script_root_path_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/VersionPut/",
                "script_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/bin/Microsoft.WinGet.RestSource.Functions.dll",
                "config_href": "https://azfun-rest.azurewebsites.net/admin/vfs/site/wwwroot/VersionPut/function.json",
                "test_data_href": "https://azfun-rest.azurewebsites.net/admin/vfs/data/Functions/sampledata/VersionPut.dat",
                "href": "https://azfun-rest.azurewebsites.net/admin/functions/VersionPut",
                "config": {
                    "generatedBy": "Microsoft.NET.Sdk.Functions.Generator-4.2.0",
                    "configurationSource": "attributes",
                    "bindings": [
                        {
                            "type": "httpTrigger",
                            "route": "packages/{packageIdentifier}/versions/{packageVersion}",
                            "methods": [
                                "put"
                            ],
                            "authLevel": "function",
                            "name": "req"
                        }
                    ],
                    "disabled": false,
                    "scriptFile": "../bin/Microsoft.WinGet.RestSource.Functions.dll",
                    "entryPoint": "Microsoft.WinGet.RestSource.Functions.VersionFunctions.VersionsPutAsync"
                },
                "invoke_url_template": "https://azfun-rest.azurewebsites.net/api/packages/{packageidentifier}/versions/{packageversion}",
                "language": "DotNetAssembly",
                "isDisabled": false
            }
        },
        {
            "type": "Microsoft.Web/sites/hostNameBindings",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_azfun_rest_name'), '/', parameters('sites_azfun_rest_name'), '.azurewebsites.net')]",
            "location": "West US",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_azfun_rest_name'))]"
            ],
            "properties": {
                "siteName": "azfun-rest",
                "hostNameType": "Verified"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/echo-api/create-resource')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Create resource",
                "method": "POST",
                "urlTemplate": "/resource",
                "templateParameters": [],
                "description": "A demonstration of a POST call based on the echo backend above. The request body is expected to contain JSON-formatted data (see example below). A policy is used to automatically transform any request sent in JSON directly to XML. In a real-world scenario this could be used to enable modern clients to speak to a legacy backend.",
                "request": {
                    "queryParameters": [],
                    "headers": [],
                    "representations": [
                        {
                            "contentType": "application/json",
                            "examples": {
                                "default": {
                                    "value": "{\r\n\t\"vehicleType\": \"train\",\r\n\t\"maxSpeed\": 125,\r\n\t\"avgSpeed\": 90,\r\n\t\"speedUnit\": \"mph\"\r\n}"
                                }
                            }
                        }
                    ]
                },
                "responses": [
                    {
                        "statusCode": 200,
                        "representations": [],
                        "headers": []
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/delete-installerdelete')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "InstallerDelete",
                "method": "DELETE",
                "urlTemplate": "/packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageVersion",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "installerIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/delete-localedelete')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "LocaleDelete",
                "method": "DELETE",
                "urlTemplate": "/packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageVersion",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageLocale",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/delete-manifestdelete')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "ManifestDelete",
                "method": "DELETE",
                "urlTemplate": "/packageManifests/{packageIdentifier}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/delete-packagedelete')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "PackageDelete",
                "method": "DELETE",
                "urlTemplate": "/packages/{packageIdentifier}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/delete-versiondelete')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "VersionDelete",
                "method": "DELETE",
                "urlTemplate": "/packages/{packageIdentifier}/versions/{packageVersion}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageVersion",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-informationget')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "InformationGet",
                "method": "GET",
                "urlTemplate": "/information",
                "templateParameters": [],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-installerget')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "InstallerGet",
                "method": "GET",
                "urlTemplate": "/packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageVersion",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "installerIdentifier",
                        "type": "string",
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-localeget')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "LocaleGet",
                "method": "GET",
                "urlTemplate": "/packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageVersion",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageLocale",
                        "type": "string",
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-manifestget')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "ManifestGet",
                "method": "GET",
                "urlTemplate": "/packageManifests/{packageIdentifier}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-packageget')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "PackageGet",
                "method": "GET",
                "urlTemplate": "/packages/{packageIdentifier}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-versionget')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "VersionGet",
                "method": "GET",
                "urlTemplate": "/packages/{packageIdentifier}/versions/{packageVersion}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageVersion",
                        "type": "string",
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/echo-api/modify-resource')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Modify Resource",
                "method": "PUT",
                "urlTemplate": "/resource",
                "templateParameters": [],
                "description": "A demonstration of a PUT call handled by the same \"echo\" backend as above. You can now specify a request body in addition to headers and it will be returned as well.",
                "responses": [
                    {
                        "statusCode": 200,
                        "representations": [],
                        "headers": []
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-installerpost')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "InstallerPost",
                "method": "POST",
                "urlTemplate": "/packages/{packageIdentifier}/versions/{packageVersion}/installers",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageVersion",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-localepost')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "LocalePost",
                "method": "POST",
                "urlTemplate": "/packages/{packageIdentifier}/versions/{packageVersion}/locales",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageVersion",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-manifestpost')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "ManifestPost",
                "method": "POST",
                "urlTemplate": "/packageManifests",
                "templateParameters": [],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-manifestsearchpost')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "ManifestSearchPost",
                "method": "POST",
                "urlTemplate": "/manifestSearch",
                "templateParameters": [],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-packagepost')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "PackagePost",
                "method": "POST",
                "urlTemplate": "/packages",
                "templateParameters": [],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-rebuildpost')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "RebuildPost",
                "method": "POST",
                "urlTemplate": "/rebuild",
                "templateParameters": [],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-updatepost')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "UpdatePost",
                "method": "POST",
                "urlTemplate": "/update",
                "templateParameters": [],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-versionpost')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "VersionPost",
                "method": "POST",
                "urlTemplate": "/packages/",
                "templateParameters": [],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/put-installerput')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "InstallerPut",
                "method": "PUT",
                "urlTemplate": "/packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageVersion",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "installerIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/put-localeput')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "LocalePut",
                "method": "PUT",
                "urlTemplate": "/packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageVersion",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageLocale",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/put-manifestput')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "ManifestPut",
                "method": "PUT",
                "urlTemplate": "/packageManifests/{packageIdentifier}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/put-packageput')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "PackagePut",
                "method": "PUT",
                "urlTemplate": "/packages/{packageIdentifier}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/put-versionput')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "VersionPut",
                "method": "PUT",
                "urlTemplate": "/packages/{packageIdentifier}/versions/{packageVersion}",
                "templateParameters": [
                    {
                        "name": "packageIdentifier",
                        "type": "string",
                        "required": true,
                        "values": []
                    },
                    {
                        "name": "packageVersion",
                        "type": "string",
                        "required": true,
                        "values": []
                    }
                ],
                "responses": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/echo-api/remove-resource')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Remove resource",
                "method": "DELETE",
                "urlTemplate": "/resource",
                "templateParameters": [],
                "description": "A demonstration of a DELETE call which traditionally deletes the resource. It is based on the same \"echo\" backend as in all other operations so nothing is actually deleted.",
                "responses": [
                    {
                        "statusCode": 200,
                        "representations": [],
                        "headers": []
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/echo-api/retrieve-header-only')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Retrieve header only",
                "method": "HEAD",
                "urlTemplate": "/resource",
                "templateParameters": [],
                "description": "The HEAD operation returns only headers. In this demonstration a policy is used to set additional headers when the response is returned and to enable JSONP.",
                "responses": [
                    {
                        "statusCode": 200,
                        "representations": [],
                        "headers": []
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/echo-api/retrieve-resource')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Retrieve resource",
                "method": "GET",
                "urlTemplate": "/resource",
                "templateParameters": [],
                "description": "A demonstration of a GET call on a sample resource. It is handled by an \"echo\" backend which returns a response equal to the request (the supplied headers and body are being returned as received).",
                "request": {
                    "queryParameters": [
                        {
                            "name": "param1",
                            "description": "A sample parameter that is required and has a default value of \"sample\".",
                            "type": "string",
                            "defaultValue": "sample",
                            "required": true,
                            "values": [
                                "sample"
                            ]
                        },
                        {
                            "name": "param2",
                            "description": "Another sample parameter, set to not required.",
                            "type": "number",
                            "values": []
                        }
                    ],
                    "headers": [],
                    "representations": []
                },
                "responses": [
                    {
                        "statusCode": 200,
                        "description": "Returned in all cases.",
                        "representations": [],
                        "headers": []
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/echo-api/retrieve-resource-cached')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "displayName": "Retrieve resource (cached)",
                "method": "GET",
                "urlTemplate": "/resource-cached",
                "templateParameters": [],
                "description": "A demonstration of a GET call with caching enabled on the same \"echo\" backend as above. Cache TTL is set to 1 hour. When you make the first request the headers you supplied will be cached. Subsequent calls will return the same headers as the first time even if you change them in your request.",
                "request": {
                    "queryParameters": [
                        {
                            "name": "param1",
                            "description": "A sample parameter that is required and has a default value of \"sample\".",
                            "type": "string",
                            "defaultValue": "sample",
                            "required": true,
                            "values": [
                                "sample"
                            ]
                        },
                        {
                            "name": "param2",
                            "description": "Another sample parameter, set to not required.",
                            "type": "string",
                            "values": []
                        }
                    ],
                    "headers": [],
                    "representations": []
                },
                "responses": [
                    {
                        "statusCode": 200,
                        "representations": [],
                        "headers": []
                    }
                ]
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/wikis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/echo-api/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "documents": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/wikis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "documents": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/backends",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget-backend-pool')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/backends', parameters('service_apim_rest_name'), 'winget-backend0')]"
            ],
            "properties": {
                "description": "winget rest source backend function pool",
                "type": "pool",
                "url": "https://www.backend-pool.com",
                "protocol": "http",
                "pool": {
                    "services": [
                        {
                            "id": "[resourceId('Microsoft.ApiManagement/service/backends', parameters('service_apim_rest_name'), 'winget-backend0')]"
                        }
                    ]
                }
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/groups/users",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/administrators/1')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'administrators')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/groups/users",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/developers/1')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'developers')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/apis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/starter/echo-api')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/apis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/unlimited/echo-api')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/starter/administrators')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/unlimited/administrators')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/starter/developers')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/unlimited/developers')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/starter/guests')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/unlimited/guests')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/starter/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<!--\r\n            IMPORTANT:\r\n            - Policy elements can appear only within the <inbound>, <outbound>, <backend> section elements.\r\n            - Only the <forward-request> policy element can appear within the <backend> section element.\r\n            - To apply a policy to the incoming request (before it is forwarded to the backend service), place a corresponding policy element within the <inbound> section element.\r\n            - To apply a policy to the outgoing response (before it is sent back to the caller), place a corresponding policy element within the <outbound> section element.\r\n            - To add a policy position the cursor at the desired insertion point and click on the round button associated with the policy.\r\n            - To remove a policy, delete the corresponding policy statement from the policy document.\r\n            - Position the <base> element within a section element to inherit all policies from the corresponding section element in the enclosing scope.\r\n            - Remove the <base> element to prevent inheriting policies from the corresponding section element in the enclosing scope.\r\n            - Policies are applied in the order of their appearance, from the top down.\r\n        -->\r\n<policies>\r\n  <inbound>\r\n    <rate-limit calls=\"5\" renewal-period=\"60\" />\r\n    <quota calls=\"100\" renewal-period=\"604800\" />\r\n    <base />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/wikis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/starter/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "documents": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/wikis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/unlimited/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "documents": []
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers",
            "apiVersion": "2024-05-15",
            "name": "[concat(parameters('databaseAccounts_cosmos_rest_name'), '/WinGet/Manifests')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts/sqlDatabases', parameters('databaseAccounts_cosmos_rest_name'), 'WinGet')]",
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmos_rest_name'))]"
            ],
            "properties": {
                "resource": {
                    "id": "Manifests",
                    "indexingPolicy": {
                        "indexingMode": "consistent",
                        "automatic": true,
                        "includedPaths": [
                            {
                                "path": "/*"
                            }
                        ],
                        "excludedPaths": [
                            {
                                "path": "/\"_etag\"/?"
                            }
                        ]
                    },
                    "partitionKey": {
                        "paths": [
                            "/id"
                        ],
                        "kind": "Hash"
                    },
                    "uniqueKeyPolicy": {
                        "uniqueKeys": []
                    },
                    "conflictResolutionPolicy": {
                        "mode": "LastWriterWins",
                        "conflictResolutionPath": "/_ts"
                    },
                    "computedProperties": []
                }
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/throughputSettings",
            "apiVersion": "2024-05-15",
            "name": "[concat(parameters('databaseAccounts_cosmos_rest_name'), '/WinGet/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts/sqlDatabases', parameters('databaseAccounts_cosmos_rest_name'), 'WinGet')]",
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmos_rest_name'))]"
            ],
            "properties": {
                "resource": {
                    "throughput": 400,
                    "autoscaleSettings": {
                        "maxThroughput": 4000
                    }
                }
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlRoleAssignments",
            "apiVersion": "2024-05-15",
            "name": "[concat(parameters('databaseAccounts_cosmos_rest_name'), '/06f1d5f8-a6c3-4819-9a37-e5ba0983af37')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmos_rest_name'))]",
                "[resourceId('Microsoft.DocumentDB/databaseAccounts/sqlRoleDefinitions', parameters('databaseAccounts_cosmos_rest_name'), '00000000-0000-0000-0000-000000000002')]"
            ],
            "properties": {
                "roleDefinitionId": "[resourceId('Microsoft.DocumentDB/databaseAccounts/sqlRoleDefinitions', parameters('databaseAccounts_cosmos_rest_name'), '00000000-0000-0000-0000-000000000002')]",
                "principalId": "5bc4dc27-79fd-4a44-909e-631525d129fe",
                "scope": "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmos_rest_name'))]"
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/blobServices/containers",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/azfunrest-applease')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/blobServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "immutableStorageWithVersioning": {
                    "enabled": false
                },
                "defaultEncryptionScope": "$account-encryption-key",
                "denyEncryptionScopeOverride": false,
                "publicAccess": "None"
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/blobServices/containers",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/azfunrest-leases')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/blobServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "immutableStorageWithVersioning": {
                    "enabled": false
                },
                "defaultEncryptionScope": "$account-encryption-key",
                "denyEncryptionScopeOverride": false,
                "publicAccess": "None"
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/blobServices/containers",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/azure-webjobs-hosts')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/blobServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "immutableStorageWithVersioning": {
                    "enabled": false
                },
                "defaultEncryptionScope": "$account-encryption-key",
                "denyEncryptionScopeOverride": false,
                "publicAccess": "None"
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/blobServices/containers",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/azure-webjobs-secrets')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/blobServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "immutableStorageWithVersioning": {
                    "enabled": false
                },
                "defaultEncryptionScope": "$account-encryption-key",
                "denyEncryptionScopeOverride": false,
                "publicAccess": "None"
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/blobServices/containers",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/blobServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "immutableStorageWithVersioning": {
                    "enabled": false
                },
                "defaultEncryptionScope": "$account-encryption-key",
                "denyEncryptionScopeOverride": false,
                "publicAccess": "None"
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/queueServices/queues",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/azfunrest-control-00')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/queueServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "metadata": {}
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/queueServices/queues",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/azfunrest-control-01')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/queueServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "metadata": {}
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/queueServices/queues",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/azfunrest-control-02')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/queueServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "metadata": {}
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/queueServices/queues",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/azfunrest-control-03')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/queueServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "metadata": {}
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/queueServices/queues",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/azfunrest-workitems')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/queueServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {
                "metadata": {}
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/tableServices/tables",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/azfunrestHistory')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/tableServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {}
        },
        {
            "type": "Microsoft.Storage/storageAccounts/tableServices/tables",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_strest_name'), '/default/azfunrestInstances')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/tableServices', parameters('storageAccounts_strest_name'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_strest_name'))]"
            ],
            "properties": {}
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/echo-api/create-resource/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'echo-api', 'create-resource')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <json-to-xml apply=\"always\" consider-accept-header=\"false\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/echo-api/retrieve-header-only/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'echo-api', 'retrieve-header-only')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n    <set-header name=\"X-My-Sample\" exists-action=\"override\">\r\n      <value>This is a sample</value>\r\n      <!-- for multiple headers with the same name add additional value elements -->\r\n    </set-header>\r\n    <jsonp callback-parameter-name=\"ProcessResponse\" />\r\n  </outbound>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/echo-api/retrieve-resource-cached/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'echo-api', 'retrieve-resource-cached')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <cache-lookup vary-by-developer=\"false\" vary-by-developer-groups=\"false\">\r\n      <vary-by-header>Accept</vary-by-header>\r\n      <vary-by-header>Accept-Charset</vary-by-header>\r\n    </cache-lookup>\r\n    <rewrite-uri template=\"/resource\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n    <cache-store duration=\"3600\" />\r\n  </outbound>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/delete-installerdelete/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'delete-installerdelete')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/delete-localedelete/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'delete-localedelete')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/delete-manifestdelete/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'delete-manifestdelete')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/delete-packagedelete/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'delete-packagedelete')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/delete-versiondelete/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'delete-versiondelete')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-informationget/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'get-informationget')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <set-backend-service id=\"public-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-installerget/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'get-installerget')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <set-backend-service id=\"query-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-localeget/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'get-localeget')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <set-backend-service id=\"query-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-manifestget/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'get-manifestget')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <set-backend-service id=\"query-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-packageget/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'get-packageget')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <set-backend-service id=\"query-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/get-versionget/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'get-versionget')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <set-backend-service id=\"query-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-installerpost/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'post-installerpost')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-localepost/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'post-localepost')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-manifestpost/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'post-manifestpost')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-manifestsearchpost/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'post-manifestsearchpost')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <set-backend-service id=\"query-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-packagepost/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'post-packagepost')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-rebuildpost/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'post-rebuildpost')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-updatepost/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'post-updatepost')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/post-versionpost/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'post-versionpost')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/put-installerput/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'put-installerput')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/put-localeput/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'put-localeput')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/put-manifestput/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'put-manifestput')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/put-packageput/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'put-packageput')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/winget/put-versionput/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apim_rest_name'), 'winget', 'put-versionput')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'winget')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <validate-azure-ad-token tenant-id=\"6e93a626-8aca-4dc1-9191-ce291b4b75a1\" header-name=\"authorization\" failed-validation-httpcode=\"403\" failed-validation-error-message=\"Forbidden\">\r\n      <client-application-ids>\r\n        <application-id>04b07795-8ddb-461a-bbee-02f9e1bf7b46</application-id>\r\n        <application-id>1950a258-227b-4e31-a9cf-717495945fc2</application-id>\r\n      </client-application-ids>\r\n      <audiences>\r\n        <audience>https://management.core.windows.net/</audience>\r\n        <audience>https://management.azure.com/</audience>\r\n      </audiences>\r\n      <required-claims>\r\n        <claim name=\"wids\" match=\"any\">\r\n          <value>9b895d92-2cd3-44c7-9d02-a6ac2d5ea5c3</value>\r\n          <value>62e90394-69f5-4237-9190-012177145e10</value>\r\n        </claim>\r\n      </required-claims>\r\n    </validate-azure-ad-token>\r\n    <set-backend-service id=\"manage-api-policy\" backend-id=\"winget-backend-pool\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/apiLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/starter/4B03F609-FD00-414E-81C5-5D7EBE0C3231')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]"
            ],
            "properties": {
                "apiId": "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/apiLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/unlimited/D640D8E5-38B3-4E54-9403-74682742F7D4')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]"
            ],
            "properties": {
                "apiId": "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apim_rest_name'), 'echo-api')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/unlimited/03EC6D54-F93C-471E-8BE1-B343B6CC7E1A')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'guests')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'guests')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/starter/272B3580-EAD0-4DE1-A489-A414E00F4C6D')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'guests')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'guests')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/unlimited/2A578534-0172-4EE1-B413-5B2F1A01C905')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'developers')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'developers')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/starter/5BA2E41E-88DD-4322-9CA7-FA5681026759')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'administrators')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'administrators')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/starter/735F2105-4291-4CD2-A366-CE6B74776D2F')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'developers')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'developers')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/unlimited/D7B9B439-CB85-4955-A74E-C3FB9E7BE3BC')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'administrators')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apim_rest_name'), 'administrators')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/subscriptions",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/677358889a937c006d070001')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/users', parameters('service_apim_rest_name'), '1')]",
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]"
            ],
            "properties": {
                "ownerId": "[resourceId('Microsoft.ApiManagement/service/users', parameters('service_apim_rest_name'), '1')]",
                "scope": "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'starter')]",
                "state": "active",
                "allowTracing": false,
                "displayName": "[parameters('subscriptions_677358889a937c006d070001_displayName')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/subscriptions",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apim_rest_name'), '/677358889a937c006d070002')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apim_rest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/users', parameters('service_apim_rest_name'), '1')]",
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'unlimited')]"
            ],
            "properties": {
                "ownerId": "[resourceId('Microsoft.ApiManagement/service/users', parameters('service_apim_rest_name'), '1')]",
                "scope": "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apim_rest_name'), 'unlimited')]",
                "state": "active",
                "allowTracing": false,
                "displayName": "[parameters('subscriptions_677358889a937c006d070002_displayName')]"
            }
        }
    ]
}
