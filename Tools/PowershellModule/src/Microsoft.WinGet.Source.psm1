<#
 Importing the Module requires that the Script be signed... or that Running scripts be approved for the computer.

 Compiled RestAPIs Compressed folder must exist in the ".\Library\RestAPI\" container.
 Desktop AppInstaller Library must exist in the ".\Library\AppInstallerLib" container.
#>

## Loads Libraries
Get-ChildItem -Path "$PSScriptRoot\Library" -Filter *.ps1 | foreach-object { . $_.FullName }

[string] $WinGetPSEdition = "Core"

## Loads the binaries from the Desktop App Installer Library - Only if running PowerShell at a specified edition
if ($PSEdition -eq $WinGetPSEdition) {
        try {
                Add-Type -Path "$PSScriptRoot\Library\WinGet.RestSource.PowershellSupport\Microsoft.Winget.PowershellSupport.dll"
                $WinGetDesktopAppInstallerLibLoaded=$true 
        }
        catch [System.Reflection.ReflectionTypeLoadException] {
                Write-Host "Message: $($_.Exception.Message)"
                Write-Host "StackTrace: $($_.Exception.StackTrace)"
                Write-Host "LoaderExceptions: $($_.Exception.LoaderExceptions)"
                throw $_
        }
}
else {
        throw "Unable to load required binaries. Verify you are using Powershell $($WinGetPSEdition)."
}

## Validates that the required Azure Modules are present when the script is imported.
[string[]]$RequiredModules = @("Az.Resources", "Az.Accounts", "Az.Websites", "Az.Functions", "powershell-yaml")

## Verifies that the Azure Modules were previously installed.
[Boolean] $TestResult = Test-PowerShellModuleExist -Modules $RequiredModules
if (!$TestResult) { 
        ## Installs the required Azure Modules if not already installed
        $RequiredModules | ForEach-Object { Install-Module $_ -Force }
}

## Verifies that the Azure Modules were successfully installed.
[Boolean] $TestResult = Test-PowerShellModuleExist -Modules $RequiredModules
if (!$TestResult) { 
        ## Modules have been identified as missing
        Write-Host ""
        $ErrorMessage = "There are missing PowerShell modules that must be installed.`n"
        $ErrorMessage += "    Some or all PowerShell functions included in this library will fail.`n"
        $ErrorMessage += "    Run the following command to install the missing modules: Install-Module Az -Force`n`n"
        
        Write-Error -Message $ErrorMessage
}