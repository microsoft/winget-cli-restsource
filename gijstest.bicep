{
    "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
    "contentVersion": "1.0.0.0",
    "parameters": {
        "subscriptions_677624190be04c0063070001_displayName": {
            "type": "SecureString"
        },
        "subscriptions_677624190be04c0063070002_displayName": {
            "type": "SecureString"
        },
        "users_1_lastName": {
            "type": "SecureString"
        },
        "storageAccounts_stgijstest_name": {
            "type": "SecureString"
        },
        "sites_funcgijstest_name": {
            "defaultValue": "funcgijstest",
            "type": "String"
        },
        "vaults_kvgijstest_name": {
            "defaultValue": "kvgijstest",
            "type": "String"
        },
        "serverfarms_aspgijstest_name": {
            "defaultValue": "aspgijstest",
            "type": "String"
        },
        "components_appigijstest_name": {
            "defaultValue": "appigijstest",
            "type": "String"
        },
        "service_apimgijstest_name": {
            "defaultValue": "apimgijstest",
            "type": "String"
        },
        "storageAccounts_stgijstest_name_1": {
            "defaultValue": "stgijstest",
            "type": "String"
        },
        "databaseAccounts_cosmosgijstest_name": {
            "defaultValue": "cosmosgijstest",
            "type": "String"
        },
        "workspaces_loggijstest_name": {
            "defaultValue": "loggijstest",
            "type": "String"
        },
        "configurationStores_appcsgijstest_name": {
            "defaultValue": "appcsgijstest",
            "type": "String"
        },
        "smartdetectoralertrules_failure_anomalies___appigijstest_name": {
            "defaultValue": "failure anomalies - appigijstest",
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
            "name": "[parameters('service_apimgijstest_name')]",
            "location": "West Europe",
            "sku": {
                "name": "Basic",
                "capacity": 2
            },
            "identity": {
                "type": "SystemAssigned"
            },
            "properties": {
                "publisherEmail": "gijs.reijn@Rabobank.com",
                "publisherName": "gijs.reijn@Rabobank.com",
                "notificationSenderEmail": "apimgmt-noreply@mail.windowsazure.com",
                "hostnameConfigurations": [
                    {
                        "type": "Proxy",
                        "hostName": "[concat(parameters('service_apimgijstest_name'), '.azure-api.net')]",
                        "negotiateClientCertificate": false,
                        "defaultSslBinding": true,
                        "certificateSource": "BuiltIn"
                    }
                ],
                "customProperties": {
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TripleDes168": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_RSA_WITH_AES_128_CBC_SHA": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_RSA_WITH_AES_256_CBC_SHA": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_RSA_WITH_AES_128_CBC_SHA256": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_RSA_WITH_AES_256_CBC_SHA256": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_RSA_WITH_AES_128_GCM_SHA256": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Tls10": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Tls11": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Ssl30": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Tls10": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Tls11": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Ssl30": "False",
                    "Microsoft.WindowsAzure.ApiManagement.Gateway.Protocols.Server.Http2": "False"
                },
                "virtualNetworkType": "None",
                "certificates": [],
                "disableGateway": false,
                "natGatewayState": "Unsupported",
                "apiVersionConstraint": {
                    "minApiVersion": "2021-08-01"
                },
                "publicNetworkAccess": "Enabled",
                "legacyPortalStatus": "Disabled",
                "developerPortalStatus": "Enabled"
            }
        },
        {
            "type": "Microsoft.AppConfiguration/configurationStores",
            "apiVersion": "2023-09-01-preview",
            "name": "[parameters('configurationStores_appcsgijstest_name')]",
            "location": "westeurope",
            "sku": {
                "name": "standard"
            },
            "properties": {
                "encryption": {},
                "publicNetworkAccess": "Enabled",
                "disableLocalAuth": true,
                "softDeleteRetentionInDays": 7,
                "enablePurgeProtection": true,
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
            "name": "[parameters('databaseAccounts_cosmosgijstest_name')]",
            "location": "West US",
            "kind": "GlobalDocumentDB",
            "identity": {
                "type": "None"
            },
            "properties": {
                "publicNetworkAccess": "Enabled",
                "enableAutomaticFailover": true,
                "enableMultipleWriteLocations": false,
                "isVirtualNetworkFilterEnabled": true,
                "virtualNetworkRules": [],
                "disableKeyBasedMetadataWriteAccess": true,
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
                "minimalTlsVersion": "Tls12",
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
                "networkAclBypassResourceIds": [],
                "capacity": {
                    "totalThroughputLimit": -1
                }
            }
        },
        {
            "type": "Microsoft.KeyVault/vaults",
            "apiVersion": "2024-04-01-preview",
            "name": "[parameters('vaults_kvgijstest_name')]",
            "location": "westeurope",
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
                            "secrets": [
                                "get",
                                "set"
                            ]
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
                "vaultUri": "[concat('https://', parameters('vaults_kvgijstest_name'), '.vault.azure.net/')]",
                "provisioningState": "Succeeded",
                "publicNetworkAccess": "Enabled"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces",
            "apiVersion": "2023-09-01",
            "name": "[parameters('workspaces_loggijstest_name')]",
            "location": "westeurope",
            "properties": {
                "sku": {
                    "name": "PerGB2018"
                },
                "retentionInDays": 365,
                "features": {
                    "legacy": 0,
                    "searchVersion": 1,
                    "enableLogAccessUsingOnlyResourcePermissions": false
                },
                "workspaceCapping": {
                    "dailyQuotaGb": -1
                },
                "publicNetworkAccessForIngestion": "Enabled",
                "publicNetworkAccessForQuery": "Enabled",
                "forceCmkForQuery": true
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts",
            "apiVersion": "2023-05-01",
            "name": "[parameters('storageAccounts_stgijstest_name_1')]",
            "location": "westeurope",
            "sku": {
                "name": "Standard_GRS",
                "tier": "Standard"
            },
            "kind": "StorageV2",
            "properties": {
                "defaultToOAuthAuthentication": false,
                "allowCrossTenantReplication": false,
                "isLocalUserEnabled": false,
                "isSftpEnabled": false,
                "minimumTlsVersion": "TLS1_2",
                "allowBlobPublicAccess": false,
                "allowSharedKeyAccess": false,
                "isHnsEnabled": false,
                "networkAcls": {
                    "bypass": "AzureServices",
                    "virtualNetworkRules": [],
                    "ipRules": [],
                    "defaultAction": "Allow"
                },
                "supportsHttpsTrafficOnly": true,
                "encryption": {
                    "identity": {},
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
                "accessTier": "Hot",
                "customDomain": {
                    "name": "[parameters('storageAccounts_stgijstest_name')]"
                }
            }
        },
        {
            "type": "Microsoft.Web/serverfarms",
            "apiVersion": "2023-12-01",
            "name": "[parameters('serverfarms_aspgijstest_name')]",
            "location": "West Europe",
            "sku": {
                "name": "S1",
                "tier": "Standard",
                "size": "S1",
                "family": "S",
                "capacity": 3
            },
            "kind": "app",
            "properties": {
                "perSiteScaling": false,
                "elasticScaleEnabled": false,
                "maximumElasticWorkerCount": 3,
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
            "name": "[parameters('smartdetectoralertrules_failure_anomalies___appigijstest_name')]",
            "location": "global",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
                    "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/echo-api')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "type": "Microsoft.ApiManagement/service/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/administrators')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/developers')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/guests')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "displayName": "Guests",
                "description": "Guests is a built-in group. Its membership is managed by the system. Unauthenticated users visiting the developer portal fall into this group.",
                "type": "system"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/AccountClosedPublisher')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/BCC')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/NewApplicationNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/NewIssuePublisherNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/PurchasePublisherNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/QuotaLimitApproachingPublisherNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/notifications",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/RequestPublisherNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "value": "<!--\r\n    IMPORTANT:\r\n    - Policy elements can appear only within the <inbound>, <outbound>, <backend> section elements.\r\n    - Only the <forward-request> policy element can appear within the <backend> section element.\r\n    - To apply a policy to the incoming request (before it is forwarded to the backend service), place a corresponding policy element within the <inbound> section element.\r\n    - To apply a policy to the outgoing response (before it is sent back to the caller), place a corresponding policy element within the <outbound> section element.\r\n    - To add a policy position the cursor at the desired insertion point and click on the round button associated with the policy.\r\n    - To remove a policy, delete the corresponding policy statement from the policy document.\r\n    - Policies are applied in the order of their appearance, from the top down.\r\n-->\r\n<policies>\r\n  <inbound />\r\n  <backend>\r\n    <forward-request />\r\n  </backend>\r\n  <outbound />\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/portalconfigs",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/delegation')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/signin')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "enabled": false
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/portalsettings",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/signup')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/starter')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/unlimited')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/master')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "scope": "[concat(resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name')), '/')]",
                "displayName": "Built-in all-access subscription",
                "state": "active",
                "allowTracing": false
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/templates",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/AccountClosedDeveloper')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/ApplicationApprovedNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/ConfirmSignUpIdentityDefault')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/EmailChangeIdentityDefault')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/InviteUserNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/NewCommentNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/NewDeveloperNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/NewIssueNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/PasswordResetByAdminNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/PasswordResetIdentityDefault')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/PurchaseDeveloperNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/QuotaLimitApproachingDeveloperNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/RejectDeveloperNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/RequestDeveloperNotificationMessage')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/1')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "firstName": "Administrator",
                "email": "gijs.reijn@Rabobank.com",
                "state": "active",
                "identities": [
                    {
                        "provider": "Azure",
                        "id": "gijs.reijn@Rabobank.com"
                    }
                ],
                "lastName": "[parameters('users_1_lastName')]"
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlDatabases",
            "apiVersion": "2024-05-15",
            "name": "[concat(parameters('databaseAccounts_cosmosgijstest_name'), '/WinGet')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmosgijstest_name'))]"
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
            "name": "[concat(parameters('databaseAccounts_cosmosgijstest_name'), '/00000000-0000-0000-0000-000000000001')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmosgijstest_name'))]"
            ],
            "properties": {
                "roleName": "Cosmos DB Built-in Data Reader",
                "type": "BuiltInRole",
                "assignableScopes": [
                    "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmosgijstest_name'))]"
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
            "name": "[concat(parameters('databaseAccounts_cosmosgijstest_name'), '/00000000-0000-0000-0000-000000000002')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmosgijstest_name'))]"
            ],
            "properties": {
                "roleName": "Cosmos DB Built-in Data Contributor",
                "type": "BuiltInRole",
                "assignableScopes": [
                    "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmosgijstest_name'))]"
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
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlRoleDefinitions",
            "apiVersion": "2024-05-15",
            "name": "[concat(parameters('databaseAccounts_cosmosgijstest_name'), '/d5427907-1b14-58a2-a5eb-b633c3cfcf9f')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmosgijstest_name'))]"
            ],
            "properties": {
                "roleName": "Reader Writer",
                "type": "CustomRole",
                "assignableScopes": [
                    "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmosgijstest_name'))]"
                ],
                "permissions": [
                    {
                        "dataActions": [
                            "Microsoft.DocumentDB/databaseAccounts/readMetadata",
                            "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/items/*",
                            "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/*"
                        ],
                        "notDataActions": []
                    }
                ]
            }
        },
        {
            "type": "microsoft.insights/components",
            "apiVersion": "2020-02-02",
            "name": "[parameters('components_appigijstest_name')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "kind": "web",
            "properties": {
                "Application_Type": "web",
                "SamplingPercentage": 100,
                "RetentionInDays": 90,
                "DisableIpMasking": true,
                "WorkspaceResourceId": "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]",
                "IngestionMode": "LogAnalytics",
                "publicNetworkAccessForIngestion": "Enabled",
                "publicNetworkAccessForQuery": "Enabled",
                "DisableLocalAuth": false,
                "ForceCustomerStorageForProfiler": false
            }
        },
        {
            "type": "microsoft.insights/components/ProactiveDetectionConfigs",
            "apiVersion": "2018-05-01-preview",
            "name": "[concat(parameters('components_appigijstest_name'), '/degradationindependencyduration')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/degradationinserverresponsetime')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/digestMailConfiguration')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/extension_billingdatavolumedailyspikeextension')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/extension_canaryextension')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/extension_exceptionchangeextension')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/extension_memoryleakextension')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/extension_securityextensionspackage')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/extension_traceseveritydetector')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/longdependencyduration')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/migrationToAlertRulesCompleted')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/slowpageloadtime')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('components_appigijstest_name'), '/slowserverresponsetime')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('microsoft.insights/components', parameters('components_appigijstest_name'))]"
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
            "name": "[concat(parameters('vaults_kvgijstest_name'), '/AppConfigPrimaryEndpoint')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('Microsoft.KeyVault/vaults', parameters('vaults_kvgijstest_name'))]"
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
            "name": "[concat(parameters('vaults_kvgijstest_name'), '/AppConfigSecondaryEndpoint')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('Microsoft.KeyVault/vaults', parameters('vaults_kvgijstest_name'))]"
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
            "name": "[concat(parameters('vaults_kvgijstest_name'), '/CosmosAccountEndpoint')]",
            "location": "westeurope",
            "dependsOn": [
                "[resourceId('Microsoft.KeyVault/vaults', parameters('vaults_kvgijstest_name'))]"
            ],
            "properties": {
                "attributes": {
                    "enabled": true
                }
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_General|AlphabeticallySortedComputers')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "General Exploration",
                "displayName": "All Computers with their most recent data",
                "version": 2,
                "query": "search not(ObjectName == \"Advisor Metrics\" or ObjectName == \"ManagedSpace\") | summarize AggregatedValue = max(TimeGenerated) by Computer | limit 500000 | sort by Computer asc\r\n// Oql: NOT(ObjectName=\"Advisor Metrics\" OR ObjectName=ManagedSpace) | measure max(TimeGenerated) by Computer | top 500000 | Sort Computer // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_General|dataPointsPerManagementGroup')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "General Exploration",
                "displayName": "Which Management Group is generating the most data points?",
                "version": 2,
                "query": "search * | summarize AggregatedValue = count() by ManagementGroupName\r\n// Oql: * | Measure count() by ManagementGroupName // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_General|dataTypeDistribution')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "General Exploration",
                "displayName": "Distribution of data Types",
                "version": 2,
                "query": "search * | extend Type = $table | summarize AggregatedValue = count() by Type\r\n// Oql: * | Measure count() by Type // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_General|StaleComputers')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "General Exploration",
                "displayName": "Stale Computers (data older than 24 hours)",
                "version": 2,
                "query": "search not(ObjectName == \"Advisor Metrics\" or ObjectName == \"ManagedSpace\") | summarize lastdata = max(TimeGenerated) by Computer | limit 500000 | where lastdata < ago(24h)\r\n// Oql: NOT(ObjectName=\"Advisor Metrics\" OR ObjectName=ManagedSpace) | measure max(TimeGenerated) as lastdata by Computer | top 500000 | where lastdata < NOW-24HOURS // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|AllEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "All Events",
                "version": 2,
                "query": "Event | sort by TimeGenerated desc\r\n// Oql: Type=Event // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|AllSyslog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "All Syslogs",
                "version": 2,
                "query": "Syslog | sort by TimeGenerated desc\r\n// Oql: Type=Syslog // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|AllSyslogByFacility')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "All Syslog Records grouped by Facility",
                "version": 2,
                "query": "Syslog | summarize AggregatedValue = count() by Facility\r\n// Oql: Type=Syslog | Measure count() by Facility // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|AllSyslogByProcessName')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "All Syslog Records grouped by ProcessName",
                "version": 2,
                "query": "Syslog | summarize AggregatedValue = count() by ProcessName\r\n// Oql: Type=Syslog | Measure count() by ProcessName // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|AllSyslogsWithErrors')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "All Syslog Records with Errors",
                "version": 2,
                "query": "Syslog | where SeverityLevel == \"error\" | sort by TimeGenerated desc\r\n// Oql: Type=Syslog SeverityLevel=error // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|AverageHTTPRequestTimeByClientIPAddress')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Average HTTP Request time by Client IP Address",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = avg(TimeTaken) by cIP\r\n// Oql: Type=W3CIISLog | Measure Avg(TimeTaken) by cIP // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|AverageHTTPRequestTimeHTTPMethod')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Average HTTP Request time by HTTP Method",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = avg(TimeTaken) by csMethod\r\n// Oql: Type=W3CIISLog | Measure Avg(TimeTaken) by csMethod // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|CountIISLogEntriesClientIPAddress')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Count of IIS Log Entries by Client IP Address",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by cIP\r\n// Oql: Type=W3CIISLog | Measure count() by cIP // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|CountIISLogEntriesHTTPRequestMethod')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Count of IIS Log Entries by HTTP Request Method",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csMethod\r\n// Oql: Type=W3CIISLog | Measure count() by csMethod // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|CountIISLogEntriesHTTPUserAgent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Count of IIS Log Entries by HTTP User Agent",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csUserAgent\r\n// Oql: Type=W3CIISLog | Measure count() by csUserAgent // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|CountOfIISLogEntriesByHostRequestedByClient')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Count of IIS Log Entries by Host requested by client",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csHost\r\n// Oql: Type=W3CIISLog | Measure count() by csHost // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|CountOfIISLogEntriesByURLForHost')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Count of IIS Log Entries by URL for the host \"www.contoso.com\" (replace with your own)",
                "version": 2,
                "query": "search csHost == \"www.contoso.com\" | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csUriStem\r\n// Oql: Type=W3CIISLog csHost=\"www.contoso.com\" | Measure count() by csUriStem // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|CountOfIISLogEntriesByURLRequestedByClient')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Count of IIS Log Entries by URL requested by client (without query strings)",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csUriStem\r\n// Oql: Type=W3CIISLog | Measure count() by csUriStem // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|CountOfWarningEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Count of Events with level \"Warning\" grouped by Event ID",
                "version": 2,
                "query": "Event | where EventLevelName == \"warning\" | summarize AggregatedValue = count() by EventID\r\n// Oql: Type=Event EventLevelName=warning | Measure count() by EventID // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|DisplayBreakdownRespondCodes')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Shows breakdown of response codes",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by scStatus\r\n// Oql: Type=W3CIISLog | Measure count() by scStatus // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|EventsByEventLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Count of Events grouped by Event Log",
                "version": 2,
                "query": "Event | summarize AggregatedValue = count() by EventLog\r\n// Oql: Type=Event | Measure count() by EventLog // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|EventsByEventsID')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Count of Events grouped by Event ID",
                "version": 2,
                "query": "Event | summarize AggregatedValue = count() by EventID\r\n// Oql: Type=Event | Measure count() by EventID // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|EventsByEventSource')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Count of Events grouped by Event Source",
                "version": 2,
                "query": "Event | summarize AggregatedValue = count() by Source\r\n// Oql: Type=Event | Measure count() by Source // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|EventsInOMBetween2000to3000')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Events in the Operations Manager Event Log whose Event ID is in the range between 2000 and 3000",
                "version": 2,
                "query": "Event | where EventLog == \"Operations Manager\" and EventID >= 2000 and EventID <= 3000 | sort by TimeGenerated desc\r\n// Oql: Type=Event EventLog=\"Operations Manager\" EventID:[2000..3000] // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|EventsWithStartedinEventID')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Count of Events containing the word \"started\" grouped by EventID",
                "version": 2,
                "query": "search in (Event) \"started\" | summarize AggregatedValue = count() by EventID\r\n// Oql: Type=Event \"started\" | Measure count() by EventID // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|FindMaximumTimeTakenForEachPage')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Find the maximum time taken for each page",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = max(TimeTaken) by csUriStem\r\n// Oql: Type=W3CIISLog | Measure Max(TimeTaken) by csUriStem // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|IISLogEntriesForClientIP')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "IIS Log Entries for a specific client IP Address (replace with your own)",
                "version": 2,
                "query": "search cIP == \"192.168.0.1\" | extend Type = $table | where Type == W3CIISLog | sort by TimeGenerated desc | project csUriStem, scBytes, csBytes, TimeTaken, scStatus\r\n// Oql: Type=W3CIISLog cIP=\"192.168.0.1\" | Select csUriStem,scBytes,csBytes,TimeTaken,scStatus // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|ListAllIISLogEntries')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "All IIS Log Entries",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | sort by TimeGenerated desc\r\n// Oql: Type=W3CIISLog // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|NoOfConnectionsToOMSDKService')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "How many connections to Operations Manager's SDK service by day",
                "version": 2,
                "query": "Event | where EventID == 26328 and EventLog == \"Operations Manager\" | summarize AggregatedValue = count() by bin(TimeGenerated, 1d) | sort by TimeGenerated desc\r\n// Oql: Type=Event EventID=26328 EventLog=\"Operations Manager\" | Measure count() interval 1DAY // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|ServerRestartTime')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "When did my servers initiate restart?",
                "version": 2,
                "query": "search in (Event) \"shutdown\" and EventLog == \"System\" and Source == \"User32\" and EventID == 1074 | sort by TimeGenerated desc | project TimeGenerated, Computer\r\n// Oql: shutdown Type=Event EventLog=System Source=User32 EventID=1074 | Select TimeGenerated,Computer // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|Show404PagesList')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Shows which pages people are getting a 404 for",
                "version": 2,
                "query": "search scStatus == 404 | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by csUriStem\r\n// Oql: Type=W3CIISLog scStatus=404 | Measure count() by csUriStem // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|ShowServersThrowingInternalServerError')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Shows servers that are throwing internal server error",
                "version": 2,
                "query": "search scStatus == 500 | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = count() by sComputerName\r\n// Oql: Type=W3CIISLog scStatus=500 | Measure count() by sComputerName // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|TotalBytesReceivedByEachAzureRoleInstance')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Total Bytes received by each Azure Role Instance",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = sum(csBytes) by RoleInstance\r\n// Oql: Type=W3CIISLog | Measure Sum(csBytes) by RoleInstance // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|TotalBytesReceivedByEachIISComputer')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Total Bytes received by each IIS Computer",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = sum(csBytes) by Computer | limit 500000\r\n// Oql: Type=W3CIISLog | Measure Sum(csBytes) by Computer | top 500000 // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|TotalBytesRespondedToClientsByClientIPAddress')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Total Bytes responded back to clients by Client IP Address",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = sum(scBytes) by cIP\r\n// Oql: Type=W3CIISLog | Measure Sum(scBytes) by cIP // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|TotalBytesRespondedToClientsByEachIISServerIPAddress')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Total Bytes responded back to clients by each IIS ServerIP Address",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = sum(scBytes) by sIP\r\n// Oql: Type=W3CIISLog | Measure Sum(scBytes) by sIP // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|TotalBytesSentByClientIPAddress')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Total Bytes sent by Client IP Address",
                "version": 2,
                "query": "search * | extend Type = $table | where Type == W3CIISLog | summarize AggregatedValue = sum(csBytes) by cIP\r\n// Oql: Type=W3CIISLog | Measure Sum(csBytes) by cIP // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PEF: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|WarningEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "All Events with level \"Warning\"",
                "version": 2,
                "query": "Event | where EventLevelName == \"warning\" | sort by TimeGenerated desc\r\n// Oql: Type=Event EventLevelName=warning // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|WindowsFireawallPolicySettingsChanged')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "Windows Firewall Policy settings have changed",
                "version": 2,
                "query": "Event | where EventLog == \"Microsoft-Windows-Windows Firewall With Advanced Security/Firewall\" and EventID == 2008 | sort by TimeGenerated desc\r\n// Oql: Type=Event EventLog=\"Microsoft-Windows-Windows Firewall With Advanced Security/Firewall\" EventID=2008 // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/savedSearches",
            "apiVersion": "2020-08-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogManagement(', parameters('workspaces_loggijstest_name'), ')_LogManagement|WindowsFireawallPolicySettingsChangedByMachines')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "category": "Log Management",
                "displayName": "On which machines and how many times have Windows Firewall Policy settings changed",
                "version": 2,
                "query": "Event | where EventLog == \"Microsoft-Windows-Windows Firewall With Advanced Security/Firewall\" and EventID == 2008 | summarize AggregatedValue = count() by Computer | limit 500000\r\n// Oql: Type=Event EventLog=\"Microsoft-Windows-Windows Firewall With Advanced Security/Firewall\" EventID=2008 | measure count() by Computer | top 500000 // Args: {OQ: True; WorkspaceId: 00000000-0000-0000-0000-000000000000} // Settings: {PTT: True; SortI: True; SortF: True} // Version: 0.1.122"
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AACAudit')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AACAudit"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AACHttpRequest')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AACHttpRequest"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADB2CRequestLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADB2CRequestLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADCustomSecurityAttributeAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADCustomSecurityAttributeAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADDomainServicesAccountLogon')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADDomainServicesAccountLogon"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADDomainServicesAccountManagement')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADDomainServicesAccountManagement"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADDomainServicesDirectoryServiceAccess')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADDomainServicesDirectoryServiceAccess"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADDomainServicesDNSAuditsDynamicUpdates')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADDomainServicesDNSAuditsDynamicUpdates"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADDomainServicesDNSAuditsGeneral')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADDomainServicesDNSAuditsGeneral"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADDomainServicesLogonLogoff')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADDomainServicesLogonLogoff"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADDomainServicesPolicyChange')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADDomainServicesPolicyChange"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADDomainServicesPrivilegeUse')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADDomainServicesPrivilegeUse"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADDomainServicesSystemSecurity')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADDomainServicesSystemSecurity"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADManagedIdentitySignInLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADManagedIdentitySignInLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADNonInteractiveUserSignInLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADNonInteractiveUserSignInLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADProvisioningLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADProvisioningLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADRiskyServicePrincipals')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADRiskyServicePrincipals"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADRiskyUsers')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADRiskyUsers"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADServicePrincipalRiskEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADServicePrincipalRiskEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADServicePrincipalSignInLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADServicePrincipalSignInLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AADUserRiskEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AADUserRiskEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ABSBotRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ABSBotRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ABSChannelToBotRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ABSChannelToBotRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ABSDependenciesRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ABSDependenciesRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACICollaborationAudit')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACICollaborationAudit"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACRConnectedClientList')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACRConnectedClientList"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACREntraAuthenticationAuditLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACREntraAuthenticationAuditLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSAdvancedMessagingOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSAdvancedMessagingOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSAuthIncomingOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSAuthIncomingOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSBillingUsage')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSBillingUsage"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallAutomationIncomingOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallAutomationIncomingOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallAutomationMediaSummary')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallAutomationMediaSummary"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallClientMediaStatsTimeSeries')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallClientMediaStatsTimeSeries"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallClientOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallClientOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallClosedCaptionsSummary')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallClosedCaptionsSummary"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallDiagnostics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallDiagnostics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallDiagnosticsUpdates')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallDiagnosticsUpdates"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallRecordingIncomingOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallRecordingIncomingOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallRecordingSummary')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallRecordingSummary"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallSummary')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallSummary"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallSummaryUpdates')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallSummaryUpdates"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSCallSurvey')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSCallSurvey"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSChatIncomingOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSChatIncomingOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSEmailSendMailOperational')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSEmailSendMailOperational"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSEmailStatusUpdateOperational')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSEmailStatusUpdateOperational"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSEmailUserEngagementOperational')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSEmailUserEngagementOperational"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSJobRouterIncomingOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSJobRouterIncomingOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSRoomsIncomingOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSRoomsIncomingOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ACSSMSIncomingOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ACSSMSIncomingOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AddonAzureBackupAlerts')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AddonAzureBackupAlerts"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AddonAzureBackupJobs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AddonAzureBackupJobs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AddonAzureBackupPolicy')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AddonAzureBackupPolicy"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AddonAzureBackupProtectedInstance')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AddonAzureBackupProtectedInstance"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AddonAzureBackupStorage')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AddonAzureBackupStorage"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFActivityRun')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFActivityRun"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFAirflowSchedulerLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFAirflowSchedulerLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFAirflowTaskLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFAirflowTaskLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFAirflowWebLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFAirflowWebLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFAirflowWorkerLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFAirflowWorkerLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFPipelineRun')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFPipelineRun"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFSandboxActivityRun')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFSandboxActivityRun"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFSandboxPipelineRun')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFSandboxPipelineRun"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFSSignInLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFSSignInLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFSSISIntegrationRuntimeLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFSSISIntegrationRuntimeLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFSSISPackageEventMessageContext')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFSSISPackageEventMessageContext"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFSSISPackageEventMessages')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFSSISPackageEventMessages"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFSSISPackageExecutableStatistics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFSSISPackageExecutableStatistics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFSSISPackageExecutionComponentPhases')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFSSISPackageExecutionComponentPhases"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFSSISPackageExecutionDataStatistics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFSSISPackageExecutionDataStatistics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADFTriggerRun')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADFTriggerRun"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADReplicationResult')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADReplicationResult"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADSecurityAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADSecurityAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADTDataHistoryOperation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADTDataHistoryOperation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADTDigitalTwinsOperation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADTDigitalTwinsOperation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADTEventRoutesOperation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADTEventRoutesOperation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADTModelsOperation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADTModelsOperation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADTQueryOperation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADTQueryOperation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADXCommand')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADXCommand"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADXDataOperation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADXDataOperation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADXIngestionBatching')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADXIngestionBatching"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADXJournal')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADXJournal"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADXQuery')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADXQuery"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADXTableDetails')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADXTableDetails"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ADXTableUsageStatistics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ADXTableUsageStatistics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AegDataPlaneRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AegDataPlaneRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AegDeliveryFailureLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AegDeliveryFailureLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AegPublishFailureLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AegPublishFailureLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AEWAssignmentBlobLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AEWAssignmentBlobLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AEWAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AEWAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AEWComputePipelinesLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AEWComputePipelinesLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AEWExperimentAssignmentSummary')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AEWExperimentAssignmentSummary"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AEWExperimentScorecardMetricPairs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AEWExperimentScorecardMetricPairs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AEWExperimentScorecards')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AEWExperimentScorecards"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AFSAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AFSAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AGCAccessLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AGCAccessLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AgriFoodApplicationAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AgriFoodApplicationAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AgriFoodFarmManagementLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AgriFoodFarmManagementLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AgriFoodFarmOperationLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AgriFoodFarmOperationLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AgriFoodInsightLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AgriFoodInsightLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AgriFoodJobProcessedLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AgriFoodJobProcessedLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AgriFoodModelInferenceLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AgriFoodModelInferenceLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AgriFoodProviderAuthLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AgriFoodProviderAuthLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AgriFoodSatelliteLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AgriFoodSatelliteLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AgriFoodSensorManagementLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AgriFoodSensorManagementLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AgriFoodWeatherLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AgriFoodWeatherLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AGSGrafanaLoginEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AGSGrafanaLoginEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AGSGrafanaUsageInsightsEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AGSGrafanaUsageInsightsEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AGWAccessLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AGWAccessLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AGWFirewallLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AGWFirewallLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AGWPerformanceLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AGWPerformanceLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AHDSDeidAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AHDSDeidAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AHDSDicomAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AHDSDicomAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AHDSDicomDiagnosticLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AHDSDicomDiagnosticLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AHDSMedTechDiagnosticLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AHDSMedTechDiagnosticLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AirflowDagProcessingLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AirflowDagProcessingLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AKSAudit')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AKSAudit"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AKSAuditAdmin')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AKSAuditAdmin"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AKSControlPlane')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AKSControlPlane"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ALBHealthEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ALBHealthEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/Alert')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "Alert"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlComputeClusterEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlComputeClusterEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlComputeClusterNodeEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlComputeClusterNodeEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlComputeCpuGpuUtilization')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlComputeCpuGpuUtilization"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlComputeInstanceEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlComputeInstanceEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlComputeJobEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlComputeJobEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlDataLabelEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlDataLabelEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlDataSetEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlDataSetEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlDataStoreEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlDataStoreEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlDeploymentEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlDeploymentEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlEnvironmentEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlEnvironmentEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlInferencingEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlInferencingEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlModelsEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlModelsEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlOnlineEndpointConsoleLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlOnlineEndpointConsoleLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlOnlineEndpointEventLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlOnlineEndpointEventLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlOnlineEndpointTrafficLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlOnlineEndpointTrafficLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlPipelineEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlPipelineEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlRegistryReadEventsLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlRegistryReadEventsLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlRegistryWriteEventsLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlRegistryWriteEventsLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlRunEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlRunEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AmlRunStatusChangedEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AmlRunStatusChangedEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AMSKeyDeliveryRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AMSKeyDeliveryRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AMSLiveEventOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AMSLiveEventOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AMSMediaAccountHealth')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AMSMediaAccountHealth"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AMSStreamingEndpointRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AMSStreamingEndpointRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AMWMetricsUsageDetails')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AMWMetricsUsageDetails"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ANFFileAccess')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ANFFileAccess"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AOIDatabaseQuery')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AOIDatabaseQuery"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AOIDigestion')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AOIDigestion"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AOIStorage')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AOIStorage"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ApiManagementGatewayLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ApiManagementGatewayLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ApiManagementWebSocketConnectionLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ApiManagementWebSocketConnectionLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/APIMDevPortalAuditDiagnosticLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "APIMDevPortalAuditDiagnosticLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppAvailabilityResults')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppAvailabilityResults"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppBrowserTimings')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppBrowserTimings"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppCenterError')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppCenterError"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppDependencies')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppDependencies"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppEnvSpringAppConsoleLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppEnvSpringAppConsoleLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppExceptions')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppExceptions"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppMetrics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppMetrics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppPageViews')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppPageViews"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppPerformanceCounters')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppPerformanceCounters"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppPlatformBuildLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppPlatformBuildLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppPlatformContainerEventLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppPlatformContainerEventLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppPlatformIngressLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppPlatformIngressLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppPlatformLogsforSpring')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppPlatformLogsforSpring"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppPlatformSystemLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppPlatformSystemLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppServiceAntivirusScanAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppServiceAntivirusScanAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppServiceAppLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppServiceAppLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppServiceAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppServiceAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppServiceAuthenticationLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppServiceAuthenticationLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppServiceConsoleLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppServiceConsoleLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppServiceEnvironmentPlatformLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppServiceEnvironmentPlatformLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppServiceFileAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppServiceFileAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppServiceHTTPLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppServiceHTTPLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppServiceIPSecAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppServiceIPSecAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppServicePlatformLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppServicePlatformLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppServiceServerlessSecurityPluginData')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppServiceServerlessSecurityPluginData"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppSystemEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppSystemEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AppTraces')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AppTraces"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ArcK8sAudit')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ArcK8sAudit"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ArcK8sAuditAdmin')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ArcK8sAuditAdmin"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ArcK8sControlPlane')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ArcK8sControlPlane"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ASCAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ASCAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ASCDeviceEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ASCDeviceEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ASRJobs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ASRJobs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ASRReplicatedItems')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ASRReplicatedItems"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ATCExpressRouteCircuitIpfix')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ATCExpressRouteCircuitIpfix"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ATCPrivatePeeringMetadata')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ATCPrivatePeeringMetadata"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AUIEventsAudit')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AUIEventsAudit"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AUIEventsOperational')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AUIEventsOperational"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AutoscaleEvaluationsLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AutoscaleEvaluationsLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AutoscaleScaleActionsLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AutoscaleScaleActionsLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AVNMConnectivityConfigurationChange')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AVNMConnectivityConfigurationChange"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AVNMIPAMPoolAllocationChange')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AVNMIPAMPoolAllocationChange"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AVNMNetworkGroupMembershipChange')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AVNMNetworkGroupMembershipChange"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AVNMRuleCollectionChange')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AVNMRuleCollectionChange"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AVSSyslog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AVSSyslog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWApplicationRule')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWApplicationRule"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWApplicationRuleAggregation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWApplicationRuleAggregation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWDnsQuery')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWDnsQuery"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWFatFlow')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWFatFlow"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWFlowTrace')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWFlowTrace"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWIdpsSignature')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWIdpsSignature"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWInternalFqdnResolutionFailure')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWInternalFqdnResolutionFailure"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWNatRule')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWNatRule"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWNatRuleAggregation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWNatRuleAggregation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWNetworkRule')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWNetworkRule"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWNetworkRuleAggregation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWNetworkRuleAggregation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZFWThreatIntel')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZFWThreatIntel"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZKVAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZKVAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZKVPolicyEvaluationDetailsLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZKVPolicyEvaluationDetailsLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZMSApplicationMetricLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZMSApplicationMetricLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZMSArchiveLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZMSArchiveLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZMSAutoscaleLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZMSAutoscaleLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZMSCustomerManagedKeyUserLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZMSCustomerManagedKeyUserLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZMSDiagnosticErrorLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZMSDiagnosticErrorLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZMSHybridConnectionsEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZMSHybridConnectionsEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZMSKafkaCoordinatorLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZMSKafkaCoordinatorLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZMSKafkaUserErrorLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZMSKafkaUserErrorLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZMSOperationalLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZMSOperationalLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZMSRunTimeAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZMSRunTimeAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AZMSVnetConnectionEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AZMSVnetConnectionEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AzureActivity')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AzureActivity"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AzureActivityV2')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AzureActivityV2"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AzureAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AzureAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AzureAttestationDiagnostics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AzureAttestationDiagnostics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AzureBackupOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AzureBackupOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AzureCloudHsmAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AzureCloudHsmAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AzureDevOpsAuditing')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AzureDevOpsAuditing"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AzureLoadTestingOperation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AzureLoadTestingOperation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AzureMetrics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AzureMetrics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/AzureMetricsV2')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "AzureMetricsV2"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/BaiClusterEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "BaiClusterEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/BaiClusterNodeEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "BaiClusterNodeEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/BaiJobEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "BaiJobEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/BlockchainApplicationLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "BlockchainApplicationLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/BlockchainProxyLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "BlockchainProxyLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CassandraAudit')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CassandraAudit"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CassandraLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CassandraLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CCFApplicationLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CCFApplicationLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CDBCassandraRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CDBCassandraRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CDBControlPlaneRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CDBControlPlaneRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CDBDataPlaneRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CDBDataPlaneRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CDBGremlinRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CDBGremlinRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CDBMongoRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CDBMongoRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CDBPartitionKeyRUConsumption')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CDBPartitionKeyRUConsumption"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CDBPartitionKeyStatistics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CDBPartitionKeyStatistics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CDBQueryRuntimeStatistics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CDBQueryRuntimeStatistics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CDBTableApiRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CDBTableApiRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ChaosStudioExperimentEventLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ChaosStudioExperimentEventLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CHSMManagementAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CHSMManagementAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CHSMServiceOperationAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CHSMServiceOperationAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CIEventsAudit')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CIEventsAudit"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CIEventsOperational')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CIEventsOperational"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CloudHsmServiceOperationAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CloudHsmServiceOperationAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ComputerGroup')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ComputerGroup"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerAppConsoleLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerAppConsoleLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerAppSystemLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerAppSystemLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerImageInventory')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerImageInventory"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerInstanceLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerInstanceLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerInventory')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerInventory"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerLogV2')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerLogV2"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerNodeInventory')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerNodeInventory"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerRegistryLoginEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerRegistryLoginEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerRegistryRepositoryEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerRegistryRepositoryEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ContainerServiceLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ContainerServiceLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/CoreAzureBackup')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "CoreAzureBackup"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksAccounts')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksAccounts"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksApps')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksApps"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksBrickStoreHttpGateway')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksBrickStoreHttpGateway"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksBudgetPolicyCentral')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksBudgetPolicyCentral"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksCapsule8Dataplane')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksCapsule8Dataplane"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksClamAVScan')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksClamAVScan"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksCloudStorageMetadata')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksCloudStorageMetadata"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksClusterLibraries')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksClusterLibraries"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksClusterPolicies')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksClusterPolicies"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksClusters')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksClusters"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksDashboards')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksDashboards"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksDatabricksSQL')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksDatabricksSQL"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksDataMonitoring')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksDataMonitoring"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksDataRooms')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksDataRooms"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksDBFS')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksDBFS"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksDeltaPipelines')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksDeltaPipelines"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksFeatureStore')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksFeatureStore"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksFiles')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksFiles"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksFilesystem')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksFilesystem"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksGenie')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksGenie"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksGitCredentials')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksGitCredentials"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksGlobalInitScripts')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksGlobalInitScripts"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksGroups')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksGroups"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksIAMRole')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksIAMRole"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksIngestion')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksIngestion"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksInstancePools')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksInstancePools"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksJobs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksJobs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksLakeviewConfig')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksLakeviewConfig"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksLineageTracking')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksLineageTracking"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksMarketplaceConsumer')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksMarketplaceConsumer"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksMarketplaceProvider')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksMarketplaceProvider"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksMLflowAcledArtifact')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksMLflowAcledArtifact"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksMLflowExperiment')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksMLflowExperiment"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksModelRegistry')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksModelRegistry"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksNotebook')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksNotebook"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksOnlineTables')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksOnlineTables"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksPartnerHub')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksPartnerHub"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksPredictiveOptimization')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksPredictiveOptimization"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksRBAC')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksRBAC"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksRemoteHistoryService')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksRemoteHistoryService"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksRepos')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksRepos"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksRFA')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksRFA"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksSecrets')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksSecrets"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksServerlessRealTimeInference')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksServerlessRealTimeInference"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksSQL')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksSQL"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksSQLPermissions')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksSQLPermissions"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksSSH')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksSSH"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksTables')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksTables"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksUnityCatalog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksUnityCatalog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksVectorSearch')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksVectorSearch"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksWebhookNotifications')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksWebhookNotifications"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksWebTerminal')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksWebTerminal"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksWorkspace')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksWorkspace"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DatabricksWorkspaceFiles')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DatabricksWorkspaceFiles"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DataTransferOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DataTransferOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DCRLogErrors')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DCRLogErrors"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DCRLogTroubleshooting')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DCRLogTroubleshooting"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DevCenterBillingEventLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DevCenterBillingEventLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DevCenterDiagnosticLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DevCenterDiagnosticLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DevCenterResourceOperationLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DevCenterResourceOperationLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DNSQueryLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DNSQueryLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DSMAzureBlobStorageLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DSMAzureBlobStorageLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DSMDataClassificationLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DSMDataClassificationLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/DSMDataLabelingLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "DSMDataLabelingLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/EGNFailedHttpDataPlaneOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "EGNFailedHttpDataPlaneOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/EGNFailedMqttConnections')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "EGNFailedMqttConnections"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/EGNFailedMqttPublishedMessages')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "EGNFailedMqttPublishedMessages"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/EGNFailedMqttSubscriptions')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "EGNFailedMqttSubscriptions"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/EGNMqttDisconnections')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "EGNMqttDisconnections"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/EGNSuccessfulHttpDataPlaneOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "EGNSuccessfulHttpDataPlaneOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/EGNSuccessfulMqttConnections')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "EGNSuccessfulMqttConnections"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/EnrichedMicrosoft365AuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "EnrichedMicrosoft365AuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ETWEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ETWEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/Event')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "Event"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ExchangeAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ExchangeAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ExchangeOnlineAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ExchangeOnlineAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/FailedIngestion')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "FailedIngestion"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/FunctionAppLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "FunctionAppLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightAmbariClusterAlerts')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightAmbariClusterAlerts"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightAmbariSystemMetrics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightAmbariSystemMetrics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightGatewayAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightGatewayAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightHadoopAndYarnLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightHadoopAndYarnLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightHadoopAndYarnMetrics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightHadoopAndYarnMetrics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightHBaseLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightHBaseLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightHBaseMetrics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightHBaseMetrics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightHiveAndLLAPLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightHiveAndLLAPLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightHiveAndLLAPMetrics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightHiveAndLLAPMetrics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightHiveQueryAppStats')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightHiveQueryAppStats"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightHiveTezAppStats')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightHiveTezAppStats"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightJupyterNotebookEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightJupyterNotebookEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightKafkaLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightKafkaLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightKafkaMetrics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightKafkaMetrics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightKafkaServerLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightKafkaServerLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightOozieLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightOozieLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightRangerAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightRangerAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSecurityLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSecurityLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSparkApplicationEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSparkApplicationEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSparkBlockManagerEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSparkBlockManagerEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSparkEnvironmentEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSparkEnvironmentEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSparkExecutorEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSparkExecutorEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSparkExtraEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSparkExtraEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSparkJobEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSparkJobEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSparkLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSparkLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSparkSQLExecutionEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSparkSQLExecutionEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSparkStageEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSparkStageEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSparkStageTaskAccumulables')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSparkStageTaskAccumulables"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightSparkTaskEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightSparkTaskEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightStormLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightStormLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightStormMetrics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightStormMetrics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HDInsightStormTopologyMetrics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HDInsightStormTopologyMetrics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/HealthStateChangeEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "HealthStateChangeEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/Heartbeat')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "Heartbeat"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/InsightsMetrics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "InsightsMetrics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/IntuneAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "IntuneAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/IntuneDeviceComplianceOrg')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "IntuneDeviceComplianceOrg"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/IntuneDevices')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "IntuneDevices"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/IntuneOperationalLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "IntuneOperationalLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/KubeEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "KubeEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/KubeHealth')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "KubeHealth"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/KubeMonAgentEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "KubeMonAgentEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/KubeNodeInventory')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "KubeNodeInventory"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/KubePodInventory')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "KubePodInventory"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/KubePVInventory')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "KubePVInventory"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/KubeServices')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "KubeServices"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LAQueryLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "LAQueryLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LASummaryLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "LASummaryLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/LogicAppWorkflowRuntime')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "LogicAppWorkflowRuntime"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MCCEventLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MCCEventLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MCVPAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MCVPAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MCVPOperationLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MCVPOperationLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MDCDetectionDNSEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MDCDetectionDNSEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MDCDetectionFimEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MDCDetectionFimEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MDCDetectionGatingValidationEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MDCDetectionGatingValidationEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MDCFileIntegrityMonitoringEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MDCFileIntegrityMonitoringEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MDECustomCollectionDeviceFileEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MDECustomCollectionDeviceFileEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MicrosoftAzureBastionAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MicrosoftAzureBastionAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MicrosoftDataShareReceivedSnapshotLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MicrosoftDataShareReceivedSnapshotLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MicrosoftDataShareSentSnapshotLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MicrosoftDataShareSentSnapshotLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MicrosoftDataShareShareLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MicrosoftDataShareShareLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MicrosoftDynamicsTelemetryPerformanceLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MicrosoftDynamicsTelemetryPerformanceLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MicrosoftDynamicsTelemetrySystemMetricsLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MicrosoftDynamicsTelemetrySystemMetricsLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MicrosoftGraphActivityLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MicrosoftGraphActivityLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MicrosoftHealthcareApisAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MicrosoftHealthcareApisAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MNFDeviceUpdates')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MNFDeviceUpdates"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MNFSystemSessionHistoryUpdates')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MNFSystemSessionHistoryUpdates"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/MNFSystemStateMessageUpdates')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "MNFSystemStateMessageUpdates"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NCBMBreakGlassAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NCBMBreakGlassAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NCBMSecurityDefenderLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NCBMSecurityDefenderLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NCBMSecurityLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NCBMSecurityLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NCBMSystemLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NCBMSystemLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NCCKubernetesLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NCCKubernetesLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NCCPlatformOperationsLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NCCPlatformOperationsLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NCCVMOrchestrationLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NCCVMOrchestrationLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NCMClusterOperationsLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NCMClusterOperationsLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NCSStorageAlerts')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NCSStorageAlerts"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NCSStorageAudits')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NCSStorageAudits"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NCSStorageLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NCSStorageLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NetworkAccessAlerts')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NetworkAccessAlerts"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NetworkAccessTraffic')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NetworkAccessTraffic"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NGXOperationLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NGXOperationLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NGXSecurityLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NGXSecurityLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NSPAccessLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NSPAccessLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NTAInsights')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NTAInsights"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NTAIpDetails')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NTAIpDetails"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NTANetAnalytics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NTANetAnalytics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NTATopologyDetails')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NTATopologyDetails"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NWConnectionMonitorDestinationListenerResult')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NWConnectionMonitorDestinationListenerResult"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NWConnectionMonitorDNSResult')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NWConnectionMonitorDNSResult"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NWConnectionMonitorPathResult')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NWConnectionMonitorPathResult"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/NWConnectionMonitorTestResult')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "NWConnectionMonitorTestResult"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/OEPAirFlowTask')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "OEPAirFlowTask"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/OEPAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "OEPAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/OEPDataplaneLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "OEPDataplaneLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/OEPElasticOperator')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "OEPElasticOperator"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/OEPElasticsearch')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "OEPElasticsearch"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/OLPSupplyChainEntityOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "OLPSupplyChainEntityOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/OLPSupplyChainEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "OLPSupplyChainEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/Operation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "Operation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/Perf')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "Perf"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/PFTitleAuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "PFTitleAuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/PowerBIAuditTenant')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "PowerBIAuditTenant"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/PowerBIDatasetsTenant')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "PowerBIDatasetsTenant"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/PowerBIDatasetsTenantPreview')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "PowerBIDatasetsTenantPreview"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/PowerBIDatasetsWorkspace')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "PowerBIDatasetsWorkspace"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/PowerBIDatasetsWorkspacePreview')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "PowerBIDatasetsWorkspacePreview"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/PowerBIReportUsageTenant')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "PowerBIReportUsageTenant"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/PowerBIReportUsageWorkspace')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "PowerBIReportUsageWorkspace"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/PurviewDataSensitivityLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "PurviewDataSensitivityLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/PurviewScanStatusLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "PurviewScanStatusLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/PurviewSecurityLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "PurviewSecurityLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/REDConnectionEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "REDConnectionEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/RemoteNetworkHealthLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "RemoteNetworkHealthLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ResourceManagementPublicAccessLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ResourceManagementPublicAccessLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SCCMAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SCCMAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SCOMAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SCOMAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ServiceFabricOperationalEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ServiceFabricOperationalEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ServiceFabricReliableActorEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ServiceFabricReliableActorEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/ServiceFabricReliableServiceEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "ServiceFabricReliableServiceEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SfBAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SfBAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SfBOnlineAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SfBOnlineAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SharePointOnlineAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SharePointOnlineAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SignalRServiceDiagnosticLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SignalRServiceDiagnosticLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SigninLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SigninLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SPAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SPAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SQLAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SQLAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SQLSecurityAuditEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SQLSecurityAuditEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageAntimalwareScanResults')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageAntimalwareScanResults"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageBlobLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageBlobLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageCacheOperationEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageCacheOperationEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageCacheUpgradeEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageCacheUpgradeEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageCacheWarningEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageCacheWarningEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageFileLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageFileLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageMalwareScanningResults')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageMalwareScanningResults"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageMoverCopyLogsFailed')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageMoverCopyLogsFailed"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageMoverCopyLogsTransferred')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageMoverCopyLogsTransferred"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageMoverJobRunLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageMoverJobRunLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageQueueLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageQueueLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/StorageTableLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "StorageTableLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SucceededIngestion')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SucceededIngestion"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseBigDataPoolApplicationsEnded')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseBigDataPoolApplicationsEnded"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseBuiltinSqlPoolRequestsEnded')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseBuiltinSqlPoolRequestsEnded"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseDXCommand')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseDXCommand"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseDXFailedIngestion')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseDXFailedIngestion"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseDXIngestionBatching')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseDXIngestionBatching"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseDXQuery')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseDXQuery"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseDXSucceededIngestion')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseDXSucceededIngestion"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseDXTableDetails')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseDXTableDetails"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseDXTableUsageStatistics')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseDXTableUsageStatistics"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseGatewayApiRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseGatewayApiRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseGatewayEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseGatewayEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseIntegrationActivityRuns')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseIntegrationActivityRuns"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseIntegrationActivityRunsEnded')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseIntegrationActivityRunsEnded"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseIntegrationPipelineRuns')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseIntegrationPipelineRuns"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseIntegrationPipelineRunsEnded')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseIntegrationPipelineRunsEnded"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseIntegrationTriggerRuns')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseIntegrationTriggerRuns"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseIntegrationTriggerRunsEnded')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseIntegrationTriggerRunsEnded"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseLinkEvent')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseLinkEvent"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseRBACEvents')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseRBACEvents"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseRbacOperations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseRbacOperations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseScopePoolScopeJobsEnded')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseScopePoolScopeJobsEnded"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseScopePoolScopeJobsStateChange')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseScopePoolScopeJobsStateChange"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseSqlPoolDmsWorkers')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseSqlPoolDmsWorkers"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseSqlPoolExecRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseSqlPoolExecRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseSqlPoolRequestSteps')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseSqlPoolRequestSteps"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseSqlPoolSqlRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseSqlPoolSqlRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/SynapseSqlPoolWaits')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "SynapseSqlPoolWaits"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/Syslog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "Syslog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/TSIIngress')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "TSIIngress"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/UCClient')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "UCClient"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/UCClientReadinessStatus')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "UCClientReadinessStatus"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/UCClientUpdateStatus')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "UCClientUpdateStatus"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/UCDeviceAlert')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "UCDeviceAlert"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/UCDOAggregatedStatus')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "UCDOAggregatedStatus"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/UCDOStatus')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "UCDOStatus"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/UCServiceUpdateStatus')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "UCServiceUpdateStatus"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/UCUpdateAlert')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "UCUpdateAlert"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/Usage')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "Usage"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/VCoreMongoRequests')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "VCoreMongoRequests"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/VIAudit')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "VIAudit"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/VIIndexing')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "VIIndexing"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/VMBoundPort')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "VMBoundPort"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/VMComputer')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "VMComputer"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/VMConnection')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "VMConnection"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/VMProcess')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "VMProcess"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/W3CIISLog')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "W3CIISLog"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WebPubSubConnectivity')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WebPubSubConnectivity"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WebPubSubHttpRequest')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WebPubSubHttpRequest"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WebPubSubMessaging')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WebPubSubMessaging"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/Windows365AuditLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "Windows365AuditLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WindowsClientAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WindowsClientAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WindowsServerAssessmentRecommendation')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WindowsServerAssessmentRecommendation"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WorkloadDiagnosticLogs')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WorkloadDiagnosticLogs"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WVDAgentHealthStatus')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WVDAgentHealthStatus"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WVDAutoscaleEvaluationPooled')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WVDAutoscaleEvaluationPooled"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WVDCheckpoints')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WVDCheckpoints"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WVDConnectionGraphicsDataPreview')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WVDConnectionGraphicsDataPreview"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WVDConnectionNetworkData')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WVDConnectionNetworkData"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WVDConnections')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WVDConnections"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WVDErrors')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WVDErrors"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WVDFeeds')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WVDFeeds"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WVDHostRegistrations')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WVDHostRegistrations"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WVDManagement')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WVDManagement"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.OperationalInsights/workspaces/tables",
            "apiVersion": "2022-10-01",
            "name": "[concat(parameters('workspaces_loggijstest_name'), '/WVDSessionHostManagement')]",
            "dependsOn": [
                "[resourceId('Microsoft.OperationalInsights/workspaces', parameters('workspaces_loggijstest_name'))]"
            ],
            "properties": {
                "totalRetentionInDays": 365,
                "plan": "Analytics",
                "schema": {
                    "name": "WVDSessionHostManagement"
                },
                "retentionInDays": 365
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/blobServices",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_stgijstest_name_1'), '/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_stgijstest_name_1'))]"
            ],
            "sku": {
                "name": "Standard_GRS",
                "tier": "Standard"
            },
            "properties": {
                "containerDeleteRetentionPolicy": {
                    "enabled": false
                },
                "cors": {
                    "corsRules": []
                },
                "deleteRetentionPolicy": {
                    "allowPermanentDelete": false,
                    "enabled": true,
                    "days": 7
                },
                "isVersioningEnabled": false
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/fileServices",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_stgijstest_name_1'), '/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_stgijstest_name_1'))]"
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
            "name": "[concat(parameters('storageAccounts_stgijstest_name_1'), '/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_stgijstest_name_1'))]"
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
            "name": "[concat(parameters('storageAccounts_stgijstest_name_1'), '/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_stgijstest_name_1'))]"
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
            "name": "[parameters('sites_funcgijstest_name')]",
            "location": "West Europe",
            "dependsOn": [
                "[resourceId('Microsoft.Web/serverfarms', parameters('serverfarms_aspgijstest_name'))]"
            ],
            "kind": "functionapp",
            "identity": {
                "type": "SystemAssigned"
            },
            "properties": {
                "enabled": true,
                "hostNameSslStates": [
                    {
                        "name": "[concat(parameters('sites_funcgijstest_name'), '.azurewebsites.net')]",
                        "sslState": "Disabled",
                        "hostType": "Standard"
                    },
                    {
                        "name": "[concat(parameters('sites_funcgijstest_name'), '.scm.azurewebsites.net')]",
                        "sslState": "Disabled",
                        "hostType": "Repository"
                    }
                ],
                "serverFarmId": "[resourceId('Microsoft.Web/serverfarms', parameters('serverfarms_aspgijstest_name'))]",
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
                "clientCertMode": "Optional",
                "hostNamesDisabled": false,
                "vnetBackupRestoreEnabled": false,
                "customDomainVerificationId": "3AE4B0D96270F9FD6245978C3E9070E9480B095802237E88AE0DBC7F011F6387",
                "containerSize": 1536,
                "dailyMemoryTimeQuota": 0,
                "httpsOnly": true,
                "redundancyMode": "None",
                "publicNetworkAccess": "Enabled",
                "storageAccountRequired": false,
                "keyVaultReferenceIdentity": "SystemAssigned"
            }
        },
        {
            "type": "Microsoft.Web/sites/basicPublishingCredentialsPolicies",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_funcgijstest_name'), '/ftp')]",
            "location": "West Europe",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_funcgijstest_name'))]"
            ],
            "properties": {
                "allow": true
            }
        },
        {
            "type": "Microsoft.Web/sites/basicPublishingCredentialsPolicies",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_funcgijstest_name'), '/scm')]",
            "location": "West Europe",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_funcgijstest_name'))]"
            ],
            "properties": {
                "allow": true
            }
        },
        {
            "type": "Microsoft.Web/sites/config",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_funcgijstest_name'), '/web')]",
            "location": "West Europe",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_funcgijstest_name'))]"
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
                "publishingUsername": "$funcgijstest",
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
                "publicNetworkAccess": "Enabled",
                "localMySqlEnabled": false,
                "managedServiceIdentityId": 16195,
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
            "type": "Microsoft.Web/sites/hostNameBindings",
            "apiVersion": "2023-12-01",
            "name": "[concat(parameters('sites_funcgijstest_name'), '/', parameters('sites_funcgijstest_name'), '.azurewebsites.net')]",
            "location": "West Europe",
            "dependsOn": [
                "[resourceId('Microsoft.Web/sites', parameters('sites_funcgijstest_name'))]"
            ],
            "properties": {
                "siteName": "funcgijstest",
                "hostNameType": "Verified"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/echo-api/create-resource')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/echo-api/modify-resource')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/echo-api/remove-resource')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/echo-api/retrieve-header-only')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/echo-api/retrieve-resource')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/echo-api/retrieve-resource-cached')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
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
            "name": "[concat(parameters('service_apimgijstest_name'), '/echo-api/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "documents": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/groups/users",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/administrators/1')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'administrators')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/groups/users",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/developers/1')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'developers')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/apis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/starter/echo-api')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/apis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/unlimited/echo-api')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/starter/administrators')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/unlimited/administrators')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/starter/developers')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/unlimited/developers')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/starter/guests')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groups",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/unlimited/guests')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ]
        },
        {
            "type": "Microsoft.ApiManagement/service/products/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/starter/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "value": "<!--\r\n            IMPORTANT:\r\n            - Policy elements can appear only within the <inbound>, <outbound>, <backend> section elements.\r\n            - Only the <forward-request> policy element can appear within the <backend> section element.\r\n            - To apply a policy to the incoming request (before it is forwarded to the backend service), place a corresponding policy element within the <inbound> section element.\r\n            - To apply a policy to the outgoing response (before it is sent back to the caller), place a corresponding policy element within the <outbound> section element.\r\n            - To add a policy position the cursor at the desired insertion point and click on the round button associated with the policy.\r\n            - To remove a policy, delete the corresponding policy statement from the policy document.\r\n            - Position the <base> element within a section element to inherit all policies from the corresponding section element in the enclosing scope.\r\n            - Remove the <base> element to prevent inheriting policies from the corresponding section element in the enclosing scope.\r\n            - Policies are applied in the order of their appearance, from the top down.\r\n        -->\r\n<policies>\r\n  <inbound>\r\n    <rate-limit calls=\"5\" renewal-period=\"60\" />\r\n    <quota calls=\"100\" renewal-period=\"604800\" />\r\n    <base />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/wikis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/starter/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "documents": []
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/wikis",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/unlimited/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "documents": []
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers",
            "apiVersion": "2024-05-15",
            "name": "[concat(parameters('databaseAccounts_cosmosgijstest_name'), '/WinGet/Manifests')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts/sqlDatabases', parameters('databaseAccounts_cosmosgijstest_name'), 'WinGet')]",
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmosgijstest_name'))]"
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
                        "kind": "Hash",
                        "version": 1
                    },
                    "defaultTtl": -1,
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
            "name": "[concat(parameters('databaseAccounts_cosmosgijstest_name'), '/WinGet/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts/sqlDatabases', parameters('databaseAccounts_cosmosgijstest_name'), 'WinGet')]",
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmosgijstest_name'))]"
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
            "name": "[concat(parameters('databaseAccounts_cosmosgijstest_name'), '/b7c6c7dd-48ad-5e9d-b3e1-918fd233432c')]",
            "dependsOn": [
                "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmosgijstest_name'))]",
                "[resourceId('Microsoft.DocumentDB/databaseAccounts/sqlRoleDefinitions', parameters('databaseAccounts_cosmosgijstest_name'), 'd5427907-1b14-58a2-a5eb-b633c3cfcf9f')]"
            ],
            "properties": {
                "roleDefinitionId": "[resourceId('Microsoft.DocumentDB/databaseAccounts/sqlRoleDefinitions', parameters('databaseAccounts_cosmosgijstest_name'), 'd5427907-1b14-58a2-a5eb-b633c3cfcf9f')]",
                "principalId": "ee4728e5-4acc-49e4-9d62-ef9bbf705b98",
                "scope": "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('databaseAccounts_cosmosgijstest_name'))]"
            }
        },
        {
            "type": "Microsoft.Storage/storageAccounts/blobServices/containers",
            "apiVersion": "2023-05-01",
            "name": "[concat(parameters('storageAccounts_stgijstest_name_1'), '/default/azure-webjobs-hosts')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/blobServices', parameters('storageAccounts_stgijstest_name_1'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_stgijstest_name_1'))]"
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
            "name": "[concat(parameters('storageAccounts_stgijstest_name_1'), '/default/azure-webjobs-secrets')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/blobServices', parameters('storageAccounts_stgijstest_name_1'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_stgijstest_name_1'))]"
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
            "name": "[concat(parameters('storageAccounts_stgijstest_name_1'), '/default/default')]",
            "dependsOn": [
                "[resourceId('Microsoft.Storage/storageAccounts/blobServices', parameters('storageAccounts_stgijstest_name_1'), 'default')]",
                "[resourceId('Microsoft.Storage/storageAccounts', parameters('storageAccounts_stgijstest_name_1'))]"
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
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/echo-api/create-resource/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apimgijstest_name'), 'echo-api', 'create-resource')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <json-to-xml apply=\"always\" consider-accept-header=\"false\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/echo-api/retrieve-header-only/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apimgijstest_name'), 'echo-api', 'retrieve-header-only')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n    <set-header name=\"X-My-Sample\" exists-action=\"override\">\r\n      <value>This is a sample</value>\r\n      <!-- for multiple headers with the same name add additional value elements -->\r\n    </set-header>\r\n    <jsonp callback-parameter-name=\"ProcessResponse\" />\r\n  </outbound>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/apis/operations/policies",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/echo-api/retrieve-resource-cached/policy')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('service_apimgijstest_name'), 'echo-api', 'retrieve-resource-cached')]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]"
            ],
            "properties": {
                "value": "<policies>\r\n  <inbound>\r\n    <base />\r\n    <cache-lookup vary-by-developer=\"false\" vary-by-developer-groups=\"false\">\r\n      <vary-by-header>Accept</vary-by-header>\r\n      <vary-by-header>Accept-Charset</vary-by-header>\r\n    </cache-lookup>\r\n    <rewrite-uri template=\"/resource\" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n    <cache-store duration=\"3600\" />\r\n  </outbound>\r\n</policies>",
                "format": "xml"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/apiLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/starter/CB60558A-D94D-4C10-9EB9-59370D1CDFCA')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]"
            ],
            "properties": {
                "apiId": "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/apiLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/unlimited/D5BA1C4B-5DE8-463D-B412-61D2CE22F5F3')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]"
            ],
            "properties": {
                "apiId": "[resourceId('Microsoft.ApiManagement/service/apis', parameters('service_apimgijstest_name'), 'echo-api')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/starter/103D283F-AFC3-42A3-8FB8-AF6764AF36B4')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'guests')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'guests')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/unlimited/10A5F9C4-D93D-4BCC-8E5D-7C1768919EC1')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'guests')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'guests')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/starter/536A1A6C-EDB5-4287-A07D-8B3AF8EFB7AE')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'administrators')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'administrators')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/unlimited/96B13FC1-8F87-4351-AB8B-64A4930563B9')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'developers')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'developers')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/unlimited/9F18CD9B-BDDD-410C-B219-47FD58B2552A')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'unlimited')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'administrators')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'administrators')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/products/groupLinks",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/starter/B290036D-E690-4D19-8EE8-B90DF417B5E0')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]",
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'developers')]"
            ],
            "properties": {
                "groupId": "[resourceId('Microsoft.ApiManagement/service/groups', parameters('service_apimgijstest_name'), 'developers')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/subscriptions",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/677624190be04c0063070001')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/users', parameters('service_apimgijstest_name'), '1')]",
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]"
            ],
            "properties": {
                "ownerId": "[resourceId('Microsoft.ApiManagement/service/users', parameters('service_apimgijstest_name'), '1')]",
                "scope": "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'starter')]",
                "state": "active",
                "allowTracing": false,
                "displayName": "[parameters('subscriptions_677624190be04c0063070001_displayName')]"
            }
        },
        {
            "type": "Microsoft.ApiManagement/service/subscriptions",
            "apiVersion": "2023-09-01-preview",
            "name": "[concat(parameters('service_apimgijstest_name'), '/677624190be04c0063070002')]",
            "dependsOn": [
                "[resourceId('Microsoft.ApiManagement/service', parameters('service_apimgijstest_name'))]",
                "[resourceId('Microsoft.ApiManagement/service/users', parameters('service_apimgijstest_name'), '1')]",
                "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'unlimited')]"
            ],
            "properties": {
                "ownerId": "[resourceId('Microsoft.ApiManagement/service/users', parameters('service_apimgijstest_name'), '1')]",
                "scope": "[resourceId('Microsoft.ApiManagement/service/products', parameters('service_apimgijstest_name'), 'unlimited')]",
                "state": "active",
                "allowTracing": false,
                "displayName": "[parameters('subscriptions_677624190be04c0063070002_displayName')]"
            }
        }
    ]
}
