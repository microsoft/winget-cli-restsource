# Windows Package Manager Repository

## Create a new private repository in Azure
To simply the creation of a Windows Package Manager private repository, the `automation.ps1` [PowerShell script](Tools/Automation.ps1) is offered. This script will create new Elements required to host a Windows Package Manager private repository in Azure.

The `automation.ps1` script has the following parameter inputs:
| Required | Parameter          | Description                                                                                                                |
|----------|--------------------|----------------------------------------------------------------------------------------------------------------------------|
| Yes      | ResourcePrefix     | A string of letters which will be prefixed to your newly created Azure resources.                                          |
| Yes      | Index              | A string of letters or numbers which will be suffixed to your newly created Azure resources.                                |
| Yes      | AzResourceGroup    | The Resource Group that will be used to contain the Azure resources.                                                       |
| No       | AzSubscriptionName | The name of the Azure Subscription that will be used to pay for the Azure resources.                                       |
| No       | AzLocation         | The Azure location where the Azure resources will be created. (Default: westus)                                            |
| No       | WorkingDirectory   | The folder location that contains this the ARM template files, as well as where the Azure Parameter files will be created. |

For more information on how to manually implement your Windows Package Manager in Azure, please visit our Docs.

**Example:**

.\Tools\automation.ps1 -ResourcePrefix "contoso-" -Index "Demo" -AzResourceGroup "WinGet_PrivateRepo_Demo"


## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
