
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
    Submits Manifest files to the Azure Private Repository
    
    .DESCRIPTION
    By running this function with the required inputs, it will connect to the Azure Tennant that hosts the Windows Package Manager Private Repository, then collects the required URL for Manifest submission before retrieving the contents of the Manifest JSON to submit.
        
    The following Azure Modules are used by this script:
        Az.Resources --> Invoke-AzResourceAction
        Az.Accounts  --> Connect-AzAccount, Get-AzContext
        Az.Websites  --> Get-AzWebapp
        Az.Functions --> Get-AzFunctionApp
    
    .PARAMETER PrivateRepoName
    Name of the Windows Package Manager Private repository. Can be identified by running: "winget source list" and using the Repository Name
    
    .PARAMETER AzureFunctionName
    Name of the Azure Function that hosts the Private Repository
    
    .PARAMETER ManifestFilePath
    Path to the JSON manifest file that will be uploaded to the Private Repository
    
    .PARAMETER AzureSubscriptionName
    [Optional] The Subscription name contains the Windows Package Manager Private Repository
    
    .EXAMPLE
    New-WinGetManifest -PrivateRepoName "Private" -ManifestFilePath "C:\Temp\App.json"

    .EXAMPLE
    New-WinGetManifest -PrivateRepoName "Private" -ManifestFilePath "C:\Temp\App.json" -AzureSubscriptionName "Subscription"
    
    .EXAMPLE
    New-WinGetManifest AzureFunctionName "contoso-function-prod" -ManifestFilePath "C:\Temp\App.json"

    .EXAMPLE
    New-WinGetManifest AzureFunctionName "contoso-function-prod" -ManifestFilePath "C:\Temp\App.json" -AzureSubscriptionName "Subscription"

    .NOTES
    General notes
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

        Write-Host "`nAzure Resources:"
        Write-Host "  Azure Function Name: $AzureFunctionName"
        Write-Host "  Azure Resource Group Name: $AzureResourceGroupName"

        $TriggerName      = "ManifestPost"
        $apiContentType   = "application/json"
        $apiMethod        = "Post"

        ## Creates the API Post Header
        $apiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $apiHeader.Add("Accept", 'application/json')

        ## Gets the JSON Content for posting to Private Repo
        $ApplicationManifest = Get-Content -Path $ManifestFilePath -Raw
        $ApplicationManifest = $($ApplicationManifest -replace "`t|`n|`r|  ","")
        $ApplicationManifest = $($($($($ApplicationManifest -replace ": ",":") -replace " { |{ ", "{") -replace ', ', ',') -replace " } | }", "}")
    }
    Process
    {
        ## Retrieves the Azure Function URL used to add new manifests to the Private Repository
        $FunctionApp = Get-AzWebApp -ResourceGroupName $AzureResourceGroupName -Name $AzureFunctionName
        $FunctionAppId = $FunctionApp.Id
        $DefaultHostName = $FunctionApp.DefaultHostName
        $FunctionKey = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default
        $AzFunctionURL = "https://" + $DefaultHostName + "/api/" + "packageManifests" + "?code=" + $FunctionKey
        
        

        #Write-Host "`n`nInvoke-RestMethod ""$AzFunctionURL"" `n`t-Headers $apiHeader `n`t-Method ""$apiMethod"" `n`t-Body ""$ApplicationManifest"" `n`t-ContentType ""$apiContentType"""

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