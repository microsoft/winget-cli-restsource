// -----------------------------------------------------------------------
// <copyright file="PackageManifestUtils.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils
{
    using System;
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGetUtil.Models.V1;

    /// <summary>
    /// Wrapper class around PackageManifest object.
    /// Supports converting yaml manifest to json object.
    /// </summary>
    public static class PackageManifestUtils
    {
        /// <summary>
        /// Merges a merged manifest object into an existing json representation of the app.
        /// </summary>
        /// <param name="manifest">Merged manifest object.</param>
        /// <param name="priorPackageManifest">Optional PackageManifest.</param>
        /// <returns>A <see cref="PackageManifest"/> representing the rest source json representation of the package.</returns>
        public static PackageManifest AddManifestToPackageManifest(
            Manifest manifest,
            PackageManifest priorPackageManifest = null)
        {
            PackageManifest packageManifest = null;

            if (priorPackageManifest == null)
            {
                Package package = new Package();
                package.PackageIdentifier = manifest.Id;
                packageManifest = new PackageManifest(package);
            }
            else
            {
                packageManifest = priorPackageManifest;
            }

            // Create a new VersionExtended object and populate it with manifest info
            // Fields are strings unless otherwise noted.
            // PackageVersion
            // Channel
            // Default locale
            //  Moniker
            //  PackageLocale
            //  Publisher
            //  PublisherUrl
            //  PublisherSupportUrl
            //  PrivacyUrl
            //  Author
            //  PackageName
            //  PackageUrl
            //  License
            //  LicenseUrl
            //  Copyright
            //  CopyrightUrl
            //  ShortDescription
            //  Description
            //  Tags
            // Locales
            //  Array of Locale where each contains:
            //      PackageLocale
            //      Publisher
            //      PublisherUrl
            //      PublisherSupportUrl
            //      PrivacyUrl
            //      Author
            //      PackageName
            //      PackageUrl
            //      License
            //      LicenseUrl
            //      Copyright
            //      CopyrightUrl
            //      ShortDescription
            //      Description
            //      Tags
            // Installers
            //  Array of Installer where each contains:
            //      InstallerIdentifier
            //      InstallerSha256
            //      InstallerUrl
            //      InstallerLocale
            //      Platform
            //          Array of strings
            //      MinimumOsVersion
            //      InstallerType
            //      Scope
            //      SignatureSha256
            //      InstallModes
            //          Array of strings
            //      InstallerSwitches
            //           Silent
            //           SilentWithProgress
            //           Interactive
            //           InstallLocation
            //           Log
            //           Upgrade
            //           Custom
            //           Repair
            //      InstallerSuccessCodes
            //          Array of ints
            //      UpgradeBehavior
            //      Commands
            //          Array of strings
            //      Protocols
            //          Array of strings
            //      FileExtensions
            //          Array of strings
            //      Dependencies
            //          WindowsFeatures
            //              Array of strings
            //          WindowsLibraries
            //              Array of strings
            //          PackageDependencies
            //              Array of package dependencies where each contains:
            //                  PackageIdentifier
            //                  MinimumVersion
            //          ExternalDependencies
            //              Array of strings
            //      PackageFamilyName
            //      ProductCode
            //      Capabilities
            //          Array of strings
            //      RestrictedCapabilities
            //          Array of strings
            //      DownloadCommandProhibited
            //          bool
            //      RepairBehavior
            //      ArchiveBinariesDependOnPath
            VersionExtended versionExtended = new VersionExtended();
            versionExtended.PackageVersion = manifest.Version;
            versionExtended.Channel = manifest.Channel;

            versionExtended.DefaultLocale = new DefaultLocale();
            versionExtended.DefaultLocale.Moniker = string.IsNullOrWhiteSpace(manifest.Moniker) ? null : manifest.Moniker;
            versionExtended.DefaultLocale.PackageLocale = manifest.PackageLocale;
            versionExtended.DefaultLocale.Publisher = manifest.Publisher;
            versionExtended.DefaultLocale.PublisherUrl = manifest.PublisherUrl;
            versionExtended.DefaultLocale.PublisherSupportUrl = manifest.PublisherSupportUrl;
            versionExtended.DefaultLocale.PrivacyUrl = manifest.PrivacyUrl;
            versionExtended.DefaultLocale.Author = manifest.Author;
            versionExtended.DefaultLocale.PackageName = manifest.PackageName;
            versionExtended.DefaultLocale.PackageUrl = manifest.PackageUrl;
            versionExtended.DefaultLocale.License = manifest.License;
            versionExtended.DefaultLocale.LicenseUrl = manifest.LicenseUrl;
            versionExtended.DefaultLocale.Copyright = manifest.Copyright;
            versionExtended.DefaultLocale.CopyrightUrl = manifest.CopyrightUrl;
            versionExtended.DefaultLocale.ShortDescription = manifest.ShortDescription;
            versionExtended.DefaultLocale.Description = manifest.Description;

            versionExtended.DefaultLocale.Tags = manifest.Tags.ToApiArray<Tags>();

            if (manifest.Localization != null)
            {
                foreach (var localization in manifest.Localization)
                {
                    Locale newLocale = new Locale();
                    newLocale.PackageLocale = localization.PackageLocale;
                    newLocale.Publisher = localization.Publisher;
                    newLocale.PublisherUrl = localization.PublisherUrl;
                    newLocale.PublisherSupportUrl = localization.PublisherSupportUrl;
                    newLocale.PrivacyUrl = localization.PrivacyUrl;
                    newLocale.Author = localization.Author;
                    newLocale.PackageName = localization.PackageName;
                    newLocale.PackageUrl = localization.PackageUrl;
                    newLocale.License = localization.License;
                    newLocale.LicenseUrl = localization.LicenseUrl;
                    newLocale.Copyright = localization.Copyright;
                    newLocale.CopyrightUrl = localization.CopyrightUrl;
                    newLocale.ShortDescription = localization.ShortDescription;
                    newLocale.Description = localization.Description;

                    newLocale.Tags = localization.Tags.ToApiArray<Tags>();

                    versionExtended.AddLocale(newLocale);
                }
            }

            if (manifest.Installers != null)
            {
                foreach (var installer in manifest.Installers)
                {
                    Installer newInstaller = new Installer();

                    // Properties only present on installer node
                    newInstaller.InstallerUrl = installer.Url;
                    newInstaller.Architecture = installer.Arch;
                    newInstaller.InstallerSha256 = installer.Sha256;
                    newInstaller.SignatureSha256 = installer.SignatureSha256;

                    // Properties present on root node which can be overridden by installer node
                    newInstaller.InstallerLocale = installer.InstallerLocale ?? manifest.InstallerLocale;
                    newInstaller.Platform = installer.Platform.ToApiArray<Platform>() ?? manifest.Platform.ToApiArray<Platform>();
                    newInstaller.MinimumOsVersion = installer.MinimumOSVersion ?? manifest.MinimumOSVersion;
                    newInstaller.InstallerType = installer.InstallerType ?? manifest.InstallerType;
                    newInstaller.Scope = installer.Scope ?? manifest.Scope;
                    newInstaller.InstallModes = installer.InstallModes.ToApiArray<InstallModes>() ?? manifest.InstallModes.ToApiArray<InstallModes>();
                    newInstaller.InstallerSwitches = AddInstallerSwitches(installer.Switches) ?? AddInstallerSwitches(manifest.Switches);
                    newInstaller.InstallerSuccessCodes = installer.InstallerSuccessCodes.ToApiArray<InstallerSuccessCodes>() ?? manifest.InstallerSuccessCodes.ToApiArray<InstallerSuccessCodes>();
                    newInstaller.UpgradeBehavior = installer.UpgradeBehavior ?? manifest.UpgradeBehavior;
                    newInstaller.Commands = installer.Commands.ToApiArray<Commands>() ?? manifest.Commands.ToApiArray<Commands>();
                    newInstaller.Protocols = installer.Protocols.ToApiArray<Protocols>() ?? manifest.Protocols.ToApiArray<Protocols>();
                    newInstaller.FileExtensions = installer.FileExtensions.ToApiArray<FileExtensions>() ?? manifest.FileExtensions.ToApiArray<FileExtensions>();
                    newInstaller.Dependencies = AddDependencies(installer.Dependencies) ?? AddDependencies(manifest.Dependencies);
                    newInstaller.PackageFamilyName = installer.PackageFamilyName ?? manifest.PackageFamilyName;
                    newInstaller.ProductCode = installer.ProductCode ?? manifest.ProductCode;
                    newInstaller.Capabilities = installer.Capabilities.ToApiArray<Capabilities>() ?? manifest.Capabilities.ToApiArray<Capabilities>();
                    newInstaller.RestrictedCapabilities = installer.RestrictedCapabilities.ToApiArray<RestrictedCapabilities>() ?? manifest.RestrictedCapabilities.ToApiArray<RestrictedCapabilities>();
                    newInstaller.DownloadCommandProhibited = installer.DownloadCommandProhibited;
                    newInstaller.RepairBehavior = installer.RepairBehavior ?? manifest.RepairBehavior;
                    newInstaller.ArchiveBinariesDependOnPath = installer.ArchiveBinariesDependOnPath;

                    newInstaller.InstallerIdentifier = string.Join("_", newInstaller.Architecture, newInstaller.InstallerLocale, newInstaller.Scope, Guid.NewGuid());
                    versionExtended.AddInstaller(newInstaller);
                }
            }

            // Are we updating an existing version metadata or writing a new version?
            if (packageManifest.Versions != null && packageManifest.Versions.Exists(version => version.PackageVersion == manifest.Version))
            {
                // Update
                packageManifest.UpdateVersion(versionExtended);
            }
            else
            {
                // Add
                packageManifest.AddVersion(versionExtended);
            }

            return packageManifest;
        }

        /// <summary>
        /// Process Installer Switches subnode.
        /// </summary>
        private static Utils.Models.Objects.InstallerSwitches AddInstallerSwitches(InstallerSwitches sourceSwitches)
        {
            if (sourceSwitches != null)
            {
                return new Utils.Models.Objects.InstallerSwitches
                {
                    Silent = string.IsNullOrWhiteSpace(sourceSwitches.Silent) ? null : sourceSwitches.Silent,
                    SilentWithProgress = string.IsNullOrWhiteSpace(sourceSwitches.SilentWithProgress) ? null : sourceSwitches.SilentWithProgress,
                    Interactive = string.IsNullOrWhiteSpace(sourceSwitches.Interactive) ? null : sourceSwitches.Interactive,
                    InstallLocation = string.IsNullOrWhiteSpace(sourceSwitches.InstallLocation) ? null : sourceSwitches.InstallLocation,
                    Log = string.IsNullOrWhiteSpace(sourceSwitches.Log) ? null : sourceSwitches.Log,
                    Upgrade = string.IsNullOrWhiteSpace(sourceSwitches.Upgrade) ? null : sourceSwitches.Upgrade,
                    Custom = string.IsNullOrWhiteSpace(sourceSwitches.Custom) ? null : sourceSwitches.Custom,
                    Repair = string.IsNullOrWhiteSpace(sourceSwitches.Repair) ? null : sourceSwitches.Repair,
                };
            }

            return null;
        }

        /// <summary>
        /// Process dependencies subnode.
        /// </summary>
        private static Utils.Models.Objects.Dependencies AddDependencies(InstallerDependency sourceDependencies)
        {
            if (sourceDependencies != null)
            {
                var newDependencies = new Utils.Models.Objects.Dependencies();

                newDependencies.WindowsFeatures = sourceDependencies.WindowsFeatures.ToApiArray<Dependencies>();
                newDependencies.WindowsLibraries = sourceDependencies.WindowsLibraries.ToApiArray<Dependencies>();

                if (sourceDependencies.PackageDependencies != null)
                {
                    newDependencies.PackageDependencies = new PackageDependency();
                    foreach (var installerPackageDependency in sourceDependencies.PackageDependencies)
                    {
                        Utils.Models.Objects.PackageDependency packageDependency = new Utils.Models.Objects.PackageDependency
                        {
                            PackageIdentifier = installerPackageDependency.PackageIdentifier,
                            MinimumVersion = installerPackageDependency.MinimumVersion,
                        };
                        newDependencies.PackageDependencies.Add(packageDependency);
                    }
                }

                newDependencies.ExternalDependencies = sourceDependencies.ExternalDependencies.ToApiArray<Dependencies>();

                return newDependencies;
            }

            return null;
        }

        private static T ToApiArray<T>(this IEnumerable<string> from)
            where T : ApiArray<string>, new()
        {
            T to = null;
            if (from != null)
            {
                to = new T();
                to.AddRange(from);
            }

            return to;
        }

        private static T ToApiArray<T>(this IEnumerable<long> from)
            where T : ApiArray<long>, new()
        {
            T to = null;
            if (from != null)
            {
                to = new T();
                to.AddRange(from);
            }

            return to;
        }
    }
}
