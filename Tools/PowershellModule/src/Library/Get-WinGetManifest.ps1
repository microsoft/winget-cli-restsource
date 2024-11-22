# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

Function Get-WinGetManifest
{
    <#
    .SYNOPSIS
    Connects to the specified source REST API, or local file system path to retrieve the package manifests, returning 
    the manifest found. Allows for retrieving results based on the package identifier when targetting the REST APIs.

    .DESCRIPTION
    Connects to the specified source REST API, or local file system path to retrieve the package Manifests, returning 
    an array of all Manifests found. Allows for retrieving results based on the package identifier.
        
    The following Azure Modules are used by this script:
        Az.Resources --> Invoke-AzResourceAction
        Az.Accounts  --> Connect-AzAccount, Get-AzContext
        Az.Websites  --> Get-AzWebapp
        Az.Functions --> Get-AzFunctionApp

    .PARAMETER Path
    Points to either a folder containing a specific application's manifest of type .json or .yaml or to a specific .json or .yaml file.

    If you are processing a multi-file manifest, point to the folder that contains all yamls. Note: all yamls within the folder must be part of
    the same package manifest.

    .PARAMETER JSON
    A JSON string containing a single application's REST source Packages Manifest that will be merged with locally processed .yaml files. This is
    used by the script infrastructure internally and is not expected to be useful to an end user using this command.

    .PARAMETER URL
    Web URL to the host site containing the REST APIs with access key (if required).

    .PARAMETER FunctionName
    Name of the Azure Function Name that contains the Windows Package Manager REST APIs.

    .PARAMETER PackageIdentifier
    [Optional] The Windows Package Manager Package Identifier of a specific Package Manifest result.

    .PARAMETER SubscriptionName
    [Optional] Name of the Azure Subscription that contains the Azure Function which contains the REST APIs.

    .EXAMPLE
    Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys"

    Returns an array of all Manifest objects based on the files found within the specified Path.

    .EXAMPLE
    Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys\Microsoft.PowerToys.json"

    Returns a Manifest object (*.json) of the specified JSON file.
    
    .EXAMPLE
    Get-WinGetManifest -FunctionName "contosorestsource" -PackageIdentifier "Windows.PowerToys"

    Returns a Manifest object of the specified Package Identifier that is queried against in the REST APIs.

    .EXAMPLE
    Get-WinGetManifest -FunctionName "contosorestsource" -PackageIdentifier "Windows.PowerToys" -SubscriptionName "Visual Studio Subscription"

    Returns a Manifest object of the specified Package Identifier that is queried against in the REST APIs from the specified Subscription Name.

    .EXAMPLE
    Get-WinGetManifest -FunctionName "contosorestSource"

    Returns an array of Manifest objects that are found in the specified Azure Function.

    #>
    [CmdletBinding(DefaultParameterSetName = 'Azure')]
    PARAM(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="File")]  [string]$Path,
        [Parameter(Position=1, Mandatory=$false,ParameterSetName="File")]  [WinGetManifest]$JSON,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Azure")] [string]$FunctionName,
        [Parameter(Position=1, Mandatory=$false,ParameterSetName="Azure")] [string]$PackageIdentifier,
        [Parameter(Position=2, Mandatory=$false,ParameterSetName="Azure")] [string]$SubscriptionName
    )
    BEGIN
    {
        [WinGetManifest[]] $Return = @()

        ###############################
        ## Determines the PowerShell Parameter Set that was used in the call of this Function.
        switch ($PsCmdlet.ParameterSetName) {
            "Azure"  {
                ###############################
                ## Connects to Azure, if not already connected.
                Write-Verbose "Testing connection to Azure."
                
                $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
                if(!($Result)) {
                    throw "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials."
                }
                
                ###############################
                ##  Verify Azure Resources Exist
                ## Sets variables as if the Azure Function Name was provided.

                $AzureResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $FunctionName}).ResourceGroupName

                if($AzureResourceGroupName) {
                    $Result = Test-AzureResource -ResourceName $FunctionName -ResourceGroup $AzureResourceGroupName
                    
                    if(!$Result) {
                        throw "Failed to confirm resources exist in Azure. Please verify and try again."
                    }
                }
                else {
                    throw "Unable to locate Function (""$FunctionName"") with Resource Group in the Azure Tenant."
                }

                if($PackageIdentifier){
                    $PackageIdentifier = "/$PackageIdentifier"
                }
        
                ###############################
                ##  REST api call  
                
                ## Specifies the REST api call that will be performed
                $TriggerName    = "ManifestGet"
                $ApiContentType = "application/json"
                $ApiMethod      = "Get"
        
                ## Creates the API Post Header
                $ApiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
                $ApiHeader.Add("Accept", 'application/json')
             }
            "File" {
                ## Convert to full path if applicable
                $Path = [System.IO.Path]::GetFullPath($Path, $pwd.Path)
                
                $ManifestFileExists  = Test-Path -Path $Path

                if(!$ManifestFileExists) {
                    $ErrReturnObject = @{
                        FilePath           = $Path
                        ManifestFileExists = $ManifestFileExists
                    }

                    Write-Error -Message "Target path did not point to an object." -TargetObject $ErrReturnObject
                    return $Return
                }

                $PathItem = Get-Item $Path
                $ManifestFile = ""
                $ApplicationManifest = ""
                $ManifestFileType = ""

                if($PathItem.PSIsContainer) {
                    ## $Path variable is pointing at a directory
                    $PathChildItemsJSON = Get-ChildItem -Path $Path -Filter "*.json"
                    $PathChildItemsYAML = Get-ChildItem -Path $Path -Filter "*.yaml"

                    Write-Verbose -Message "Path pointed to a directory, found $($PathChildItemsJSON.count) JSON files, and $($PathChildItemsYAML.count) YAML files."
                    
                    $ErrReturnObject = @{
                        JSONFiles = $PathChildItemsJSON
                        YAMLFiles = $PathChildItemsYAML
                        JSONCount = $PathChildItemsJSON.count
                        YAMLCount = $PathChildItemsYAML.count
                    }

                    ## Validating found objects
                    if($PathChildItemsJSON.count -eq 0 -and $PathChildItemsYAML.count -eq 0) {
                        ## No JSON or YAML files were found in the directory.
                        $ErrorMessage    = "Directory does not contain any JSON or YAML files."
                        Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                        return $Return
                    }
                    elseif($PathChildItemsJSON.count -gt 0 -and $PathChildItemsYAML.count -gt 0) {
                        ## A combination of JSON and YAML Files were found.
                        $ErrorMessage    = "Directory contains a combination of JSON and YAML files."
                        Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                        return $Return
                    }
                    elseif($PathChildItemsJSON.count -gt 1) {
                        ## More than one Package Manifest's JSON files was found.
                        $ErrorMessage    = "Directory contains more than one JSON file."
                        Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                        return $Return
                    }
                    elseif($PathChildItemsJSON.count -eq 1) {
                        ## Single JSON has been found in the target folder.
                        Write-Verbose -Message "Single JSON has been found in the specified directory."
                        $ManifestFile        = $PathChildItemsJSON
                        $ApplicationManifest = Get-Content -Path $PathChildItemsJSON.FullName -Raw
                        $ManifestFileType    = ".json"
                    }
                    elseif($PathChildItemsYAML.count -gt 0) {
                        Write-Verbose -Message "YAML has been found in the specified directory."
                        ## YAML has been found in the target folder.
                        $ManifestFile     = $PathChildItemsYAML
                        $ManifestFileType = ".yaml"
                        $ApplicationManifest = Get-Content -Path $PathChildItemsYAML[0].FullName -Raw
                    }
                }
                else {
                    ## $Path variable is pointing at a file
                    Write-Verbose -Message "Retrieving the Package Manifest for: $Path"
            
                    ## Gets the Manifest object and contents of the Manifest - identifying the manifest file extension.
                    $ApplicationManifest = Get-Content -Path $Path -Raw
                    $ManifestFile        = Get-Item -Path $Path
                    $ManifestFileType    = $ManifestFile.Extension

                    Write-Verbose -Message "Retrieved content from the manifest ($($ManifestFile.Name))."
                }
            }
        }        
    }
    PROCESS
    {
        switch ($PsCmdlet.ParameterSetName) {
            "Azure" {
                Write-Verbose -Message "Retrieving Azure Function Web Applications matching to: $FunctionName."

                ## Retrieves the Azure Function URL used to add new manifests to the REST source
                $FunctionApp = Get-AzWebApp -ResourceGroupName $AzureResourceGroupName -Name $FunctionName -ErrorAction SilentlyContinue -ErrorVariable err
                        
                ## can function key be part of the header
                Write-Verbose -Message "Constructing the REST API call."
                
                $FunctionAppId   = $FunctionApp.Id
                $DefaultHostName = $FunctionApp.DefaultHostName
                $FunctionKey     = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default
                $ApiHeader.Add("x-functions-key", $FunctionKey)
                $AzFunctionURL   = "https://" + $DefaultHostName + "/api/" + "packageManifests" + $PackageIdentifier
                
                ## Publishes the Manifest to the Windows Package Manager REST source
                Write-Verbose -Message "Invoking the REST API call."

                $Results = Invoke-RestMethod $AzFunctionURL -Headers $ApiHeader -Method $ApiMethod -ContentType $ApiContentType
                Write-Verbose "Found ($($Results.Data.Count)) Manifests that matched."

                foreach ($Result in $Results.Data){
                    Write-Verbose -Message "Parsing through the returned results: $Result"
                    $Return += [WinGetManifest]::New($Result)
                }
            }
            "File" {
                switch ($ManifestFileType) {
                    ## If the path resolves to a JSON file
                    ".json" {
                        $Result = Test-WinGetManifest -Manifest $ApplicationManifest

                        if($Result) {
                            ## Sets the return result to be the contents of the JSON file if the Manifest test passed.
                            $Return = $ApplicationManifest 

                            Write-Information "Returned Manifest from JSON file: $($Return.PackageIdentifier)"
                        }
                    }
                    ## If the path resolves to a YAML file
                    ".yaml" {
                        ## Directory - *.yaml files included within.
                        Write-Verbose -Message "YAML Files have been found in the target directory. Building a JSON manifest with found files."
                        if($Json){
                            Write-Verbose "Prior manifest provided. New manifest will be merged with prior manifest."
                            $Return += [Microsoft.WinGet.RestSource.PowershellSupport.YamlToRestConverter]::AddManifestToPackageManifest($Path, $JSON.GetJson());
                        }
                        else{
                            Write-Verbose "Prior manifest not provided."
                            $Return += [Microsoft.WinGet.RestSource.PowershellSupport.YamlToRestConverter]::AddManifestToPackageManifest($Path, "");
                        }

                        Write-Information "Returned Manifest from YAML file: $($Return.PackageIdentifier)"
                    }
                    default {
                        $ErrorMessage = "Incorrect file type. Verify the file is of type '*.yaml' or '*.json' and try again."
                        $ErrReturnObject = @{
                            ApplicationManifest = $ApplicationManifest
                            ManifestFile        = $ManifestFile
                            $ManifestFileType   = $ManifestFileType
                        }

                        Write-Error -Message $ErrorMessage -Category InvalidType -TargetObject $ErrReturnObject
                    }
                }
            }
        }
    }
    END
    {
        ## Returns results
        Write-Verbose -Message "Returning ($($Return.count)) manifests based on search."
        return $Return
    }
}