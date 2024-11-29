# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
#
# Module script for module 'Microsoft.WinGet.Source'
#

## Loads the binaries from the Desktop App Installer Library - Only if running PowerShell at a specified edition
try {
    $architecture = [System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture
    
    switch ($architecture) {
        "X64" {
            Copy-Item -Path "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\runtimes\win-x64\native\WinGetUtil.dll" -Destination "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\WinGetUtil.dll" -Force
        }
        "X86" {
            Copy-Item -Path "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\runtimes\win-x86\native\WinGetUtil.dll" -Destination "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\WinGetUtil.dll" -Force
        }
        "Arm64" {
            Copy-Item -Path "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\runtimes\win-arm64\native\WinGetUtil.dll" -Destination "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\WinGetUtil.dll" -Force
        }
        Default {
            throw "Powershell Core runtime architecture not supported"
        }
    }
    
    Add-Type -Path "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\Microsoft.Winget.PowershellSupport.dll"
}
catch {
    ## Exceptions thrown by Add-Type will not fail the Import-Module. Catch and re-throw to fail the Import-Module.
    throw $_
}

## Load classes first
. $PSScriptRoot\Library\WinGetManifest.ps1

## Loads Libraries
Get-ChildItem -Path "$PSScriptRoot\Library" -Filter *.ps1 | foreach-object { . $_.FullName }

## Validates that the required Azure Modules are present when the script is imported.
[string[]]$RequiredModules = @("Az")

foreach ($RequiredModule in $RequiredModules) {
    ## Tests if the module is installed
    $Result = Test-PowerShellModuleExist -Name $RequiredModule
    
    ## Install the missing module
    if(!($Result)) {
        Install-Module $RequiredModule -Force -AllowClobber
    }
}

## Verifies that the Azure Modules were successfully installed.
[Boolean] $TestResult = Test-PowerShellModuleExist -Modules $RequiredModules
if (!$TestResult) { 
    ## Modules have been identified as missing
    throw "There are missing PowerShell modules that must be installed. Some or all PowerShell functions included in this library will fail."
}
