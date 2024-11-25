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

    The following Azure Modules are used by this script:
        Az.Resources --> Invoke-AzResourceAction
        Az.Accounts  --> Connect-AzAccount, Get-AzContext
        Az.Websites  --> Get-AzWebapp
        Az.Functions --> Get-AzFunctionApp

    .PARAMETER FunctionName
    Name of the Azure Function that hosts the REST source.

    .PARAMETER Path
    The path to the Package Manifest file or folder hosting either a JSON or YAML file(s) that will be uploaded to the REST source. 
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
        [Parameter(Position=1, Mandatory=$true)]  [string]$Path,
        [Parameter(Position=2, Mandatory=$false)] [string]$SubscriptionName = ""
    )
    BEGIN
    {
        ###############################
        ## Connects to Azure, if not already connected.
        Write-Verbose -Message "Validating connection to azure, will attempt to connect if not already connected."
        $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
        if(!($Result)) {
            throw "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials."
        }

        ###############################
        ## Determines the PowerShell Parameter Set that was used in the call of this Function.
        ## Sets variables as if the Azure Function Name was provided.
        Write-Verbose -Message "Determines the Azure Function Resource Group Name"
        $ResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $FunctionName}).ResourceGroupName

        ###############################
        ##  Verify Azure Resources Exist
        Write-Verbose -Message "Verifying that the Azure Resource $FunctionName exists.."
        $Result = Test-AzureResource -ResourceName $FunctionName -ResourceGroup $ResourceGroupName
        if(!$Result) {
            throw "Failed to confirm resources exist in Azure. Please verify and try again."
        }

        ###############################
        ## Gets the content from the Package Manifest (*.JSON, or *.YAML) file for posting to REST source.
        Write-Verbose -Message "Retrieving a copy of the app Manifest file for submission to WinGet source."
        $ApplicationManifest = Get-WinGetManifest -Path $Path
        if(!$ApplicationManifest) {
            throw "Failed to retrieve a proper manifest. Verify and try again."
        }

        Write-Verbose -Message "Contents of ($($ApplicationManifest.Count)) manifests have been retrieved [$ApplicationManifest]"

        #############################################
        ##############  REST api call  ##############

        ## Specifies the REST api call that will be performed
        Write-Verbose -Message "Setting the REST API Invoke Actions."
        $ApiContentType = "application/json"
        $ApiMethod      = "Post"

        ## Retrieves the Azure Function URL used to add new manifests to the REST source
        Write-Verbose -Message "Retrieving the Azure Function $FunctionName to build out the REST API request."
        $FunctionApp = Get-AzWebApp -ResourceGroupName $ResourceGroupName -Name $FunctionName

        ## can function key be part of the header
        $FunctionAppId   = $FunctionApp.Id
        $DefaultHostName = $FunctionApp.DefaultHostName
        
        ## Creates the API Post Header
        $ApiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $ApiHeader.Add("Accept", 'application/json')
        
        $AzFunctionURL   = "https://" + $DefaultHostName + "/api/" + "packageManifests"
    }
    PROCESS
    {
        foreach ($Manifest in $ApplicationManifest) {
            Write-Verbose -Message "Confirming that the Package ID doesn't already exist in Azure for $($Manifest.PackageIdentifier)."
            $GetResult = Get-WinGetManifest -FunctionName $FunctionName -SubscriptionName $SubscriptionName -PackageIdentifier $Manifest.PackageIdentifier

            ## If the package already exists, return Error
            $GetResult | foreach-object {
                Write-Verbose -Message "Reviewing the name found ""$($_.PackageIdentifier)"" matches with what we are looking to add ""$($Manifest.PackageIdentifier)"""
                IF($_.PackageIdentifier -eq $Manifest.PackageIdentifier) {
                    $ApiMethod = "Put"
                    $AzFunctionURL += "/$($ApplicationManifest.PackageIdentifier)"
                    $ApplicationManifest = Get-WinGetManifest -Path $Path -JSON $_
                }
            }

            ## Determines the REST API that will be called, generates keys, and performs either Add (Post) or Update (Put) action.
            Write-Verbose -Message "The Manifest will be added using the $ApiMethod REST API."
            $TriggerName = "Manifest$ApiMethod"
            $FunctionKey = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default
            $ApiHeader.Add("x-functions-key", $FunctionKey)

            $Response += Invoke-RestMethod $AzFunctionURL -Headers $ApiHeader -Method $ApiMethod -Body $ApplicationManifest.GetJson() -ContentType $ApiContentType -ErrorVariable ErrorInvoke

            if($ErrorInvoke) {
                $ErrReturnObject = @{
                    AzFunctionURL       = $AzFunctionURL
                    ApiHeader           = $ApiHeader
                    ApiMethod           = $ApiMethod
                    ApiContentType      = $ApiContentType
                    ApplicationManifest = $Manifest.GetJson()
                    Response            = $Response
                    InvokeError         = $ErrorInvoke
                }

                Write-Error -Message "Failed to add manifest." -TargetObject $ErrReturnObject
            }
        }
    }
    END
    {
        ## If a package is created, return the objects data
        if($Response.Data) {
            return $Response.Data
        }
        ## If no new package is created, return a $null
        else {
            return $null
        }
    }
}