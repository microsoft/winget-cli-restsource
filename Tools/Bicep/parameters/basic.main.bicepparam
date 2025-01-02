using '../src/main.bicep'

param publisherEmail = ''
param cosmosDbPrimaryLocation = 'West Europe' // TODO: Follow issue: https://github.com/Azure/bicep/issues/15987
param cosmosDbSecondaryLocation = 'North Europe'
