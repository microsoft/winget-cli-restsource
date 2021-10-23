
Function Remove-WinGetManifest
{

    [CmdletBinding(DefaultParameterSetName = 'WinGet')]
    PARAM(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Custom")][string]$URL,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Azure")]  [string]$FunctionName,
        [Parameter(Position=1, Mandatory=$false)] [string]$ManifestIdentifier = "",
        [Parameter(Position=2, Mandatory=$false)] [string]$SubscriptionName = ""
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
                Write-Verbose -Message "Confirming that the Manifest ID exists in Azure for $ManifestIdentifier."
                $GetResult = Get-WinGetManifest -FunctionName $FunctionName -SubscriptionName $SubscriptionName -ManifestIdentifier $ManifestIdentifier
                
                ## If the package doesn't exist, return Error
                $GetResult | foreach-object {
                    IF($_.PackageIdentifier -eq $ManifestIdentifier) {
                        Write-Verbose -Message "Manifest matching to $ManifestIdentifier has been found."
                        $Found = $true
                        #Break
                    }
                }

                ## If the Manifest ID did not match with the list of found Manifest ID's then return error and exit.
                If(!$Found){
                    $ErrReturnObject = @{
                        SubmittedManifest = $ApplicationManifest
                        FoundManifest     = $_
                        SearchResults     = $GetResult
                    }

                    Write-Error -Message "Manifest does not exist for the specified ID, removal of the Manifest will not continue..." -Category ResourceExists -TargetObject $ErrReturnObject
                    return
                }

                
                Write-Verbose -Message "Retrieving Azure Function Web Applications matching to: $FunctionName."
                Write-Verbose -Message "Constructing the REST API call for removal of manifest."

                $Response = Invoke-RestMethod $AzFunctionURL -Headers $apiHeader -Method $apiMethod -ContentType $apiContentType  -ErrorVariable errInvoke

                if($errInvoke -ne $()) {
                    $ErrReturnObject = @{
                        AzFunctionURL       = $AzFunctionURL
                        apiHeader           = $apiHeader
                        apiMethod           = $apiMethod
                        apiContentType      = $apiContentType
                        ApplicationManifest = $ApplicationManifest
                        Response            = $Response
                        InvokeError         = $errInvoke
                    }

                    ## If the Post failed, then return User specific error messages:
                    if($errInvoke -eq "Failure (409)"){
                        Write-Warning -Message "Manifest file already exists."
                    }
                    else {
                        Write-Error -Message "Failed to remove Manifest from $FunctionName. Verify the information you provided and try again." -Category ResourceUnavailable -TargetObject $ErrReturnObject
                    }
                }
            }
        }
    }
    END
    {
        return $Response.Data
    }
}