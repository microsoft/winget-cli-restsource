---
Description: Details how to create a Windows Package Manager private repository.
title: Windows Package Manager private repository
ms.date: 08/30/2021
ms.topic: article
keywords: windows, package manager, windows package manager, private repo, private repository
ms.localizationpriority: medium
---

# Windows Package Mananger private repository

The Windows Package Manager provides a distribution channel for software packages containing their tools and applications. The instructions contained within this document provide guidance on how to setup a private respository that can be connected to using the Windows Package Manager. Providing a comprehensive package manager solution that consists of a command line tool and a set of services for installing applications on Windows 10.

## Automatically create a private repository

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

The PowerShell script has been configured to work with `Get-Help` providing further details about the script, as well as examples of how to use it.

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

## Manually create a private repository

The following instructions will provide a walkthrough on how to manually create a Windows Package Manager private repository in Azure. Each section will provide you with details pertaining to the specific object being created, and the functionality that it will provide.

The following instructions assumes the following Azure objects are named as follows:
| Azure Resource          | Value                      |
|-------------------------|----------------------------|
| Azure Resource Group    | WinGet_PrivateRepo         |
| Azure Subscription Name | Contoso Azure Subscription |
| Azure Location          | West US                    |
| Azure Key Vault Name    | contoso-keyvault-demo      |

*We assume that the Resource Group and Subscription are pre-existing in your Azure Tenant.*

### Extract the Windows Package Manager Rest Source

The Windows Package Manager Rest Source contains the APIs required to provide a Windows Package Manager private repository.

**How to:**

1. Download a local copy of the Windows Package Manager Rest Source from GitHub (github: [winget-cli-restsource](https://github.com/microsoft/winget-cli-restsource))
    1. Select *Code* from within GitHub.
    1. Select *Download ZIP* from the drop-down menu.
    1. Extract the newly downloaded ZIP file to *C:\Projects\\*

### Application Insights

Application Insights, a feature of Azure Monitor, is an extensible Application Performance Management (APM) service for developers and DevOps professionals. Azure's Application Insights is will be used to monitor the health of the Windows Package Manager private repository, as well as provide powerful analytical insights to help with diagnosing any issues, and identify user experiences.

For more information on Azure Application Insights, visit their Docs site: [What is Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview).

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

An Azure storage account contains all of your Azure Storage data objects: blobs, file shares, queues, tables, and disks. The Storage account created below will be used to store the Azure Function binaries, 

For more information on Azure Storage Accounts, visit their Docs site: [Storage account overview](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-overview).

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


### App Service plan

An Azure App Service plan defines a set of compute resources for a web app to run. These compute resources are analogous to the server farm in conventional web hostings. The Azure Function that will be created to provide the Windows Package Manager private repository will operate within this Azure App Service plan allowing it to scale to the demand.

For more information on App Service plans, visit their Docs site: [Azure App Service plan overview](https://docs.microsoft.com/en-us/azure/app-service/overview-hosting-plans).

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

1. Select the **Review + crerate** button.
1. Assuming the Validation has passed, select the **Create** button.
1. After your deployment has completed, continue to the steps in the next section.

### Azure Cosmos Database

Azure Cosmos DB is a fully managed platform-as-a-service (PaaS). 

For more information on Cosmos Databases, visit their Docs site: [Welcome to Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction).

#### Azure Cosmos account

The Azure Cosmos account is the fundamental unit of global distribtution and high availability. Your Azure Cosmos account contains a unique DNS name, and can virtually manage an unlimited amount of data and provisioned throughput. We will first create our Azure Cosmos account, before creating the individual Windows Package Manager Azure Cosmos database.

For more information on Azure Cosmos database, visit their Docs site: [Azure Cosmos DB resource model](https://docs.microsoft.com/en-us/azure/cosmos-db/account-databases-containers-items#elements-in-an-azure-cosmos-account)

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


#### Azure Cosmos database

A single or multiple Azure Cosmos databases can be created under a specific Azure Cosmos account. The Azure Cosmos Database is analogous to a namespace. 

For more information on Azure Cosmos database, visit their Docs site: [Azure Cosmos DB resource model](https://docs.microsoft.com/en-us/azure/cosmos-db/account-databases-containers-items#azure-cosmos-databases)

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

#### Azure Cosmos container

An Azure Cosmos container is the unit of scalability both for provisioned throughput and storage. A container is horizontally partitioned and then replicated across multiple regions. This allows for the Windows Package Manager application manifests, and database to span multiple Azure regions.

For more information on Azure Cosmos containers, visit their Docs site: [Azure Cosmos DB resource model](https://docs.microsoft.com/en-us/azure/cosmos-db/account-databases-containers-items#azure-cosmos-containers)

**How to:**

N/A

### Azure Key Vault

An Azure Key Vault centralizes the storage of application secrets, allowing you to control their distribution. The Key Vault greatly reduces the chances that secrets may be accidentally leaked. We will use the Azure Key Vault to securely store specific connection account details that will be used by the Azure Function.

For more information on Azure Key Vault, visit their Docs site: [About Azure Key Vault secrets](https://docs.microsoft.com/en-us/azure/key-vault/secrets/about-secrets)

**How to:**

1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. Select *contoso-cdba-demo* from the list of resources.
1. Select the **Create 'Items' container** button next to **Step 1**.

#### Azure Key Vault Secrets

Azure Key Vault Secrets provide secure storage of generic secrets, such as passwords and database connection strings. Using the Azure Key Vault previously created, we will create secrets for each of the following:

| Key Vault secret name | Description                                 |
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

### Azure Function

An Azure Function is a serverless solution that allows you to write less code, maintain less infrastructure, and save on costs. This Azure Function will provide the interactive functionality of the Windows Package Manager private repository, responding to rest api requests.

For more information on Azure Functions, visit their Docs site: [Introduction to Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview)

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

1. Open your Microsoft Edge browser, and navigate to your Azure Portal ([https://portal.azure.com](https://portal.azure.com))
1. In the search bar at the top of the Azure Portal, type *Resource Groups* and select **Resource groups** from the drop down.
1. Select your Resource Group ("*WinGet_PrivateRepo*") from the list.
1. Select **contoso_function_demo** for the Function from the list of resources.
1. Select **Identity** from the left side navigation.
1. In the **Identity** section, set the following:
    - Status: On

*The following instructions assumes that your Key Vault name is: ***contoso-keyvault-demo***. If this does not match to your Azure environment, please update the URI's replacing this with the name of your Azure Key Vault.*

| Application Setting Name                 | Value                                                                                                       |
|------------------------------------------|-------------------------------------------------------------------------------------------------------------|
| AzureWebJobsStorage                      | @Microsoft.KeyVault(SecretUri=https://contoso-keyvault-demo.vault.azure.net/secrets/AzStorageAccountKey/)   |
| CosmosAccountEndpoint                    | @Microsoft.KeyVault(SecretUri=https://contoso-keyvault-demo.vault.azure.net/secrets/CosmosAccountEndpoint/) |
| CosmosAccountKey                         | @Microsoft.KeyVault(SecretUri=https://cosmos-keyvault-demo.vault.azure.net/secrets/CosmosAccountKey/)       |
| FunctionName                             | contoso-function-demo                                                                                       |
| ServerIdentifier                         | contoso-asp-demo                                                                                            |
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

