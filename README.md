# Windows Package Manager Repository

## Automatically create a rest source

The `Microsoft.WinGet.Source` PowerShell module provides the [New-WinGetSource](.\PowerShell\New-WinGetSource.md) cmdlet to simplify the creation of a Windows Package Manager rest source. This PowerShell cmdlet will initiate a connection to Azure if not currently connected. Validating that the connection is established with a specific Subscription (if specified). Generate the ARM Parameter files with specified values, then create Azure resources with the generated ARM Parameter files and the provided ARM Template files.

The `New-WinGetSource` PowerShell cmdlet makes use of the following input parameters. For more information on how to use this cmdlet, use the `Get-Help New-WinGetSource -Full` or visit the [New-WinGetSource PowerShell Article](.\PowerShell\New-WinGetSource.md) in Docs.

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
