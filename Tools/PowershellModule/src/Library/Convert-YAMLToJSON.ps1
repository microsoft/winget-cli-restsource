# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Convert-YamlToJson
{
    <#
    .SYNOPSIS
    #
    
    .DESCRIPTION
    #
        
    .PARAMETER SubscriptionName
    #

    .PARAMETER SubscriptionId
    #

    .EXAMPLE
    #

    .EXAMPLE
    #

    .EXAMPLE
    #

    #>
    
    PARAM(
        [Parameter(Position=0, Mandatory=$false)] [string]$Path
    )
    BEGIN
    {
        $Files = Get-ChildItem $Path
        $PackageManifest = [PackageManifest]::new()
    }
    PROCESS
    {
        $PackageManifest.ConvertFromYAML($Files)
    }
    END
    {
        Return $PackageManifest
    }
}