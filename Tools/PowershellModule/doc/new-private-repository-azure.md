# Windows Package Manager REST source

The Windows Package Manager provides a distribution channel for software packages containing their tools and applications. The instructions contained within this document provide guidance on how to setup a REST source that can be connected to using the Windows Package Manager. Providing a comprehensive package manager solution that consists of a command line tool and a set of services for installing applications on Windows 10.

#### Available Guidance
    - Managing the Windows Package Manager REST source with PowerShell.
    - Managing the Windows Package Manager REST source manually.

## Windows Package Manager REST source automation
To simplify the management and interaction with the Windows Package Manager REST source, the `Microsoft.WinGet.Source` PowerShell module has been made available. This PowerShell module provides cmdlets that enable you to Add, Remove and Get Application Manifests from your Windows Package Manager REST source, as well as stand up a new Windows Package Manager REST source in Azure.

#### Available Guidance
    - Download and Installation of the `Microsoft.WinGet.Source` PowerShell Module.
    - Automate the creation of a Windows Package Manager REST source.
    - Publishing Application Manifests to the Windows Package Manager REST source.
    - Retrieving published Application Manifests from the Windows Package Manager REST source.
    - Removing published Applications Manifests from the Windows Package Manager REST source.

### Download and install the PowerShell module
The following steps must be performed before the PowerShell cmdlets are available for use with the Windows Package Manager REST source.

**How to:**
1. Open an Edge Browser.
1. Navigate to [https://github.com/microsoft/winget-cli-restsource/releases](https://github.com/microsoft/winget-cli-restsource/releases).
1. Download the latest release of the Microsoft.WinGet.Source PowerShell module.
1. Open a File Explorer window, and navigate to where you downloaded the Microsoft.WinGet.Source PowerShell module.
1. Right-click on the Microsoft.WinGet.Source PowerShell module, and select **Extract all** from the drop-down menu.
1. In the new Window, select the **Extract** button.
1. After the extraction has completed, navigate to `WinGet.RestSource-Winget.Powershell.Source`.
1. In combination press [Ctrl]+[Shift]+Right-click on the Microsoft.WinGet.Source.psd1 file. Select **Copy Path** from the drop-down menu.
1. Open an **Administrative PowerShell** window.
1. Run the following command from the Administrative PowerShell window:
```Powershell
PS C:\> Import-Module [Paste the path to the Microsoft.WinGet.Source.psd1 file]
```

## Automatically create a REST source

The `Microsoft.WinGet.Source` PowerShell module provides the [New-WinGetSource](.\PowerShell\New-WinGetSource.md) cmdlet to simplify the creation of a Windows Package Manager REST source. This PowerShell cmdlet will initiate a connection to Azure if not currently connected. Validating that the connection is established with a specific Subscription (if specified). Generate the ARM Parameter files with specified values, then create Azure resources with the generated ARM Parameter files and the provided ARM Template files.

The `New-WinGetSource` PowerShell cmdlet makes use of the following input parameters. For more information on how to use this cmdlet, use the `Get-Help New-WinGetSource -Full` or visit the [New-WinGetSource PowerShell Article](.\PowerShell\New-WinGetSource.md) in Docs.

| Required | Parameter                  | Description                                                                                                                                |
|----------|----------------------------|--------------------------------------------------------------------------------------------------------------------------------------------|
| Yes      | Name                       | A string of letters which will be prefixed to your newly created Azure resources.                                                          |
| Yes      | Index                      | A string of letters or numbers which will be suffix to your newly created Azure resources.                                                 |
| Yes      | ResourceGroup              | The Resource Group that will be used to contain the Azure resources.                                                                       |
| No       | SubscriptionName           | The name of the Azure Subscription that will be used to pay for the Azure resources.                                                       |
| No       | Region                     | The Azure location where the Azure resources will be created. (Default: westus)                                                            |
| No       | ParameterOutput            | The folder location that contains new items will be created in.                                                                            |
| No       | RestSourcePath             | Path to the compiled REST API Zip file. (Default: .\Library\RestAPI\WinGet.RestSource.Functions.zip)                                                         |
| No       | ImplementationPerformance  | specifies the performance of the resources to be created for the Windows Package Manager REST source. ["Demo", "Basic", "Enhanced"]        |
| No       | ShowConnectionInstructions | If specified, the instructions for connecting to the Windows Package Manager REST source. (Default: False)                                 |

> [!Note]
> The PowerShell Module must be re-imported each time the PowerShell window is closed.

**How to:**

1. From the Administrative PowerShell window run the following:
```PowerShell
PS C:\> New-WinGetSource -Name "contoso" -ResourceGroup "WinGetPrivateSource" -Region "westus" -ImplementationPerformance "Demo" -ShowConnectionInstructions
```
1. After the above has completed, copy and run the connection information provided for your newly created Windows Package Manager REST source to add it to your WinGet client.

## Add manifests to the REST source

The Windows Package Manager REST source provides a hosting solution allowing you to add your Application Manifests to. After the creation of your Windows Package Manager REST source has completed, you'll need to add Application Manifests for your users to install from. Using the `Microsoft.WinGet.Source` PowerShell module, the [Add-WinGetManifest](.\PowerShell\Add-WinGetManifest.md) cmdlet will add new Application Manifests to your Windows Package Manager REST source.

The `Add-WinGetManifest` PowerShell cmdlet supports targetting a specific *.json file, a folder containing multiple *.json files, or a folder containing *.yaml files. For more information on how to use this cmdlet, use the `Get-Help Add-WinGetManifest -Full` or visit the [Add-WinGetManifest PowerShell Article](.\PowerShell\New-WinGetManifest.md) in Docs.

> [!Note]
> The PowerShell Module must be re-imported each time the PowerShell window is closed.

**How to:**

1. From the Administrative PowerShell Window run the following:
```PowerShell
PS C:\> Add-WinGetManifest -FunctionName "contoso" -Path "C:\WinGet\Manifests\Windows.PowerToys.json"
```

## Get manifests from the REST source

The Windows Package Manager REST source provides a hosting solution allowing you to add your Application Manifests to. The `Microsoft.WinGet.Source` PowerShell module provides the [Get-WinGetManifest](.\PowerShell\Get-WinGetManifest.md) cmdlet that will query for all, or a specific Application Manifest found in a specified Windows Package Manager REST source.

The `Get-WinGetManifest` PowerShell cmdlet supports targetting a specific *.json file, a folder containing multiple *.json files or *.yaml files as well as targetting an existing Windows Package Manager REST source. For more information on how to use this cmdlet, use the `Get-Help Get-WinGetManifest -Full` or visit the [Get-WinGetManifest PowerShell Article](.\PowerShell\Get-WinGetManifest.md) in Docs.

> [!Note]
> The PowerShell Module must be re-imported each time the PowerShell window is closed.

**How to:**

1. From the Administrative PowerShell Window run the following:
```PowerShell
PS C:\> Get-WinGetManifest -FunctionName "contoso" -ManifestIdentifier "Windows.PowerToys"
```

## Remove manifests from a REST source

The Windows Package Manager REST source provides a hosting solution allowing you to add your Application Manifests to. The `Microsoft.WinGet.Source` PowerShell module provides the [Remove-WinGetManifest](.\PowerShell\Remove-WinGetManifest.md) cmdlet that will remove a specific Application Manifest from the specified Windows Package Manager REST source.

The `Remove-WinGetManifest` PowerShell cmdlet supports targetting an existing Windows Package Manager REST source for a specific Manifest Identifier. For more information on how to use this cmdlet, use the `Get-Help Remove-WinGetManifest -Full` or visit the [Remove-WinGetManifest PowerShell Article](.\PowerShell\Remove-WinGetManifest.md) in Docs.

> [!Note]
> The PowerShell Module must be re-imported each time the PowerShell window is closed.

**How to:**

1. From the Administrative PowerShell Window run the following:
```PowerShell
PS C:\> Remove-WinGetManifest -FunctionName "contoso" -ManifestIdentifier "Windows.PowerToys"
```

## Manually create a REST source

The following instructions assumes the following Azure objects are named as follows:
| Azure Resource          | Value                      |
|-------------------------|----------------------------|
| Azure Resource Group    | WinGet_PrivateRepo         |
| Azure Subscription Name | Contoso Azure Subscription |
| Azure Location          | West US                    |
| Azure Key Vault Name    | contoso-keyvault-demo      |

*We assume that the Resource Group and Subscription are pre-existing in your Azure Tenant.*

### Extract the Windows Package Manager REST Source

The Windows Package Manager REST Source contains the APIs required to provide a Windows Package Manager REST source.

**How to:**

1. Download a local copy of the Windows Package Manager REST Source from GitHub (github: [winget-cli-restsource](https://github.com/microsoft/winget-cli-restsource/releases))
    1. Download *.Zip file from the latest release.
    1. Extract the newly downloaded ZIP file to *C:\Projects\\*

### Application Insights

Application Insights, a feature of Azure Monitor, is an extensible Application Performance Management (APM) service for developers and DevOps professionals. Azure's Application Insights is will be used to monitor the health of the Windows Package Manager REST source, as well as provide powerful analytical insights to help with diagnosing any issues, and identify user experiences.

For more information on Azure Application Insights, visit their Docs article: [What is Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview).

**How to:**

1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. At the top of the window, select the **+ Create** button.
1. In the search bar, type in "*Application Insights*".
1. Select **Application Insights** from the search results.
1. Select the **Create** button.
1. In the **Project Details** ensure the following values have been set:
    - Subscription: "Contoso Azure Subscription" 
    - Resource Group: "WinGet_PrivateRepo".

1. In the **Instance Details** enter the following values:
    - Name: contoso-appinsights-demo
    - Region: West US
    - Resource Mode: Classic

1. Select the **Review + create** button at the bottom of the blade.
1. Assuming the Validation as passed, select the **Create** button.
1. After your deployment has completed, continue to the steps in the next section.

### Storage Account

An Azure storage account contains all of your Azure Storage data objects: blobs, file shares, queues, tables, and disks. The Storage account created below will be used to store the Azure Function binaries.

For more information on Azure Storage Accounts, visit their Docs article: [Storage account overview](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-overview).

**How to:**

1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. At the top of the window, select the **+ Create** button.
1. In the search bar, type in "*Storage Account*".
1. Select **Storage Account** from the search results.
1. Select the **Create** button.
1. In the **Project Details** ensure the following values have been set:
    - Subscription: "Contoso Azure Subscription" 
    - Resource Group: "WinGet_PrivateRepo"

1. In the **Instance Details** enter the following values:
    - Storage account name: contosostoreaccountdemo
    - Region: West US
    - Performance: Standard
    - Redundancy: Geo-redundant storage (GRS)
    - Make read access to data available in the event of regional unavailability: Enabled

1. Select the **Next : Advanced** button.
1. In the **Security** section, ensure that the following values are set:
    - Require secure transfer for REST API operations: Enabled
    - Enable infrastructure encryption: Disabled
    - Enable blob public access: Disabled
    - Enable Storage account key access: Enabled
    - Default to Azure Active Directory authorization in the Azure portal: Disabled
    - Minimum TLS version: Version 1.2

1. Select the **Next : Networking** button.
1. In the **Network connectivity** section, ensure that the following has been set:
    - Connectivity method: Public endpoint (all networks)

1. In the **Network routing** section, ensure that the following has been set:
    - Routing preference: Microsoft network routing

1. Select the **Next : Data protection** button.

1. In the **Recovery** section, ensure that the following has been set:
	- Enable point-in-time restore for containers: Disable
	- Enable soft delete for blobs: Disable
	- Enable soft delete for containers: Disable
- Enable soft delete for files: Disable

1. Select the **Review and create** button
1. Assuming the Validation has passed, select the **Create** button.
1. After your deployment has completed, continue to the steps in the next section.

### App Service plan

An Azure App Service plan defines a set of compute resources for a web app to run. These compute resources are analogous to the server farm in conventional web hostings. The Azure Function that will be created to provide the Windows Package Manager REST source will operate within this Azure App Service plan allowing it to scale to the demand.

For more information on App Service plans, visit their Docs article: [Azure App Service plan overview](https://docs.microsoft.com/en-us/azure/app-service/overview-hosting-plans).

**How to:**

1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. At the top of the window, select the **+ Create** button.
1. In the search bar, type in "*App Service plan*".
1. Select **App Service plan** from the search results.
1. Select the **Create** button.
1. In the **Project Details** ensure the following values have been set:
    - Subscription: "Contoso Azure Subscription" 
    - Resource Group: "WinGet_PrivateRepo"

1. In the **App Service plan details** ensure that the following values have been set:
    - Name: contoso-asp-demo
    - Operating System: Windows
    - Region: West US

1. In the **Pricing Tier** section, ensure that the following values have been set:
    - Sku and size: Premium V2 P1v2

1. Select the **Review + create** button.
1. Assuming the Validation has passed, select the **Create** button.
1. After your deployment has completed, continue to the steps in the next section.

### Azure Cosmos Database

Azure Cosmos DB is a fully managed platform-as-a-service (PaaS). 

For more information on Cosmos Databases, visit their Docs article: [Welcome to Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction).

#### Azure Cosmos account

The Azure Cosmos account is the fundamental unit of global distribtution and high availability. Your Azure Cosmos account contains a unique DNS name, and can virtually manage an unlimited amount of data and provisioned throughput. We will first create our Azure Cosmos account, before creating the individual Windows Package Manager Azure Cosmos database.

For more information on Azure Cosmos database, visit their Docs article: [Azure Cosmos DB resource model](https://docs.microsoft.com/en-us/azure/cosmos-db/account-databases-containers-items#elements-in-an-azure-cosmos-account)

**How to:**

1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. At the top of the window, select the **+ Create** button.
1. In the search bar, type in "*Azure Cosmos DB*".
1. Select **Azure Cosmos DB** from the search results.
1. Select the **Create** button.
1. Select the **Create** button in relation to *Core (SQL) - Recommended*.
1. In the **Project Details** ensure the following values have been set:
    - Subscription: "Contoso Azure Subscription" 
    - Resource Group: "WinGet_PrivateRepo"

1. In the **Instance Details** section, ensure that the following values have been set:
    - Account Name: contoso-cdba-demo
    - Location: West US
    - Capacity mode: Provisioned throughput

1. Select the **Next: Global Distribution** button.
1. In the **Global Distribution** section, ensure that the following values have been set:
    - Geo-Redundancy: Enabled
    - Multi-region Writes: Enabled

1. Select the **Next: Networking** button.
1. In the **Network connectivity** section, ensure that the following values have been set:
    - Connectivity: Public endpoint (selected networks)

1. In the **Configure Firewall** section, ensure that the following values have been set:
    - Allow access from Azure Portal: Allow
    - Allow access from my IP: Allow

1. Select the **Next: Backup Policy** button.
1. In the **Backup Policy** section, ensure that the following values have been set:
    - Backup policy: Periodic
    - Backup interval: 240
    - Backup retention: 720
    - Backup storage redundancy: Geo-redundant backup storage

1. Select the **Next: Encryption** button.
1. In the **Data Encryption** section, ensure that the following values have been set:
    - Data Encryption: Service-managed key

1. Select the **Review + create** button.
1. Assuming the Validation has passed, select the **Create** button.
1. Wait for the deployment to complete.
1. Select the **Go to Resource** button.
1. Select **Firewall and virtual networks** from the left side navigation.
1. Enable the Exceptions rule **Accept connections from within public Azure datacenters** checkbox.
1. Select the **Save** button.

#### Azure Cosmos database

A single or multiple Azure Cosmos databases can be created under a specific Azure Cosmos account. The Azure Cosmos Database is analogous to a namespace. 

For more information on Azure Cosmos database, visit their Docs article: [Azure Cosmos DB resource model](https://docs.microsoft.com/en-us/azure/cosmos-db/account-databases-containers-items#azure-cosmos-databases)

**How to:**
1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. Select **contoso-cdba-demo** for the Cosmos DB Account in the list of resources.
1. Select **Data Explorer** from the left side navigation.
1. Select the drop-down arrow next to **New Container**
1. Select **New Database** from the drop-down menu.

1. In the **New Database** panel, enter the following information:
	Database id:  WinGet
	Provision throughput: Enable
	Database Throughput (autoscale): AutoScale
	Database Max RU/s: 4000
1. Select the **Ok** button.


#### Azure Cosmos container

An Azure Cosmos container is the unit of scalability both for provisioned throughput and storage. A container is horizontally partitioned and then replicated across multiple regions. This allows for the Windows Package Manager application manifests, and database to span multiple Azure regions.

For more information on Azure Cosmos containers, visit their Docs article: [Azure Cosmos DB resource model](https://docs.microsoft.com/en-us/azure/cosmos-db/account-databases-containers-items#azure-cosmos-containers)

**How to:**
1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. Select **contoso-cdba-demo** for the Cosmos DB Account in the list of resources.
1. Select **Data Explorer** from the left side navigation.
1. Hover over WinGet, and select the ellipses (…).
1. Select **New Container** from the drop-down menu.
1. In the **New Container** panel, enter the following information:
	Database id: Use existing
	Database id (Value): WinGet
	Container id: Manifests
	Partition key: /id
1. Select the **Ok** button.

### Azure Key Vault

An Azure Key Vault centralizes the storage of application secrets, allowing you to control their distribution. The Key Vault greatly reduces the chances that secrets may be accidentally leaked. We will use the Azure Key Vault to securely store specific connection account details that will be used by the Azure Function.

For more information on Azure Key Vault, visit their Docs article: [About Azure Key Vault secrets](https://docs.microsoft.com/en-us/azure/key-vault/secrets/about-secrets)

**How to:**
1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. At the top of the window, select the **+ Create** button.
1. In the search bar, type in "*Key Vault*".
1. Select **Key Vault** from the search results.
1. Select the **Create** button.
1. In the **Project Details** ensure the following values have been set:
    - Subscription: "Contoso Azure Subscription" 
    - Resource Group: "WinGet_PrivateRepo"

1. In the **Instance details** section, ensure that the following values have been set:
    - Key vault name: contoso-keyvault-demo
    - Region: West US
    - Pricing tier: Standard

1. In the **Recovery options** section, ensure that the following values have been set:
    - Soft-delete: Enabled
    - Days to retain deleted vaults: 90
    - Purge Protection: Enable purge protection

1. Select the **Next : Access policy** button.
1. In the  **Access policy** section, esure that the following values have been set:
    - Enable Access to - Azure Virtual Machines for deployment: Enabled
    - Enable Access to - Azure Resource Manager for template deployment: Enabled
    - Enable Access to - Azure Disk Encryption for volume encryption: Enabled
    - Permission model: Vault access policy

1. Select the **Next : Networking** button.
1. In the **Network** section, ensure that the following values have been set:
    - Connectivity method: Public endpoint (all networks)

1. Select the **Review + create** button.
1. Assuming the Validation has passed, select the **Create** button.
1. Wait for the deployment to complete.


#### Azure Key Vault Secrets

Azure Key Vault Secrets provide secure storage of generic secrets, such as passwords and database connection strings. Using the Azure Key Vault previously created, we will create secrets for each of the following:

| Key Vault secret name | Description                                 |
|-----------------------|---------------------------------------------|
| AzStorageAccountKey   | Connection string to Azure Storage Account. |
| CosmosAccountEndpoint | Endpoint                                    |
| CosmosAccountKey      | The Cosmos Database Account Key.            |

With the use of Azure Key Vault, the secrets could be enabled to automatically rotate improving the environments security.

**How to:**

1. Create the **AzStorageAccountKey** Secret in your Azure Key Vault.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **contosostoreaccountdemo** for the Storage account from the list of resources.
    1. Select **Access keys** from the left side navigation.
    1. Select the **Show keys** button at the top of the page.
    1. Copy the text in the **Connection string**.
    1. In the Search bar at the top of the Azure Portal, type **Key vault**, and select **Key vaults** from the drop-down menu.
    1. Select **contoso-keyvault-demo** from the list of Key Vaults.
    1. Select **Secrets** from the left side navigation.
    1. Select **+ Generage/Import** button at the top of the page.
    1. Ensure the **Create a secret** section looks like the following:
        - Upload options: Manual
        - Name: AzStorageAccountKey
        - Value: Paste the Access Key retrieved from the storage account.
        - Enabled: Yes
    1. Select the **Create** button.

1. Create the CosmosAccountEndpoint Secret in your Azure Key Vault.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **cosmos-cdba-demo** for the Cosmos DB Account from the list of resources.
    1. Select **Keys** from the left side navigation.
    1. Copy the value under **URI**.
    1. In the Search bar at the top of the Azure Portal, type **Key vault**, and select **Key vaults** from the drop-down menu.
    1. Select **contoso-keyvault-demo** from the list of Key Vaults.
    1. Select **Secrets** from the left side navigation.
    1. Select **+ Generage/Import** button at the top of the page.
    1. Ensure the **Create a secret** section looks like the following:
        - Upload options: Manual
        - Name: CosmosAccountEndpoint
        - Value: Paste the URI retrieved from the Cosmos DB Account.
        - Enabled: Yes
    1. Select the **Create** button.

1. Create the CosmosAccountKey Secret in your Azure Key Vault.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **cosmos-cdba-demo** for the Cosmos DB Account from the list of resources.
    1. Select **Keys** from the left side navigation.
    1. Copy the value under **Primary Key**.
    1. In the Search bar at the top of the Azure Portal, type **Key vault**, and select **Key vaults** from the drop-down menu.
    1. Select **contoso-keyvault-demo** from the list of Key Vaults.
    1. Select **Secrets** from the left side navigation.
    1. Select **+ Generage/Import** button at the top of the page.
    1. Ensure the **Create a secret** section looks like the following:
        - Upload options: Manual
        - Name: CosmosAccountKey
        - Value: Paste the Primary Key retrieved from the Cosmos DB Account.
        - Enabled: Yes
    1. Select the **Create** button.

### Azure Function

An Azure Function is a serverless solution that allows you to write less code, maintain less infrastructure, and save on costs. This Azure Function will provide the interactive functionality of the Windows Package Manager REST source, responding to REST api requests.

For more information on Azure Functions, visit their Docs article: [Introduction to Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview)

**How to:**

1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. At the top of the window, select the **+ Create** button.
1. In the search bar, type in "*Function App*".
1. Select **Function App** from the search results.
1. Select the **Create** button.
1. In the **Project Details** ensure the following values have been set:
    - Subscription: "Contoso Azure Subscription" 
    - Resource Group: "WinGet_PrivateRepo"

1. In the **Instance Details** section, ensure that the following values have been set:
    - Function App name: contoso-function-demo
    - Publish: Code
    - Runtime stack: .NET
    - Version: 3.1
    - Region: West US
1. Select the **Next** button.
1. In the **Hosting** section, Set the following:
    - Storage Account: contosoaccountdemo
    - Operating System: Windows
    - Plan type: App Service plan
    - Windows Plan (West US): contoso-asp-demo
1. Select the **Next : Monitoring** button.
1. In the **Monitoring** section, set the following:
    - Enable Application Insights: Yes
    - Application Insights: contoso_appinsights_demo
1. Select the **Review + create** button.
1. Assuming the Validation has passed, select the **Create** button.
1. Wait for the deployment to complete before going to the next section.

1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. Select **contoso_function_demo** for the Function from the list of resources.
1. Select **Identity** from the left side navigation.
1. In the **Identity** section, set the following:
    - Status: On
1. Select the **Azure role assignments** button.
1. Select the **+ Add role assignment** button.
1. Select the drop-down menu below **Scope** and select **Key Vault** from the list of options.
1. Select the drop-down menu below **Resource** and select **contoso-keyvault-demo** from the list of options.
1. Select the drop-down menu below **Role** and select **Reader** from the list of options.
1. Select the **Save** button.

1. Select **Access policies** from the left side navigation.
1. Select the **+ Add Access Policy** link.
1. Select **Get** and **List** for Key permissions, Secret permissions, and Certificate permissions.
1. Select the **None selected** link next to **Select principal**.
1. In the search bar, type **contoso_function_demo**.
1. Select **contoso_function_demo** from the results.
1. Select the **Select** button.
1. Select the **Add** button.
1. Select the **Save** button.



*The following instructions assumes that your Key Vault name is: ***contoso-keyvault-demo***. If this does not match to your Azure environment, please update the URI's replacing this with the name of your Azure Key Vault.*

| Application Setting Name                 | Value                                                                                                       |
|------------------------------------------|-------------------------------------------------------------------------------------------------------------|
| AzureWebJobsStorage                      | @Microsoft.KeyVault(SecretUri=https://contoso-keyvault-demo.vault.azure.net/secrets/AzStorageAccountKey/)   |
| CosmosAccountEndpoint                    | @Microsoft.KeyVault(SecretUri=https://contoso-keyvault-demo.vault.azure.net/secrets/CosmosAccountEndpoint/) |
| CosmosAccountKey                         | @Microsoft.KeyVault(SecretUri=https://cosmos-keyvault-demo.vault.azure.net/secrets/CosmosAccountKey/)       |
| FunctionName                             | contoso-function-demo                                                                                       |
| ServerIdentifier                         | contoso-asp-demo                                                                                            |
| CosmosDatabase                           | WinGet                                                                                                      |
| CosmosContainer                          | Manifests                                                                                                   |
| WEBSITE_CONTENTAZUREFILECONNECTIONSTRING | @Microsoft.KeyVault(SecretUri=https://cosmos-keyvault-demo.vault.azure.net/secrets/AzStorageAccountKey/)    |
| WEBSITE_CONTENTSHARE                     | azfun-pkgman3pr-westus-test                                                                                 |
| WEBSITE_LOAD_CERTIFICATES                | *                                                                                                           |
| WEBSITE_RUN_FROM_PACKAGE                 | 1                                                                                                           |

**Complete the following steps for each item listed in the table above.**

1. Configure Application Settings for the Website runs from packages.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **contoso-function-demo** for the Key Vault in the list of resources.
    1. Select **Configuration** from the left side navigation.
    1. Select **+ New application setting** from the **Application settings** section.
    1. In the **Add/Edit application settings** page, enter the following:
        - Name:  [Application Setting Name]
        - Value: [Value]
        - Deployment slot setting: Disabled

#### Import Windows Package Manager API

**How to:**

1. Open a PowerShell window on your computer.

    ``` PowerShell
    PS C:\> $ArchiveFunctionZip = "C:\Projects\winget-cli-restsource\src\WinGet.RestSource.Infrastructure\CompiledFunctions.zip"
    PS C:\> Connect-AzAccount -SubscriptionName "Contoso Azure Subscription"
    PS C:\> Publish-AzWebApp -ArchivePath $ArchiveFunctionZip -ResourceGroupName "WinGet_PrivateRepository" -Name "contoso-function-demo" -Force
    ```

