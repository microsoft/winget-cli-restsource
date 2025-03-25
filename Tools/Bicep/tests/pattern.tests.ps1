#Requires -Version 7

param (
    [Parameter(Mandatory = $false)]
    [string] $repoRootPath = (Get-Item -Path $PSScriptRoot).Parent.FullName,

    [Parameter(Mandatory = $false)]
    [switch] $TestBasicPattern
)

BeforeDiscovery {
    Write-Verbose ("repoRootPath: $repoRootPath") -Verbose

    if (-not (Get-Command bicep -ErrorAction SilentlyContinue)) {
        # Create the install folder
        $installPath = "$env:USERPROFILE\.bicep"
        $installDir = New-Item -ItemType Directory -Path $installPath -Force
        $installDir.Attributes += 'Hidden'
        # Fetch the latest Bicep CLI binary
        (New-Object Net.WebClient).DownloadFile("https://github.com/Azure/bicep/releases/latest/download/bicep-win-x64.exe", "$installPath\bicep.exe")
        # Add bicep to your PATH
        $currentPath = (Get-Item -path "HKCU:\Environment" ).GetValue('Path', '', 'DoNotExpandEnvironmentNames')
        if (-not $currentPath.Contains("%USERPROFILE%\.bicep")) { setx PATH ($currentPath + ";%USERPROFILE%\.bicep") }
        if (-not $env:path.Contains($installPath)) { $env:path += ";$installPath" }
    }

    $patternsToTest = [System.Collections.ArrayList]@()
    
    if ($TestBasicPattern.IsPresent) {
        $pattern = @{
            BicepFile     = (Join-Path $repoRootPath 'ptn' 'basic.bicep')
            ParameterFile = (Join-Path $repoRootPath 'parameters' 'basic.bicepparam') 
        }

        $patternsToTest.Add($pattern)   
    }
}

Describe 'Bicep Linter' -Tag 'Linting' {
    Context 'General linting' {

        It '[<BicepFile>] Should not have any linting errors' -TestCases $patternsToTest {
            param(
                [string] $BicepFile
            )

            $Result = bicep lint $BicepFile --diagnostics-format sarif | ConvertFrom-Json
            $Result.runs.results.Count | Should -Be 0
        }
    }
}
