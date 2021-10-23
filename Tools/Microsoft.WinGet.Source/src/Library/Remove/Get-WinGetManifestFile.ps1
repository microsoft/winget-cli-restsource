<#
    .SYNOPSIS
    Gets a manifest from a REST source. Note, this initial alpha implementation only supports private sources running on Azure. 
    Once we are informed of an implementation that is not on Azure, we wil make this a bit more generic.
    
    .DESCRIPTION
    By running this function with the required inputs, it will connect to a REST source, then retrieve the Manifest for the specified package.
    
    .PARAMETER Source
    Name of the Windows Package Manager private source. Can be identified by running: "Get-WinGetSource" and using the source Name
    
    .PARAMETER Id
    Used to specify the Id of the package

    .PARAMETER Name
    Used to specify the Name of the package

    .EXAMPLE
    Get-WinGetManifest -id "Publisher.Package"

    This example expects only a single configured REST source with a package containing "Publisher.Package" as a valid identifier.

    .EXAMPLE
    Get-WinGetManifest -id "Publisher.Package" -source "Private"

    This example expects the REST source named "Private" with a package containing "Publisher.Package" as a valid identifier.

    .EXAMPLE
    Get-WinGetManifest -Name "Package"

    This example expects the REST source named "Private" with a package containing "Package" as a valid name.
#>

Function Get-WinGetManifestFile
{
    PARAM(
        [Parameter(Position=0, Mandatory=$true)] [string]$Path
    )
    BEGIN
    {
        Write-LogEntry -LogEntry "Retrieving the Application Manifest" -Severity 1

        $Result              = $true
        $ManifestFile        = ""
        $ManifestFileExists  = Test-Path -Path $Path -PathType Leaf

        if($ManifestFileExists) {
            ## Gets the Manifest object and contents of the Manifest - identifying the manifest file extension.
            $ApplicationManifest = Get-Content -Path $Path -Raw
            $ManifestFile        = Get-Item -Path $Path
            $ManifestFileType    = $ManifestFile.Extension
        }
        else {
            ## The Manifest path did not resolve to a file.
            $Result = $false
            Write-LogEntry -LogEntry "Unable to locate the Manifest File, verify the file exists and try again." -Severity 3
        }
    }
    PROCESS
    {
        switch ($ManifestFileType) {
            ## If the File type is JSON
            ".json" {
                ## Removes spacing from the JSON content
                $ApplicationManifest = $($ApplicationManifest -replace "`t|`n|`r|  ","")
                $ApplicationManifest = $($($($($ApplicationManifest -replace ": ",":") -replace " { |{ ", "{") -replace ', ', ',') -replace " } | }", "}")

                $Result = Test-WinGetManifest -Manifest $ApplicationManifest
                if($Result) {
                    ## Sets the return result to be the contents of the JSON file if the Manifest test passed.
                    $Result = $ApplicationManifest
                }
            }
            ## If the File type is YAML
            ".yaml" {
                $Result = Test-WinGetManifest -Manifest $ApplicationManifest
                if($Result) {
                    ## Sets the return result to be the contents of the JSON file if the Manifest test passed.
                    $Result = $ApplicationManifest
                }
            }
            default {
                if($ManifestFileExists) {
                    $Result = $false
                    Write-LogEntry -LogEntry "Incorrect filetype. Verify the file if of type '*.yaml' or '*.json' and try again." -Severity 3
                }
            }
        }
    }
    END
    {
        return $Result
    }
}