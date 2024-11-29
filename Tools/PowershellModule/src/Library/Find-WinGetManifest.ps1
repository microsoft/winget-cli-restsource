# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

Function Find-WinGetManifest
{
    <#
    .SYNOPSIS
    Connects to the specified source REST API to retrieve the package manifests, returning manifest package identifier, name, publisher and versions.

    .DESCRIPTION
    Connects to the specified source REST API to retrieve the package manifests, returning manifest package identifier, name, publisher and versions.
    This function does not return the full WinGetManifest since the results may be very large. Use Get-WinGetManifest for retrieving full WinGetManifest.

    .PARAMETER FunctionName
    Name of the Azure Function Name that contains the Windows Package Manager REST APIs.

    .PARAMETER Query
    [Optional] The query to be performed against rest source. Empty query will return all package manifests.

    .PARAMETER PackageIdentifier
    [Optional] Filter the search results with PackageIdentifier

    .PARAMETER PackageName
    [Optional] Filter the search results with PackageName

    .PARAMETER SubscriptionName
    [Optional] Name of the Azure Subscription that contains the Azure Function which contains the REST APIs.

    .PARAMETER Exact
    [Optional] If specified, the rest source search will be performed with exact match.

    .EXAMPLE
    Find-WinGetManifest -FunctionName "contosorestsource" -Query "PowerToys"

    Searches rest source for package manifests with PowerToys.

    .EXAMPLE
    Find-WinGetManifest -FunctionName "contosorestsource" -Query "PowerToys" -PackageIdentifier "Windows.PowerToys"

    Searches rest source for package manifests with PowerToys and filter the result with package identifier Windows.PowerToys.

    .EXAMPLE
    Find-WinGetManifest -FunctionName "contosorestsource" -Query "PowerToys" -PackageName "Windows PowerToys" -Exact

    Searches rest source for package manifests with PowerToys and filter the result with package name Windows PowerToys. Use exact match.

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)]  [string]$FunctionName,
        [Parameter(Position=1, Mandatory=$false)] [string]$Query = "",
        [Parameter(Position=2, Mandatory=$false)] [string]$PackageIdentifier = "",
        [Parameter(Position=3, Mandatory=$false)] [string]$PackageName = "",
        [Parameter(Position=4, Mandatory=$false)] [string]$SubscriptionName = "",
        [Parameter()] [switch]$Exact
    )
    BEGIN
    {
        [PSCustomObject[]] $Return = @()

        ###############################
        ## Connects to Azure, if not already connected.
        Write-Verbose "Testing connection to Azure."
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

        ## Retrieves the Azure Function URL used to add new manifests to the REST source
        Write-Verbose -Message "Retrieving Azure Function Web Applications matching to: $FunctionName."
        $FunctionApp = Get-AzWebApp -ResourceGroupName $ResourceGroupName -Name $FunctionName

        $FunctionAppId   = $FunctionApp.Id
        $DefaultHostName = $FunctionApp.DefaultHostName
        
        $TriggerName    = "ManifestSearchPost"
        $ApiContentType = "application/json"
        $ApiMethod      = "Post"

        ## Creates the API Post Header
        $ApiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $ApiHeader.Add("Accept", 'application/json')
        $FunctionKey = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default
        $ApiHeader.Add("x-functions-key", $FunctionKey)

        $AzFunctionURL = "https://" + $DefaultHostName + "/api/manifestSearch"
    }
    PROCESS
    {
        Write-Verbose -Message "Invoking the REST API call."
        
        $RequestBody = @{
            Query = @{
                KeyWord = $Query
                MatchType = $Exact ? "Exact" : "Substring"
            }
            Filters = @()
        }

        if (![string]::IsNullOrWhiteSpace($PackageIdentifier)) {
            $RequestBody.Filters += @{
                PackageMatchField = "PackageIdentifier"
                RequestMatch = @{
                    KeyWord = $PackageIdentifier
                    MatchType = $Exact ? "Exact" : "CaseInsensitive"
                }
            }
        }
        if (![string]::IsNullOrWhiteSpace($PackageName)) {
            $RequestBody.Filters += @{
                PackageMatchField = "PackageName"
                RequestMatch = @{
                    KeyWord = $PackageName
                    MatchType = $Exact ? "Exact" : "CaseInsensitive"
                }
            }
        }

        $RequestBodyJson = ConvertTo-Json $RequestBody -Depth 8 -Compress
        Write-Verbose "Search Request: $RequestBodyJson"

        $ContinuationToken = $null
        do {
            if ($ContinuationToken) {
                $ApiHeader["ContinuationToken"] = $ContinuationToken
            }
            
            $Response = Invoke-RestMethod $AzFunctionURL -Headers $ApiHeader -Method $ApiMethod -Body $RequestBodyJson -ContentType $ApiContentType -ErrorVariable ErrorInvoke
     
            if ($ErrorInvoke) {
                $ErrorMessage = "Failed to get search result from $FunctionName. Verify the information you provided and try again."
                $ErrReturnObject = @{
                    AzFunctionURL       = $AzFunctionURL
                    ApiMethod           = $ApiMethod
                    ApiContentType      = $ApiContentType
                    Response            = $Response
                    InvokeError         = $ErrorInvoke
                }
            
                Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                return
            }
            else {
                Write-Verbose "Found ($($Response.Data.Count)) Manifests that matched."
            
                foreach ($ResponseData in $Response.Data) {
                    Write-Verbose -Message "Parsing through the returned results: $ResponseData"
                    $ManifestInfo = [PSCustomObject]@{
                        PackageIdentifier = $ResponseData.PackageIdentifier
                        PackageName = $ResponseData.PackageName
                        Publisher = $ResponseData.Publisher
                        Versions = [string[]]@()
                    }
                    foreach ($Version in $ResponseData.Versions) {
                        $ManifestInfo.Versions += $Version.PackageVersion
                    }
                    $Return += $ManifestInfo
                }
            }
     
            $ContinuationToken = $Response.ContinuationToken
        } while (![string]::IsNullOrWhiteSpace($ContinuationToken))
    }
    END
    {
        ## Returns results
        Write-Verbose -Message "Returning ($($Return.Count)) manifests based on search."
        return $Return
    }
}