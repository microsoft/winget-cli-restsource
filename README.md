# Welcome to the winget-cli-restsource repository

## Building the client

### Prerequisites

* [Git Large File Storage (LFS)](https://git-lfs.github.com/)
* [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/)
* The following workloads:
   * .NET desktop development
   * Azure development
   * ASP<area>.NET and web development

Open `src\WinGet.RestSource.sln` in Visual Studio and build. We currently only build using the solution; command line methods of building a VS solution should work as well.

## Running locally

The REST functions can be run locally, but to use winget with them, the functions must be run using HTTPS, this is pre-configured by the `launchSettings.json` file.

1. In the `src\WinGet.RestSource.Functions` directory, run `generate_self_sign_cert.ps1` in PowerShell.
   * This will generate a test pfx and install it into the Root store.
   * It will automatically be used as the HTTPS cert during local execution, thanks to `launchSettings.json`
2. Create a CosmosDB database instance in Azure, using either the above instructions, or [manually](https://docs.microsoft.com/en-us/azure/cosmos-db/sql/create-cosmosdb-resources-portal).
   * Navigate to the Keys section of your CosmosDB instance in the Azure portal to find your connection information.
   * If you've used the ARM templates as described above, your Database will be named `WinGet` and your Collection will be `Manifests`
3. Copy `src\WinGet.RestSource.Functions\local.settings.template.json` to `local.settings.json` and populate required fields from the above Keys section.
4. Run the `WinGet.RestSource.Functions` project locally in Visual Studio using F5.
5. Add it as a source in winget with: `winget source add -n "winget-pkgs-restsource" -a https://localhost:7071/api/ -t "Microsoft.Rest"`

Your commands to winget will now use your locally running REST instance as the primary source.

## Running Unit Tests

Running unit tests are a great way to ensure that functionality is preserved across major changes. You can run these tests in Visual Studio Test Explorer.

### Testing Prerequisites

* Install the [Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21)
* Copy the `WinGet.RestSource.UnitTest\Test.runsettings.template.json` template configuration to `Test.runsettings.json`
  * The defaults should work for your local Cosmos DB emulator instance. You can change the configuration to point to a Cosmos DB instance in Azure instead.
  * Alternatively, all of the test configuration properties can be set as environment variables. This is useful for overriding properties in an ADO build.

In Visual Studio, run the tests from the menu with Test > Run All Tests

## Automatically create a rest source

The `Microsoft.WinGet.Source` PowerShell module provides the [New-WinGetSource](/Tools/PowershellModule/doc/PowerShell/New-WinGetSource.md) cmdlet to simplify the creation of a Windows Package Manager rest source. This PowerShell cmdlet will initiate a connection to Azure if not currently connected. Validating that the connection is established with a specific Subscription (if specified). Generate the ARM Parameter files with specified values, then create Azure resources with the generated ARM Parameter files and the provided ARM Template files.

The `New-WinGetSource` PowerShell cmdlet makes use of the following input parameters. For more information on how to use this cmdlet, use the `Get-Help New-WinGetSource -Full` or visit the [New-WinGetSource PowerShell Article](./Tools/PowershellModule/doc/PowerShell/New-WinGetSource.md) in Docs.

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
