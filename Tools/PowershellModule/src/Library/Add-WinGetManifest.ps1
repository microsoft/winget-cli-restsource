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
    retrieving the contents of the Application Manifest to submit.

    The following Azure Modules are used by this script:
        Az.Resources --> Invoke-AzResourceAction
        Az.Accounts  --> Connect-AzAccount, Get-AzContext
        Az.Websites  --> Get-AzWebapp
        Az.Functions --> Get-AzFunctionApp

    .PARAMETER FunctionName
    Name of the Azure Function that hosts the REST source.

    .PARAMETER Path
    The path to the Application Manifest file or folder hosting either a JSON or YAML file(s) that will be uploaded to the REST source. 
    This path may contain a single Application Manifest file, or a folder containing files for a single Application Manifest. Does not support 
    targetting a single folder of multiple different applications.

    .PARAMETER SubscriptionName
    [Optional] The Subscription name contains the Windows Package Manager REST source

    .EXAMPLE
    Add-WinGetManifest -FunctionName "contosoRESTSource" -Path "C:\AppManifests\Microsoft.PowerToys\PowerToys.json"

    Connects to Azure, then runs the Azure Function "contosoRESTSource" REST APIs to add the specified Manifest file (*.json) 
    to the Windows Package Manager REST source

    .EXAMPLE
    Add-WinGetManifest -FunctionName "contosoRESTSource" -Path "C:\AppManifests\Microsoft.PowerToys\"

    Connects to Azure, then runs the Azure Function "contosoRESTSource" REST APIs to adds the Manifest file(s) (*.json / *.yaml) 
    found in the specified folder to the Windows Package Manager REST source
    
    .EXAMPLE
    Add-WinGetManifest -FunctionName "contosoRESTSource" -Path "C:\AppManifests\Microsoft.PowerToys\PowerToys.json" -SubscriptionName "Visual Studio Subscription"

    Connects to Azure and the specified Subscription, then runs the Azure Function "contosoRESTSource" REST APIs to add the 
    specified Manifest file (*.json) to the Windows Package Manager REST source.
    #>

    PARAM(
        [Parameter(Position=0, Mandatory=$true)]  [string]$FunctionName,
        [Parameter(Position=1, Mandatory=$true)]  [string]$Path,
        [Parameter(Position=2, Mandatory=$false)] [string]$SubscriptionName = ""
    )
    BEGIN
    {
        ###############################
        ## Validates that the Azure Modules are installed
        $RequiredModules = @("Az.Resources", "Az.Accounts", "Az.Websites", "Az.Functions")
        $Result = Test-PowerShellModuleExist -Modules $RequiredModules
        $Response = @()
        
        $AzureFunctionName = $FunctionName

        if(!$Result) {
            throw "Unable to run script, missing required PowerShell modules"
        }

        ###############################
        ## Connects to Azure, if not already connected.
        Write-Verbose -Message "Validating connection to azure, will attempt to connect if not already connected."
        $Result = Connect-ToAzure
        if(!($Result)) {
            throw "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials."
        }

        ###############################
        ## Determines the PowerShell Parameter Set that was used in the call of this Function.
        ## Sets variables as if the Azure Function Name was provided.
        Write-Verbose -Message "Determines the Azure Function Resource Group Name"
        $AzureResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $AzureFunctionName}).ResourceGroupName

        ###############################
        ##  Verify Azure Resources Exist
        Write-Verbose -Message "Verifying that the Azure Resource $AzureFunctionName exists.."
        $Result = Test-AzureResource -FunctionName $AzureFunctionName -ResourceGroup $AzureResourceGroupName
        if(!$Result) {
            throw "Failed to confirm resources exist in Azure. Please verify and try again."
        }

        ###############################
        ## Gets the content from the Application Manifest (*.JSON, or *.YAML) file for posting to REST source.
        Write-Verbose -Message "Retrieving a copy of the app Manifest file for submission to WinGet source."
        $ApplicationManifest = Get-WinGetManifest -Path $Path
        if(!$ApplicationManifest) {
            throw "Failed to retrieve a proper manifest. Verify and try again."
        }

        Write-Verbose -Message "Contents of the ($($ApplicationManifest.Count)) manifests have been retrieved [$ApplicationManifest]"

        #############################################
        ##############  REST api call  ##############

        ## Specifies the REST api call that will be performed
        Write-Verbose -Message "Setting the REST API Invoke Actions."
        $apiContentType = "application/json"
        $apiMethod      = "Post"

        ## Retrieves the Azure Function URL used to add new manifests to the REST source
        Write-Verbose -Message "Retrieving the Azure Function $AzureFunctionName to build out the REST API request."
        $FunctionApp = Get-AzWebApp -ResourceGroupName $AzureResourceGroupName -Name $AzureFunctionName -ErrorAction SilentlyContinue -ErrorVariable err

        ## can function key be part of the header
        $FunctionAppId   = $FunctionApp.Id
        $DefaultHostName = $FunctionApp.DefaultHostName
        
        ## Creates the API Post Header
        $apiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $apiHeader.Add("Accept", 'application/json')
        
        $AzFunctionURL   = "https://" + $DefaultHostName + "/api/" + "packageManifests"
    }
    PROCESS
    {
        foreach ($Manifest in $ApplicationManifest) {
            Write-Verbose -Message "Confirming that the Manifest ID doesn't already exist in Azure for $($Manifest.PackageIdentifier)."
            $GetResult = Get-WinGetManifest -FunctionName $AzureFunctionName -SubscriptionName $SubscriptionName -ManifestIdentifier $Manifest.PackageIdentifier

            $ManifestObject = $Manifest
            $TriggerName = "ManifestPost"
            
            ## If the package already exists, return Error
            $GetResult | foreach-object {
                Write-Verbose -Message "Reviewing the name found ""$($_.PackageIdentifier)"" matches with what we are looking to add ""$($ManifestObject.PackageIdentifier)"""
                IF($_.PackageIdentifier -eq $ManifestObject.PackageIdentifier) {
                    $apiMethod = "Put"
                    $TriggerName = "VersionPost"
                    $AzFunctionURL += "/$($ApplicationManifest.PackageIdentifier)"
                    $ApplicationManifest = Get-WinGetManifest -Path $Path -JSON $_
                }
            }

            ## Determines the REST API that will be called, generates keys, and performs either Add (Post) or Update (Put) action.
            Write-Verbose -Message "The Manifest will be added using the $apiMethod REST API."
            $TriggerName = "Manifest$apiMethod"
            $FunctionKey = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default
            $apiHeader.Add("x-functions-key", $FunctionKey)

            $Response += Invoke-RestMethod $AzFunctionURL -Headers $apiHeader -Method $apiMethod -Body $ApplicationManifest.GetJson() -ContentType $apiContentType -ErrorVariable errInvoke

            if($errInvoke -ne @{}) {
                $ErrReturnObject = @{
                    AzFunctionURL       = $AzFunctionURL
                    apiHeader           = $apiHeader
                    apiMethod           = $apiMethod
                    apiContentType      = $apiContentType
                    ApplicationManifest = $Manifest.GetJson()
                    Response            = $Response
                    InvokeError         = $errInvoke
                }

                ## If the Post failed, then return User specific error messages:
                if($errInvoke -eq "Failure (409)"){
                    Write-Warning -Message "Manifest file already exists."
                }
                else {
                    Write-Error -Message "Unhandled Error" -TargetObject $ErrReturnObject
                }
            }
        }
    }
    END
    {
        ## If a package is created, return the objects data
        if($Response.Data) {
            return $Response.Data
        }
        ## If no new package is created, return a boolean: False
        else {
            return $False
        }
    }
}