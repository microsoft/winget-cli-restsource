

Function New-WinGetManifest
{
    <#
    .SYNOPSIS
    Submits Manifest files to the Azure Private REpository
    
    .DESCRIPTION
    By running this function with the required inputs, it will connect to the Azure Tennant that hosts the Windows Package Manager Private Repository, then collects the required URL for Manifest submission before retrieving the contents of the Manifest JSON to submit.
    
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
        [Parameter(Position=2, Mandatory=$false)] [string]$AzureSubscriptionName
    )
    Begin
    {
        ## Identifies if PowerShell session is currently connected to Azure.
        $Result = Get-AzContext

        If($null -eq $Result)
        {
            ## No active connections to Azure
            Write-Host "Not connected to Azure, please connect to your Azure Subscription"
            
            ## Determines that a connection to Azure is neccessary, and if a Subscription Name was provided, connect to that Subscription
            If($AzureSubscriptionName)
                { Connect-AzAccount }
            else 
                { Connect-AzAccount -Subscription $AzureSubscriptionName }
        }

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