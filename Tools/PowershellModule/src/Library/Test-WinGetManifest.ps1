
Function Test-WinGetManifest
{
    <#
    .SYNOPSIS
    Incomplete

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
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="File")]  [string]$Path,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Object")]$Manifest,
        [Parameter(Position=1, Mandatory=$false)]  [switch]$ReturnManifest
    )
    BEGIN
    {
        Write-Verbose -Message "Validating the Manifest ($($PSCmdlet.ParameterSetName))."
        switch ($($PSCmdlet.ParameterSetName)) {
            "File"{
                if((Test-Path -Path $Path)) {
                    ## Retrieves the contents of the Manifest file
                    $Manifest = Get-Content -Path $Path -Raw
                }
            }
            "Object" {
            }
        }

        $ManifestFileTypeJSON = $false
        $Return = $true
    }
    PROCESS
    {

    }
    END
    {
        Write-Verbose -Message "Testing the Manifest has passed: $Return"
        ## Determines what will be returned from the Function
        if($Return) {
            ## Returns the Manifest only if the test passes. If the test fails return False
            if($Return) {
                return $Manifest
            }
            else {
                return $Return
            }
        }
        else {
            ## Returns the result of the test. If all test pass, the result is True, otherwise will return False
            return $Return
        }
    }
}