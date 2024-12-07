# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
#
# Module script for module 'Microsoft.WinGet.Source'
#

## Loads the binaries from the Desktop App Installer Library - Only if running PowerShell at a specified edition
try {
    $Architecture = [System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture.ToString().ToLower()
    
    if ($Architecture -eq "x64" -or $Architecture -eq "x86" -or $Architecture -eq "arm64") {
        Copy-Item -Path "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\runtimes\win-$Architecture\native\WinGetUtil.dll" -Destination "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\WinGetUtil.dll" -Force -ErrorAction SilentlyContinue
    }
    else {
        Write-Error "Powershell Core runtime architecture not supported" -ErrorAction Stop
    }

    if (!$(Test-Path -Path "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\WinGetUtil.dll")) {
        Write-Error "WinGetUtil.dll not found under $PSScriptRoot\Library\WinGet.RestSource.PowershellSupport directory. Please manually copy the dll from $PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\runtimes\win-$Architecture\native directory and try again." -ErrorAction Stop
    }
    
    Add-Type -Path "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\Microsoft.Winget.PowershellSupport.dll" -ErrorAction Stop
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
