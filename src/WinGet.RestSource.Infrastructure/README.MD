# Azure Resource Manager (ARM) Templates

## ARM Template Design

These ARM templates are meant to be reusable in order to provide a number of different configurations and redundancy levels of the service. For testing and development you can create an environment consisting of a single Cosmos DB, One Function App (one ASP, one Storage Account, and One Azure Function instance). This can easily be expanded to add resiliency and redundancy by having a multi-region Cosmos DB, multi-region Function Apps (multiple ASPs, multiple Storage Accounts, and multiple Azure Function instance in different regions), in conjunction with Azure API Mangement and Front Door.

## Elements

### API Managment

This can be placed in front of the function apps to provide AAD authentication or failover-routing.

### Applicaiton Insights

This is used for logging and analytics.

### App Service Plan

This is the compute used for processing requests against the Cosmos DB by the Azure Function Apps.

### Azure Function

This is the Function App that executes on the ASP.

### Front Door

This can be used to add failover properties to either the Azure Function Runtimes and/or in front of multiple API Mangment instances (to save money over the premium instances).

### Key Vault

This is used to store all the secrets.

### Storage Account

This is used by the Azure Function.