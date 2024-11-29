# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Test-WinGetManifest
{
    <#
    .SYNOPSIS
    [TODO: Stub for a backlog issue, essentially does nothing today.]

    .DESCRIPTION
    Validates a WinGet Manifest. 

    .PARAMETER Path
    Points to either a folder containing a specific application's manifest of type .json or .yaml or to a specific .json or .yaml file.

    If you are processing a multi-file manifest, point to the folder that contains all yamls. Note: all yamls within the folder must be part of
    the same package manifest.

    .PARAMETER Manifest
    The WinGetManifest object

    .EXAMPLE
    Test-WinGetManifest -Path "C:\WinGetManifest"


    .EXAMPLE
    Test-WinGetManifest -Manifest $Manifest


    #>
    [CmdletBinding(DefaultParameterSetName = 'File')]
    PARAM(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="File")] [string]$Path,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Object")] [WinGetManifest]$Manifest
    )

    $Return = $false
    
    switch ($($PSCmdlet.ParameterSetName)) {
        "File"{
            ## Convert to full path if applicable
            $Path = [System.IO.Path]::GetFullPath($Path, $pwd.Path)
            
            $PathFound = Test-Path -Path $Path;
            if ($PathFound)
            {
                ## Construct $Manifest from path then validate
                $Return = $true;
            }
            else
            {
                Write-Error "Manifest path not found: $Path"
            }
        }
        "Object" {
            ## Validate manifest
            $Return = $true;
        }
    }

    Write-Information "Testing the Manifest has passed: $Return"
    return $Return
}