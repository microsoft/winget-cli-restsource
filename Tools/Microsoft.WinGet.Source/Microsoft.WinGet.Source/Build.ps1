<#
    .SYNOPSIS
        Builds PowerShell Module by copying binaries and files.
    
    .PARAMETER OutDir
        Output directory where module files will be placed.
    
    .PARAMETER BinaryDir
        Directory where built DLL files are located.
#>

[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $OutDir = 'build\bin',

    [Parameter(Mandatory)]
    [string]
    $BinaryDir
)

New-Item "$OutDir" -Force -ItemType Directory
Copy-Item "$PSScriptRoot\Microsoft.WinGet.Source.psd1" "$OutDir" -Force -ErrorAction Stop
Copy-Item "$BinaryDir\*.dll" "$OutDir" -Force -ErrorAction Stop
Write-Host "Done!" -ForegroundColor Green
