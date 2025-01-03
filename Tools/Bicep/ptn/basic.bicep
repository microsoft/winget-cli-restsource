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

@description('Optional. The name of the API Management service.')
param apiServiceName string = 'apim${resourceGroup().name}'

module userAssignedIdentity 'br/public:avm/res/managed-identity/user-assigned-identity:0.4.0' = {
  name: 'userAssignedIdentity'
  params: {
    name: 'id-${resourceGroup().name}'
    location: location
  }
}

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

module function 'br/public:avm/res/web/site:0.13.0' = {
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
      AzFuncRestSourceEndpoint: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/AzFuncRestSourceEndpoint/)'
      AzureWebJobsStorage__accountName: storageAccount.outputs.name
      CosmosAccountEndpoint: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/CosmosAccountEndpoint/)'
      CosmosContainer: 'Manifests'
      CosmosDatabase: 'WinGet'
      FunctionHostKey: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/AzureFunctionHostKey/)'
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
      'WinGet:AppConfig:PrimaryEndpoint': '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/AppConfigPrimaryEndpoint/)'
      'WinGet:AppConfig:SecondaryEndpoint': '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/AppConfigSecondaryEndpoint/)'
      'WinGetRest::SubscriptionId': subscription().subscriptionId
      'WinGetRest:Telemetry:Metrics': null
      'WinGetRest:Telemetry:Role': null
      'WinGetRest:Telemetry:Tenant': null
    }
    roleAssignments: [
      {
        principalId: userAssignedIdentity.outputs.principalId
        roleDefinitionIdOrName: 'de139f84-1756-47ae-9be6-808fbbe84772'
        description: 'Assign the Website Contributor to the User Assigned Identity.'
      }
    ]
  }
}

module cosmosDb 'br/public:avm/res/document-db/database-account:0.10.1' = {
  name: 'cosmosDb'
  params: {
    name: cosmosDbName
    location: 'westus'
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
        locationName: 'West US'
        isZoneRedundant: false
      }
      {
        failoverPriority: 1
        locationName: 'Central US'
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

// TODO: Follow issue: https://github.com/Azure/azure-powershell/issues/23558
module deploymentScript 'br/public:avm/res/resources/deployment-script:0.5.1' = {
  name: 'deploymentScript'
  params: {
    name: 'ds-${resourceGroup().name}'
    kind: 'AzurePowerShell'
    timeout: 'PT30M'
    runOnce: true
    azPowerShellVersion: '12.3'
    retentionInterval: 'PT1H'
    location: location
    arguments: '-FunctionAppId ${function.outputs.resourceId} -ResourceGroupName ${resourceGroup().name} -FunctionName ${functionName}'
    scriptContent: '''
        param (
            [string] $FunctionAppId,
            [string] $ResourceGroupName,
            [string] $FunctionName
        )

        # Generate function key 
        function New-FunctionAppKey
        {
            $private:characters = 'abcdefghiklmnoprstuvwxyzABCDEFGHIJKLMENOPTSTUVWXYZ'
            $private:randomChars = 1..64 | ForEach-Object { Get-Random -Maximum $characters.length }
        
            # Set the output field separator to empty instead of space
            $private:ofs=""
            return [String]$characters[$randomChars]
        }

        $NewFunctionKeyValue = New-FunctionAppKey
        $Result = Invoke-AzRestMethod -Path "$FunctionAppId/host/default/functionKeys/WinGetRestSourceAccess?api-version=2024-04-01" -Method PUT -Payload (@{properties=@{value = $NewFunctionKeyValue}} | ConvertTo-Json -Depth 8)
        if ($Result.StatusCode -ne 200 -and $Result.StatusCode -ne 201) {
            Throw "Failed to create Azure Function key. $($Result.Content)"
        }

        # Define variables
        $repo = "microsoft/winget-cli-restsource"
        $file = "WinGet.RestSource-WinGet.RestSource.Functions.zip"
        $releases = "https://api.github.com/repos/$repo/releases"

        # Get the latest ZIP file URI
        $zipUri = ((Invoke-RestMethod -Uri $releases)[0].assets | Where-Object {$_.Name -eq $file}).browser_download_url
        $out = Join-Path -Path ([System.IO.Path]::GetTempPath()) -ChildPath $file

        # Extract the file on container
        Write-Host "Retrieving $zipUri to $out"
        Invoke-RestMethod -Uri $zipUri -OutFile $out

        # Publish file
        try {
            Write-Host "Publishing $out the Azure Function $FunctionName"
            Publish-AzWebApp -ResourceGroupName $ResourceGroupName -Name $FunctionName -ArchivePath $out -Confirm:$false -Force -Restart -ErrorAction Stop
        } catch {
            Throw "Failed to publish the Azure Function $FunctionName. Error: $_" 
        }

        # Send key to output
        $DeploymentScriptOutputs = @{}
        $DeploymentScriptOutputs['functionAppHostKey'] = $NewFunctionKeyValue
          '''
    managedIdentities: { userAssignedResourceIds: [userAssignedIdentity.outputs.resourceId] }
  }
}

module apiManagement 'br/public:avm/res/api-management/service:0.6.0' = {
  name: 'apiManagement'
  params: {
    name: apiServiceName
    publisherEmail: publisherEmail
    publisherName: publisherEmail
    sku: 'Basic'
    managedIdentities: { systemAssigned: true }
    skuCapacity: 1
    hostnameConfigurations: [
      {
        type: 'Proxy'
        hostName: '${apiServiceName}.azure-api.net'
        negotiateClientCertificate: false
        defaultSslBinding: true
        certificateSource: 'BuiltIn'
      }
    ]
    apis: [
      {
        displayName: 'winget rest source api'
        description: 'winget rest source api'
        name: 'winget'
        path: 'winget'
      }
    ]
    namedValues: [
      {
        name: 'winget-functions-access-key'
        displayName: 'winget-functions-access-key'
        keyVault: {
          secretIdentifier: 'https://${keyVaultName}.vault.azure.net/secrets/AzureFunctionHostKey'
        }
        secret: true
      }
    ]
    backends: [
      {
        name: 'winget-backend'
        url: 'https://${function.outputs.name}.azurewebsites.net/api'
        tls: {
          validateCertificateChain: true
          validateCertificateName: true
        }
        credentials: {
          header: {
            'x-functions-key': [
              '{{winget-functions-access-key}}'
            ]
          }
        }
      }
    ]
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
            'list'
          ]
        }
        tenantId: subscription().tenantId
      }
      {
        objectId: function.outputs.systemAssignedMIPrincipalId
        permissions: {
          secrets: [
            'get'
          ]
        }
      }
      {
        objectId: apiManagement.outputs.systemAssignedMIPrincipalId
        permissions: {
          secrets: [
            'get'
          ]
        }
      }
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
      {
        name: 'AzureFunctionHostKey'
        value: deploymentScript.outputs.outputs.functionAppHostKey
      }
    ]
  }
}

// region API Management operations
//TODO: Track issue: https://github.com/Azure/bicep-registry-modules/issues/2419

var backEndPolicy = '<policies> <inbound> <set-backend-service id=\'public-api-policy\' backend-id=\'winget-backend\'/> </inbound> <backend> </backend> <outbound> </outbound> <on-error> </on-error> </policies>'

resource existingApiManagement 'Microsoft.ApiManagement/service/apis@2024-06-01-preview' existing = {
  name: '${apiServiceName}/winget'
  dependsOn: [
    apiManagement
  ]
}

resource service_apim_rest_name_winget_get_informationget 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'get-informationget'
  properties: {
    displayName: 'InformationGet'
    method: 'GET'
    urlTemplate: '/information'
    templateParameters: []
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_delete_installerdelete 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'delete-installerdelete'
  properties: {
    displayName: 'InstallerDelete'
    method: 'DELETE'
    urlTemplate: '/packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageVersion'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'installerIdentifier'
        type: 'string'
        required: true
        values: []
      }
    ]
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_get_installerget 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'get-installerget'
  properties: {
    displayName: 'InstallerGet'
    method: 'GET'
    urlTemplate: '/packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageVersion'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'installerIdentifier'
        type: 'string'
        values: []
      }
    ]
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_post_localepost 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'post-localepost'
  properties: {
    displayName: 'LocalePost'
    method: 'POST'
    urlTemplate: '/packages/{packageIdentifier}/versions/{packageVersion}/locales'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageVersion'
        type: 'string'
        required: true
        values: []
      }
    ]
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_post_manifestpost 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'post-manifestpost'
  properties: {
    displayName: 'ManifestPost'
    method: 'POST'
    urlTemplate: '/packageManifests'
    templateParameters: []
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_post_manifestsearchpost 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'post-manifestsearchpost'
  properties: {
    displayName: 'ManifestSearchPost'
    method: 'POST'
    urlTemplate: '/manifestSearch'
    templateParameters: []
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_post_packagepost 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'post-packagepost'
  properties: {
    displayName: 'PackagePost'
    method: 'POST'
    urlTemplate: '/packages'
    templateParameters: []
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_post_rebuildpost 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'post-rebuildpost'
  properties: {
    displayName: 'RebuildPost'
    method: 'POST'
    urlTemplate: '/rebuild'
    templateParameters: []
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_post_updatepost 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'post-updatepost'
  properties: {
    displayName: 'UpdatePost'
    method: 'POST'
    urlTemplate: '/update'
    templateParameters: []
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_post_versionpost 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'post-versionpost'
  properties: {
    displayName: 'VersionPost'
    method: 'POST'
    urlTemplate: '/packages/'
    templateParameters: []
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_put_installerput 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'put-installerput'
  properties: {
    displayName: 'InstallerPut'
    method: 'PUT'
    urlTemplate: '/packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageVersion'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'installerIdentifier'
        type: 'string'
        required: true
        values: []
      }
    ]
    policies: backEndPolicy
    responses: []
  }
}

resource service_apim_rest_name_winget_put_localeput 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'put-localeput'
  properties: {
    displayName: 'LocalePut'
    method: 'PUT'
    urlTemplate: '/packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageVersion'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageLocale'
        type: 'string'
        required: true
        values: []
      }
    ]
    policies: backEndPolicy
    responses: []
  }
}

resource service_apim_rest_name_winget_put_manifestput 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'put-manifestput'
  properties: {
    displayName: 'ManifestPut'
    method: 'PUT'
    urlTemplate: '/packageManifests/{packageIdentifier}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
    ]
    policies: backEndPolicy
    responses: []
  }
}

resource service_apim_rest_name_winget_put_packageput 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'put-packageput'
  properties: {
    displayName: 'PackagePut'
    method: 'PUT'
    urlTemplate: '/packages/{packageIdentifier}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
    ]
    policies: backEndPolicy
    responses: []
  }
}

resource service_apim_rest_name_winget_put_versionput 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'put-versionput'
  properties: {
    displayName: 'VersionPut'
    method: 'PUT'
    urlTemplate: '/packages/{packageIdentifier}/versions/{packageVersion}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageVersion'
        type: 'string'
        required: true
        values: []
      }
    ]
    policies: backEndPolicy
    responses: []
  }
}

resource service_apim_rest_name_winget_get_versionget 'Microsoft.ApiManagement/service/apis/operations@2023-09-01-preview' = {
  parent: existingApiManagement
  name: 'get-versionget'
  properties: {
    displayName: 'VersionGet'
    method: 'GET'
    urlTemplate: '/packages/{packageIdentifier}/versions/{packageVersion}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageVersion'
        type: 'string'
        values: []
      }
    ]
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_delete_versiondelete 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'delete-versiondelete'
  properties: {
    displayName: 'VersionDelete'
    method: 'DELETE'
    urlTemplate: '/packages/{packageIdentifier}/versions/{packageVersion}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageVersion'
        type: 'string'
        required: true
        values: []
      }
    ]
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_post_installerpost 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'post-installerpost'
  properties: {
    displayName: 'InstallerPost'
    method: 'POST'
    urlTemplate: '/packages/{packageIdentifier}/versions/{packageVersion}/installers'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageVersion'
        type: 'string'
        required: true
        values: []
      }
    ]
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_delete_localedelete 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'delete-localedelete'
  properties: {
    displayName: 'LocaleDelete'
    method: 'DELETE'
    urlTemplate: '/packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageVersion'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageLocale'
        type: 'string'
        required: true
        values: []
      }
    ]
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_delete_manifestdelete 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'delete-manifestdelete'
  properties: {
    displayName: 'ManifestDelete'
    method: 'DELETE'
    urlTemplate: '/packageManifests/{packageIdentifier}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
    ]
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_get_localeget 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'get-localeget'
  properties: {
    displayName: 'LocaleGet'
    method: 'GET'
    urlTemplate: '/packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageVersion'
        type: 'string'
        required: true
        values: []
      }
      {
        name: 'packageLocale'
        type: 'string'
        values: []
      }
    ]
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_get_manifestget 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'get-manifestget'
  properties: {
    displayName: 'ManifestGet'
    method: 'GET'
    urlTemplate: '/packageManifests/{packageIdentifier}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
    ]
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_delete_packagedelete 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
  parent: existingApiManagement
  name: 'delete-packagedelete'
  properties: {
    displayName: 'PackageDelete'
    method: 'DELETE'
    urlTemplate: '/packages/{packageIdentifier}'
    templateParameters: [
      {
        name: 'packageIdentifier'
        type: 'string'
        required: true
        values: []
      }
    ]
    responses: []
    policies: backEndPolicy
  }
}

resource service_apim_rest_name_winget_get_packageget 'Microsoft.ApiManagement/service/apis/operations@2024-06-01-preview' = {
    parent: existingApiManagement
    name: 'get-packageget'
    properties: {
      displayName: 'PackageGet'
      method: 'GET'
      urlTemplate: '/packages/{packageIdentifier}'
      templateParameters: [
        {
          name: 'packageIdentifier'
          type: 'string'
          values: []
        }
      ]
      responses: []
      policies: backEndPolicy
    }
  }

// endregion API Management operations

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

output apiManagementUrl string = 'https://${apiManagement.outputs.name}.azure-api.net/winget/'
