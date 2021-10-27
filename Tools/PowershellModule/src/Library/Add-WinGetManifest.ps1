
Function Add-WinGetManifest
{
    <#
    .SYNOPSIS
    Submits a Manifest file(s) to the Azure rest source

    .DESCRIPTION
    By running this function with the required inputs, it will connect to the Azure Tenant that hosts the Windows Package Manager rest source, then collects the required URL for Manifest submission before retrieving the contents of the Manifest JSON to submit.
        
    The following Azure Modules are used by this script:
        Az.Resources --> Invoke-AzResourceAction
        Az.Accounts  --> Connect-AzAccount, Get-AzContext
        Az.Websites  --> Get-AzWebapp
        Az.Functions --> Get-AzFunctionApp

    .PARAMETER FunctionName
    Name of the Azure Function that hosts the rest source.

    .PARAMETER Path
    The Path to the JSON manifest file or folder hosting the JSON / YAML files that will be uploaded to the rest source. This path may contain a single JSON / YAML file, or a folder containing multiple JSON / YAML files. Does not support targetting a single folder of multiple different applications in *.yaml format.

    .PARAMETER SubscriptionName
    [Optional] The Subscription name contains the Windows Package Manager rest source

    .EXAMPLE
    Add-WinGetManifest -FunctionName "PrivateSource" -Path "C:\AppManifests\Microsoft.PowerToys\PowerToys.json"

    Connects to Azure, then runs the Azure Function "PrivateSource" Rest APIs to add the specified Manifest file (*.json) to the Windows Package Manager rest source

    .EXAMPLE
    Add-WinGetManifest -FunctionName "PrivateSource" -Path "C:\AppManifests\Microsoft.PowerToys\"

    Connects to Azure, then runs the Azure Function "PrivateSource" Rest APIs to adds the Manifest file(s) (*.json / *.yaml) found in the specified folder to the Windows Package Manager rest source
    
    .EXAMPLE
    Add-WinGetManifest -FunctionName "PrivateSource" -Path "C:\AppManifests\Microsoft.PowerToys\PowerToys.json" -SubscriptionName "Visual Studio Subscription"

    Connects to Azure and the specified Subscription, then runs the Azure Function "PrivateSource" Rest APIs to add the specified Manifest file (*.json) to the Windows Package Manager rest source
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
        ## Gets the JSON Content for posting to rest source
        Write-Verbose -Message "Retrieving a copy of the app Manifest file for submission to WinGet source."
        $ApplicationManifest = Get-WinGetManifest -Path $Path
        if(!$ApplicationManifest) {
            throw "Failed to retrieve a proper manifest. Verify and try again."
        }

        Write-Verbose -Message "Contents of the ($($ApplicationManifest.Count)) manifests have been retrieved [$ApplicationManifest]"

        #############################################
        ##############  Rest api call  ##############

        ## Specifies the Rest api call that will be performed
        Write-Verbose -Message "Setting the REST API Invoke Actions."
        $TriggerName    = "ManifestPost"
        $apiContentType = "application/json"
        $apiMethod      = "Post"

        ## Retrieves the Azure Function URL used to add new manifests to the rest source
        Write-Verbose -Message "Retrieving the Azure Function $AzureFunctionName to build out the Rest API request."
        $FunctionApp = Get-AzWebApp -ResourceGroupName $AzureResourceGroupName -Name $AzureFunctionName -ErrorAction SilentlyContinue -ErrorVariable err

        ## can function key be part of the header
        $FunctionAppId   = $FunctionApp.Id
        $DefaultHostName = $FunctionApp.DefaultHostName
        $FunctionKey     = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default
        
        ## Creates the API Post Header
        $apiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $apiHeader.Add("Accept", 'application/json')
        $apiHeader.Add("x-functions-key", $FunctionKey)
        
        $AzFunctionURL   = "https://" + $DefaultHostName + "/api/" + "packageManifests"
    }
    PROCESS
    {
        ## Add foreach loop
        foreach ($Manifest in $ApplicationManifest) {
            Write-Verbose -Message "Confirming that the Manifest ID doesn't already exist in Azure for $($Manifest.PackageIdentifier)."
            $GetResult = Get-WinGetManifest -FunctionName $AzureFunctionName -SubscriptionName $SubscriptionName -ManifestIdentifier $Manifest.PackageIdentifier

            #$ManifestObject = $Manifest | ConvertFrom-Json
            $ManifestObject = $Manifest
            
            ## If the package already exists, return Error
            $GetResult | foreach-object {
                IF($_.PackageIdentifier -eq $ManifestObject.PackageIdentifier) {
                    $ErrReturnObject = @{
                        SubmittedManifest = $Manifest
                        FoundManifest     = $_
                    }

                    Write-Error -Message "Manifest is already existing for the specified ID, removal of the Manifest is required to continue..." -Category ResourceExists -RecommendedAction "Remove existing problematic manifest, then re-run. Or Update current matching manifest." -TargetObject $ErrReturnObject
                    $apiMethod = "Put"; Break
                }
            }

            ## Do not update (Put) and only add (Post)
            if($apiMethod -eq "Post"){
                Write-Verbose -Message "Adding the Manifest to the WinGet Source $FunctionName.`n$($Manifest.GetJson())"
                
                $Response += Invoke-RestMethod $AzFunctionURL -Headers $apiHeader -Method $apiMethod -Body $Manifest.GetJson() -ContentType $apiContentType  -ErrorVariable errInvoke

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