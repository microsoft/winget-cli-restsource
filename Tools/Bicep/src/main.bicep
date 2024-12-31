@description('Required. The email address of the owner of the service.')
param publisherEmail string

@description('Required. The name of the primary region for Cosmos Database.')
param cosmosDbPrimaryLocation string

@description('Required. The name of the primary region for Cosmos Database.')
param cosmosDbSecondaryLocation string

@description('Optional. Name for the Application Insights')
param appInsightsName string = 'appi${resourceGroup().name}'

@description('Optional. Name of the Key Vault. Must be globally unique.')
@maxLength(24)
param keyVaultName string = 'kv${resourceGroup().name}'

@description('Optional. Name of the App Configuration Store.')
param appConfigName string = 'appcs${resourceGroup().name}'

@description('Optional. Name of the Storage Account')
param storageAccountName string = 'st${resourceGroup().name}'

@description('Optional. Name of the App Service')
param aspName string = 'asp${resourceGroup().name}'

@allowed([
  'Standard_LRS'
  'Standard_GRS'
  'Standard_RAGRS'
  'Standard_ZRS'
  'Premium_LRS'
  'Premium_ZRS'
  'Standard_GZRS'
  'Standard_RAGZRS'
])
@description('Optional. Storage Account Sku Name.')
param storageSkuName string = 'Standard_GRS'

@description('Optional. Name of the Cosmos Database')
param cosmosDbName string = 'cosmos${resourceGroup().name}'

@description('Optional. Name of the Azure Function')
param functionName string = 'func${resourceGroup().name}'

@description('Optional. The location of the resources. Defaults to Resource Group location.')
param location string = resourceGroup().location

@description('Optional. Specifies the SKU for the vault.')
@allowed([
  'premium'
  'standard'
])
param keyVaultSku string = 'standard'

@description('Optional. The name of the SKU will determine the tier, size, family of the App Service Plan. This defaults to S1.')
param serverFarmSkuName string = 'S1'

// resource user 'Microsoft.Graph/users@v1.0' existing = {
//     objectId: deployer().objectId
//   } TODO: Track issue: https://github.com/microsoftgraph/msgraph-bicep-types/issues/135 if this becomes available we can pass in the email to API management

module logAnalytics 'br/public:avm/res/operational-insights/workspace:0.9.1' = {
  name: 'logAnalytics'
  params: {
    name: 'log${resourceGroup().name}'
    location: location
  }
}

module appInsights 'br/public:avm/res/insights/component:0.4.2' = {
  name: 'appInsights'
  params: {
    name: appInsightsName
    location: location
    kind: 'web'
    applicationType: 'web'
    retentionInDays: 90
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
    workspaceResourceId: logAnalytics.outputs.resourceId
    // ingestionMode: 'ApplicationInsights' // TODO: Track issue: https://github.com/Azure/bicep-registry-modules/issues/4097
  }
}

module appConfig 'br/public:avm/res/app-configuration/configuration-store:0.6.0' = {
  name: 'appConfig'
  params: {
    name: appConfigName
    location: location
    sku: 'Standard'
    disableLocalAuth: true
    softDeleteRetentionInDays: 7
    // keyValues: [
    //   {
    //     name: '${appConfigName}/.appconfig.featureflag~2FGenevaLogging'
    //     value: '{"id":"GenevaLogging","description":"Feature flag to use Geneva Monitoring.","enabled":false,"conditions":{"client_filters":[]}}'
    //     contentType: 'application/vnd.microsoft.appconfig.ff+json;charset=utf-8'
    //   }
    // ]
  }
}

module asp 'br/public:avm/res/web/serverfarm:0.4.0' = {
  name: 'asp'
  params: {
    name: aspName
    location: location
    skuName: serverFarmSkuName
    maximumElasticWorkerCount: 1
    reserved: false
  }
}

module storageAccount 'br/public:avm/res/storage/storage-account:0.15.0' = {
  name: 'storageAccount'
  params: {
    name: storageAccountName
    location: location
    skuName: storageSkuName
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
      ipRules: []
      virtualNetworkRules: []
    }
    blobServices: {
      containers: [
        {
          name: 'default'
          publicAccess: 'None'
        }
      ]
      containerDeleteRetentionPolicyEnabled: false
    }
    requireInfrastructureEncryption: false
    // The encryption property should be set by AVM
    // encryption: {
    //   services: {
    //     file: {
    //       keyType: 'Account'
    //       enabled: true
    //     }
    //     blob: {
    //       keyType: 'Account'
    //       enabled: true
    //     }
    //   }
    //   keySource: 'Microsoft.Storage'
    // }
    allowSharedKeyAccess: false
  }
}

module keyVault 'br/public:avm/res/key-vault/vault:0.11.1' = {
  name: 'keyVault'
  params: {
    name: keyVaultName
    location: location
    sku: keyVaultSku
    enableRbacAuthorization: false
    enableSoftDelete: true
    enableVaultForDeployment: true
    enableVaultForTemplateDeployment: true
    enableVaultForDiskEncryption: true
    enablePurgeProtection: true
    softDeleteRetentionInDays: 90
    networkAcls: {
      defaultAction: 'allow'
      bypass: 'AzureServices'
      ipRules: []
      virtualNetworkRules: []
    }
    accessPolicies: [
      {
        objectId: deployer().objectId
        permissions: {
          secrets: [
            'get'
            'set'
          ]
        }
        tenantId: subscription().tenantId
      }
      // TODO: Add APIM and Azure function Get Secrets
    ]
    secrets: [
      {
        name: 'CosmosAccountEndpoint'
        value: 'https://${cosmosDbName}.documents.azure.com:443/'
      }
      {
        name: 'AppConfigPrimaryEndpoint'
        value: appConfig.outputs.endpoint
      }
      {
        name: 'AppConfigSecondaryEndpoint'
        value: appConfig.outputs.endpoint
      }
      // TODO: Add Azure function host key
    ]
  }
}

module function 'br/public:avm/res/web/site:0.12.1' = {
  name: 'function'
  params: {
    name: functionName
    kind: 'functionapp'
    serverFarmResourceId: asp.outputs.resourceId
    location: location
    clientAffinityEnabled: true
    managedIdentities: { systemAssigned: true }
    siteConfig: {
      alwaysOn: true
      minTlsVersion: '1.2'
    }
    httpsOnly: true
    appSettingsKeyValuePairs: {
      APPINSIGHTS_INSTRUMENTATIONKEY: appInsights.outputs.instrumentationKey
      AzFuncRestSourceEndpoint: '@Microsoft.KeyVault(SecretUri=${keyVault.outputs.secrets[0].uri}/AzFuncRestSourceEndpoint)'
      AzureWebJobsStorage__accountName: storageAccount.outputs.name
      CosmosAccountEndpoint: '@Microsoft.KeyVault(SecretUri=${keyVault.outputs.secrets[0].uri}/CosmosAccountEndpoint)'
      CosmosContainer: 'Manifests'
      CosmosDatabase: 'WinGet'
      FunctionHostKey: '@Microsoft.KeyVault(SecretUri=${keyVault.outputs.secrets[0].uri}/AzureFunctionHostKey)'
      FunctionName: functionName
      FUNCTIONS_EXTENSION_VERSION: '~4'
      FUNCTIONS_WORKER_RUNTIME: 'dotnet'
      FUNCTIONS_INPROC_NET8_ENABLED: 1
      ManifestCacheEndpoint: null
      MicrosoftEntraIdResource: null 
      MicrosoftEntraIdResourceScope: null
      ServerIdentifier: 'WinGetRestSource-${resourceGroup().name}'
      WEBSITE_FIRST_PARTY_ID: 'AntMDS'
      WEBSITE_LOAD_CERTIFICATES: '*'
      'WinGet:AppConfig:PrimaryEndpoint': '@Microsoft.KeyVault(SecretUri=${keyVault.outputs.secrets[0].uri}/AppConfigPrimaryEndpoint)'
      'WinGet:AppConfig:SecondaryEndpoint': '@Microsoft.KeyVault(SecretUri=${keyVault.outputs.secrets[0].uri}/AppConfigSecondaryEndpoint)'
      'WinGetRest::SubscriptionId': subscription().subscriptionId
      'WinGetRest:Telemetry:Metrics': null
      'WinGetRest:Telemetry:Role': null
      'WinGetRest:Telemetry:Tenant': null
    }
  }
}

module cosmosDb 'br/public:avm/res/document-db/database-account:0.10.1' = {
  name: 'cosmosDb'
  params: {
    name: cosmosDbName
    location: location
    disableLocalAuth: true
    networkRestrictions: {
      publicNetworkAccess: 'Enabled'
      networkAclBypass: 'None'
      ipRules: [
        '13.91.105.215'
        '4.210.172.107'
        '13.88.56.148'
        '40.91.218.243'
        '0.0.0.0'
      ]
    }
    locations: [
      {
        failoverPriority: 0
        locationName: cosmosDbPrimaryLocation
        isZoneRedundant: false
      }
      {
        failoverPriority: 1
        locationName: cosmosDbSecondaryLocation
        isZoneRedundant: false
      }
    ]
    sqlRoleAssignmentsPrincipalIds: [
      function.outputs.systemAssignedMIPrincipalId
    ]
    sqlRoleDefinitions: [
      { name: '00000000-0000-0000-0000-000000000002' } // The built-in role definition ID for the Cosmos DB Account Contributor role
    ]
    enableFreeTier: false
    databaseAccountOfferType: 'Standard'
    backupPolicyType: 'Periodic'
    backupIntervalInMinutes: 240
    backupRetentionIntervalInHours: 720
    backupStorageRedundancy: 'Geo'
    defaultConsistencyLevel: 'Session'
    totalThroughputLimit: null
    maxStalenessPrefix: 100
    maxIntervalInSeconds: 5
    sqlDatabases: [
      {
        name: 'WinGet'
        autoscaleSettingsMaxThroughput: 4000
        containers: [
          {
            name: 'Manifests'
            paths: [
              '/id'
            ]
            indexingPolicy: {
              automatic: true
              includedPaths: [
                {
                  path: '/*'
                }
              ]
              indexingMode: 'consistent'
              excludedPaths: [
                {
                  path: '/"_etag"/?'
                }
              ]
            }
            conflictResolutionPolicy: {
              mode: 'LastWriterWins'
              conflictResolutionPath: '/_ts'
            }
          }
        ]
      }
    ]
  }
}

module apiManagement 'br/public:avm/res/api-management/service:0.6.0' = {
  name: 'apiManagement'
  params: {
    name: 'apim${resourceGroup().name}'
    publisherEmail: publisherEmail
    publisherName: publisherEmail
    sku: 'Basic'
    managedIdentities: { systemAssigned: true }
  }
}

module roleAssignmentBlobDataOwner 'br/public:avm/ptn/authorization/resource-role-assignment:0.1.1' = {
  name: 'BlobDataOwner'
  params: {
    resourceId: storageAccount.outputs.resourceId
    principalId: function.outputs.systemAssignedMIPrincipalId
    roleDefinitionId: 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
    principalType: 'ServicePrincipal'
    roleName: 'Storage Blob Data Owner'
    description: 'Assign the Storage Blob Data Owner role to the Azure Function on the Storage Account.'
  }
}

module roleAssignmentStorageAccountContributor 'br/public:avm/ptn/authorization/resource-role-assignment:0.1.1' = {
  name: 'StorageAccountContributor'
  params: {
    resourceId: storageAccount.outputs.resourceId
    principalId: function.outputs.systemAssignedMIPrincipalId
    roleDefinitionId: '17d1049b-9a84-46fb-8f53-869881c3d3ab'
    principalType: 'ServicePrincipal'
    roleName: 'Storage Account Contributor'
    description: 'Assign the Storage Account Contributor role to the Azure Function on the Storage Account.'
  }
}

module roleAssignmentQueueDataContributor 'br/public:avm/ptn/authorization/resource-role-assignment:0.1.1' = {
  name: 'QueueDataContributor'
  params: {
    resourceId: storageAccount.outputs.resourceId
    principalId: function.outputs.systemAssignedMIPrincipalId
    roleDefinitionId: '974c5e8b-45b9-4653-ba55-5f855dd0fb88'
    principalType: 'ServicePrincipal'
    roleName: 'Storage Queue Data Contributor'
    description: 'Assign the Storage Queue Data Contributor role to the Azure Function on the Storage Account.'
  }
}

module roleAssignmentQueueDataMessageProcessor 'br/public:avm/ptn/authorization/resource-role-assignment:0.1.1' = {
  name: 'QueueDataMessageProcessor'
  params: {
    resourceId: storageAccount.outputs.resourceId
    principalId: function.outputs.systemAssignedMIPrincipalId
    roleDefinitionId: '8a0f0c08-91a1-4084-bc3d-661d67233fed'
    principalType: 'ServicePrincipal'
    roleName: 'Storage Queue Data Message Processor'
    description: 'Assign the Storage Queue Data Message Processor role to the Azure Function on the Storage Account.'
  }
}

module roleAssignmentQueueDataMessageSender 'br/public:avm/ptn/authorization/resource-role-assignment:0.1.1' = {
  name: 'QueueDataMessageSender'
  params: {
    resourceId: storageAccount.outputs.resourceId
    principalId: function.outputs.systemAssignedMIPrincipalId
    roleDefinitionId: 'c6a89b2d-59bc-44d0-9896-0f6e12d7b80a'
    principalType: 'ServicePrincipal'
    roleName: 'Storage Queue Data Message Sender'
    description: 'Assign the Storage Queue Data Message Sender role to the Azure Function on the Storage Account.'
  }
}

module roleAssignmentTableDataContributor 'br/public:avm/ptn/authorization/resource-role-assignment:0.1.1' = {
  name: 'TableDataContributor'
  params: {
    resourceId: storageAccount.outputs.resourceId
    principalId: function.outputs.systemAssignedMIPrincipalId
    roleDefinitionId: '0a9a7e1f-b9d0-4cc4-a60d-0319b160aaa3'
    principalType: 'ServicePrincipal'
    roleName: 'Storage Table Data Contributor'
    description: 'Assign the Storage Table Data Contributor role to the Azure Function on the Storage Account.'
  }
}

module roleAssignmentAppConfigDataReader 'br/public:avm/ptn/authorization/resource-role-assignment:0.1.1' = {
  name: 'AppConfigDataReader'
  params: {
    resourceId: appConfig.outputs.resourceId
    principalId: function.outputs.systemAssignedMIPrincipalId
    roleDefinitionId: '516239f1-63e1-4d78-a4de-a74fb236a071'
    principalType: 'ServicePrincipal'
    roleName: 'App Configuration Data Reader'
    description: 'Assign the App Configuration Data Reader role to the Azure Function on the App Configuration.'
  }
}
