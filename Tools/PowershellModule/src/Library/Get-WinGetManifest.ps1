# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

Function Get-WinGetManifest
{
    <#
    .SYNOPSIS
    Connects to the specified source REST API, or local file system path to retrieve the application Manifests, returning 
    the manifest found. Allows for retrieving results based on the package identifier when targetting the REST APIs.

    .DESCRIPTION
    Connects to the specified source REST API, or local file system path to retrieve the application Manifests, returning 
    an array of all Manifests found. Allows for retrieving results based on the package identifier.
        
    The following Azure Modules are used by this script:
        Az.Resources --> Invoke-AzResourceAction
        Az.Accounts  --> Connect-AzAccount, Get-AzContext
        Az.Websites  --> Get-AzWebapp
        Az.Functions --> Get-AzFunctionApp

    .PARAMETER Path
    Points to either a folder containing a specific application's manifest of type .json or .yaml or to a specific .json or .yaml file.
    If you are processing a multi-file manifest, point to the folder that contains all yamls. Note: all yamls within the folder must be part of
    the same application.

    .PARAMETER JSON
    A JSON string containing a single application's REST source Application Manifest that will be merged with locally processed files. This is
    used by the script infrastructure internally and is not expected to be useful to an end user using this command.

    .PARAMETER URL
    Web URL to the host site containing the REST APIs with access key (if required).

    .PARAMETER FunctionName
    Name of the Azure Function Name that contains the Windows Package Manager REST APIs.

    .PARAMETER ManifestIdentifier
    [Optional] The Windows Package Manager Package Identifier of a specific Application Manifest result.

    .PARAMETER SubscriptionName
    [Optional] Name of the Azure Subscription that contains the Azure Function which contains the REST APIs.

    .EXAMPLE
    Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys"

    Returns an array of all Manifest objects based on the files found within the specified Path.

    .EXAMPLE
    Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys\Microsoft.PowerToys.json"

    Returns a Manifest object (*.json) of the specified JSON file.
    
    .EXAMPLE
    Get-WinGetManifest -FunctionName "contosoRESTSource" -ManifestIdentifier "Windows.PowerToys"

    Returns a Manifest object of the specified Package Identifier that is queried against in the REST APIs.

    .EXAMPLE
    Get-WinGetManifest -FunctionName "contosoRESTSource" -ManifestIdentifier "Windows.PowerToys" -SubscriptionName "Visual Studio Subscription"

    Returns a Manifest object of the specified Package Identifier that is queried against in the REST APIs from the specified Subscription Name.

    .EXAMPLE
    Get-WinGetManifest -FunctionName "RESTSource"

    Returns an array of Manifest objects that are found in the specified Azure Function.

    #>
    [CmdletBinding(DefaultParameterSetName = 'Azure')]
    PARAM(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="File")]  [string]$Path,
        [Parameter(Position=1, Mandatory=$false,ParameterSetName="File")]  [WinGetManifest]$JSON,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Azure")] [string]$FunctionName,
        [Parameter(Position=1, Mandatory=$false,ParameterSetName="Azure")] [string]$ManifestIdentifier,
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
                ## Validates that the Azure Modules are installed
                Write-Verbose -Message "Testing required PowerShell modules are installed."
                
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
                
                ###############################
                ##  Verify Azure Resources Exist
                ## Sets variables as if the Azure Function Name was provided.

                $AzureResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $FunctionName}).ResourceGroupName

                if($AzureResourceGroupName) {
                    $Result = Test-AzureResource -FunctionName $FunctionName -ResourceGroup $AzureResourceGroupName
                }
                else {
                    throw "Unable to locate Function (""$FunctionName"") in the Azure Tenant."
                }
                
                if(!$Result) {
                    throw "Failed to confirm resources exist in Azure. Please verify and try again."
                }

                if($ManifestIdentifier){
                    $ManifestIdentifier = "/$ManifestIdentifier"
                }
        
                ###############################
                ##  REST api call  
                
                ## Specifies the REST api call that will be performed
                $TriggerName    = "ManifestGet"
                $apiContentType = "application/json"
                $apiMethod      = "Get"
        
                ## Creates the API Post Header
                $apiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
                $apiHeader.Add("Accept", 'application/json')
             }
            "File" {
                $ManifestFileExists  = Test-Path -Path $Path

                if(!$ManifestFileExists) {
                    ## The target path does not exist
                    $ErrReturnObject = @{
                        FilePath           = $Path
                        ManifestFileExists = $ManifestFileExists
                    }

                    Write-Error -Message "Target path did not point to an object." -TargetObject $ErrReturnObject
                    return
                }

                $PathProperties = Get-ItemProperty $Path
                $ManifestFile   = ""

                if($PathProperties.Attributes -like "*Directory*") {
                    ## $Path variable is pointing at a directory
                    $PathChildItemsJSON = Get-ChildItem -Path $Path -Filter "*.json"
                    $PathChildItemsYAML = Get-ChildItem -Path $Path -Filter "*.yaml"

                    $VerboseMessage = "Path pointed to a directory, found $($PathChildItemsJSON.count) JSON files, and $($PathChildItemsYAML.count) YAML files."
                    Write-Verbose -Message $VerboseMessage

                    ## Validating found objects
                    if($PathChildItemsJSON.count -eq 0 -and $PathChildItemsYAML.count -eq 0) {
                        ## No JSON or YAML files were found in the directory.
                        $ErrorMessage    = "Directory does not contain any combination of JSON and YAML files."
                        $ErrReturnObject = @{
                            JSONFiles = $PathChildItemsJSON
                            YAMLFiles = $PathChildItemsYAML
                            JSONCount = $PathChildItemsJSON.count
                            YAMLCount = $PathChildItemsYAML.count
                        }
                        
                        $ManifestFileType = "Error"
                        Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                    }
                    elseif($PathChildItemsJSON.count -gt 0 -and $PathChildItemsYAML.count -gt 0) {
                        ## A combination of JSON and YAML Files were found.
                        $ErrorMessage    = "Directory contains a combination of JSON and YAML files."
                        $ErrReturnObject = @{
                            JSONFiles = $PathChildItemsJSON
                            YAMLFiles = $PathChildItemsYAML
                            JSONCount = $PathChildItemsJSON.count
                            YAMLCount = $PathChildItemsYAML.count
                        }
                        
                        $ManifestFileType = "Error"
                        Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                    }
                    elseif($PathChildItemsJSON.count -gt 1) {
                        ## More than one Application Manifest's JSON files was found.
                        $ErrorMessage    = "Directory contains more than one JSON file."
                        $ErrReturnObject = @{
                            JSONFiles = $PathChildItemsJSON
                            YAMLFiles = $PathChildItemsYAML
                            JSONCount = $PathChildItemsJSON.count
                            YAMLCount = $PathChildItemsYAML.count
                        }
                        
                        $ManifestFileType = "Error"
                        Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                    }
                    elseif($PathChildItemsJSON.count -eq 1) {
                        ## Single JSON has been found in the target folder.
                        Write-Verbose -Message "Single JSON has been found in the specified directory."
                        $ManifestFile        = $PathChildItemsJSON
                        $ApplicationManifest = Get-Content -Path $PathChildItemsJSON.FullName -Raw
                        $ManifestFileType    = $PathChildItemsJSON.Extension
                    }
                    elseif($PathChildItemsYAML.count -gt 0) {
                        Write-Verbose -Message "Single YAML has been found in the specified directory."
                        ## YAML has been found in the target folder.
                        $ManifestFile     = $PathChildItemsYAML
                        $ManifestFileType = ".yaml"
                        $ApplicationManifest = Get-Content -Path $PathChildItemsYAML[0].FullName -Raw
                    }
                }
                else {
                    ## $Path variable is pointing at a file
                    Write-Verbose -Message "Retrieving the Application Manifest for: $Path"
            
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
                $apiHeader.Add("x-functions-key", $FunctionKey)
                $AzFunctionURL   = "https://" + $DefaultHostName + "/api/" + "packageManifests" + $ManifestIdentifier
                
                ## Publishes the Manifest to the Windows Package Manager REST source
                Write-Verbose -Message "Invoking the REST API call."

                $Results = Invoke-RestMethod $AzFunctionURL -Headers $apiHeader -Method $apiMethod -ContentType $apiContentType
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

                            Write-Verbose -Message "Returned Manifest from JSON file: $($Return.PackageIdentifier)"
                        }
                    }
                    ## If the path resolves to a YAML file
                    ".yaml" {
                        ## Directory - *.yaml files included within.
                        if($WinGetDesktopAppInstallerLibLoaded) {
                            Write-Verbose -Message "YAML Files have been found in the target directory. Building a JSON manifest with found files."
                            if($Json){
                                $Return += [Microsoft.WinGet.RestSource.PowershellSupport.YamlToRestConverter]::AddManifestToPackageManifest($Path, $JSON.GetJson());
                            }
                            else{
                                $Return += [Microsoft.WinGet.RestSource.PowershellSupport.YamlToRestConverter]::AddManifestToPackageManifest($Path, "");
                            }

                            Write-Verbose -Message "Returned Manifest from YAML file: $($Return.PackageIdentifier)"
                        }
                        else {
                            Write-Error -Message "Unable to process YAML files. Re-import the module to reload the required dependencies." -Category ResourceUnavailable
                        }
                    }
                    "Error" {
                    }
                    default {
                        if($ManifestFileExists) {
                            $ErrorMessage = "Incorrect filetype. Verify the file is of type '*.yaml' or '*.json' and try again."
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
    }
    END
    {
        ## Returns results
        Write-Verbose -Message "Returning ($($Return.count)) manifests based on search."
        return $Return
    }
}