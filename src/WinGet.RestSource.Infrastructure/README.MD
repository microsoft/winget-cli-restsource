# Azure Resource Manager (ARM) Templates

## ARM Template Design

These ARM templates are meant to be reusable in order to provide a number of different configurations and redundancy levels of the service. For testing and development you can create an environment consisting of a single Cosmos DB, One Function App (one ASP, one Storage Account, and One Azure Function instance). This can easily be expanded to add resiliency and redundancy by having a multi-region Cosmos DB, multi-region Function Apps (multiple ASPs, multiple Storage Accounts, and multiple Azure Function instance in different regions), in conjunction with Azure API Mangement and Front Door.

## Deploying your private repository
The `Microsoft.WinGet.Source` PowerShell module provides the [New-WinGetSource](Tools/PowerShell/Docs/PowerShell/New-WinGetSource.md) cmdlet to simplify the creation of a Windows Package Manager rest source. This PowerShell cmdlet will initiate a connection to Azure if not currently connected. Validating that the connection is established with a specific Subscription (if specified). Generate the ARM Parameter files with specified values, then create Azure resources with the generated ARM Parameter files and the provided ARM Template files.

The `New-WinGetSource` PowerShell cmdlet makes use of the following input parameters. For more information on how to use this cmdlet, use the `Get-Help New-WinGetSource -Full` or visit the [New-WinGetSource PowerShell Article](PowerShell/New-WinGetSource.md) in Docs.

| Required | Parameter                  | Description                                                                                                                                |
|----------|----------------------------|--------------------------------------------------------------------------------------------------------------------------------------------|
| Yes      | Name                       | A string of letters which will be prefixed to your newly created Azure resources.                                                          |
| Yes      | Index                      | A string of letters or numbers which will be suffix to your newly created Azure resources.                                                 |
| Yes      | ResourceGroup              | The Resource Group that will be used to contain the Azure resources.                                                                       |
| No       | SubscriptionName           | The name of the Azure Subscription that will be used to pay for the Azure resources.                                                       |
| No       | Region                     | The Azure location where the Azure resources will be created. (Default: westus)                                                            |
| No       | ParameterOutput            | The folder location that contains new items will be created in.                                                                            |
| No       | RestSourcePath             | Path to the compiled Rest API Zip file. (Default: .\RestAPI\CompiledFunctions.ps1)                                                         |
| No       | ImplementationPerformance  | specifies the performance of the resources to be created for the Windows Package Manager rest source. ["Demo", "Basic", "Enhanced"]        |
| No       | ShowConnectionInstructions | If specified, the instructions for connecting to the Windows Package Manager rest source. (Default: False)                                 |

> [!Note]
> The PowerShell Module must be re-imported each time the PowerShell window is closed.

**How to:**

1. From the Administrative PowerShell window run the following:
```PowerShell
PS C:\> New-WinGetSource -Name "contoso" -ResourceGroup "WinGetPrivateSource" -Region "westus" -ImplementationPerformance "Demo" -ShowConnectionInstructions
```
1. After the above has completed, copy and run the connection information provided for your newly created Windows Package Manager rest source to add it to your WinGet client.


### Elements

#### API Management

This can be placed in front of the function apps to provide AAD authentication or failover-routing.

#### Application Insights

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
