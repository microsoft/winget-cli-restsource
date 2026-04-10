# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

function Get-WinGetManifest {
    <#
    .SYNOPSIS
    Connects to the specified Windows Package Manager source, or local file system path to retrieve the package manifest, returning
    the manifest found.

    .DESCRIPTION
    Connects to the specified Windows Package Manager source, or local file system path to retrieve the package Manifest, returning
    the manifest found. Allows for retrieving results based on the package identifier when targeting the Windows Package Manager source.

    .PARAMETER Path
    Points to either a folder containing a specific Manifest of type .json or .yaml or to a specific .json or .yaml file.

    If you are processing a multi-file Manifest, point to the folder that contains all yamls. Note: all yamls within the folder must be part of
    the same Manifest.

    .PARAMETER PriorManifest
    A WinGetManifest object containing a single Windows Package Manager REST source Manifest that will be merged with locally processed .yaml files.
    This is used by the script infrastructure internally.

    .PARAMETER FunctionName
    Name of the Azure Function that contains the Windows Package Manager REST source.

    .PARAMETER PackageIdentifier
    Supports input from pipeline. The Windows Package Manager Package Identifier of a specific Package Manifest result.

    .PARAMETER SubscriptionName
    [Optional] The name of the subscription containing the Windows Package Manager REST source.

    .EXAMPLE
    Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys"

    Returns a Manifest object based on the files found within the specified Path.

    .EXAMPLE
    Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys\Microsoft.PowerToys.json"

    Returns a Manifest object (*.json) of the specified JSON file.

    .EXAMPLE
    Get-WinGetManifest -FunctionName "contosorestsource" -PackageIdentifier "Windows.PowerToys"

    Returns a Manifest object of the specified Package Identifier that is queried against the Windows Package Manager REST source.

    .EXAMPLE
    Get-WinGetManifest -FunctionName "contosorestsource" -PackageIdentifier "Windows.PowerToys" -SubscriptionName "Visual Studio Subscription"

    Returns a Manifest object of the specified Package Identifier that is queried against the Windows Package Manager REST source from the specified Subscription Name.

    #>
    [CmdletBinding(DefaultParameterSetName = 'Azure')]
    param(
        [Parameter(Position = 0, Mandatory = $true, ParameterSetName = 'File')]  [string]$Path,
        [Parameter(Mandatory = $false, ParameterSetName = 'File')]  [WinGetManifest]$PriorManifest = $null,
        [Parameter(Position = 0, Mandatory = $true, ParameterSetName = 'Azure')] [string]$FunctionName,
        [Parameter(Position = 1, Mandatory = $true, ParameterSetName = 'Azure', ValueFromPipeline = $true)][ValidateNotNullOrEmpty()] [string]$PackageIdentifier,
        [Parameter(Mandatory = $false, ParameterSetName = 'Azure')] [string]$SubscriptionName = ''
    )
    begin {
        [WinGetManifest[]] $Return = @()

        ###############################
        ## Determines the PowerShell Parameter Set that was used in the call of this Function.
        switch ($PsCmdlet.ParameterSetName) {
            'Azure' {
                ###############################
                ## Connects to Azure, if not already connected.
                Write-Verbose 'Validating connection to azure, will attempt to connect if not already connected.'
                $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
                if (!($Result)) {
                    Write-Error 'Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials.' -ErrorAction Stop
                }

                ###############################
                ## Gets Resource Group name of the Azure Function
                Write-Verbose -Message 'Determines the Azure Function Resource Group Name'
                $ResourceGroupName = $(Get-AzFunctionApp).Where({ $_.Name -eq $FunctionName }).ResourceGroupName
                if (!$ResourceGroupName) {
                    Write-Error "Failed to confirm Azure Function exists in Azure. Please verify and try again. Function Name: $FunctionName" -ErrorAction Stop
                }

                ## Retrieves the Azure Function URL used to add new manifests to the REST source
                Write-Verbose -Message "Retrieving Azure Function Web Applications matching to: $FunctionName."
                $FunctionApp = Get-AzFunctionApp -ResourceGroupName $ResourceGroupName -Name $FunctionName

                $FunctionAppId = $FunctionApp.Id
                $DefaultHostName = $FunctionApp.DefaultHostName

                $TriggerName = 'ManifestGet'
                $ApiMethod = 'Get'

                ## Creates the API Post Header
                $ApiHeader = New-Object 'System.Collections.Generic.Dictionary[[String],[String]]'
                $ApiHeader.Add('Accept', 'application/json')
                $FunctionKey = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default
                $ApiHeader.Add('x-functions-key', $FunctionKey)
            }
            'File' {
                ## Nothing to prepare
            }
        }        
    }
    process {
        switch ($PsCmdlet.ParameterSetName) {
            'Azure' {
                $AzFunctionURL = 'https://' + $DefaultHostName + '/api/packageManifests/' + $PackageIdentifier
                
                ## Publishes the Manifest to the Windows Package Manager REST source
                Write-Verbose -Message 'Invoking the REST API call.'

                $Response = Invoke-RestMethod $AzFunctionURL -Headers $ApiHeader -Method $ApiMethod -ErrorVariable ErrorInvoke

                if ($ErrorInvoke) {
                    $ErrorMessage = "Failed to get Manifest from $FunctionName. Verify the information you provided and try again."
                    $ErrReturnObject = @{
                        AzFunctionURL = $AzFunctionURL
                        ApiMethod     = $ApiMethod
                        Response      = $Response
                        InvokeError   = $ErrorInvoke
                    }
                
                    Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                } else {
                    Write-Verbose "Found ($($Response.Data.Count)) Manifests that matched."

                    foreach ($ResponseData in $Response.Data) {
                        Write-Verbose -Message "Parsing through the returned results: $ResponseData"
                        $Return += [WinGetManifest]::CreateFromObject($ResponseData)
                        Write-Information "Returned Manifest from JSON file: $($Return[-1].PackageIdentifier)"
                    }
                }
            }
            'File' {
                ## Convert to full path if applicable
                $Path = [System.IO.Path]::GetFullPath($Path, $pwd.Path)
                
                $ManifestFileExists = Test-Path -Path $Path

                if (!$ManifestFileExists) {
                    $ErrReturnObject = @{
                        FilePath           = $Path
                        ManifestFileExists = $ManifestFileExists
                    }

                    Write-Error -Message 'Target path did not point to an object.' -TargetObject $ErrReturnObject
                    return
                }

                $PathItem = Get-Item $Path
                $ManifestFile = ''
                $ApplicationManifest = ''
                $ManifestFileType = ''

                if ($PathItem.PSIsContainer) {
                    ## $Path variable is pointing at a directory
                    $PathChildItemsJSON = Get-ChildItem -Path $Path -Filter '*.json'
                    $PathChildItemsYAML = Get-ChildItem -Path $Path -Filter '*.yaml'

                    Write-Verbose -Message "Path pointed to a directory, found $($PathChildItemsJSON.count) JSON files, and $($PathChildItemsYAML.Count) YAML files."
                    
                    $ErrReturnObject = @{
                        JSONFiles = $PathChildItemsJSON
                        YAMLFiles = $PathChildItemsYAML
                        JSONCount = $PathChildItemsJSON.Count
                        YAMLCount = $PathChildItemsYAML.Count
                    }

                    ## Validating found objects
                    if ($PathChildItemsJSON.Count -eq 0 -and $PathChildItemsYAML.Count -eq 0) {
                        ## No JSON or YAML files were found in the directory.
                        $ErrorMessage = 'Directory does not contain any JSON or YAML files.'
                        Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                        return
                    } elseif ($PathChildItemsJSON.Count -gt 0 -and $PathChildItemsYAML.Count -gt 0) {
                        ## A combination of JSON and YAML Files were found.
                        $ErrorMessage = 'Directory contains a combination of JSON and YAML files.'
                        Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                        return
                    } elseif ($PathChildItemsJSON.Count -gt 1) {
                        ## More than one Package Manifest's JSON files was found.
                        $ErrorMessage = 'Directory contains more than one JSON file.'
                        Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                        return
                    } elseif ($PathChildItemsJSON.Count -eq 1) {
                        ## Single JSON has been found in the target folder.
                        Write-Verbose -Message 'Single JSON has been found in the specified directory.'
                        $ManifestFile = $PathChildItemsJSON
                        $ApplicationManifest = Get-Content -Path $PathChildItemsJSON[0].FullName -Raw
                        $ManifestFileType = '.json'
                    } elseif ($PathChildItemsYAML.Count -gt 0) {
                        Write-Verbose -Message 'YAML has been found in the specified directory.'
                        ## YAML has been found in the target folder.
                        $ManifestFile = $PathChildItemsYAML
                        $ManifestFileType = '.yaml'
                    }
                } else {
                    ## $Path variable is pointing at a file
                    Write-Verbose -Message "Retrieving the Package Manifest for: $Path"
            
                    ## Gets the Manifest object and contents of the Manifest - identifying the manifest file extension.
                    $ApplicationManifest = Get-Content -Path $Path -Raw
                    $ManifestFile = Get-Item -Path $Path
                    $ManifestFileType = $ManifestFile.Extension.ToLower()

                    Write-Verbose -Message "Retrieved content from the manifest ($($ManifestFile.Name))."
                }
                
                switch ($ManifestFileType) {
                    ## If the path resolves to a JSON file
                    '.json' {
                        ## Sets the return result to be the contents of the JSON file if the Manifest test passed.
                        $Return += [WinGetManifest]::CreateFromString($ApplicationManifest) 
                        Write-Information "Returned Manifest from JSON file: $($Return[-1].PackageIdentifier)"
                    }
                    ## If the path resolves to a YAML file
                    '.yaml' {
                        ## Directory - *.yaml files included within.
                        Write-Verbose -Message 'YAML Files have been found in the target directory. Building a JSON manifest with found files.'
                        if ($PriorManifest) {
                            Write-Verbose 'Prior manifest provided. New manifest will be merged with prior manifest.'
                            $Return += [WinGetManifest]::CreateFromString([Microsoft.WinGet.RestSource.PowershellSupport.YamlToRestConverter]::AddManifestToPackageManifest($Path, $PriorManifest.GetJson()))
                        } else {
                            Write-Verbose 'Prior manifest not provided.'
                            $Return += [WinGetManifest]::CreateFromString([Microsoft.WinGet.RestSource.PowershellSupport.YamlToRestConverter]::AddManifestToPackageManifest($Path, ''))
                        }

                        Write-Information "Returned Manifest from JSON file: $($Return[-1].PackageIdentifier)"
                    }
                    default {
                        $ErrorMessage = "Incorrect file type. Verify the file is of type '*.yaml' or '*.json' and try again."
                        $ErrReturnObject = @{
                            ApplicationManifest = $ApplicationManifest
                            ManifestFile        = $ManifestFile
                            $ManifestFileType   = $ManifestFileType
                        }

                        Write-Error -Message $ErrorMessage -TargetObject $ErrReturnObject
                    }
                }
            }
        }
    }
    end {
        ## Returns results
        Write-Verbose -Message "Returning ($($Return.Count)) manifests based on search."
        return $Return
    }
}
