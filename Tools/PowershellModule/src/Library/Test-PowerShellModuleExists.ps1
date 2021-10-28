# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Test-PowerShellModuleExist
{
    <#
    .SYNOPSIS
    Verifies that the provided Modules are installed.

    .DESCRIPTION
    Verifies that the provided Modules are installed.

    .PARAMETER Name
    String of the PowerShell Module Name.

    .PARAMETER Modules
    String array that contains the names of all PowerShell modules.

    .EXAMPLE
    Test-PowerShellModuleExist -Name "Az.Accounts"

    Verifies that the PowerShell Module "Az.Accounts" is instaled. Returns $true if installed.

    .EXAMPLE
    Test-PowerShellModuleExist -Modules $("Az.Accounts", "Az.Resources")

    Parses through all values in the variable array to validate that they are installed. Returns a Boolean $true if installed.

    #>
    [CmdletBinding(DefaultParameterSetName = 'Multiple')]
    PARAM(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Single")] [string]$Name,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Multiple")] [string[]]$Modules
    )
    BEGIN
    { 
        ## Validation result to be returned is True until proven otherwise.
        $ValidationStatus = $true
        $TestResult       = @()
    }
    PROCESS
    {
        switch ($PsCmdlet.ParameterSetName) {
            "Multiple" {
                foreach ($RequiredModule in $RequiredModules) {
                    ## Tests if the module is installed
                    $Result = Test-PowerShellModuleExist -Name $RequiredModule
                    $TestResult += @{
                        TestedModule    = $RequiredModule
                        ModuleInstalled = $Result
                    }
                    
                    ## Specifies that a module is missing
                    if(!($Result)) {
                        $ValidationStatus = $false
                    }
                }

                if(!($ValidationStatus)) {
                    ## Module Validation failed
                    $ErrReturnObject = @{
                        TestResults = $TestResult
                    }
                    
                    Write-Error "Missing required PowerShell modules. Run the following command to install the missing modules: Install-Module Az" -Category NotInstalled -TargetObject $ErrReturnObject
                }
            }
            "Single" { 
                ## Determines if the PowerShell Module is missing, If missing false if missing
                if(!$(Get-Module -ListAvailable -Name $RequiredModule) ) {
                    $ValidationStatus = $false
                } 
            }
        }
    }
    END
    {
        ## Returns a value only if the module is missing
        Return $ValidationStatus
    }
}