<#
        Importing the Module requires that the Script be signed... or that Running scripts be approved for the computer.

        Compiled RestAPIs Compressed folder must exist in the ".\Library\RestAPI\" container.
        Desktop AppInstaller Library must exist in the ".\Library\AppInstallerLib" container.
#>

## Loads Libraries
Get-ChildItem -Path "$PSScriptRoot\Library" -Filter *.ps1 | foreach-object { . $_.FullName }

[version] $WinGetModulePsVersion    = $(Get-Host).Version
[version] $WinGetLibMinVersionPS51  = [version]::New("5.1")
$WinGetDesktopAppInstallerLibLoaded = $false

## Loads the binaries from the Desktop App Installer Library - Only if running PowerShell at a specified version
if ($WinGetModulePsVersion -ge $WinGetLibMinVersionPS51) {
        if([intPtr]::size -eq 4){
                ## PowerShell window is in x86 architecture.
                Add-Type -Path "$PSScriptRoot\Library\HelperLib\x86\Microsoft.Winget.PowershellSupport.dll"
                $WinGetDesktopAppInstallerLibLoaded = $true
        }
        else{
                ## PowerShell window is in x64 architecture.
                Add-Type -Path "$PSScriptRoot\Library\HelperLib\x64\Microsoft.Winget.PowershellSupport.dll"
                $WinGetDesktopAppInstallerLibLoaded = $true
        }
}
else {
        throw "Unable to load required binaries. Verify your PowerShell version is greater than $($WinGetLibMinVersionPS51.ToString())."
}

## Validates that the required Azure Modules are present when the script is imported.
[string[]]$RequiredModules = @("Az.Resources", "Az.Accounts", "Az.Websites", "Az.Functions")

## Verifies that the Azure Modules were previously installed.
[Boolean] $TestResult = Test-PowerShellModuleExist -Modules $RequiredModules
if(!$TestResult) { 
        ## Installs the required Azure Modules if not already installed
        $RequiredModules | ForEach-Object { Install-Module $_ -Force }
}

## Verifies that the Azure Modules were successfully installed.
[Boolean] $TestResult = Test-PowerShellModuleExist -Modules $RequiredModules
if(!$TestResult) { 
        ## Modules have been identified as missing
        Write-Host ""
        $ErrorMessage = "There are missing PowerShell modules that must be installed.`n"
        $ErrorMessage += "    Some or all PowerShell functions included in this library will fail.`n"
        $ErrorMessage += "    Run the following command to install the missing modules: Install-Module Az -Force`n`n"
        
        Write-Error -Message $ErrorMessage
}