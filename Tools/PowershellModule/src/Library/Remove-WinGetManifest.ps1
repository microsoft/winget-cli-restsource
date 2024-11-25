# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Remove-WinGetManifest
{
    <#
    .SYNOPSIS
    Removes a Manifest file from the Azure REST source

    .DESCRIPTION
    This function will connect to the Azure Tenant that hosts the Windows Package Manager REST source, removing the 
    specified package Manifest.

    .PARAMETER FunctionName
    Name of the Azure Function that hosts the REST source.

    .PARAMETER PackageIdentifier
    The Package Id that represents the App Manifest to be removed.

    .PARAMETER SubscriptionName
    [Optional] The Subscription name that contains the Windows Package Manager REST source

    .EXAMPLE
    Remove-WinGetManifest -FunctionName "contosorestsource" -PackageIdentifier "Windows.PowerToys"

    Connects to Azure, then runs the Azure Function "contosorestsource" REST APIs to remove the specified Manifest file from 
    the Windows Package Manager REST source

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)]  [string]$FunctionName,
        [Parameter(Position=2, Mandatory=$true)]  [string]$PackageIdentifier,
        [Parameter(Position=2, Mandatory=$false)] [string]$PackageVersion = "",
        [Parameter(Position=3, Mandatory=$false)] [string]$SubscriptionName = ""
    )
    BEGIN
    {
        ###############################
        ## Connects to Azure, if not already connected.
        Write-Verbose -Message "Testing connection to Azure."
        $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
        if(!($Result)) {
            throw "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials."
        }
        
        ## Sets variables as if the Azure Function Name was provided.
        $ResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $FunctionName}).ResourceGroupName

        ###############################
        ##  Verify Azure Resources Exist
        $Result = Test-AzureResource -ResourceName $FunctionName -ResourceGroup $ResourceGroupName
        if(!$Result) {
            throw "Failed to confirm resources exist in Azure. Please verify and try again."
        }

        if($PackageIdentifier){
            $PackageIdentifier = "$PackageIdentifier"
        }
        
        ###############################
        ##  REST api call  
        
        ## Specifies the REST api call that will be performed
        $TriggerName = ""
        if([string]::IsNullOrWhiteSpace($PackageVersion)) {
            $TriggerName = "ManifestDelete"
        }
        else {
            $TriggerName = "VersionDelete"
        }

        $ApiContentType = "application/json"
        $ApiMethod      = "Delete"

        $FunctionApp = Get-AzWebApp -ResourceGroupName $ResourceGroupName -Name $FunctionName
        
        ## can function key be part of the header
        $FunctionAppId   = $FunctionApp.Id
        $DefaultHostName = $FunctionApp.DefaultHostName
        $FunctionKey     = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default

        ## Creates the API Post Header
        $ApiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $ApiHeader.Add("Accept", 'application/json')
        $ApiHeader.Add("x-functions-key", $FunctionKey)

        $AzFunctionURL = ""
        if([string]::IsNullOrWhiteSpace($PackageVersion)) {
            $AzFunctionURL = "https://" + $DefaultHostName + "/api/packageManifests/" + $PackageIdentifier
        }
        else {
            $AzFunctionURL = "https://" + $DefaultHostName + "/api/packages/" + $PackageIdentifier + "/versions/" + $PackageVersion
        }
    }
    PROCESS
    {
        Write-Verbose -Message "Retrieving Azure Function Web Applications matching to: $FunctionName."
        Write-Verbose -Message "Constructing the REST API call for removal of manifest."

        $Response = Invoke-RestMethod $AzFunctionURL -Headers $ApiHeader -Method $ApiMethod -ContentType $ApiContentType -ErrorVariable ErrorInvoke

        if($ErrorInvoke) {
            $ErrorMessage = "Failed to remove Manifest from $FunctionName. Verify the information you provided and try again."
            $ErrReturnObject = @{
                AzFunctionURL       = $AzFunctionURL
                ApiHeader           = $ApiHeader
                ApiMethod           = $ApiMethod
                ApiContentType      = $ApiContentType
                Response            = $Response
                InvokeError         = $ErrorInvoke
            }

            ## If the Post failed, then return User specific error messages:
            Write-Error -Message $ErrorMessage -Category ResourceUnavailable -TargetObject $ErrReturnObject
        }
    }
    END
    {
        return $Response.Data
    }
}