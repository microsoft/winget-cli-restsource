// -----------------------------------------------------------------------
// <copyright file="PackageManifestUtils.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.PowershellSupport.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.WinGet.RestSource.PowershellSupport.Helpers;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGetUtil.Models.V1;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Wrapper class around PackageManifest object.
    /// Supports converting yaml manifest to json object.
    /// </summary>
    public sealed class PackageManifestUtils
    {
        /// <summary>
        /// Merges a merged manifest object into an existing json representation of the app.
        /// </summary>
        /// <param name="manifest">Merged manifest object.</param>
        /// <param name="priorManifest">Prior json data to merge with.</param>
        /// <returns>A <see cref="PackageManifest"/> representing the rest source json representation of the package.</returns>
        public static PackageManifest AddManifestToPackageManifest(
            Manifest manifest,
            string priorManifest)
        {
            PackageManifest packageManifest = null;

            if (!string.IsNullOrWhiteSpace(priorManifest))
            {
                var data = JObject.Parse(priorManifest);
                string priorManifestData = data["Data"].ToString();

                packageManifest = Parser.StringParser<PackageManifest>(priorManifestData);
            }
            else
            {
                Package package = new Package();
                package.PackageIdentifier = manifest.Id;
                packageManifest = new PackageManifest(package);
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

            versionExtended.DefaultLocale.Tags = PackageManifestUtils.AddToList<Tags>(manifest.Tags);

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

                    newLocale.Tags = PackageManifestUtils.AddToList<Tags>(localization.Tags);

                    versionExtended.AddLocale(newLocale);
                }
            }

            if (manifest.Installers != null)
            {
                foreach (var installer in manifest.Installers)
                {
                    Installer newInstaller = new Installer();
                    newInstaller.InstallerIdentifier = string.Join("_", installer.Arch, installer.InstallerLocale, installer.Scope, Guid.NewGuid());
                    newInstaller.InstallerSha256 = installer.Sha256;
                    newInstaller.InstallerUrl = installer.Url;
                    newInstaller.InstallerLocale = installer.InstallerLocale;
                    newInstaller.Architecture = installer.Arch;

                    newInstaller.Platform = PackageManifestUtils.AddToList<Platform>(installer.Platform);

                    newInstaller.MinimumOsVersion = installer.MinimumOSVersion;
                    newInstaller.InstallerType = installer.InstallerType;
                    newInstaller.Scope = installer.Scope;
                    newInstaller.SignatureSha256 = installer.SignatureSha256;

                    newInstaller.InstallModes = PackageManifestUtils.AddToList<InstallModes>(installer.InstallModes);

                    // Process Installer Switches subnode.
                    if (installer.Switches != null)
                    {
                        newInstaller.InstallerSwitches = new Utils.Models.Objects.InstallerSwitches
                        {
                            Silent = string.IsNullOrWhiteSpace(installer.Switches.Silent) ? null : installer.Switches.Silent,
                            SilentWithProgress = string.IsNullOrWhiteSpace(installer.Switches.SilentWithProgress) ? null : installer.Switches.SilentWithProgress,
                            Interactive = string.IsNullOrWhiteSpace(installer.Switches.Interactive) ? null : installer.Switches.Interactive,
                            InstallLocation = string.IsNullOrWhiteSpace(installer.Switches.InstallLocation) ? null : installer.Switches.InstallLocation,
                            Log = string.IsNullOrWhiteSpace(installer.Switches.Log) ? null : installer.Switches.Log,
                            Upgrade = string.IsNullOrWhiteSpace(installer.Switches.Upgrade) ? null : installer.Switches.Upgrade,
                            Custom = string.IsNullOrWhiteSpace(installer.Switches.Custom) ? null : installer.Switches.Custom,
                        };
                    }

                    if (installer.InstallerSuccessCodes != null)
                    {
                        newInstaller.InstallerSuccessCodes = PackageManifestUtils.AddToList<InstallerSuccessCodes>(installer.InstallerSuccessCodes.ConvertAll<long>(i => (long)i));
                    }

                    newInstaller.UpgradeBehavior = installer.UpgradeBehavior;

                    newInstaller.Commands = PackageManifestUtils.AddToList<Commands>(installer.Commands);
                    newInstaller.Protocols = PackageManifestUtils.AddToList<Protocols>(installer.Protocols);
                    newInstaller.FileExtensions = PackageManifestUtils.AddToList<FileExtensions>(installer.FileExtensions);

                    // Process dependencies subnode.
                    if (installer.Dependencies != null)
                    {
                        newInstaller.Dependencies = new Utils.Models.Objects.Dependencies();

                        newInstaller.Dependencies.WindowsFeatures = PackageManifestUtils.AddToList<Dependencies>(installer.Dependencies.WindowsFeatures);
                        newInstaller.Dependencies.WindowsLibraries = PackageManifestUtils.AddToList<Dependencies>(installer.Dependencies.WindowsLibraries);

                        if (installer.Dependencies.PackageDependencies != null)
                        {
                            newInstaller.Dependencies.PackageDependencies = new PackageDependency();
                            foreach (var installerPackageDependency in installer.Dependencies.PackageDependencies)
                            {
                                Utils.Models.Objects.PackageDependency packageDependency = new Utils.Models.Objects.PackageDependency
                                {
                                    PackageIdentifier = installerPackageDependency.PackageIdentifier,
                                    MinimumVersion = installerPackageDependency.MinimumVersion,
                                };
                                newInstaller.Dependencies.PackageDependencies.Add(packageDependency);
                            }
                        }

                        newInstaller.Dependencies.ExternalDependencies = PackageManifestUtils.AddToList<Dependencies>(installer.Dependencies.ExternalDependencies);
                    }

                    newInstaller.PackageFamilyName = installer.PackageFamilyName;
                    newInstaller.ProductCode = installer.ProductCode;
                    newInstaller.Capabilities = PackageManifestUtils.AddToList<Capabilities>(installer.Capabilities);
                    newInstaller.RestrictedCapabilities = PackageManifestUtils.AddToList<RestrictedCapabilities>(installer.RestrictedCapabilities);

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

        private static T AddToList<T>(IEnumerable<string> from)
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

        private static T AddToList<T>(IEnumerable<long> from)
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
