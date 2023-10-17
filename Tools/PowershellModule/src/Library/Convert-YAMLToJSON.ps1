# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Convert-YamlToJson
{
    <#
    .SYNOPSIS
    Converts the YAML files in a specific directory to a PowerShell Object that can be exported to a JSON file.
    
    .DESCRIPTION
    Converts the YAML files in a specific directory to a PowerShell Object that can be exported to a JSON file.
        
    .PARAMETER Path
    Path to the directory containing YAML files.

    .EXAMPLE
    Convert-YamlToJson -Path "C:\Folder\""

    #>
    
    PARAM(
        [Parameter(Position=0, Mandatory=$false)] [string]$Path
    )
    BEGIN
    {
        $Files = Get-ChildItem $Path
        $PackageManifest = [PackageManifest]::new()
    }
    PROCESS
    {
        $PackageManifest.ConvertFromYAML($Files)
    }
    END
    {
        Return $PackageManifest
    }
}