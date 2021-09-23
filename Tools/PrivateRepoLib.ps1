<#
Created On: 2021-09-22
Created By: Roy MacLachlan

Description:
This library can be dot loaded into memory allowing for access to pre-created functions enabling an
easier Windows Package Manager private source manifest experience. The functions in this file support
Adding a new Manifest to your Windows Package Manager private source in Azure.

After dot loading this file, it'll complete a validation check to ensure you have the required Azure
manifests installed. If any manifest is missing, instructions on how to install will be displayed to
the screen.

Example:
To dot load this file into memory:
. .\PrivateRepoLib.ps1
#>

class ARMObject
{
    [ValidateSet("AppInsight", "Keyvault", "StorageAccount", "asp", "CosmosDBAccount", "CosmosDBDatabase", "CosmosDBContainer", "Function", "FrontDoor")]
    [ValidateNotNullOrEmpty()][string] $ObjectType
    [ValidateNotNullOrEmpty()][string] $ParameterPath
    [ValidateNotNullOrEmpty()][string] $TemplatePath
    $Parameters = @{
        '$Schema' = "1.0.0.0"
        contentVersion = "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#"
        Parameters = @{}
    }

    ARMObject ([System.Collections.Hashtable]$Var)
    {
        $this.ObjectType    = $Var.ObjectType
        $this.ParameterPath = $Var.ParameterPath
        $this.TemplatePath  = $Var.TemplatePath
        $this.Parameters.contentVersion = $Var.TemplatePath.contentVersion
        $this.Parameters.Parameters     = $Var.TemplatePath.Parameters
        IF($null -ne $Var.TemplatePath.'$Schema')
            { $this.Parameters.'$Schema'      = $Var.TemplatePath.'$Schema' }
        IF($null -ne $Var.TemplatePath.contentVersion)
            { $this.Parameters.contentVersion = $Var.TemplatePath.contentVersion }
        IF($null -ne $Var.TemplatePath.Parameters)
            { $this.Parameters.Parameters     = $Var.TemplatePath.Parameters }
    }
    ARMObject ([string] $a, [string] $b, [string] $c)
    {
        $this.ObjectType    = $a
        $this.ParameterPath = $b
        $this.TemplatePath  = $c
    }
    ARMObject ([string] $a, [string] $b, [string] $c, $d)
    {
        $this.ObjectType    = $a
        $this.ParameterPath = $b
        $this.TemplatePath  = $c
        $this.Parameters    = $d
    }
    [boolean]TestParameterPath()
    {
        Return Test-Path -Path $this.ParameterPath
    }
    [boolean]TestTemplatePath()
    {
        Return Test-Path -Path $this.TemplatePath
    }
}

Function Test-RequiredModules
{
    Param(
        [Parameter(Position=0, Mandatory=$true)] [string]$RequiredModule
    )
    Begin
    {}
    Process
    {
        ## Determines if the PowerShell Module is missing, If missing Returns the name of the missing module
        IF(!$(Get-Module -ListAvailable -Name $RequiredModule) )
            { $Result = $RequiredModule }
    }
    End
    {
        ## Returns a value only if the module is missing
        Return $Result
    }
}


Function New-WinGetManifest
{
    <#
    .SYNOPSIS
    Submits Manifest files to the Azure private source
    
    .DESCRIPTION
    By running this function with the required inputs, it will connect to the Azure Tennant that hosts the Windows Package Manager private source, then collects the required URL for Manifest submission before retrieving the contents of the Manifest JSON to submit.
        
    The following Azure Modules are used by this script:
        Az.Resources --> Invoke-AzResourceAction
        Az.Accounts  --> Connect-AzAccount, Get-AzContext
        Az.Websites  --> Get-AzWebapp
        Az.Functions --> Get-AzFunctionApp
    
    .PARAMETER PrivateRepoName
    Name of the Windows Package Manager private source. Can be identified by running: "winget source list" and using the Repository Name
    
    .PARAMETER AzureFunctionName
    Name of the Azure Function that hosts the private source
    
    .PARAMETER ManifestFilePath
    Path to the JSON manifest file that will be uploaded to the private source
    
    .PARAMETER AzureSubscriptionName
    [Optional] The Subscription name contains the Windows Package Manager private source
    
    .EXAMPLE
    New-WinGetManifest -PrivateRepoName "Private" -ManifestFilePath "C:\Temp\App.json"

    .EXAMPLE
    New-WinGetManifest -PrivateRepoName "Private" -ManifestFilePath "C:\Temp\App.json" -AzureSubscriptionName "Subscription"
    
    .EXAMPLE
    New-WinGetManifest AzureFunctionName "contoso-function-prod" -ManifestFilePath "C:\Temp\App.json"

    .EXAMPLE
    New-WinGetManifest AzureFunctionName "contoso-function-prod" -ManifestFilePath "C:\Temp\App.json" -AzureSubscriptionName "Subscription"
    #>

    [CmdletBinding(DefaultParameterSetName = 'WinGet')]
    Param(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="WinGet")] [string]$PrivateRepoName,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Azure")]  [string]$AzureFunctionName,
        [Parameter(Position=1, Mandatory=$true)]  [string]$ManifestFilePath,
        [Parameter(Position=2, Mandatory=$false)] [string]$AzureSubscriptionName = ""
    )
    Begin
    {
        ##################################################
        ############  Verifies PreRequisites  ############

        ## List of the required Azure modules.
        $RequiredModules = @("Az.Resources", "Az.Accounts", "Az.Websites", "Az.Functions")
        $Result = @()
        foreach( $RequiredModule in $RequiredModules )
            { $Result += Test-RequiredModules -RequiredModule $RequiredModule }
        
        ## If a module is determined to be missing, throw an error
        If($Result)
        {
            ## Modules have been identified as missing
            $ErrorMessage = "`nMissing required PowerShell modules`n"
            $ErrorMessage += "    Run the following command to install the missing modules: Install-Module Az`n`n"
            
            Write-Host $ErrorMessage -ForegroundColor Yellow
            Throw "Unable to run script, missing required PowerShell modules"
        }

        #############################################
        ############  Connects to Azure  ############

        ## Identifies if PowerShell session is currently connected to Azure.
        $Result = Get-AzContext

        If($null -eq $Result)
        {
            ## No active connections to Azure
            Write-Host "Not connected to Azure, please connect to your Azure Subscription"
            
            ## Determines that a connection to Azure is neccessary, and if a Subscription Name was provided, connect to that Subscription
            If($AzureSubscriptionName -eq "")
                { Connect-AzAccount }
            else 
                { Connect-AzAccount -SubscriptionName $AzureSubscriptionName }

            ## If the connection fails, or the user cancels the login request, then throw an error.
            $Result = Get-AzContext
            If($null -eq $Result)
                { Throw "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials." }
        }

        #############################################
        ##############  Parameter Set  ##############
        
        ## Determines the PowerShell Parameter Set that was used in the call of this Function.
        switch ($PsCmdlet.ParameterSetName) {
            "WinGet" { 
                ## Sets variables as if the Windows Package Manager was Private Repo Name was specified.
                $AzureFunctionName      = $(Winget source list -n $PrivateRepoName)[4].split("/")[2].Split(".")[0]
                $AzureResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $AzureFunctionName}).ResourceGroupName
             }
            "Azure"  { 
                ## Sets variables as if the Azure Function Name was provided.
                $AzureResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $AzureFunctionName}).ResourceGroupName
             }
        }

        #############################################
        #############  Verify Resources  ############

        $AzureFunctionExists = $(Get-AzFunctionApp).Where({$_.Name -eq $AzureFunctionName}).Count

        ## If there is an error with the value, it will be shown as "Red". If no issues then display the text as green.
        Write-Host "`nAzure Resources:"
        Write-Host "  Azure Function:       " -NoNewline; If($AzureFunctionExists)   { Write-Host $AzureFunctionName      -ForegroundColor Green }Else{ IF($AzureFunctionName){ Write-Host "$AzureFunctionName" -ForegroundColor Red }Else{ Write-Host "<null>" -ForegroundColor Red }}
        Write-Host "  Azure Resource Group: " -NoNewline; If($AzureResourceGroupName){ Write-Host $AzureResourceGroupName -ForegroundColor Green }Else{ Write-Host "<null>" -ForegroundColor Red }

        ## If either the Azure Function Name or the Azure Resource Group Name are null, error.
        IF(!$AzureFunctionName -or !$AzureResourceGroupName -or !$AzureFunctionExists)
        {
            Write-Host "`nERROR: Both the Azure Function and Resource Group names can not be null. Please verify that the Azure function exists.`n" -ForegroundColor Red
            Return $false
        }

        ## Gets the JSON Content for posting to Private Repo
        Write-Host "Retrieving the Application Manifest"
        $ApplicationManifest = Get-Content -Path $ManifestFilePath -Raw
        $ApplicationManifest = $($ApplicationManifest -replace "`t|`n|`r|  ","")
        $ApplicationManifest = $($($($($ApplicationManifest -replace ": ",":") -replace " { |{ ", "{") -replace ', ', ',') -replace " } | }", "}")

        IF(!$ApplicationManifest)
        {
            Write-Host "  ERROR: Application Manifest is empty." -ForegroundColor Red
            Return $false
        }

        #############################################
        ##############  Rest api call  ##############

        ## Specifies the Rest api call that will be performed
        $TriggerName      = "ManifestPost"
        $apiContentType   = "application/json"
        $apiMethod        = "Post"

        ## Creates the API Post Header
        $apiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $apiHeader.Add("Accept", 'application/json')
    }
    Process
    {
        ## Retrieves the Azure Function URL used to add new manifests to the private source
        $FunctionApp = Get-AzWebApp -ResourceGroupName $AzureResourceGroupName -Name $AzureFunctionName -ErrorAction SilentlyContinue -ErrorVariable err

        $FunctionAppId = $FunctionApp.Id
        $DefaultHostName = $FunctionApp.DefaultHostName
        $FunctionKey = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default
        $AzFunctionURL = "https://" + $DefaultHostName + "/api/" + "packageManifests" + "?code=" + $FunctionKey
        
        ## Publishes the Manifest to the Windows Package Manager private source
        $Response = Invoke-RestMethod $AzFunctionURL -Headers $apiHeader -Method $apiMethod -Body $ApplicationManifest -ContentType $apiContentType
    }
    End
    {
        Return $Response
    }

}


## Validates that the required Azure Modules are present when the script is imported.
$RequiredModules = @("Az.Resources", "Az.Accounts", "Az.Websites", "Az.Functions")
$Result = @()
foreach( $RequiredModule in $RequiredModules )
    { $Result += Test-RequiredModules -RequiredModule $RequiredModule }

## If a module is determined to be missing, throw an error
If($Result)
{
    ## Modules have been identified as missing
    $ErrorMessage = "`nThere are missing PowerShell modules that must be installed.`n"
    $ErrorMessage += "    Some or all PowerShell functions included in this library will fail.`n"
    $ErrorMessage += "    Run the following command to install the missing modules: Install-Module Az -Force`n`n"
    
    Write-Host $ErrorMessage -ForegroundColor Yellow
}