# Welcome to the winget-cli-restsource repository

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
