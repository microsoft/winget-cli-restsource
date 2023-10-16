class Locale
{
    [System.Collections.ArrayList]$Moniker  = @()
    [string]$PackageLocale              = ""
    [string]$Publisher                  = ""
    [string]$PublisherUrl               = ""
    [string]$PublisherSupportUrl        = ""
    [string]$PrivacyUrl                 = ""
    [string]$Author                     = ""
    [string]$PackageName                = ""
    [string]$PackageUrl                 = ""
    [string]$License                    = ""
    [string]$LicenseUrl                 = ""
    [string]$Copyright                  = ""
    [string]$CopyrightUrl               = ""
    [string]$ShortDescription           = ""
    [string]$Description                = ""
    [System.Collections.ArrayList]$Tags = @()

    Locale ()
    {}
    Locale ([Locale] $a)
    {
        $this = $a
    }
    Locale ($a)
    {
        $this.PackageLocale         = $a.PackageLocale
        $this.Publisher             = $a.Publisher
        $this.PublisherUrl          = $a.PublisherUrl
        $this.PublisherSupportUrl   = $a.PublisherSupportUrl
        $this.PrivacyUrl            = $a.PrivacyUrl
        $this.Author                = $a.Author
        $this.PackageName           = $a.PackageName
        $this.PackageUrl            = $a.PackageUrl
        $this.License               = $a.License
        $this.LicenseUrl            = $a.LicenseUrl
        $this.Copyright             = $a.Copyright
        $this.CopyrightUrl          = $a.CopyRightUrl
        $this.ShortDescription      = $a.ShortDescription
        $this.Description           = $a.Description
        
        foreach ($Tag in $a.Tags){
            $this.Tags.add([string]::new($Tag))
        }
        foreach ($Moniker in $a.Moniker){
            $this.Moniker.Add([string]::new($Moniker))
        }
    }
}

class PackageDependencies
{
    [string]$PackageIdentifier                          = ""
    [string]$MinimumVersion                             = ""
    [System.Collections.ArrayList]$ExternalDependencies = @()

    Locale ()
    {}
    Locale ([Locale] $a)
    {
        $this = $a
    }
    Locale ($a)
    {
        $this.PackageIdentifier    = $a.PackageIdentifier
        $this.MinimumVersion       = $a.MinimumVersion
        $this.ExternalDependencies = $a.ExternalDependencies
    }
}

class Dependencies
{
    [System.Collections.ArrayList]$WindowsFeatures      = @()
    [System.Collections.ArrayList]$WindowsLibraries     = @()
    [System.Collections.ArrayList]$PackageDependencies  = @()

    Dependencies ()
    {
        $this.WindowsFeatures.Add([string]::New(""))
        $this.WindowsLibraries.Add([string]::New(""))
        $this.PackageDependencies.Add([PackageDependencies]::New())
    }
    Dependencies ([Dependencies] $a)
    {
        $this = $a
    }
    Dependencies ($a)
    {
        $this.WindowsFeatures     = $a.WindowsFeatures
        $this.WindowsLibraries    = $a.WindowsLibraries
        $this.PackageDependencies = $a.PackageDependencies
    }
}

class Installer
{
    [string]$Architecture                                   = ""
    [string]$InstallerIdentifier                            = ""
    [string]$InstallerSha256                                = ""
    [string]$InstallerUrl                                   = ""
    [string]$InstallerLocale                                = ""
    [System.Collections.ArrayList]$Platform                 = @()
    [string]$MinimumOsVersion                               = ""
    [string]$InstallerType                                  = ""
    [string]$Scope                                          = ""
    [string]$SignatureSha256                                = ""
    [System.Collections.ArrayList]$InstallModes             = @()
    [System.Object]$InstallerSwitches                       = ""   #Silent, SilentWithProgress, Interactive, InstallLocation, Log, Upgrade, Custom
    [System.Collections.ArrayList]$InstallerSuccessCodes    = @()
    [string]$UpgradeBehavior                                = "install"
    [System.Collections.ArrayList]$Commands                 = @()
    [System.Collections.ArrayList]$Protocols                = @()
    [System.Collections.ArrayList]$FileExtensions           = @()
    [System.Collections.ArrayList]$Dependencies             = @()
    [string]$PackageFamilyName                              = ""
    [string]$ProductCode                                    = ""
    [System.Collections.ArrayList]$Capabilities             = @()
    [System.Collections.ArrayList]$RestrictedCapabilities   = @()
    [Boolean]$InstallerAbortsTerminal                       = $false
    $ReleaseDate                                            = "0001-01-01T00:00:00"
    [Boolean]$InstallLocationRequired                       = $false
    [Boolean]$RequireExplicitUpgrade                        = $false
    [Boolean]$DisplayInstallWarnings                        = $false
    [Boolean]$DownloadCommandProhibited                     = $false

    Installer ()
    {}
    Installer ([Installer] $a)
    {
        $this = $a
    }
    Installer ($a)
    {
        $this.Architecture              = $a.Architecture
        $this.InstallerIdentifier       = $a.InstallerIdentifier
        $this.InstallerSha256           = $a.InstallerSha256
        $this.InstallerUrl              = $a.InstallerUrl
        $this.InstallerLocale           = $a.InstallerLocale
        $this.MinimumOsVersion          = $a.MinimumOsVersion
        $this.InstallerType             = $a.InstallerType
        $this.Scope                     = $a.Scope
        $this.SignatureSha256           = $a.SignatureSha256
        $this.InstallModes              = $a.InstallModes
        $this.InstallerSwitches         = $a.InstallerSwitches
        $this.InstallerSuccessCodes     = $a.InstallerSuccessCodes
        $this.UpgradeBehavior           = $a.UpgradeBehavior
        $this.Commands                  = $a.Commands
        $this.Protocols                 = $a.Protocols
        $this.FileExtensions            = $a.FileExtensions
        $this.Dependencies              = $a.Dependencies
        $this.PackageFamilyName         = $a.PackageFamilyName
        $this.ProductCode               = $a.ProductCode
        $this.Capabilities              = $a.Capabilities
        $this.RestrictedCapabilities    = $a.RestrictedCapabilities

        foreach ($platform in $a.Platform){
            $this.Platform.Add([string]::new($Platform))
        }
    }
}

class Version
{
    [string] $PackageVersion
    [Locale] $DefaultLocale
    [System.Collections.ArrayList]$Locales              = @()
    [System.Collections.ArrayList]$Installers           = @()

    Version ()
    {}
    Version ([Version] $a)
    {
        $this = $a
    }
    Version ($a)
    {
        $this.PackageVersion = $a.PackageVersion
        #$this.DefaultLocale  = [Locale]::new($a.DefaultLocale)

        #foreach ($Locale in $a.Locales){
        #    $this.Locales.Add($Locale)
        #}
        #foreach ($Installer in $a.Installers){
        #    $this.Installers.Add([Installer]::New($Installer))
        #}
    }
    AddInstaller ($a)
    {
        foreach ($Installer in $a.Installers)
        {   
            $this.Installers.Add([Installer]::new($a))
            $i = $this.Installers.Count -1
            
            $this.Installers[$i].Scope                     = $Installer.Scope
            $this.Installers[$i].InstallerURL              = $Installer.InstallerURL
            $this.Installers[$i].InstallerSha256           = $Installer.InstallerSha256
            $this.Installers[$i].Architecture              = $Installer.Architecture
        }
    }
    AddLocale ($a)
    {
        $i = $this.Locales.Count -1
        if($this.Locales[$i] -ne $([Locale]::new())){
            $this.Locales.Add([Locale]::new())
        }

        $i = $this.Locales.Count -1
        $this.locales[$i] = [Locale]::new($a)
    }
    AddDefaultLocale ($a)
    {
        $this.DefaultLocale = [Locale]::new($a)
    }
}

class PackageManifest
{
    $PackageIdentifier                      = ""
    [System.Collections.ArrayList]$Versions = @()
    [System.Collections.ArrayList]$ExternalDependencies = @()
    

    PackageManifest ()
    {
    }
    PackageManifest ([PackageManifest] $a)
    {
        $this = $a
    }
    PackageManifest ($a)
    {
        $this.PackageIdentifier  = $a.PackageIdentifier
        $this.Versions           = $a.Version
    }
    AddVersion ($a)
    {
        $this.Versions.Add([Version]::new($a))
    }
    AddVersion ($a, $b, $c)
    {
        $this.Versions.Add([Version]::new($a))
        $i = $this.Versions.Count -1

        foreach ($installer in $b){
            $this.Versions[$i].AddInstaller($installer)
        }
        foreach ($locale in $c){
            $this.Versions.AddLocale($locale)

            if($locale.ManifestType -eq "defaultLocale"){
                $this.Versions.AddDefaultLocale($locale)
            }
        }
    }
    ConvertFromYAML ($a)
    {
        [System.Collections.ArrayList]$YAMLInstallers = @()
        [System.Collections.ArrayList]$YAMLLocales    = @()
        [System.Collections.ArrayList]$YAMLVersions   = @()

        foreach ($file in $a){
            $FileContents = Get-Content -Raw -Path $file.FullName

            if ($FileContents.Contains('$schema=https://aka.ms/winget-manifest.installer')){
                $ConvertedContents = ConvertFrom-Yaml -Yaml $FileContents
                $YAMLInstallers.Add(($ConvertedContents))
            }
            elseif ($FileContents.Contains('$schema=https://aka.ms/winget-manifest.defaultLocale')){
                $ConvertedContents = ConvertFrom-Yaml -Yaml $FileContents
                $YAMLLocales.Add(($ConvertedContents))
            }
            elseif ($FileContents.Contains('$schema=https://aka.ms/winget-manifest.version')){
                $ConvertedContents = ConvertFrom-Yaml -Yaml $FileContents
                $YAMLVersions.Add(($ConvertedContents))
            }
        }
        
        $this.PackageIdentifier = $YAMLVersions[0].PackageIdentifier
        $this.AddVersion($YAMLVersions, $YAMLInstallers, $YAMLLocales)
    }
}