# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

class WinGetAgreement
{
    [string]$AgreementLabel
    [string]$Agreement
    [string]$AgreementUrl

    WinGetAgreement () {}
}

class WinGetDocumentation
{
    [string]$DocumentLabel
    [string]$DocumentUrl

    WinGetDocumentation () {}
}

class WinGetIcon
{
    [string]$IconUrl
    [string]$IconFileType
    [string]$IconResolution
    [string]$IconTheme
    [string]$IconSha256

    WinGetIcon () {}
}

class WinGetLocale
{
    [string]$PackageLocale
    [string]$Publisher
    [string]$PublisherUrl
    [string]$PublisherSupportUrl
    [string]$PrivacyUrl
    [string]$Author
    [string]$PackageName
    [string]$PackageUrl
    [string]$License
    [string]$LicenseUrl
    [string]$Copyright
    [string]$CopyRightUrl
    [string]$ShortDescription
    [string]$Description
    [string[]]$Tags
    [string]$ReleaseNotes
    [string]$ReleaseNotesUrl
    [WinGetAgreement[]]$Agreements
    [string]$PurchaseUrl
    [string]$InstallationNotes
    [WinGetDocumentation[]]$Documentations
    [WinGetIcon[]]$Icons

    ## Default locale only
    [string]$Moniker

    WinGetLocale () {}
}

class WinGetInstallerSwitch
{
    [string]$Silent
    [string]$SilentWithProgress
    [string]$Interactive
    [string]$InstallLocation
    [string]$Log
    [string]$Upgrade
    [string]$Custom
    [string]$Repair

    WinGetInstallerSwitch () {}
}

class WinGetExpectedReturnCode
{
    [long]$InstallerReturnCode
    [string]$ReturnResponse
    [string]$ReturnResponseUrl

    WinGetExpectedReturnCode () {}
}

class WinGetPackageDependency
{
    [string]$PackageIdentifier
    [string]$MinimumVersion

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
    [string]$DisplayName
    [string]$Publisher
    [string]$DisplayVersion
    [string]$ProductCode
    [string]$UpgradeCode
    [string]$InstallerType

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
    [string]$RelativeFilePath
    [string]$PortableCommandAlias

    WinGetNestedInstallerFile () {}
}

class WinGetInstallationMetadataFile
{
    [string]$RelativeFilePath
    [string]$FileSha256
    [string]$FileType
    [string]$InvocationParameter
    [string]$DisplayName

    WinGetInstallationMetadataFile () {}
}

class WinGetInstallationMetadata
{
    [string]$DefaultInstallLocation
    [WinGetInstallationMetadataFile[]]$Files
    
    WinGetInstallationMetadata () {}
}

class WinGetInstaller
{
    [string]$InstallerIdentifier
    [string]$InstallerSha256
    [string]$InstallerUrl
    [string]$Architecture
    [string]$InstallerLocale
    [string[]]$Platform
    [string]$MinimumOsVersion
    [string]$InstallerType
    [string]$Scope
    [string]$SignatureSha256
    [string[]]$InstallModes
    [WinGetInstallerSwitch]$InstallerSwitches
    [long[]]$InstallerSuccessCodes
    [WinGetExpectedReturnCode[]]$ExpectedReturnCodes
    [string]$UpgradeBehavior
    [string[]]$Commands
    [string[]]$Protocols
    [string[]]$FileExtensions
    [WinGetDependencies]$Dependencies
    [string]$PackageFamilyName
    [string]$ProductCode
    [string[]]$Capabilities
    [string[]]$RestricedCapabilities
    [string]$MSStoreProductIdentifier
    [bool]$InstallerAbortsTerminal
    [string]$ReleaseDate
    [bool]$InstallLocationRequired
    [bool]$RequireExplicitUpgrade
    [string]$ElevationRequirement
    [string[]]$UnsupportedOSArchitectures
    [WinGetAppsAndFeaturesEntry[]]$AppsAndFeaturesEntries
    [WinGetMarkets]$Markets
    [string]$NestedInstallerType
    [WinGetNestedInstallerFile[]]$NestedInstallerFiles
    [bool]$DisplayInstallWarnings
    [string[]]$UnsupportedArguments
    [WinGetInstallationMetadata]$InstallationMetadata
    [bool]$DownloadCommandProhibited
    [string]$RepairBehavior
    [bool]$ArchiveBinariesDependOnPath

    WinGetInstaller () {}
}

class WinGetVersion
{
    [string]$PackageVersion
    [string]$Channel
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
