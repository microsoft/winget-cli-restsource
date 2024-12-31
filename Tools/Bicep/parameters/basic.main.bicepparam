using '../src/main.bicep'

param publisherEmail = 'gijs.reijn@Rabobank.com'
param cosmosDbPrimaryLocation = 'West Europe' // TODO: Follow issue: https://github.com/Azure/bicep/issues/15987
param cosmosDbSecondaryLocation = 'North Europe'
