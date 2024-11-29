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
    Supports input from pipeline by property. The Package Id that represents the App Manifest to be removed.

    .PARAMETER PackageVersion
    [Optional] Supports input from pipeline by property. The Package version that represents the App Manifest to be removed.
    If empty, the whole package (all versions) will be removed.

    .PARAMETER SubscriptionName
    [Optional] The Subscription name that contains the Windows Package Manager REST source

    .EXAMPLE
    Remove-WinGetManifest -FunctionName "contosorestsource" -PackageIdentifier "Windows.PowerToys"

    Connects to Azure, then runs the Azure Function "contosorestsource" REST APIs to remove the specified Manifest file from 
    the Windows Package Manager REST source

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)]  [string]$FunctionName,
        [Parameter(Position=1, Mandatory=$true, ValueFromPipelineByPropertyName=$true)]  [string]$PackageIdentifier,
        [Parameter(Position=2, Mandatory=$false, ValueFromPipelineByPropertyName=$true)] [string]$PackageVersion = "",
        [Parameter(Position=3, Mandatory=$false)] [string]$SubscriptionName = ""
    )
    BEGIN
    {
        [PSCustomObject[]]$Return = @()
        
        ###############################
        ## Connects to Azure, if not already connected.
        Write-Verbose -Message "Validating connection to azure, will attempt to connect if not already connected."
        $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
        if(!($Result)) {
            Write-Error "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials." -ErrorAction Stop
        }

        ###############################
        ## Gets Resource Group name of the Azure Function
        Write-Verbose -Message "Determines the Azure Function Resource Group Name"
        $ResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $FunctionName}).ResourceGroupName
        if(!$ResourceGroupName) {
            Write-Error "Failed to confirm Azure Function exists in Azure. Please verify and try again. Function Name: $FunctionName" -ErrorAction Stop
        }

        ###############################
        ##  Verify Azure Resources Exist
        Write-Verbose -Message "Verifying that the Azure Resource $FunctionName exists.."
        $Result = Test-AzureResource -ResourceName $FunctionName -ResourceGroup $ResourceGroupName
        if(!$Result) {
            Write-Error "Failed to confirm resources exist in Azure. Please verify and try again." -ErrorAction Stop
        }
        
        ###############################
        ##  REST api call  
        
        ## Specifies the REST api call that will be performed
        $TriggerNameManifestDelete = "ManifestDelete"
        $TriggerNameVersionDelete = "VersionDelete"

        $ApiContentType = "application/json"
        $ApiMethod      = "Delete"

        $FunctionApp = Get-AzWebApp -ResourceGroupName $ResourceGroupName -Name $FunctionName
        
        ## can function key be part of the header
        $FunctionAppId   = $FunctionApp.Id
        $DefaultHostName = $FunctionApp.DefaultHostName
        $FunctionKeyManifestDelete = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerNameManifestDelete" -Action listkeys -Force).default
        $FunctionKeyVersionDelete = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerNameVersionDelete" -Action listkeys -Force).default

        ## Creates the API Post Header
        $ApiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $ApiHeader.Add("Accept", 'application/json')

        $AzFunctionURLBase = "https://" + $DefaultHostName + "/api/"
    }
    PROCESS
    {
        Write-Verbose -Message "Constructing the REST API call for removal of manifest."

        $AzFunctionURL = $AzFunctionURLBase
        if([string]::IsNullOrWhiteSpace($PackageVersion)) {
            $AzFunctionURL += "packageManifests/" + $PackageIdentifier
            $ApiHeader.Remove("x-functions-key")
            $ApiHeader.Add("x-functions-key", $FunctionKeyManifestDelete)
        }
        else {
            $AzFunctionURL += "packages/" + $PackageIdentifier + "/versions/" + $PackageVersion
            $ApiHeader.Remove("x-functions-key")
            $ApiHeader.Add("x-functions-key", $FunctionKeyVersionDelete)
        }

        $Response = Invoke-RestMethod $AzFunctionURL -Headers $ApiHeader -Method $ApiMethod -ContentType $ApiContentType -ErrorVariable ErrorInvoke

        if($ErrorInvoke) {
            $ErrorMessage = "Failed to remove Manifest from $FunctionName. Verify the information you provided and try again."
            $ErrReturnObject = @{
                AzFunctionURL       = $AzFunctionURL
                ApiMethod           = $ApiMethod
                ApiContentType      = $ApiContentType
                Response            = $Response
                InvokeError         = $ErrorInvoke
            }

            Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
        }
        else
        {
            $Return += [PSCustomObject]@{
                PackageIdentidier = $PackageIdentifier
                PackageVersion = $PackageVersion
            }
        }
    }
    END
    {
        return $Return
    }
}