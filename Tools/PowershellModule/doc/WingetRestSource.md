# Create a Windows Package Manager REST source

This section provides guidance on how to create a REST source for the Windows Package Manager. ISVs or Publishers may host and manage a REST source if they would like full control of the packages available in a source. An independently hosted source may choose to expose the read endpoints publicly, restrict access to specific IP address via the addition of a traffic shaping module or restrict access by Microsoft Entra Id authentication.  The basic setups configured by the cmdlets and examples result in a source that is publicly readable but requires an authorization key to manage.

Windows Package Manager offers a comprehensive package manager solution including a command line tool and a set of services for installing applications. For more general package submission information, see [submit packages to Windows Package Manager](https://learn.microsoft.com/windows/package-manager/package/).

## Manage Windows Package Manager REST source with PowerShell

To simplify the management and interaction with the Windows Package Manager REST source, the `Microsoft.WinGet.RestSource` PowerShell module has been made available. This PowerShell module provides cmdlets that enable you to Add, Find, Remove and Get package manifests from your Windows Package Manager REST source, as well as stand up a new Windows Package Manager REST source in Azure.

The following steps are for managing a Windows Package Manager REST source with PowerShell:

1. Download and install the `Microsoft.WinGet.RestSource` PowerShell Module.
2. Automate the creation of a Windows Package Manager REST source.
3. Publish Package Manifests to the Windows Package Manager REST source.
4. Find Package Manifests from Windows Package Manager REST source.
5. Retrieve published Package Manifests from the Windows Package Manager REST source.
6. Remove published Package Manifests from the Windows Package Manager REST source.

### PowerShell Pre-Requisites

**The `Microsoft.WinGet.RestSource` PowerShell module requires PowerShell Core version 7.4 or later.**

Before getting started with the Windows Package Manager REST source with PowerShell, here are a few recommended steps you should complete before using the PowerShell module to ensure you can successfully stand up a new Windows Package Manager REST source in Azure.

1. Open PowerShell as an Administrator
2. Set the PowerShell execution policies

    ```Powershell
    PS C:\> Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
    ```

3. Install the Azure Az module

    ```Powershell
    PS C:\> Install-Module -Name Az -AllowClobber
    ```

4. Connect to Azure with an authenticated account

    ```Powershell
    PS C:\> Connect-AzAccount
    ```

5. Select the required subscription using Set-AzContext

    ```Powershell
    PS C:\> Set-AzContext -Subscription [Paste the Azure Subscription here]
    ```

### Download and install the PowerShell module

#### Get PowerShell Module from PowerShell Gallery (Recommended)

1. Install the PowerShell module from PowerShell Gallery

    ```Powershell
    PS C:\> Install-PSResource -Name Microsoft.WinGet.RestSource -Prerelease
    ```

#### Get PowerShell Module from Github Releases

The following steps will get PowerShell from Github Releases for use with the Windows Package Manager REST source.

1. Open an Edge Browser.
2. Navigate to [https://github.com/microsoft/winget-cli-restsource/releases](https://github.com/microsoft/winget-cli-restsource/releases).
3. Download the latest release of the Microsoft.WinGet.RestSource PowerShell module. (WinGet.RestSource-Winget.PowerShell.Source.zip)
4. Open a File Explorer window, and navigate to where you downloaded the Microsoft.WinGet.RestSource PowerShell module.
5. Right-click on WinGet.RestSource-Winget.PowerShell.Source.zip, and select **Extract all** from the drop-down menu.
6. In the new window, select the **Extract** button.
7. After the extraction has completed, navigate to '<extracted folder>\WinGet.RestSource-Winget.Powershell.Source'.
8. Open an **Administrative PowerShell** window.
9. Ensure the downloaded `Microsoft.WinGet.RestSource` files are not blocked

    ```Powershell
    PS C:\> Get-ChildItem -Path [Paste the path to the root folder of Microsoft.WinGet.RestSource] -Recurse | Unblock-File
    ```

10. In combination, press [Ctrl]+[Shift]+Right-click on the `Microsoft.WinGet.RestSource.psd1` file. Select **Copy Path** from the drop-down menu.
11. Run the following command from the Administrative PowerShell window:

    ```Powershell
    PS C:\> Import-Module [Paste the path to the Microsoft.WinGet.RestSource.psd1 file]
    ```

> [!Note]
> If PowerShell module is extracted from zip file downloaded from Github releases page, the PowerShell module must be re-imported each time the PowerShell window is closed.  
> To prevent having to re-install this PowerShell module each time, install from PowerShell Gallery or review the instructions in the [Installing a PowerShell module on Microsoft Docs](https://learn.microsoft.com/powershell/scripting/developer/module/installing-a-powershell-module?view=powershell-7.4)

### Automate the creation of a Windows Package Manager REST source

The `Microsoft.WinGet.RestSource` PowerShell module provides the [New-WinGetSource](PowerShell/New-WinGetSource.md) cmdlet to simplify the creation of a Windows Package Manager REST source. This PowerShell cmdlet will initiate a connection to Azure if not currently connected. Validating that the connection is established with a specific Subscription (if specified). Generate the ARM Template Parameter files with specified values, then create Azure resources with the generated ARM Template Parameter files and the provided ARM Template files.

The `New-WinGetSource` PowerShell cmdlet makes use of the following input parameters. For more information on how to use this cmdlet, use the `Get-Help New-WinGetSource -Full` or visit the [New-WinGetSource PowerShell Article](PowerShell/New-WinGetSource.md) in Docs.

| Required | Parameter                                | Description                                                                                                                      |
| -------- | ---------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------- |
| Yes      | Name                                     | A string of letters as the base name for your newly created Azure resources.                                                     |
| No       | ResourceGroup                            | The Resource Group that will be used to contain the Azure resources. (Default: WinGetRestSource)                                 |
| No       | SubscriptionName                         | The name of the Azure Subscription that will be used to pay for the Azure resources. (Default: current subscription)             |
| No       | Region                                   | The Azure location where the Azure resources will be created. (Default: westus)                                                  |
| No       | TemplateFolderPath                       | The folder location where ARM templates can be found. (Default: Templates provided by the Powershell module)                     |
| No       | ParameterOutputPath                      | The folder location where the ARM template parameter files will be created. (Default: Current Directory\Parameters)              |
| No       | RestSourcePath                           | Path to the compiled REST API Zip file. (Default: Zip file provided by the Powershell module)                                    |
| No       | PublisherName                            | The Windows Package Manager REST source publisher name. (Default: Signed in user email Or WinGetRestSource@DomainName)           |
| No       | PublisherEmail                           | The Windows Package Manager REST source publisher email. (Default: Signed in user email Or WinGetRestSource@DomainName)          |
| No       | ImplementationPerformance                | Specifies the performance of the Azure resources for the Windows Package Manager REST source. ["Developer", "Basic", "Enhanced"] |
| No       | RestSourceAuthentication                 | The Windows Package Manager REST source authentication type. ["None", "MicrosoftEntraId"] (Default: None)                        |
| No       | CreateNewMicrosoftEntraIdAppRegistration | If specified, a new Microsoft Entra Id app registration will be created. (Default: False)                                        |
| No       | MicrosoftEntraIdResource                 | Microsoft Entra Id authentication resource. (Default: None)                                                                      |
| No       | MicrosoftEntraIdResourceScope            | Microsoft Entra Id authentication resource scope. (Default: None)                                                                |
| No       | ShowConnectionInstructions               | If specified, the instructions for connecting to the new Windows Package Manager REST source will be provided. (Default: False)  |
| No       | MaxRetryCount                            | Max ARM templates deployment retry count upon failure. (Default: 5)                                                              |

To create a new Windows Package Manager REST source, open the Administrative PowerShell Window and run the following:

1. From an Administrative PowerShell window run the following:
    For public access WIndows Package Manager REST source

    ```PowerShell
    PS C:\> New-WinGetSource -Name "contosorestsource" -ResourceGroup "WinGet" -Region "westus" -ImplementationPerformance "Basic" -ShowConnectionInstructions -InformationAction Continue -Verbose
    ```

    **Or** for Microsoft Entra Id protected WIndows Package Manager REST source

    ```PowerShell
    PS C:\> New-WinGetSource -Name "contosorestsource" -ResourceGroup "WinGet" -Region "westus" -ImplementationPerformance "Basic" -RestSourceAuthentication "MicrosoftEntraId" -CreateNewMicrosoftEntraIdAppRegistration -ShowConnectionInstructions -InformationAction Continue -Verbose
    ```

2. After the above command has completed, copy and run the connection information provided for your newly created Windows Package Manager REST source to add to your winget client.

> [!Note]
> This is a long running process which can take up to 1 hour depending on network, Azure service load, etc.

### Add manifests to the REST source

After the creation of your Windows Package Manager REST source has completed, you'll need to add Package Manifests for your users to install from. Using the `Microsoft.WinGet.RestSource` PowerShell module, the [Add-WinGetManifest](PowerShell/Add-WinGetManifest.md) cmdlet will add new Package Manifests to your Windows Package Manager REST source.

The `Add-WinGetManifest` PowerShell cmdlet supports targeting a specific `*.json` file, or a folder containing `*.yaml` files for a single package manifest. For more information on how to use this cmdlet, use the command: `Get-Help Add-WinGetManifest -Full` or visit the [Add-WinGetManifest article in the PowerShell docs](PowerShell/Add-WinGetManifest.md).

To add an Package Manifest, open the Administrative PowerShell Window and run the following:

```PowerShell
PS C:\> Add-WinGetManifest -FunctionName "contoso" -Path "C:\WinGet\Manifests\Windows.PowerToys\1.0.0"
```

### Find manifests from the REST source

The `Microsoft.WinGet.RestSource` PowerShell module provides the [Find-WinGetManifest](PowerShell/Find-WinGetManifest.md) cmdlet that will find Package Manifests from the specified Windows Package Manager REST source.

The `Find-WinGetManifest` PowerShell cmdlet supports targeting an existing Windows Package Manager REST source. For more information on how to use this cmdlet, use the `Get-Help Find-WinGetManifest -Full` or visit the [Find-WinGetManifest article in the PowerShell docs](PowerShell/Find-WinGetManifest.md).

To find package manifests, open the Administrative PowerShell Window and run the following:

```PowerShell
PS C:\> Find-WinGetManifest -FunctionName "contoso" -Query "PowerToys"
```

To find all package manifests, open the Administrative PowerShell Window and run the following:

```PowerShell
PS C:\> Find-WinGetManifest -FunctionName "contoso" -Query ""
```

### Get manifests from the REST source

The `Microsoft.WinGet.RestSource` PowerShell module provides the [Get-WinGetManifest](PowerShell/Get-WinGetManifest.md) cmdlet that will use ManifestGET against a specified Windows Package Manager REST source to fetch all packages or a specific package by Package Identifier.

Alternatively, the `Get-WinGetManifest` PowerShell cmdlet supports targeting a specific `*.json` file or `*.yaml` files in a folder as well as targeting an existing Windows Package Manager REST source. For more information on how to use this cmdlet, use the `Get-Help Get-WinGetManifest -Full` or visit the [Get-WinGetManifest article in the PowerShell docs](PowerShell/Get-WinGetManifest.md).

To get a Package Manifest from Windows Package Manager REST source, open the Administrative PowerShell Window and run the following:

```PowerShell
PS C:\> Get-WinGetManifest -FunctionName "contoso" -PackageIdentifier "Windows.PowerToys"
```

### Remove manifests from a REST source

The `Microsoft.WinGet.RestSource` PowerShell module provides the [Remove-WinGetManifest](PowerShell/Remove-WinGetManifest.md) cmdlet that will remove a specific Package Manifest from the specified Windows Package Manager REST source.

The `Remove-WinGetManifest` PowerShell cmdlet supports targeting an existing Windows Package Manager REST source for a specific Package Identifier. For more information on how to use this cmdlet, use the `Get-Help Remove-WinGetManifest -Full` or visit the [Remove-WinGetManifest article in the PowerShell docs](PowerShell/Remove-WinGetManifest.md).

To remove all versions of a package, open the Administrative PowerShell Window and run the following:

```PowerShell
PS C:\> Remove-WinGetManifest -FunctionName "contoso" -PackageIdentifier "Windows.PowerToys"
```

To remove a specific version of a package, open the Administrative PowerShell Window and run the following:

```PowerShell
PS C:\> Remove-WinGetManifest -FunctionName "contoso" -PackageIdentifier "Windows.PowerToys" -PackageVersion "1.0.0"
```

## Manage Windows Package Manager REST source with Azure Bicep

Azure Bicep can be used as an infrastructure as Code (IaC) language to deploy the required Azure resources without using the `Microsoft.WinGet.RestClient` module, offering a more intuitive and declarative approach. The [Bicep](../../Bicep/) folder consist out of multiple patterns, mimicking the `ImplementationPerformance` parameter for each scenario e.g. `Free`, `Basic`, and `Enhanced`.  

The following steps are for managing a Windows Package Manager REST source with Azure Bicep:

1. Download and install `Bicep`.
2. Create Azure Resource Group.
3. Deploy the pattern of choose.

### Download and install Bicep

**To deploy the required Azure resources with Bicep, you require version 0.32.4 or later.**

#### Get Bicep from WinGet (Recommended)

1. Install Bicep using WinGet

    ```powershell
    PS C:\> winget install Microsoft.Bicep --source winget
    ```

#### Get Bicep using PowerShell

The following steps will get the latest version of Bicep using PowerShell:

1. Open PowerShell
2. Execute the following code snippet to install Bicep in your user profile.

    ```powershell
    $installPath = "$env:USERPROFILE\.bicep"
    $installDir = New-Item -ItemType Directory -Path $installPath -Force
    $installDir.Attributes += 'Hidden'
    (New-Object Net.WebClient).DownloadFile("https://github.com/Azure/bicep/releases/latest/download/bicep-win-x64.exe", "$installPath\bicep.exe")
    $currentPath = (Get-Item -path "HKCU:\Environment" ).GetValue('Path', '', 'DoNotExpandEnvironmentNames')
    if (-not $currentPath.Contains("%USERPROFILE%\.bicep")) { setx PATH ($currentPath + ";%USERPROFILE%\.bicep") }
    if (-not $env:path.Contains($installPath)) { $env:path += ";$installPath" }
    bicep --help
    ```

> [!NOTE]
> For alternative methods, check out the following documentation on this [link](https://github.com/maikvandergaag/bicep-1/blob/main/docs/installing.md#setup-your-bicep-development-environment).

### Create Azure Resource Group

To create the Azure Resource Group, you can follow the steps below:

1. Open PowerShell if it isn't currently running
2. Execute the following command by replacing the placeholder values: `New-AzResourceGroup -Name <name> -Location <location>`

> [!NOTE]
> The `Az` module is required from the [PowerShell pre-requisites](#powershell-pre-requisites)

### Deploy pattern of choose

The following steps demonstrate how you can deploy the Windows Package Manager REST source using the `Basic` pattern:

1. Open PowerShell if it isn't currently running.
2. Navigate to the folder containing the Bicep files.
3. Execute the following command to deploy the `Basic` pattern, replacing the placeholder values:

    ```powershell
    PS C:\> New-AzResourceGroupDeployment -ResourceGroupName <ResourceGroupName> -TemplateFile basic.bicep -publisherEmail <publisherEmail> -cosmosDbPrimaryLocation <cosmosDbPrimaryLocation> -cosmosDbSecondaryLocation <cosmosDbSecondaryLocation>
    ```

> [!NOTE]
> The `cosmosDbPrimaryLocation` and `cosmosDbSecondaryLocation` parameters need to be filled in based on the resource group location because Bicep currently does not support a function to automatically retrieve these values. This ensures that the Cosmos DB is deployed in the correct regions for high availability and disaster recovery.

4. Monitor the deployment process and ensure all resources are created successfully.
