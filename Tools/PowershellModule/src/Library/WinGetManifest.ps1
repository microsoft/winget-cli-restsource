# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

class WinGetAgreement
{
    $AgreementLabel
    $Agreement
    $AgreementUrl

    WinGetAgreement () {}
}

class WinGetDocumentation
{
    $DocumentLabel
    $DocumentUrl

    WinGetDocumentation () {}
}

class WinGetIcon
{
    $IconUrl
    $IconFileType
    $IconResolution
    $IconTheme
    $IconSha256

    WinGetIcon () {}
}

class WinGetLocale
{
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
    [string[]]$Tags
    $ReleaseNotes
    $ReleaseNotesUrl
    [WinGetAgreement[]]$Agreements
    $PurchaseUrl
    $InstallationNotes
    [WinGetDocument[]]$Documentations
    [WinGetIcon[]]$Icons

    ## Default locale only
    $Moniker

    WinGetLocale () {}
}

class WinGetInstallerSwitch
{
    $Silent
    $SilentWithProgress
    $Interactive
    $InstallLocation
    $Log
    $Upgrade
    $Custom
    $Repair

    WinGetInstallerSwitch () {}
}

class WinGetExpectedReturnCode
{
    [long]$InstallerReturnCode
    $ReturnResponse
    $ReturnResponseUrl

    WinGetExpectedReturnCode () {}
}

class WinGetPackageDependency
{
    $PackageIdentifier
    $MinimumVersion

    WinGetPackageDependency () {}
}

class WinGetDependencies
{
    [string[]]$WindowsFeatures
    [string[]]$WindowsLibraries
    [WinGetPackageDependency[]]$PackageDependencies
    [string[]]$ExternalDependencies

    WinGetDependencies () {}
}

class WinGetAppsAndFeaturesEntry
{
    $DisplayName
    $Publisher
    $DisplayVersion
    $ProductCode
    $UpgradeCode
    $InstallerType

    WinGetAppsAndFeaturesEntry () {}
}

class WinGetMarkets
{
    [string[]]$AllowedMarkets
    [string[]]$ExcludedMarkets

    WinGetMarkets () {}
}

class WinGetNestedInstallerFile
{
    $RelativeFilePath
    $PortableCommandAlias

    WinGetNestedInstallerFile () {}
}

class WinGetInstallationMetadataFile
{
    $RelativeFilePath
    $FileSha256
    $FileType
    $InvocationParameter
    $DisplayName

    WinGetInstallationMetadataFile () {}
}

class WinGetInstallationMetadata
{
    $DefaultInstallLocation
    [WinGetInstallationMetadataFile[]]$Files
    
    WinGetInstallationMetadata () {}
}

class WinGetInstaller
{
    $InstallerIdentifier
    $InstallerSha256
    $InstallerUrl
    $Architecture
    $InstallerLocale
    [string[]]$Platform
    $MinimumOsVersion
    $InstallerType
    $Scope
    $SignatureSha256
    [string[]]$InstallModes
    [WinGetInstallerSwitch]$InstallerSwitches
    [long[]]$InstallerSuccessCodes
    [WinGetExpectedReturnCode[]]$ExpectedReturnCodes
    $UpgradeBehavior
    [string[]]$Commands
    [string[]]$Protocols
    [string[]]$FileExtensions
    [WinGetDependencies]$Dependencies
    $PackageFamilyName
    $ProductCode
    [string[]]$Capabilities
    [string[]]$RestricedCapabilities
    $MSStoreProductIdentifier
    [bool]$InstallerAbortsTerminal
    $ReleaseDate
    [bool]$InstallLocationRequired
    [bool]$RequireExplicitUpgrade
    $ElevationRequirement
    [string[]]$UnsupportedOSArchitectures
    [WinGetAppsAndFeaturesEntry[]]$AppsAndFeaturesEntries
    [WinGetMarkets]$Markets
    $NestedInstallerType
    [WinGetNestedInstallerFile[]]$NestedInstallerFiles
    [bool]$DisplayInstallWarnings
    [string[]]$UnsupportedArguments
    [WinGetInstallationMetadata]$InstallationMetadata
    [bool]$DownloadCommandProhibited
    $RepairBehavior
    [bool]$ArchiveBinariesDependOnPath

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
        ## Not using ConvertTo-Json here since we want more control on null property handling
        $options = [System.Text.Json.JsonSerializerOptions]::new()
        $options.WriteIndented = $false
        $options.MaxDepth = 16
        $options.DefaultIgnoreCondition = [System.Text.Json.Serialization.JsonIgnoreCondition]::WhenWritingNull
        $options.Converters.Add([System.Text.Json.Serialization.JsonStringEnumConverter]::new())
        $options.Encoder = [System.Text.Encodings.Web.JavaScriptEncoder]::UnsafeRelaxedJsonEscaping
        
        return [System.Text.Json.JsonSerializer]::Serialize($this, $options)
    }
    
    static [WinGetManifest] CreateFromString ([string] $a)
    {
        Write-Verbose -Message "Creating a WinGetManifest object from String object"
        
        $options = [System.Text.Json.JsonSerializerOptions]::new()

        return [System.Text.Json.JsonSerializer]::Deserialize($a, [WinGetManifest], $options)
    }
    
    static [WinGetManifest] CreateFromObject ([psobject] $a)
    {
        Write-Verbose -Message "Creating a WinGetManifest object from PsObject object"
        
        $json = ConvertTo-Json $a -Depth 16 -Compress

        return [WinGetManifest]::CreateFromString($json)
    }
}
