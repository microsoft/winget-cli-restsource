# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
class WinGetLocale
{
    $Moniker
    $PackageLocale
    $Publisher
    $PublisherUrl
    $PublisherSupportUrl
    $PrivacyUrl
    $Author
    $PackageName
    $PackageUrl
    $License
    $LicenseUrl
    $Copyright
    $CopyRightUrl
    $ShortDescription
    $Description
    $ReleaseNotes
    $ReleaseNotesUrl
    $Agreements
    $Tags

    WinGetLocale ([string] $a)
    {
        $Converted = $a | ConvertFrom-Json

        $this.Moniker             = $Converted.Moniker
        $this.PackageLocale       = $Converted.PackageLocale
        $this.Publisher           = $Converted.Publisher
        $this.PublisherUrl        = $Converted.PublisherUrl
        $this.PublisherSupportUrl = $Converted.PublisherSupportUrl
        $this.PrivacyUrl          = $Converted.PrivacyUrl
        $this.Author              = $Converted.Author
        $this.PackageName         = $Converted.PackageName
        $this.PackageUrl          = $Converted.PackageUrl
        $this.License             = $Converted.License
        $this.LicenseUrl          = $Converted.LicenseUrl
        $this.Copyright           = $Converted.Copyright
        $this.CopyRightUrl        = $Converted.CopyRightUrl
        $this.ShortDescription    = $Converted.ShortDescription
        $this.Description         = $Converted.Description
        $this.ReleaseNotes        = $Converted.ReleaseNotes
        $this.ReleaseNotesUrl     = $Converted.ReleaseNotesUrl
        $this.Agreements          = $Converted.Agreements
        $this.Tags                = $Converted.Tags
    }
    WinGetLocale ([WinGetLocale] $a)
    {
        $this.Moniker             = $a.Moniker
        $this.PackageLocale       = $a.PackageLocale
        $this.Publisher           = $a.Publisher
        $this.PublisherUrl        = $a.PublisherUrl
        $this.PublisherSupportUrl = $a.PublisherSupportUrl
        $this.PrivacyUrl          = $a.PrivacyUrl
        $this.Author              = $a.Author
        $this.PackageName         = $a.PackageName
        $this.PackageUrl          = $a.PackageUrl
        $this.License             = $a.License
        $this.LicenseUrl          = $a.LicenseUrl
        $this.Copyright           = $a.Copyright
        $this.CopyRightUrl        = $a.CopyRightUrl
        $this.ShortDescription    = $a.ShortDescription
        $this.Description         = $a.Description
        $this.ReleaseNotes        = $a.ReleaseNotes
        $this.ReleaseNotesUrl     = $a.ReleaseNotesUrl
        $this.Agreements          = $a.Agreements
        $this.Tags                = $a.Tags
    }
    WinGetLocale ([psobject] $a) {
        $this.Moniker             = $a.Moniker
        $this.PackageLocale       = $a.PackageLocale
        $this.Publisher           = $a.Publisher
        $this.PublisherUrl        = $a.PublisherUrl
        $this.PublisherSupportUrl = $a.PublisherSupportUrl
        $this.PrivacyUrl          = $a.PrivacyUrl
        $this.Author              = $a.Author
        $this.PackageName         = $a.PackageName
        $this.PackageUrl          = $a.PackageUrl
        $this.License             = $a.License
        $this.LicenseUrl          = $a.LicenseUrl
        $this.Copyright           = $a.Copyright
        $this.CopyRightUrl        = $a.CopyRightUrl
        $this.ShortDescription    = $a.ShortDescription
        $this.Description         = $a.Description
        $this.ReleaseNotes        = $a.ReleaseNotes
        $this.ReleaseNotesUrl     = $a.ReleaseNotesUrl
        $this.Agreements          = $a.Agreements
        $this.Tags                = $a.Tags
    }
    
    [WinGetLocale[]] Add ([WinGetLocale] $a)
    {
        $FirstValue  = [WinGetLocale]::New($this)
        $SecondValue = [WinGetLocale]::New($a)

        [WinGetLocale[]]$Result = @([WinGetLocale]::New($FirstValue), [WinGetLocale]::New($SecondValue))

        Return $Result
    }

    [WinGetLocale[]] Add ([psobject]$a)
    {
        $FirstValue  = [WinGetLocale]::New($this)
        $SecondValue = [WinGetLocale]::New($a)
        
        [WinGetLocale[]] $Combined = @([WinGetLocale]::New($FirstValue), [WinGetLocale]::New($SecondValue))

        Return $Combined
    }

    [WinGetLocale[]] Add ([String[]]$a)
    {
        $FirstValue  = [WinGetLocale]::New($this)
        $SecondValue = [WinGetLocale]::New($a)
        
        [WinGetLocale[]] $Combined = @([WinGetLocale]::New($FirstValue), [WinGetLocale]::New($SecondValue))

        Return $Combined
    }
}

class WinGetInstaller
{
    $InstallerIdentifier
    $InstallerSha256
    $InstallerUrl
    $Architecture
    $InstallerLocale
    $Platform
    $MinimumOsVersion
    $InstallerType
    $Scope
    $SignatureSha256
    $InstallModes
    $InstallerSwitches
    $InstallerSuccessCodes
    $ExpectedReturnCodes
    $UpgradeBehavior
    $Commands
    $Protocols
    $FileExtensions
    $Dependencies
    $PackageFamilyName
    $ProductCode
    $Capabilities
    $RestricedCapabilities
    $MSStoreProductIdentifier
    $InstallerAbortsTerminal
    $ReleaseDate
    $InstallLocationRequired
    $RequireExplicitUpgrade
    $ElevationRequirement
    $UnsupportedOSArchitectures
    $AppsAndFeaturesEntries
    $Markets
    $DownloadCommandProhibited
    $RepairBehavior
    $ArchiveBinariesDependOnPath

    WinGetInstaller ([string] $a)
    {
        $Converted = $a | ConvertFrom-Json

        $this.InstallerIdentifier       = $Converted.InstallerIdentifier
        $this.InstallerSha256           = $Converted.InstallerSha256
        $this.InstallerUrl              = $Converted.InstallerUrl
        $this.Architecture              = $Converted.Architecture
        $this.InstallerLocale           = $Converted.InstallerLocale
        $this.Platform                  = $Converted.Platform
        $this.MinimumOsVersion          = $Converted.MinimumOsVersion
        $this.InstallerType             = $Converted.InstallerType
        $this.Scope                     = $Converted.Scope
        $this.SignatureSha256           = $Converted.SignatureSha256
        $this.InstallModes              = $Converted.InstallModes
        $this.InstallerSwitches         = $Converted.InstallerSwitches
        $this.InstallerSuccessCodes     = $Converted.InstallerSuccessCodes
        $this.ExpectedReturnCodes       = $Converted.ExpectedReturnCodes
        $this.UpgradeBehavior           = $Converted.UpgradeBehavior
        $this.Commands                  = $Converted.Commands
        $this.Protocols                 = $Converted.Protocols
        $this.FileExtensions            = $Converted.FileExtensions
        $this.Dependencies              = $Converted.Dependencies
        $this.PackageFamilyName         = $Converted.PackageFamilyName
        $this.ProductCode               = $Converted.ProductCode
        $this.Capabilities              = $Converted.Capabilities
        $this.RestricedCapabilities     = $Converted.RestricedCapabilities
        $this.MSStoreProductIdentifier  = $Converted.MSStoreProductIdentifier
        $this.InstallerAbortsTerminal   = $Converted.InstallerAbortsTerminal
        $this.ReleaseDate               = $Converted.ReleaseDate
        $this.InstallLocationRequired   = $Converted.InstallLocationRequired
        $this.RequireExplicitUpgrade    = $Converted.RequireExplicitUpgrade
        $this.ElevationRequirement      = $Converted.ElevationRequirement
        $this.UnsupportedOSArchitectures= $Converted.UnsupportedOSArchitectures
        $this.AppsAndFeaturesEntries    = $Converted.AppsAndFeaturesEntries
        $this.Markets                   = $Converted.Markets
        $this.DownloadCommandProhibited = $Converted.DownloadCommandProhibited
        $this.RepairBehavior            = $Converted.RepairBehavior
        $this.ArchiveBinariesDependOnPath = $Converted.ArchiveBinariesDependOnPath
    }
    WinGetInstaller ([WinGetInstaller] $a)
    {
        $this.InstallerIdentifier       = $a.InstallerIdentifier
        $this.InstallerSha256           = $a.InstallerSha256
        $this.InstallerUrl              = $a.InstallerUrl
        $this.Architecture              = $a.Architecture
        $this.InstallerLocale           = $a.InstallerLocale
        $this.Platform                  = $a.Platform
        $this.MinimumOsVersion          = $a.MinimumOsVersion
        $this.InstallerType             = $a.InstallerType
        $this.Scope                     = $a.Scope
        $this.SignatureSha256           = $a.SignatureSha256
        $this.InstallModes              = $a.InstallModes
        $this.InstallerSwitches         = $a.InstallerSwitches
        $this.InstallerSuccessCodes     = $a.InstallerSuccessCodes
        $this.ExpectedReturnCodes       = $a.ExpectedReturnCodes
        $this.UpgradeBehavior           = $a.UpgradeBehavior
        $this.Commands                  = $a.Commands
        $this.Protocols                 = $a.Protocols
        $this.FileExtensions            = $a.FileExtensions
        $this.Dependencies              = $a.Dependencies
        $this.PackageFamilyName         = $a.PackageFamilyName
        $this.ProductCode               = $a.ProductCode
        $this.Capabilities              = $a.Capabilities
        $this.RestricedCapabilities     = $a.RestricedCapabilities
        $this.MSStoreProductIdentifier  = $a.MSStoreProductIdentifier
        $this.InstallerAbortsTerminal   = $a.InstallerAbortsTerminal
        $this.ReleaseDate               = $a.ReleaseDate
        $this.InstallLocationRequired   = $a.InstallLocationRequired
        $this.RequireExplicitUpgrade    = $a.RequireExplicitUpgrade
        $this.ElevationRequirement      = $a.ElevationRequirement
        $this.UnsupportedOSArchitectures= $a.UnsupportedOSArchitectures
        $this.AppsAndFeaturesEntries    = $a.AppsAndFeaturesEntries
        $this.Markets                   = $a.Markets
        $this.DownloadCommandProhibited = $a.DownloadCommandProhibited
        $this.RepairBehavior            = $a.RepairBehavior
        $this.ArchiveBinariesDependOnPath = $a.ArchiveBinariesDependOnPath
    }
    WinGetInstaller ([psobject] $a) {
        $this.InstallerIdentifier       = $a.InstallerIdentifier
        $this.InstallerSha256           = $a.InstallerSha256
        $this.InstallerUrl              = $a.InstallerUrl
        $this.Architecture              = $a.Architecture
        $this.InstallerLocale           = $a.InstallerLocale
        $this.Platform                  = $a.Platform
        $this.MinimumOsVersion          = $a.MinimumOsVersion
        $this.InstallerType             = $a.InstallerType
        $this.Scope                     = $a.Scope
        $this.SignatureSha256           = $a.SignatureSha256
        $this.InstallModes              = $a.InstallModes
        $this.InstallerSwitches         = $a.InstallerSwitches
        $this.InstallerSuccessCodes     = $a.InstallerSuccessCodes
        $this.ExpectedReturnCodes       = $a.ExpectedReturnCodes
        $this.UpgradeBehavior           = $a.UpgradeBehavior
        $this.Commands                  = $a.Commands
        $this.Protocols                 = $a.Protocols
        $this.FileExtensions            = $a.FileExtensions
        $this.Dependencies              = $a.Dependencies
        $this.PackageFamilyName         = $a.PackageFamilyName
        $this.ProductCode               = $a.ProductCode
        $this.Capabilities              = $a.Capabilities
        $this.RestricedCapabilities     = $a.RestricedCapabilities
        $this.MSStoreProductIdentifier  = $a.MSStoreProductIdentifier
        $this.InstallerAbortsTerminal   = $a.InstallerAbortsTerminal
        $this.ReleaseDate               = $a.ReleaseDate
        $this.InstallLocationRequired   = $a.InstallLocationRequired
        $this.RequireExplicitUpgrade    = $a.RequireExplicitUpgrade
        $this.ElevationRequirement      = $a.ElevationRequirement
        $this.UnsupportedOSArchitectures= $a.UnsupportedOSArchitectures
        $this.AppsAndFeaturesEntries    = $a.AppsAndFeaturesEntries
        $this.Markets                   = $a.Markets
        $this.DownloadCommandProhibited = $a.DownloadCommandProhibited
        $this.RepairBehavior            = $a.RepairBehavior
        $this.ArchiveBinariesDependOnPath = $a.ArchiveBinariesDependOnPath
    }
    
    [WinGetInstaller[]] Add ([WinGetInstaller] $a)
    {
        $FirstValue  = [WinGetInstaller]::New($this)
        $SecondValue = [WinGetInstaller]::New($a)

        [WinGetInstaller[]]$Result = @([WinGetInstaller]::New($FirstValue), [WinGetInstaller]::New($SecondValue))

        Return $Result
    }

    [WinGetInstaller[]] Add ([String[]]$a)
    {
        $FirstValue  = [WinGetInstaller]::New($this)
        $SecondValue = [WinGetInstaller]::New($a)
        
        [WinGetInstaller[]] $Combined = @([WinGetInstaller]::New($FirstValue), [WinGetInstaller]::New($SecondValue))

        Return $Combined
    }
}

class WinGetVersion
{
    $PackageVersion
    $Channel
    [WinGetLocale]      $DefaultLocale
    [WinGetInstaller[]] $Installers
    [WinGetLocale[]]    $Locales

    WinGetVersion ([string] $a)
    {
        $Converted = $a | ConvertFrom-Json

        $this.PackageVersion = $Converted.PackageVersion
        $this.Channel        = $Converted.Channel
        $this.DefaultLocale  = $Converted.DefaultLocale
        $this.Installers     = $Converted.Installers
        $this.Locales        = $Converted.Locales
    }
    WinGetVersion ([WinGetVersion] $a)
    {
        $this.PackageVersion = $a.PackageVersion
        $this.Channel        = $a.Channel
        $this.DefaultLocale  = $a.DefaultLocale
        $this.Installers     = $a.Installers
        $this.Locales        = $a.Locales
    }
    WinGetVersion ([psobject] $a) {
        $this.PackageVersion = $a.PackageVersion
        $this.Channel        = $a.Channel
        $this.DefaultLocale  = $a.DefaultLocale
        $this.Installers     = $a.Installers
        $this.Locales        = $a.Locales
    }
    
    [WinGetVersion[]] Add ([WinGetVersion] $a)
    {
        $FirstValue  = [WinGetVersion]::New($this)
        $SecondValue = [WinGetVersion]::New($a)

        [WinGetVersion[]]$Result = @([WinGetVersion]::New($FirstValue), [WinGetVersion]::New($SecondValue))

        Return $Result
    }

    [WinGetVersion[]] Add ([String[]]$a)
    {
        $FirstValue  = [WinGetVersion]::New($this)
        $SecondValue = [WinGetVersion]::New($a)
        
        [WinGetVersion[]] $Combined = @([WinGetVersion]::New($FirstValue), [WinGetVersion]::New($SecondValue))

        Return $Combined
    }
}

class WinGetManifest
{
    [string] $PackageIdentifier
    [WinGetVersion[]] $Versions

    WinGetManifest ([string] $a)
    {
        Write-Verbose -Message "Creating a WinGetManifest object from String object"

        $Converted = $a | ConvertFrom-Json
        $this.PackageIdentifier = $Converted.PackageIdentifier
        $this.Versions          = $Converted.Versions
    }
    WinGetManifest ([WinGetManifest] $a)
    {
        Write-Verbose -Message "Creating a WinGetManifest object from WinGetManifest object"

        $this.PackageIdentifier = $a.PackageIdentifier
        $this.Versions          = $a.Versions
    }
    WinGetManifest ([psobject] $a) {
        Write-Verbose -Message "Creating a WinGetManifest object from PsObject object"
        
        $this.PackageIdentifier = $a.PackageIdentifier
        $this.Versions          = $a.Versions
    }
    
    [WinGetManifest[]] Add ([WinGetManifest] $a)
    {
        $FirstValue  = [WinGetManifest]::New($this)
        $SecondValue = [WinGetManifest]::New($a)

        Write-Verbose -Message "Combining a WinGetManifest with the WinGetManifest object"
        [WinGetManifest[]]$Result = @([WinGetManifest]::New($FirstValue), [WinGetManifest]::New($SecondValue))

        Return $Result
    }

    [WinGetManifest[]] Add ([String[]]$a)
    {
        $FirstValue  = [WinGetManifest]::New($this)
        $SecondValue = [WinGetManifest]::New($a)
        
        Write-Verbose -Message "Combining a String[] with the WinGetManifest object"
        [WinGetManifest[]] $Combined = @([WinGetManifest]::New($FirstValue), [WinGetManifest]::New($SecondValue))

        Return $Combined
    }

    [WinGetManifest[]] Add ([psobject]$a)
    {
        $FirstValue  = [WinGetManifest]::New($this)
        $SecondValue = [WinGetManifest]::New($a)
        
        Write-Verbose -Message "Combining a PsObject with the WinGetManifest object"
        [WinGetManifest[]] $Combined = @([WinGetManifest]::New($FirstValue), [WinGetManifest]::New($SecondValue))

        Return $Combined
    }

    [WinGetVersion[]] AddVersion ([WinGetVersion[]] $a)
    {
        $FirstValue  = [WinGetVersion[]]::New($this.Versions)
        $SecondValue = [WinGetVersion[]]::New($a)

        Write-Verbose -Message "Adding new Version to Manifest object"
        [WinGetVersion[]] $Combined = @([WinGetVersion[]]::New($FirstValue), [WinGetVersion[]]::New($SecondValue))
        #$this.Versions = $Combined

        Return $Combined
    }

    [string] GetJson ()
    {
        [string]$Return = $this | ConvertTo-Json -Depth 8

        ## Removes spacing from the JSON content
        $Return = $($Return -replace "`t|`n|`r|  ","")
        $Return = $($($($($Return -replace ": ",":") -replace " { |{ ", "{") -replace ', ', ',') -replace " } | }", "}")

        Return $Return
    }
}
