# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
#
# Module manifest for module 'Microsoft.WinGet.Source'
#

@{
    # Script module or binary module file associated with this manifest.
    RootModule = 'Microsoft.WinGet.Source.psm1'

    # Version number of this module.
    ModuleVersion = '0.1.0'

    # ID used to uniquely identify this module
    GUID = 'b70c845d-ddb1-4454-bfc2-a874783c2d04'

    # Company or vendor of this module
    CompanyName = 'Microsoft'

    # Copyright statement for this module
    Copyright = '(c) Microsoft. All rights reserved.'

    # Description of the functionality provided by this module
    Description = 'This module provides support for working with Windows Package Manager REST based sources.'

    # Minimum version of the PowerShell engine required by this module
    PowerShellVersion = '5.1'

    # Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
    FunctionsToExport = @("Add-WinGetManifest", "Get-WinGetManifest", "Remove-WinGetManifest", "New-WinGetSource", "Convert-YamlToJson")

    # Cmdlets to export from this module, for best performance, do not use wild cards and do not delete the entry, use an empty array if there are no cmdlets to export.
    CmdletsToExport = @()

    # Variables to export from this module
    VariablesToExport = '*'

    # Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
    AliasesToExport = @()

    # Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
    PrivateData = @{
        PSData = @{
            # Tags applied to this module. These help with module discovery in online galleries.
            Tags = @("winget")

            # A URL to the main website for this project.
            ProjectUri = 'https://github.com/microsoft/winget-cli-restsource'

            PreRelease = 'alpha'
        }
    }
}
    
    