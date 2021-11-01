# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Remove-WinGetManifest
{
    <#
    .SYNOPSIS
    Removes a Manifest file from the Azure rest source

    .DESCRIPTION
    This function will connect to the Azure Tenant that hosts the Windows Package Manager rest source, removing the 
    specified application Manifest.
        
    The following Azure Modules are used by this script:
        Az.Resources
        Az.Accounts
        Az.Websites
        Az.Functions

    .PARAMETER FunctionName
    Name of the Azure Function that hosts the rest source.

    .PARAMETER ManifestIdentifier
    THe Manifest Id that represents the App Manifest to be removed.

    .PARAMETER SubscriptionName
    [Optional] The Subscription name contains the Windows Package Manager rest source

    .EXAMPLE
    Remove-WinGetManifest -FunctionName "contosoRestSource" -ManifestIdentifier "Windows.PowerToys"

    Connects to Azure, then runs the Azure Function "contosoRestSource" Rest APIs to remove the specified Manifest file from 
    the Windows Package Manager rest source

    #>
    [CmdletBinding(DefaultParameterSetName = 'WinGet')]
    PARAM(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Azure")] [string]$FunctionName,
        [Parameter(Position=2, Mandatory=$true)]  [string]$ManifestIdentifier,
        [Parameter(Position=3, Mandatory=$false)] [string]$SubscriptionName   = ""
    )
    BEGIN
    {
        $Found = $false

        switch ($PsCmdlet.ParameterSetName) {
            "Azure"  {
                ###############################
                ## Validates that the Azure Modules are installed
                Write-Verbose -Message "Testing required PowerShell Modules are installed."
                $RequiredModules = @("Az.Resources", "Az.Accounts", "Az.Websites", "Az.Functions")
                $Result = Test-PowerShellModuleExist -Modules $RequiredModules

                if(!$Result) {
                    throw "Unable to run script, missing required PowerShell modules"
                }

                ###############################
                ## Connects to Azure, if not already connected.
                Write-Verbose -Message "Testing connection to Azure."
                $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
                if(!($Result)) {
                    throw "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials."
                }
                
                ## Sets variables as if the Azure Function Name was provided.
                $AzureResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $FunctionName}).ResourceGroupName

                ###############################
                ##  Verify Azure Resources Exist
                $Result = Test-AzureResource -FunctionName $FunctionName -ResourceGroup $AzureResourceGroupName
                if(!$Result) {
                    throw "Failed to confirm resources exist in Azure. Please verify and try again."
                }

                if($ManifestIdentifier){
                    $ManifestIdentifier = "$ManifestIdentifier"
                }
        
                ###############################
                ##  Rest api call  
                
                ## Specifies the Rest api call that will be performed
                $TriggerName    = "ManifestDelete"
                $apiContentType = "application/json"
                $apiMethod      = "Delete"

                $FunctionApp = Get-AzWebApp -ResourceGroupName $AzureResourceGroupName -Name $FunctionName -ErrorAction SilentlyContinue -ErrorVariable err
        
                ## can function key be part of the header
                $FunctionAppId   = $FunctionApp.Id
                $DefaultHostName = $FunctionApp.DefaultHostName
                $FunctionKey     = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default

                ## Creates the API Post Header
                $apiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
                $apiHeader.Add("Accept", 'application/json')
                $apiHeader.Add("x-functions-key", $FunctionKey)

                $AzFunctionURL   = "https://" + $DefaultHostName + "/api/" + "packageManifests/" + $ManifestIdentifier
            }
        }
    }
    PROCESS
    {
        switch ($PsCmdlet.ParameterSetName) {
            "Azure" {
                Write-Verbose -Message "Retrieving Azure Function Web Applications matching to: $FunctionName."
                Write-Verbose -Message "Constructing the REST API call for removal of manifest."

                $Response = Invoke-RestMethod $AzFunctionURL -Headers $apiHeader -Method $apiMethod -ContentType $apiContentType  -ErrorVariable errInvoke

                if($errInvoke -ne $()) {
                    $ErrorMessage = "Failed to remove Manifest from $FunctionName. Verify the information you provided and try again."
                    $ErrReturnObject = @{
                        AzFunctionURL       = $AzFunctionURL
                        apiHeader           = $apiHeader
                        apiMethod           = $apiMethod
                        apiContentType      = $apiContentType
                        Response            = $Response
                        InvokeError         = $errInvoke
                    }

                    ## If the Post failed, then return User specific error messages:
                    Write-Error -Message $ErrorMessage -Category ResourceUnavailable -TargetObject $ErrReturnObject
                }
            }
        }
    }
    END
    {
        return $Response.Data
    }
}