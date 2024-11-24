# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Test-WinGetManifest
{
    <#
    .SYNOPSIS
    [Stub for a backlog issue, essentially does nothing today.]

    .DESCRIPTION


    .PARAMETER SubscriptionName


    .PARAMETER SubscriptionId


    .EXAMPLE
    Test-WinGetManifest -Path ""


    .EXAMPLE
    Test-WinGetManifest -Manifest ""


    #>
    [CmdletBinding(DefaultParameterSetName = 'File')]
    PARAM(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="File")] [string]$Path,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Object")] $Manifest
    )
    BEGIN
    {
        Write-Verbose -Message "Validating the Manifest ($($PSCmdlet.ParameterSetName))."
        
        $Return = $true
        
        switch ($($PSCmdlet.ParameterSetName)) {
            "File"{
                ## Convert to full path if applicable
                $Path = [System.IO.Path]::GetFullPath($Path, $pwd.Path)
                
                $PathFound = Test-Path -Path $Path;
                
                if ($PathFound)
                {
                    ## Construct $Manifest from path then validate
                }
                else
                {
                    Write-Error "Manifest path not found: $Path"
                    $Return = $false;
                }
            }
            "Object" {
            }
        }

        
    }
    PROCESS
    {

    }
    END
    {
        Write-Information "Testing the Manifest has passed: $Return"
        return $Return
    }
}