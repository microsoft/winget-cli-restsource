@description('Name for the Application Insights')
param appInsightsName string = 'appi${uniqueString(resourceGroup().id)}'

@description('Name for the KeyVault')
param keyVaultName string = 'kv${uniqueString(resourceGroup().id)}'

@description('Specifies the name of the App Configuration store.')
param appConfigName string = 'appcs${uniqueString(resourceGroup().id)}'

@description('Storage Account Name')
param storageAccountName string = 'st${uniqueString(resourceGroup().id)}'

@description('App Service Name')
param aspName string = 'asp${uniqueString(resourceGroup().id)}'

@description('Storage Account Type')
param accountType string = 'Standard_GRS'

@description('Cosmos Database Name.')
param cosmosDbName string = 'cosmos${uniqueString(resourceGroup().id)}'

@description('Azure Function Name')
param functionName string = 'func${uniqueString(resourceGroup().id)}'

@description('Location for resources.')
param location string = resourceGroup().location

@description('Key Vault sku.')
param sku string = 'Standard'

@description('Optional. The name of the SKU will Determine the tier, size, family of the App Service Plan. This defaults to P1v2.')
param serverFarmSkuName string = 'P1V2'

module logAnalytics 'br/public:avm/res/operational-insights/workspace:0.9.1' = {
  name: 'logAnalytics'
  params: {
    name: 'log-${uniqueString(resourceGroup().name)}'
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
    skuName: accountType
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

module cosmosDb 'br/public:avm/res/document-db/database-account:0.10.1' = {
  name: 'cosmosDb'
  params: {
    name: 'cosmos${uniqueString(resourceGroup().name)}'
    location: location
    disableLocalAuth: true
    networkRestrictions: {
      publicNetworkAccess: 'Enabled'
      networkAclBypass: 'AzureServices'
      ipRules: [
        '13.91.105.215'
        '4.210.172.107'
        '13.88.56.148'
        '40.91.218.243'
        '0.0.0.0'
      ]
    }

    enableFreeTier: false
    databaseAccountOfferType: 'Standard'
    backupPolicyType: 'Periodic'
    backupIntervalInMinutes: 240
    backupRetentionIntervalInHours: 720
    backupStorageRedundancy: 'Geo'
    defaultConsistencyLevel: 'Session'
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
                  path: '/_etag/?'
                }
              ]
            }
            conflictResolutionPolicy: {
              mode: 'LastWriterWins'
              conflictResolutionPath: '/_ts'
            }
            // uniqueKeyPolicyKeys: [
            //   {
            //     paths: [
            //       '/id'
            //     ]
            //   }
            // ]
          }
        ]
      }
    ]
  }
}

module keyVault 'br/public:avm/res/key-vault/vault:0.11.1' = {
  name: 'keyVault'
  params: {
    name: keyVaultName
    location: location
    sku: sku
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
    ]
    secrets: [
      {
        name: 'CosmosAccountEndpoint'
        value: cosmosDb.outputs.endpoint
      }
      {
        name: 'AppConfigPrimaryEndpoint'
        value: appConfig.outputs.endpoint
      }
      {
        name: 'AppConfigSecondaryEndpoint'
        value: appConfig.outputs.endpoint
      }
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
      ServerIdentifier: '${resourceGroup().name}-TODO'
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
