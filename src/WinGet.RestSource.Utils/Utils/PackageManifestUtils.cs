// -----------------------------------------------------------------------
// <copyright file="PackageManifestUtils.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.AspNetCore.Http;
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
            VersionExtended versionExtended = new VersionExtended();
            versionExtended.PackageVersion = manifest.Version;
            versionExtended.Channel = manifest.Channel;

            versionExtended.DefaultLocale = new DefaultLocale();
            versionExtended.DefaultLocale.Moniker = manifest.Moniker;
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
            versionExtended.DefaultLocale.ReleaseNotes = manifest.ReleaseNotes;
            versionExtended.DefaultLocale.ReleaseNotesUrl = manifest.ReleaseNotesUrl;
            versionExtended.DefaultLocale.Agreements = AddAgreements(manifest.Agreements);
            versionExtended.DefaultLocale.PurchaseUrl = manifest.PurchaseUrl;
            versionExtended.DefaultLocale.InstallationNotes = manifest.InstallationNotes;
            versionExtended.DefaultLocale.Documentations = AddDocumentations(manifest.Documentations);
            versionExtended.DefaultLocale.Icons = AddIcons(manifest.Icons);

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
                    newLocale.ReleaseNotes = localization.ReleaseNotes;
                    newLocale.ReleaseNotesUrl = localization.ReleaseNotesUrl;
                    newLocale.Agreements = AddAgreements(localization.Agreements);
                    newLocale.PurchaseUrl = localization.PurchaseUrl;
                    newLocale.InstallationNotes = localization.InstallationNotes;
                    newLocale.Documentations = AddDocumentations(localization.Documentations);
                    newLocale.Icons = AddIcons(localization.Icons);

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
                    newInstaller.MSStoreProductIdentifier = installer.ProductId;

                    // Properties present on root node which can be overridden by installer node
                    newInstaller.InstallerLocale = installer.InstallerLocale ?? manifest.InstallerLocale;
                    newInstaller.Platform = installer.Platform.ToApiArray<Platform>() ?? manifest.Platform.ToApiArray<Platform>();
                    newInstaller.MinimumOsVersion = installer.MinimumOSVersion ?? manifest.MinimumOSVersion;
                    newInstaller.InstallerType = installer.InstallerType ?? manifest.InstallerType;
                    newInstaller.Scope = installer.Scope ?? manifest.Scope;
                    newInstaller.InstallModes = installer.InstallModes.ToApiArray<InstallModes>() ?? manifest.InstallModes.ToApiArray<InstallModes>();
                    newInstaller.InstallerSwitches = AddInstallerSwitches(installer.Switches) ?? AddInstallerSwitches(manifest.Switches);
                    newInstaller.InstallerSuccessCodes = installer.InstallerSuccessCodes.ToApiArray<InstallerSuccessCodes>() ?? manifest.InstallerSuccessCodes.ToApiArray<InstallerSuccessCodes>();
                    newInstaller.ExpectedReturnCodes = AddExpectedReturnCodes(installer.ExpectedReturnCodes) ?? AddExpectedReturnCodes(manifest.ExpectedReturnCodes);
                    newInstaller.UpgradeBehavior = installer.UpgradeBehavior ?? manifest.UpgradeBehavior;
                    newInstaller.Commands = installer.Commands.ToApiArray<Commands>() ?? manifest.Commands.ToApiArray<Commands>();
                    newInstaller.Protocols = installer.Protocols.ToApiArray<Protocols>() ?? manifest.Protocols.ToApiArray<Protocols>();
                    newInstaller.FileExtensions = installer.FileExtensions.ToApiArray<FileExtensions>() ?? manifest.FileExtensions.ToApiArray<FileExtensions>();
                    newInstaller.Dependencies = AddDependencies(installer.Dependencies) ?? AddDependencies(manifest.Dependencies);
                    newInstaller.PackageFamilyName = installer.PackageFamilyName ?? manifest.PackageFamilyName;
                    newInstaller.ProductCode = installer.ProductCode ?? manifest.ProductCode;
                    newInstaller.Capabilities = installer.Capabilities.ToApiArray<Capabilities>() ?? manifest.Capabilities.ToApiArray<Capabilities>();
                    newInstaller.RestrictedCapabilities = installer.RestrictedCapabilities.ToApiArray<RestrictedCapabilities>() ?? manifest.RestrictedCapabilities.ToApiArray<RestrictedCapabilities>();
                    newInstaller.InstallerAbortsTerminal = installer.InstallerAbortsTerminal ?? manifest.InstallerAbortsTerminal;
                    newInstaller.ReleaseDate = TryParseDateTime(installer.ReleaseDate) ?? TryParseDateTime(manifest.ReleaseDate);
                    newInstaller.InstallLocationRequired = installer.InstallLocationRequired ?? manifest.InstallLocationRequired;
                    newInstaller.RequireExplicitUpgrade = installer.RequireExplicitUpgrade ?? manifest.RequireExplicitUpgrade;
                    newInstaller.ElevationRequirement = installer.ElevationRequirement ?? manifest.ElevationRequirement;
                    newInstaller.UnsupportedOSArchitectures = installer.UnsupportedOSArchitectures.ToApiArray<UnsupportedOSArchitectures>() ?? manifest.UnsupportedOSArchitectures.ToApiArray<UnsupportedOSArchitectures>();
                    newInstaller.AppsAndFeaturesEntries = AddAppsAndFeaturesEntries(installer.AppsAndFeaturesEntries) ?? AddAppsAndFeaturesEntries(manifest.AppsAndFeaturesEntries);
                    newInstaller.Markets = AddMarkets(installer.Markets) ?? AddMarkets(manifest.Markets);
                    newInstaller.NestedInstallerType = installer.NestedInstallerType ?? manifest.NestedInstallerType;
                    newInstaller.NestedInstallerFiles = AddNestedInstallerFiles(installer.NestedInstallerFiles) ?? AddNestedInstallerFiles(manifest.NestedInstallerFiles);
                    newInstaller.DisplayInstallWarnings = installer.DisplayInstallWarnings ?? manifest.DisplayInstallWarnings;
                    newInstaller.UnsupportedArguments = installer.UnsupportedArguments.ToApiArray<UnsupportedArguments>() ?? manifest.UnsupportedArguments.ToApiArray<UnsupportedArguments>();
                    newInstaller.InstallationMetadata = AddInstallationMetadata(installer.InstallationMetadata) ?? AddInstallationMetadata(manifest.InstallationMetadata);
                    newInstaller.DownloadCommandProhibited = installer.DownloadCommandProhibited ?? manifest.DownloadCommandProhibited;
                    newInstaller.RepairBehavior = installer.RepairBehavior ?? manifest.RepairBehavior;
                    newInstaller.ArchiveBinariesDependOnPath = installer.ArchiveBinariesDependOnPath ?? manifest.ArchiveBinariesDependOnPath;

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

        private static Utils.Models.Arrays.Agreements AddAgreements(List<PackageAgreement> sourceAgreements)
        {
            if (sourceAgreements != null)
            {
                var agreements = new Utils.Models.Arrays.Agreements();
                foreach (var sourceAgreement in sourceAgreements)
                {
                    var agreement = new Utils.Models.Objects.SourceAgreement
                    {
                        AgreementLabel = sourceAgreement.AgreementLabel,
                        Agreement = sourceAgreement.Agreement,
                        AgreementUrl = sourceAgreement.AgreementUrl,
                    };
                    agreements.Add(agreement);
                }

                return agreements;
            }

            return null;
        }

        private static Utils.Models.Arrays.Documentations AddDocumentations(List<ManifestDocumentation> sourceDocumentations)
        {
            if (sourceDocumentations != null)
            {
                var documentations = new Utils.Models.Arrays.Documentations();
                foreach (var sourceDocumentation in sourceDocumentations)
                {
                    var documentation = new Utils.Models.Objects.Documentation
                    {
                        DocumentLabel = sourceDocumentation.DocumentLabel,
                        DocumentUrl = sourceDocumentation.DocumentUrl,
                    };
                    documentations.Add(documentation);
                }

                return documentations;
            }

            return null;
        }

        private static Utils.Models.Arrays.Icons AddIcons(List<ManifestIcon> sourceIcons)
        {
            if (sourceIcons != null)
            {
                var icons = new Utils.Models.Arrays.Icons();
                foreach (var sourceIcon in sourceIcons)
                {
                    var icon = new Utils.Models.Objects.Icon
                    {
                        IconUrl = sourceIcon.IconUrl,
                        IconFileType = sourceIcon.IconFileType,
                        IconResolution = sourceIcon.IconResolution,
                        IconTheme = sourceIcon.IconTheme,
                        IconSha256 = sourceIcon.IconSha256,
                    };
                    icons.Add(icon);
                }

                return icons;
            }

            return null;
        }

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

        private static Utils.Models.Arrays.ExpectedReturnCodes AddExpectedReturnCodes(List<InstallerExpectedReturnCode> sourceExpectedReturnCodes)
        {
            if (sourceExpectedReturnCodes != null)
            {
                var expectedReturnCodes = new Utils.Models.Arrays.ExpectedReturnCodes();
                foreach (var sourceExpectedReturnCode in sourceExpectedReturnCodes)
                {
                    var expectedReturnCode = new Utils.Models.Objects.ExpectedReturnCode
                    {
                        InstallerReturnCode = sourceExpectedReturnCode.InstallerReturnCode,
                        ReturnResponse = sourceExpectedReturnCode.ReturnResponse,
                        ReturnResponseUrl = sourceExpectedReturnCode.ReturnResponseUrl,
                    };
                    expectedReturnCodes.Add(expectedReturnCode);
                }

                return expectedReturnCodes;
            }

            return null;
        }

        private static DateTime? TryParseDateTime(string dateTimeString)
        {
            DateTime result;
            if (DateTime.TryParse(dateTimeString, out result))
            {
                return result;
            }

            return null;
        }

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

        private static Utils.Models.Arrays.AppsAndFeaturesEntries AddAppsAndFeaturesEntries(List<InstallerArpEntry> sourceEntries)
        {
            if (sourceEntries != null)
            {
                var entries = new Utils.Models.Arrays.AppsAndFeaturesEntries();
                foreach (var sourceEntry in sourceEntries)
                {
                    var entry = new Utils.Models.Objects.AppsAndFeatures
                    {
                        DisplayName = sourceEntry.DisplayName,
                        Publisher = sourceEntry.Publisher,
                        DisplayVersion = sourceEntry.DisplayVersion,
                        ProductCode = sourceEntry.ProductCode,
                        UpgradeCode = sourceEntry.UpgradeCode,
                        InstallerType = sourceEntry.InstallerType,
                    };
                    entries.Add(entry);
                }

                return entries;
            }

            return null;
        }

        private static Utils.Models.Objects.Markets AddMarkets(InstallerMarkets sourceMarkets)
        {
            if (sourceMarkets != null)
            {
                var markets = new Utils.Models.Objects.Markets();

                markets.AllowedMarkets = sourceMarkets.AllowedMarkets.ToApiArray<Utils.Models.Arrays.Markets>();
                markets.ExcludedMarkets = sourceMarkets.ExcludedMarkets.ToApiArray<Utils.Models.Arrays.Markets>();

                return markets;
            }

            return null;
        }

        private static Utils.Models.Arrays.NestedInstallerFiles AddNestedInstallerFiles(List<InstallerNestedInstallerFile> sourceNestedInstallerFiles)
        {
            if (sourceNestedInstallerFiles != null)
            {
                var nestedInstallerFiles = new Utils.Models.Arrays.NestedInstallerFiles();
                foreach (var sourceNestedInstallerFile in sourceNestedInstallerFiles)
                {
                    var nestedInstallerFile = new Utils.Models.Objects.NestedInstallerFile
                    {
                        RelativeFilePath = sourceNestedInstallerFile.RelativeFilePath,
                        PortableCommandAlias = sourceNestedInstallerFile.PortableCommandAlias,
                    };
                    nestedInstallerFiles.Add(nestedInstallerFile);
                }

                return nestedInstallerFiles;
            }

            return null;
        }

        private static Utils.Models.Objects.InstallationMetadata AddInstallationMetadata(InstallerInstallationMetadata sourceMetaData)
        {
            if (sourceMetaData != null)
            {
                var metadata = new Utils.Models.Objects.InstallationMetadata();

                metadata.DefaultInstallLocation = sourceMetaData.DefaultInstallLocation;

                if (sourceMetaData.Files != null)
                {
                    metadata.InstallationMetadataFiles = new Utils.Models.Arrays.InstallationMetadataFiles();
                    foreach (var sourceFile in sourceMetaData.Files)
                    {
                        var file = new Utils.Models.Objects.InstallationMetadataFile
                        {
                            RelativeFilePath = sourceFile.RelativeFilePath,
                            FileSha256 = sourceFile.FileSha256,
                            FileType = sourceFile.FileType,
                            InvocationParameter = sourceFile.InvocationParameter,
                            DisplayName = sourceFile.DisplayName,
                        };
                        metadata.InstallationMetadataFiles.Add(file);
                    }
                }

                return metadata;
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
