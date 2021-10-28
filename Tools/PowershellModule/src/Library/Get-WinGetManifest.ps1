# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

Function Get-WinGetManifest
{
    <#
    .SYNOPSIS
    Connects to the specified source Rest API, or local file system path to retrieve the application Manifests, returning an array of all Manifests found. Allows for retrieving results based on the name when targetting the Rest APIs.

    .DESCRIPTION
    Connects to the specified source Rest API, or local file system path to retrieve the application Manifests, returning an array of all Manifests found. Allows for retrieving results based on the name.
        
    The following Azure Modules are used by this script:
        Az.Resources --> Invoke-AzResourceAction
        Az.Accounts  --> Connect-AzAccount, Get-AzContext
        Az.Websites  --> Get-AzWebapp
        Az.Functions --> Get-AzFunctionApp

    .PARAMETER Path
    Path to a file (*.json) or folder containing *.yaml or *.json files.

    .PARAMETER URL
    Web URL to the host site containing the Rest APIs with access key (if required).

    .PARAMETER FunctionName
    Name of the Azure Function Name that contains the Windows Package Manager Rest APIs.

    .PARAMETER ManifestIdentifier
    [Optional] The Windows Package Manager Package Identifier of a specific Manifest result

    .PARAMETER SubscriptionName
    [Optional] Name of the Azure Subscription that contains the Azure Function which contains the Rest APIs.

    .EXAMPLE
    Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys"

    Returns an array of all Manifest objects based on the files found within the specified Path.

    .EXAMPLE
    Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys\Microsoft.PowerToys.json"

    Returns a Manifest object (*.json) of the specified JSON file.
    
    .EXAMPLE
    Get-WinGetManifest -FunctionName "PrivateSource" -ManifestIdentifier "Windows.PowerToys"

    Returns a Manifest object of the specified Manifest Identifier that is queried against in the Rest APIs.

    .EXAMPLE
    Get-WinGetManifest -FunctionName "PrivateSource" -ManifestIdentifier "Windows.PowerToys" -SubscriptionName "Visual Studio Subscription"

    Returns a Manifest object of the specified Manifest Identifier that is queried against in the Rest APIs from the specified Subscription Name.

    .EXAMPLE
    Get-WinGetManifest -FunctionName "PrivateSource"

    Returns an array of Manifest objects that are found in the specified Azure Function.

    #>
    [CmdletBinding(DefaultParameterSetName = 'Azure')]
    PARAM(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="File")]  [string]$Path,
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
                
                ## Sets variables as if the Azure Function Name was provided.
                $AzureResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $FunctionName}).ResourceGroupName

                ###############################
                ##  Verify Azure Resources Exist
                $Result = Test-AzureResource -FunctionName $FunctionName -ResourceGroup $AzureResourceGroupName
                if(!$Result) {
                    throw "Failed to confirm resources exist in Azure. Please verify and try again."
                }

                if($ManifestIdentifier){
                    $ManifestIdentifier = "/$ManifestIdentifier"
                }
        
                ###############################
                ##  Rest api call  
                
                ## Specifies the Rest api call that will be performed
                $TriggerName    = "ManifestGet"
                $apiContentType = "application/json"
                $apiMethod      = "Get"
        
                ## Creates the API Post Header
                $apiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
                $apiHeader.Add("Accept", 'application/json')
             }
            "File" {
                $ManifestFileExists  = Test-Path -Path $Path

                IF(!$ManifestFileExists) {
                    $ErrReturnObject = @{
                        FilePath           = $Path
                        ManifestFileExists = $ManifestFileExists
                    }

                    Write-Error -Message "Target path did not point to an object." -TargetObject $ErrReturnObject
                    Return
                }

                $PathProperties = Get-ItemProperty $Path
                $ManifestFile   = ""

                ## If $Path variable is pointing at a directory
                if($PathProperties.Attributes -eq "Directory") {
                    $PathChildItemsJSON = Get-ChildItem -Path $Path -Filter "*.json"
                    $PathChildItemsYAML = Get-ChildItem -Path $Path -Filter "*.yaml"

                    Write-Verbose -Message "Path pointed to a directory, found $($PathChildItemsJSON.count) JSON files, and $($PathChildItemsYAML.count) YAML files."

                    $ApplicationManifest = ""
                    $ManifestFile        = Get-Item -Path $Path
                    $ManifestFileType    = "Directory"
                }
                ## If $Path variable is pointing at a file
                else {
                    ## Single file was provided
                    Write-Verbose -Message "Retrieving the Application Manifest for: $Path"
            
                    if($ManifestFileExists) {
                        ## Gets the Manifest object and contents of the Manifest - identifying the manifest file extension.
                        $ApplicationManifest = Get-Content -Path $Path -Raw
                        $ManifestFile        = Get-Item -Path $Path
                        $ManifestFileType    = $ManifestFile.Extension

                        Write-Verbose -Message "Retrieved content from the manifest ($($ManifestFile.Name))."
                    }
                }
            }
        }        
    }
    PROCESS
    {
        switch ($PsCmdlet.ParameterSetName) {
            "Azure" {
                Write-Verbose -Message "Retrieving Azure Function Web Applications matching to: $FunctionName."

                ## Retrieves the Azure Function URL used to add new manifests to the rest source
                $FunctionApp = Get-AzWebApp -ResourceGroupName $AzureResourceGroupName -Name $FunctionName -ErrorAction SilentlyContinue -ErrorVariable err
                        
                ## can function key be part of the header
                Write-Verbose -Message "Constructing the REST API call."
                
                $FunctionAppId   = $FunctionApp.Id
                $DefaultHostName = $FunctionApp.DefaultHostName
                $FunctionKey     = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default
                $apiHeader.Add("x-functions-key", $FunctionKey)
                $AzFunctionURL   = "https://" + $DefaultHostName + "/api/" + "packageManifests" + $ManifestIdentifier
                
                ## Publishes the Manifest to the Windows Package Manager rest source
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
                        $Result = Test-WinGetManifest -Manifest $ApplicationManifest
                        if($Result) {
                            IF($WinGetDesktopAppInstallerLibLoaded) {
                                Write-Verbose -Message "YAML Files have been found in the target directory. Building a JSON manifest with found files."
                                $ApplicationManifest = [Microsoft.WinGet.RestSource.PowershellSupport.YamlToRestConverter]::AddManifestToPackageManifest($Path, "");
                            }
                            
                            ## Sets the return result to be the contents of the JSON file if the Manifest test passed.
                            $Return = $ApplicationManifest
                            Write-Verbose -Message "Returned Manifest from YAML file: $($Return.PackageIdentifier)"
                        }
                    }
                    ## If the path resolves to a Directory
                    "Directory"{
                        ## If a directory is provided, parse through the directory items for Manifest files
                        if($PathChildItemsJSON.Count -gt 0) {
                            Write-Verbose -Message "Multiple JSON files have been found. Will retrieve all WinGet manifests in the directory."
                            foreach ($Item in $PathChildItemsJSON) {
                                ## Re-runs current Function for each individual file.
                                $Return += [WinGetManifest]::New($(Get-WinGetManifest -Path $Item.FullName))
                            }

                            Write-Verbose "Found ($($Return.Count)) Manifests that matched."
                        }
                        if($PathChildItemsYAML.Count -gt 0) {
                            Write-Verbose -Message "YAML Files have been found in the target directory. Building a JSON manifest with found files."
                            $Return += [Microsoft.WinGet.RestSource.PowershellSupport.YamlToRestConverter]::AddManifestToPackageManifest($Path, "");
                        }
                    }
                    default {
                        if($ManifestFileExists) {
                            $ErrReturnObject = @{
                                ApplicationManifest = $ApplicationManifest
                                ManifestFile        = $ManifestFile
                                $ManifestFileType   = $ManifestFileType
                            }

                            Write-Error -Message "Incorrect filetype. Verify the file is of type '*.yaml' or '*.json' and try again." -Category InvalidType -TargetObject $ErrReturnObject
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