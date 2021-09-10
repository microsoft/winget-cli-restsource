# Azure Resource Manager (ARM) Templates

## ARM Template Design

These ARM templates are meant to be reusable in order to provide a number of different configurations and redundancy levels of the service. For testing and development you can create an environment consisting of a single Cosmos DB, One Function App (one ASP, one Storage Account, and One Azure Function instance). This can easily be expanded to add resiliency and redundancy by having a multi-region Cosmos DB, multi-region Function Apps (multiple ASPs, multiple Storage Accounts, and multiple Azure Function instance in different regions), in conjunction with Azure API Mangement and Front Door.

## Deploying your private repository
To simply the creation of a Windows Package Manager private repository, the `automation.ps1` PowerShell script is offered. This script will create new Elements required to host a Windows Package Manager private repository in Azure.


The `automation.ps1` script has the following parameter inputs:
| Required | Parameter          | Description                                                                                                                |
|----------|--------------------|----------------------------------------------------------------------------------------------------------------------------|
| Yes      | ResourcePrefix     | A string of letters which will be prefixed to your newly created Azure resources.                                          |
| Yes      | Index              | A string of letters or numbers which will be sufixed to your newly created Azure resources.                                |
| Yes      | AzResourceGroup    | The Resource Group that will be used to contain the Azure resources.                                                       |
| No       | AzSubscriptionName | The name of the Azure Subscription that will be used to pay for the Azure resources.                                       |
| No       | AzLocation         | The Azure location where the Azure resources will be created. (Default: westus)                                            |
| No       | WorkingDirectory   | The folder location that contains this the ARM template files, as well as where the Azure Parameter files will be created. |

For more information on how to manually implement your Windows Package Manager in Azure, please visit our Docs.

**Example:**

.\src\WinGet.RestSource.Infrastructure\automation.ps1 -ResourcePrefix "contoso-" -Index "Demo" -AzResourceGroup "WinGet_PrivateRepo_Demo"


### Elements

### API Management

This can be placed in front of the function apps to provide AAD authentication or failover-routing.

### Application Insights

This is used for logging and analytics.

#### App Service Plan

This is the compute used for processing requests against the Cosmos DB by the Azure Function Apps.

#### Azure Function

This is the Function App that executes on the ASP.

#### Front Door

This can be used to add failover properties to either the Azure Function Runtimes and/or in front of multiple API Management instances (to save money over the premium instances).

#### Key Vault

This is used to store all the secrets.

#### Storage Account

This is used by the Azure Function.