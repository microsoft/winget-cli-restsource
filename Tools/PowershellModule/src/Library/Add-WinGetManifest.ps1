# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Add-WinGetManifest
{
    <#
    .SYNOPSIS
    Submits a Manifest file to the Azure REST source

    .DESCRIPTION
    By running this function with the required inputs, it will connect to the Azure Tenant that hosts the 
    Windows Package Manager REST source, then collects the required URL for Manifest submission before 
    retrieving the contents of the Package Manifest to submit.

    .PARAMETER FunctionName
    Name of the Azure Function that hosts the REST source.

    .PARAMETER Path
    Supports input from pipeline. The path to the Package Manifest file or folder hosting either a JSON or YAML file(s) that will be uploaded to the REST source. 
    This path may contain a single Package Manifest file, or a folder containing files for a single Package Manifest. Does not support 
    targeting a single folder of multiple different applications.

    .PARAMETER SubscriptionName
    [Optional] The Subscription name contains the Windows Package Manager REST source

    .EXAMPLE
    Add-WinGetManifest -FunctionName "contosorestsource" -Path "C:\AppManifests\Microsoft.PowerToys\PowerToys.json"

    Connects to Azure, then runs the Azure Function "contosorestsource" REST APIs to add the specified Manifest file (*.json) 
    to the Windows Package Manager REST source

    .EXAMPLE
    Add-WinGetManifest -FunctionName "contosorestsource" -Path "C:\AppManifests\Microsoft.PowerToys\"

    Connects to Azure, then runs the Azure Function "contosorestsource" REST APIs to adds the Manifest file(s) (*.json / *.yaml) 
    found in the specified folder to the Windows Package Manager REST source
    
    .EXAMPLE
    Add-WinGetManifest -FunctionName "contosorestsource" -Path "C:\AppManifests\Microsoft.PowerToys\PowerToys.json" -SubscriptionName "Visual Studio Subscription"

    Connects to Azure and the specified Subscription, then runs the Azure Function "contosorestsource" REST APIs to add the 
    specified Manifest file (*.json) to the Windows Package Manager REST source.
    #>

    PARAM(
        [Parameter(Position=0, Mandatory=$true)]  [string]$FunctionName,
        [Parameter(Position=1, Mandatory=$true, ValueFromPipeline=$true)] [string]$Path,
        [Parameter(Position=2, Mandatory=$false)] [string]$SubscriptionName = ""
    )
    BEGIN
    {
        [WinGetManifest[]] $Return = @()
        
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

        #############################################
        ##############  REST api call  ##############

        ## Specifies the REST api call that will be performed
        $ApiContentType = "application/json"
        $ApiMethodPost  = "Post"
        $ApiMethodGet   = "Get"
        $ApiMethodPut   = "Put"

        ## Retrieves the Azure Function URL used to add new manifests to the REST source
        Write-Verbose -Message "Retrieving the Azure Function $FunctionName to build out the REST API request."
        $FunctionApp = Get-AzFunctionApp -ResourceGroupName $ResourceGroupName -Name $FunctionName

        $FunctionAppId   = $FunctionApp.Id
        $DefaultHostName = $FunctionApp.DefaultHostName
        $FunctionKeyPost = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/ManifestPost" -Action listkeys -Force).default
        $FunctionKeyGet = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/ManifestGet" -Action listkeys -Force).default
        $FunctionKeyPut = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/ManifestPut" -Action listkeys -Force).default
        
        
        ## Creates the API Post Header
        $ApiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $ApiHeader.Add("Accept", 'application/json')
        
        $AzFunctionURLBase   = "https://" + $DefaultHostName + "/api/packageManifests/"
    }
    PROCESS
    {
        ###############################
        ## Gets the content from the Package Manifest (*.JSON, or *.YAML) file for posting to REST source.
        Write-Verbose -Message "Retrieving a copy of the app Manifest file for submission to WinGet source."
        $ApplicationManifest = Get-WinGetManifest -Path $Path
        if($ApplicationManifest.Count -ne 1) {
            Write-Error "Failed to retrieve a proper manifest. Verify and try again."
            return
        }

        $Manifest = $ApplicationManifest[0]
        Write-Verbose -Message "Contents of manifest have been retrieved. Package Identifier: $($Manifest.PackageIdentifier)."
        
        Write-Verbose -Message "Confirming that the Package ID doesn't already exist in Azure for $($Manifest.PackageIdentifier)."
        $ApiHeader.Add["x-functions-key"] = $FunctionKeyGet
        $AzFunctionURL = $AzFunctionURLBase + $Manifest.PackageIdentifier
        $Response = Invoke-RestMethod $AzFunctionURL -Headers $ApiHeader -Method $ApiMethodGet -ErrorVariable ErrorInvoke

        if ($ErrorInvoke) {
            ## No existing manifest retrieved, submit as new manifest
            Write-Verbose "No manifest that matched. Package Identifier: $($Manifest.PackageIdentifier)"

            $ApiMethod = $ApiMethodPost
            $AzFunctionURL = $AzFunctionURLBase
            $ApiHeader["x-functions-key"] = $FunctionKeyPost
        }
        else {
            ## Existing manifest retrieved, submit as update existing manifest
            Write-Verbose "Found manifest that matched. Package Identifier: $($Manifest.PackageIdentifier)"
            
            if($Response.Data.Count -ne 1) {
                Write-Error "Found conflicting manifests. Package Identifier: $($Manifest.PackageIdentifier)"
                return
            }
            
            $ApiMethod = $ApiMethodPut
            $AzFunctionURL = $AzFunctionURLBase + $Manifest.PackageIdentifier
            $ApiHeader["x-functions-key"] = $FunctionKeyPut
        }
        
        Write-Verbose -Message "The Manifest will be added using the $ApiMethod REST API."

        $Response = Invoke-RestMethod $AzFunctionURL -Headers $ApiHeader -Method $ApiMethod -Body $Manifest.GetJson() -ContentType $ApiContentType -ErrorVariable ErrorInvoke

        if($ErrorInvoke) {
            $ErrReturnObject = @{
                AzFunctionURL       = $AzFunctionURL
                ApiMethod           = $ApiMethod
                ApiContentType      = $ApiContentType
                ApplicationManifest = $Manifest.GetJson()
                Response            = $Response
                InvokeError         = $ErrorInvoke
            }

            Write-Error -Message "Failed to add manifest." -TargetObject $ErrReturnObject
        }
        else {
            if ($Response.Data.Count -ne 1) {
                Write-Warning "Returned conflicting manifests after adding the manifest. Package Identifier: $($Manifest.PackageIdentifier)"
            }
            
            foreach ($ResponseData in $Response.Data){
                Write-Verbose "Parsing through the returned results: $ResponseData"
                $Return += [WinGetManifest]::CreateFromObject($ResponseData)
            }
        }
    }
    END
    {
        return $Return
    }
}