<#
        Importing the Module requires that the Script be signed... or that Running scripts be approved for the computer.

        Compiled RestAPIs Compressed folder must exist in the ".\Library\RestAPI\" container.
        Desktop AppInstaller Libs must exist in the ".\Library\AppInstallerLib" container.
#>

## Loads Librarys
Get-ChildItem -Path "$PSScriptRoot\Library" -Filter *.ps1 | foreach-object { . $_.FullName }

[version] $WinGetModulePsVersion    = $(Get-Host).Version
[version] $WinGetLibMinVersionPS51  = [version]::New("5.1")
$WinGetDesktopAppInstallerLibLoaded = $false

## Loads the binaries from the Desktop App Installer Library - Only if running PowerShell at a specified version
if ($WinGetModulePsVersion -ge $WinGetLibMinVersionPS51) {
        $WinGetDesktopAppInstallerLibLoaded = $true
        Add-Type -Path "$PSScriptRoot\Library\AppInstallerLib\Microsoft.Winget.PowershellSupport.dll"
}
else {
        throw "Unable to load required binaries. Verify your PowerShell version is greater than $($WinGetLibMinVersionPS51.ToString())."
}

## Validates that the required Azure Modules are present when the script is imported.
[string[]]$RequiredModules = @("Az.Resources", "Az.Accounts", "Az.Websites", "Az.Functions")

## Verifies that the Azure Modules were previously installed.
[Boolean] $TestResult = Test-PowerShellModuleExist -Modules $RequiredModules
If(!$TestResult)
{ 
        ## Installs the required Azure Modules if not already installed
        $RequiredModules | ForEach-Object { Install-Module $_ -Force }
}

## Verifies that the Azure Modules were successfully installed.
[Boolean] $TestResult = Test-PowerShellModuleExist -Modules $RequiredModules
If(!$TestResult)
{ 
        ## Modules have been identified as missing
        Write-Host ""
        $ErrorMessage = "There are missing PowerShell modules that must be installed.`n"
        $ErrorMessage += "    Some or all PowerShell functions included in this library will fail.`n"
        $ErrorMessage += "    Run the following command to install the missing modules: Install-Module Az -Force`n`n"
        
        Write-Error -Message $ErrorMessage
}