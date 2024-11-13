# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
#
# Module script for module 'Microsoft.WinGet.Source'
#

## Loads Libraries
Get-ChildItem -Path "$PSScriptRoot\Library" -Filter *.ps1 | foreach-object { . $_.FullName }

[string] $WinGetPSEdition = "Core"
[string] $WinGetPSVersion = "7.4.0"

## Loads the binaries from the Desktop App Installer Library - Only if running PowerShell at a specified edition
if ($PSEdition -eq $WinGetPSEdition && [System.Version]$PSVersion -ge [System.Version]$WinGetPSVersion) {
    try {
        Add-Type -Path "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\Microsoft.Winget.PowershellSupport.dll"
    }
    catch {
        ## Exceptions thrown by Add-Type will not fail the Import-Module. Catch and re-throw to fail the Import-Module.
        throw $_
    }
}
else {
    throw "Unable to load required binaries. Verify you are using Powershell $WinGetPSEdition or later."
}

## Validates that the required Azure Modules are present when the script is imported.
[string[]]$RequiredModules = @("Az", "powershell-yaml")

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
    $ErrorMessage = "There are missing PowerShell modules that must be installed. Some or all PowerShell functions included in this library will fail."    
    throw $ErrorMessage
}