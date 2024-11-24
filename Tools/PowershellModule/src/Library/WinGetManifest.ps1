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
    #$Agreements
    $Tags

    WinGetLocale () {}
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
    #$Markets
    $DownloadCommandProhibited
    $RepairBehavior
    $ArchiveBinariesDependOnPath

    WinGetInstaller () {}
}

class WinGetVersion
{
    $PackageVersion
    $Channel
    [WinGetLocale]      $DefaultLocale
    [WinGetInstaller[]] $Installers
    [WinGetLocale[]]    $Locales

    WinGetVersion () {}
}

class WinGetManifest
{
    [string] $PackageIdentifier
    [WinGetVersion[]] $Versions
    
    WinGetManifest () {}
    
    [string] GetJson ()
    {
        return [WinGetManifest]::SerializeJson($this)
    }
    
    static [WinGetManifest] CreateFromString([string] $a)
    {
        Write-Verbose -Message "Creating a WinGetManifest object from String object"
        
        $options = [System.Text.Json.JsonSerializerOptions]::new()

        return [System.Text.Json.JsonSerializer]::Deserialize($a, [WinGetManifest], $options)
    }
    
    static [WinGetManifest] CreateFromObject([psobject] $a)
    {
        Write-Verbose -Message "Creating a WinGetManifest object from PsObject object"
        
        $json = [WinGetManifest]::SerializeJson($a)

        return [WinGetManifest]::CreateFromString($json)
    }
    
    static [string] SerializeJson ([psobject] $toSerialize)
    {
        $options = [System.Text.Json.JsonSerializerOptions]::new()
        $options.WriteIndented = $false
        $options.DefaultIgnoreCondition = [System.Text.Json.Serialization.JsonIgnoreCondition]::WhenWritingNull
        $options.Converters.Add([System.Text.Json.Serialization.JsonStringEnumConverter]::new())
        $options.Encoder = [System.Text.Encodings.Web.JavaScriptEncoder]::UnsafeRelaxedJsonEscaping

        Return [System.Text.Json.JsonSerializer]::Serialize($toSerialize, $options)
    }
}
