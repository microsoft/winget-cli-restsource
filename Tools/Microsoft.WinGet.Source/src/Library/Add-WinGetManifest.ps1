
Function Add-WinGetManifest
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

    .PARAMETER FunctionName
    Name of the Azure Function that hosts the private source

    .PARAMETER Path
    Path to the JSON manifest file that will be uploaded to the private source

    .PARAMETER SubscriptionName
    [Optional] The Subscription name contains the Windows Package Manager private source

    .EXAMPLE
    Add-WinGetManifest -Source "Private" -Path "C:\Temp\App.json"

    .EXAMPLE
    Add-WinGetManifest -Source "Private" -Path "C:\Temp\App.json" -SubscriptionName "Subscription"

    .EXAMPLE
    Add-WinGetManifest -FunctionName "contoso-function-prod" -Path "C:\Temp\App.json"

    .EXAMPLE
    Add-WinGetManifest -FunctionName "contoso-function-prod" -Path "C:\Temp\App.json" -SubscriptionName "Subscription"
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
        ## Gets the JSON Content for posting to Private Repo
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

        ## Retrieves the Azure Function URL used to add new manifests to the private source
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
        Write-Verbose -Message "Confirming that the Manifest ID doesn't already exist in Azure for $($ApplicationManifest.PackageIdentifier)."
        $GetResult = Get-WinGetManifest -FunctionName $AzureFunctionName -SubscriptionName $SubscriptionName -ManifestIdentifier $ApplicationManifest.PackageIdentifier

        #$ApplicationManifestObject = $ApplicationManifest | ConvertFrom-Json
        $ApplicationManifestObject = $ApplicationManifest
        
        ## If the package already exists, return Error
        $GetResult | foreach-object {
            IF($_.PackageIdentifier -eq $ApplicationManifestObject.PackageIdentifier) {
                $ErrReturnObject = @{
                    SubmittedManifest = $ApplicationManifest
                    FoundManifest     = $_
                }

                Write-Error -Message "Manifest is already existing for the specified ID, removal of the Manifest is required to continue..." -Category ResourceExists -RecommendedAction "Remove existing problematic manifest, then re-run. Or Update current matching manifest." -TargetObject $ErrReturnObject
                $apiMethod = "Put"; Break
            }
        }

        ## Do not update (Put) and only add (Post)
        if($apiMethod -eq "Post"){
            Write-Verbose -Message "Adding the Manifest to the WinGet Source $FunctionName.`n$($ApplicationManifest.GetJson())"
            
            $Response = Invoke-RestMethod $AzFunctionURL -Headers $apiHeader -Method $apiMethod -Body $ApplicationManifest.GetJson() -ContentType $apiContentType  -ErrorVariable errInvoke

            if($errInvoke -ne @{}) {
                $ErrReturnObject = @{
                    AzFunctionURL       = $AzFunctionURL
                    apiHeader           = $apiHeader
                    apiMethod           = $apiMethod
                    apiContentType      = $apiContentType
                    ApplicationManifest = $ApplicationManifest.GetJson()
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