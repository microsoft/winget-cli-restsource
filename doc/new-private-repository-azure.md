

# New private repository

## PowerShell script - automated

A PowerShell script is provided inside of the *.\src\WinGet.RestSource.Infrastructure* folder that will simplify the creation of Azure resources to host your own Windows Package Manager private repository.

The `automation.ps1` script has the following parameter inputs:
| Required | Parameter          | Description                                                                                                                |
|----------|--------------------|----------------------------------------------------------------------------------------------------------------------------|
| Yes      | ResourcePrefix     | A string of letters which will be prefixed to your newly created Azure resources.                                          |
| Yes      | Index              | A string of letters or numbers which will be sufixed to your newly created Azure resources.                                |
| Yes      | AzResourceGroup    | The Resource Group that will be used to contain the Azure resources.                                                       |
| No       | AzSubscriptionName | The name of the Azure Subscription that will be used to pay for the Azure resources.                                       |
| No       | AzLocation         | The Azure location where the Azure resources will be created. (Default: westus)                                            |
| No       | WorkingDirectory   | The folder location that contains this the ARM template files, as well as where the Azure Parameter files will be created. |

**How to:**

1. Download a local copy of the Windows Package Manager Rest Source from GitHub (github: [winget-cli-restsource](https://github.com/microsoft/winget-cli-restsource))
    1. Select *Code* from within GitHub.
    1. Select *Download ZIP* from the drop-down menu.
    1. Extract the newly downloaded ZIP file to *C:\Projects\\*

1. Open PowerShell, and run the following command:
    ```powershell
    PS C:\> C:\Projects\winget-cli-restsource\src\WinGet.RestSource.Infrastructure\automation.ps1 -ResourcePrefix "contoso" -Index "demo" -AzResourceGroup "WinGet_PrivateRepository"
    ```

1. When the script completes, it'll display a command that can be used to include the Private repository to your Windows Package Manager client.

## Azure Portal - manually


The following instructions makes the following assumptions:
| Azure Resource          | Value                      |
|-------------------------|----------------------------|
| Azure Resource Group    | WinGet_PrivateRepo         |
| Azure Subscription Name | Contoso Azure Subscription |
| Azure Location          | West US                    |

*We assume that the above values exist inside of your enterprises Azure Tenant.*

### Extract the Windows Package Manager Rest Source

The Windows Package Manager Rest Source contains the required APIs required to provide a Windows Package Manager private repository.

**How to:**

1. Download a local copy of the Windows Package Manager Rest Source from GitHub (github: [winget-cli-restsource](https://github.com/microsoft/winget-cli-restsource))
    1. Select *Code* from within GitHub.
    1. Select *Download ZIP* from the drop-down menu.
    1. Extract the newly downloaded ZIP file to *C:\Projects\\*

### App Insights


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
    - Resource Mode: Workspace-based

1. In the **Worspace Details** enter the following values:
    - Subscription: "Contoso Azure Subscription"
    - Log Analytics Workspace: <"Default value">

1. Select the **Review + create** button at the bottom of the blade.
1. Assuming the Validation as passed, select the **Create** button.
1. After your deployment has completed, continue to the steps in the next section.

### Storage Account


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

1. Select the **Review and create** button
1. Assuming the Validation has passed, select the **Create** button.
1. After your deployment has completed, continue to the steps in the next section.


### App Service Plan


**How to:**

1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. At the top of the window, select the **+ Create** button.
1. In the search bar, type in "*App Service Plan*".
1. Select **App Service Plan** from the search results.
1. Select the **Create** button.
1. In the **Project Details** ensure the following values have been set:
    - Subscription: "Contoso Azure Subscription" 
    - Resource Group: "WinGet_PrivateRepo"

1. In the **App Service Plan details** ensure that the following values have been set:
    - Name: contoso-asp-demo
    - Operating System: Windows
    - Region: West US

1. In the **Pricing Tier** section, ensure that the following values have been set:
    - Sku and size: Premium V2 P1v2

1. Select the **Review + crerate** button.
1. Assuming the Validation has passed, select the **Create** button.
1. After your deployment has completed, continue to the steps in the next section.

### Cosmos Database


#### Cosmos Account


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
    - Connectivity: Private endpoint

1. In the **Configure Firewall** section, ensure that the following values have been set:
    - Allow access from Azure Portal: Allow
    - Allow access from my IP: Deny

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
1. After your deployment has completed, continue to the steps in the next section.


#### Cosmos Database


**How to:**
1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. Select **contoso-cdba-demo** for the Cosmos DB Account in the list of resources.
1. Select **Browse** from the left side navigation.
1. Select **+ Add Collection** at the top of the page.
1. In the **New Container section, enter the following information:
    - Database id: 
        - Create new: Enabled
        - Name: WinGet
        - Database throughput(autoscale): Autoscale
        - Database Max RU/s: 4000
        - Container id: Manifests
        - Partition key: /id
1. Select the **Ok** button.

#### Cosmos Container


**How to:**



### Key Vault

**How to:**

1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. Select *contoso-cdba-demo* from the list of resources.
1. Select the **Create 'Items' container** button next to **Step 1**.

#### Key Vault Secrets



| Key Vault Secrets     | Description                                 |
|-----------------------|---------------------------------------------|
| AzStorageAccountKey   | Connection string to Azure Storage Account. |
| CosmosAccountEndpoint | Endpoint                                    |
| CosmosAccountKey      | The Cosmos Database Account Key.            |

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

1. Select the **Next : Access policy** button.
1. In the  **Access policy** section, esure that the following values have been set:
    - Enable Access to - Azure Virtual Machines for deployment: Enabled
    - Enable Access to - Azure Resource Manager for template deployment: Enabled
    - Enable Access to - Azure Disk Encryption for volume encryption
    - Permission model: Vault access policy

1. Select the **Next : Networking** button.
1. In the **Network** section, ensure that the following values have been set:
    - Connectivity method: Public endpoint (all networks)

1. Select the **Review + create** button.
1. Assuming the Validation has passed, select the **Create** button.
1. Wait for the deployment to complete.

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

### Function


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
    - Plan type: App service plan
    - Windows Plan (West US): contoso-asp-demo
1. Select the **Next : Monitoring** button.
1. In the **Monitoring** section, set the following:
    - Enable Application Insights: Yes
    - Application Insights: contoso_appinsights_demo
1. Select the **Review + create** button.
1. Assuming the Validation has passed, select the **Create** button.
1. Wait for the deployment to complete before going to the next section.

1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. Select **contoso_function_demo** for the Function from the list of resources.
1. Select **Identity** from the left side navigation.
1. In the **Identity** section, set the following:
    - Status: On
    


*The following instructions assumes that your Key Vault name is: contoso-keyvault-demo. If this is incorrect, please update the URI's replacing this with the name of your Azure Key Vault.*

1. Configure Application Settings for the Storage Account Connection Key
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **contoso-function-demo** for the Key Vault in the list of resources.
    1. Select **Configuration** from the left side navigation.
    1. Select **+ New application setting** from the **Application settings** section.
    1. In the **Add/Edit application settings** page, enter the following:
        - Name: AzureWebJobsStorage
        - Value: @Microsoft.KeyVault(SecretUri=https://contoso-keyvault-demo.vault.azure.net/secrets/AzStorageAccountKey/)
        - Deployment slot setting: Disabled

1. Configure Application Settings for the Cosmos SB Account Endpoint.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **contoso-function-demo** for the Key Vault in the list of resources.
    1. Select **Configuration** from the left side navigation.
    1. Select **+ New application setting** from the **Application settings** section.
    1. In the **Add/Edit application settings** page, enter the following:
        - Name: CosmosAccountEndpoint
        - Value: @Microsoft.KeyVault(SecretUri=https://contoso-keyvault-demo.vault.azure.net/secrets/CosmosAccountEndpoint/)
        - Deployment slot setting: Disabled

1. Configure Application Settings for the Cosmos DB Account Key.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **contoso-function-demo** for the Key Vault in the list of resources.
    1. Select **Configuration** from the left side navigation.
    1. Select **+ New application setting** from the **Application settings** section.
    1. In the **Add/Edit application settings** page, enter the following:
        - Name: CosmosAccountKey
        - Value: @Microsoft.KeyVault(SecretUri=https://cosmos-keyvault-demo.vault.azure.net/secrets/CosmosAccountKey/)
        - Deployment slot setting: Disabled

1. Configure Application Settings for the Function Name.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **contoso-function-demo** for the Key Vault in the list of resources.
    1. Select **Configuration** from the left side navigation.
    1. Select **+ New application setting** from the **Application settings** section.
    1. In the **Add/Edit application settings** page, enter the following:
        - Name: FunctionName
        - Value: contoso-function-demo
        - Deployment slot setting: Disabled

1. Configure Application Settings for the Server Identifier.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **contoso-function-demo** for the Key Vault in the list of resources.
    1. Select **Configuration** from the left side navigation.
    1. Select **+ New application setting** from the **Application settings** section.
    1. In the **Add/Edit application settings** page, enter the following:
        - Name: ServerIdentifier
        - Value: contoso-asp-demo
        - Deployment slot setting: Disabled

1. Configure Application Settings for the Website content Azure File Connection String.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **contoso-function-demo** for the Key Vault in the list of resources.
    1. Select **Configuration** from the left side navigation.
    1. Select **+ New application setting** from the **Application settings** section.
    1. In the **Add/Edit application settings** page, enter the following:
        - Name: WEBSITE_CONTENTAZUREFILECONNECTIONSTRING
        - Value: @Microsoft.KeyVault(SecretUri=https://cosmos-keyvault-demo.vault.azure.net/secrets/AzStorageAccountKey/)
        - Deployment slot setting: Disabled

1. Configure Application Settings for the Website content Website Content Share.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **contoso-function-demo** for the Key Vault in the list of resources.
    1. Select **Configuration** from the left side navigation.
    1. Select **+ New application setting** from the **Application settings** section.
    1. In the **Add/Edit application settings** page, enter the following:
        - Name: WEBSITE_CONTENTSHARE
        - Value: azfun-pkgman3pr-westus-test
        - Deployment slot setting: Disabled

1. Configure Application Settings for the Website Load Certificates.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **contoso-function-demo** for the Key Vault in the list of resources.
    1. Select **Configuration** from the left side navigation.
    1. Select **+ New application setting** from the **Application settings** section.
    1. In the **Add/Edit application settings** page, enter the following:
        - Name: WEBSITE_LOAD_CERTIFICATES
        - Value: *
        - Deployment slot setting: Disabled

1. Configure Application Settings for the Website runs from packages.
    1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
    1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
    1. Select **contoso-function-demo** for the Key Vault in the list of resources.
    1. Select **Configuration** from the left side navigation.
    1. Select **+ New application setting** from the **Application settings** section.
    1. In the **Add/Edit application settings** page, enter the following:
        - Name: WEBSITE_RUN_FROM_PACKAGE
        - Value: 1
        - Deployment slot setting: Disabled

#### Import Windows Package Manager API

**How to:**

1. Open a PowerShell window on your computer.

    ``` PowerShell
    PS C:\> $ArchiveFunctionZip = "C:\Projects\winget-cli-restsource\src\WinGet.RestSource.Infrastructure\CompiledFunctions.zip"
    PS C:\> Connect-AzAccount -SubscriptionName "Contoso Azure Subscription"
    PS C:\> Publish-AzWebApp -ArchivePath $ArchiveFunctionZip -ResourceGroupName "WinGet_PrivateRepository" -Name "contoso-function-demo" -Force
    ```

