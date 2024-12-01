// -----------------------------------------------------------------------
// <copyright file="Installer.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Models.Objects;
    using Microsoft.WinGet.RestSource.Utils.Validators;
    using Microsoft.WinGet.RestSource.Utils.Validators.DateTimeValidators;
    using Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;
    using Capabilities = Microsoft.WinGet.RestSource.Utils.Models.Arrays.Capabilities;
    using Commands = Microsoft.WinGet.RestSource.Utils.Models.Arrays.Commands;
    using Dependencies = Microsoft.WinGet.RestSource.Utils.Models.Objects.Dependencies;
    using FileExtensions = Microsoft.WinGet.RestSource.Utils.Models.Arrays.FileExtensions;
    using Platform = Microsoft.WinGet.RestSource.Utils.Models.Arrays.Platform;
    using Protocols = Microsoft.WinGet.RestSource.Utils.Models.Arrays.Protocols;

    /// <summary>
    /// Installer.
    /// </summary>
    public class Installer : IApiObject<Installer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Installer"/> class.
        /// </summary>
        public Installer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Installer"/> class.
        /// </summary>
        /// <param name="installer">Installer.</param>
        public Installer(Installer installer)
        {
            this.Update(installer);
        }

        /// <summary>
        /// Gets or sets InstallerIdentifier.
        /// </summary>
        [InstallerIdentifierValidator]
        public string InstallerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets InstallerSha256.
        /// </summary>
        [Sha256Validator]
        public string InstallerSha256 { get; set; }

        /// <summary>
        /// Gets or sets InstallerUrl.
        /// </summary>
        [UrlValidator]
        public string InstallerUrl { get; set; }

        /// <summary>
        /// Gets or sets Architecture.
        /// </summary>
        [ArchitectureValidator]
        public string Architecture { get; set; }

        /// <summary>
        /// Gets or sets InstallerLocale.
        /// </summary>
        [LocaleValidator]
        public string InstallerLocale { get; set; }

        /// <summary>
        /// Gets or sets Platform.
        /// </summary>
        public Platform Platform { get; set; }

        /// <summary>
        /// Gets or sets MinimumOsVersion.
        /// </summary>
        [MinimumOSVersionValidator]
        public string MinimumOsVersion { get; set; }

        /// <summary>
        /// Gets or sets InstallerType.
        /// </summary>
        [InstallerTypeValidator]
        public string InstallerType { get; set; }

        /// <summary>
        /// Gets or sets Scope.
        /// </summary>
        [ScopeValidator]
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets SignatureSha256.
        /// </summary>
        [Sha256Validator]
        public string SignatureSha256 { get; set; }

        /// <summary>
        /// Gets or sets InstallModes.
        /// </summary>
        public InstallModes InstallModes { get; set; }

        /// <summary>
        /// Gets or sets InstallerSwitches.
        /// </summary>
        public InstallerSwitches InstallerSwitches { get; set; }

        /// <summary>
        /// Gets or sets InstallerSuccessCodes.
        /// </summary>
        public InstallerSuccessCodes InstallerSuccessCodes { get; set; }

        /// <summary>
        /// Gets or sets ExpectedReturnCodes.
        /// </summary>
        public ExpectedReturnCodes ExpectedReturnCodes { get; set; }

        /// <summary>
        /// Gets or sets UpgradeBehavior.
        /// </summary>
        [UpgradeBehaviorValidator]
        public string UpgradeBehavior { get; set; }

        /// <summary>
        /// Gets or sets Commands.
        /// </summary>
        public Commands Commands { get; set; }

        /// <summary>
        /// Gets or sets Protocols.
        /// </summary>
        public Protocols Protocols { get; set; }

        /// <summary>
        /// Gets or sets FileExtensions.
        /// </summary>
        public FileExtensions FileExtensions { get; set; }

        /// <summary>
        /// Gets or sets Dependencies.
        /// </summary>
        public Dependencies Dependencies { get; set; }

        /// <summary>
        /// Gets or sets PackageFamilyName.
        /// </summary>
        [PackageFamilyNameValidator]
        public string PackageFamilyName { get; set; }

        /// <summary>
        /// Gets or sets ProductCode.
        /// </summary>
        [ProductCodeValidator]
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets or sets Capabilities.
        /// </summary>
        public Capabilities Capabilities { get; set; }

        /// <summary>
        /// Gets or sets RestrictedCapabilities.
        /// </summary>
        public RestrictedCapabilities RestrictedCapabilities { get; set; }

        /// <summary>
        /// Gets or sets MSStoreProductIdentifier.
        /// </summary>
        [MSStoreProductIdentifierValidator]
        public string MSStoreProductIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether InstallerAbortsTerminal is set.
        /// This indicates if the package aborts the terminal during installation.
        /// </summary>
        public bool? InstallerAbortsTerminal { get; set; }

        /// <summary>
        /// Gets or sets ReleaseDate.
        /// </summary>
        [ReleaseDateValidator]
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the installer requires an install location provided.
        /// </summary>
        public bool? InstallLocationRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the installer requires explicit upgrade.
        /// </summary>
        public bool? RequireExplicitUpgrade { get; set; }

        /// <summary>
        /// Gets or sets the installer's elevation requirement.
        /// </summary>
        [ElevationRequirementValidator]
        public string ElevationRequirement { get; set; }

        /// <summary>
        /// Gets or sets UnsupportedOSArchitectures.
        /// </summary>
        public UnsupportedOSArchitectures UnsupportedOSArchitectures { get; set; }

        /// <summary>
        /// Gets or sets AppsAndFeaturesEntries.
        /// </summary>
        public AppsAndFeaturesEntries AppsAndFeaturesEntries { get; set; }

        /// <summary>
        /// Gets or sets Markets.
        /// </summary>
        public Objects.Markets Markets { get; set; }

        /// <summary>
        /// Gets or sets NestedInstallerType.
        /// </summary>
        [NestedInstallerTypeValidator]
        public string NestedInstallerType { get; set; }

        /// <summary>
        /// Gets or sets NestedInstallerFiles.
        /// </summary>
        public NestedInstallerFiles NestedInstallerFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether displayInstallWarnings.
        /// </summary>
        public bool? DisplayInstallWarnings { get; set; }

        /// <summary>
        /// Gets or sets unsupportedArguments.
        /// </summary>
        public UnsupportedArguments UnsupportedArguments { get; set; }

        /// <summary>
        /// Gets or sets installationMetadata.
        /// </summary>
        public InstallationMetadata InstallationMetadata { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the installer is prohibited from being downloaded for offline installation.
        /// </summary>
        public bool? DownloadCommandProhibited { get; set; }

        /// <summary>
        /// Gets or sets RepairBehavior.
        /// </summary>
        [RepairBehaviorValidator]
        public string RepairBehavior { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the install location should be added directly to the PATH environment variable. Only applies to an archive containing portable packages.
        /// </summary>
        public bool? ArchiveBinariesDependOnPath { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(Installer left, Installer right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(Installer left, Installer right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(Installer obj)
        {
            this.InstallerIdentifier = obj.InstallerIdentifier;
            this.InstallerSha256 = obj.InstallerSha256;
            this.InstallerUrl = obj.InstallerUrl;
            this.Architecture = obj.Architecture;
            this.InstallerLocale = obj.InstallerLocale;
            this.Platform = obj.Platform;
            this.MinimumOsVersion = obj.MinimumOsVersion;
            this.InstallerType = obj.InstallerType;
            this.Scope = obj.Scope;
            this.SignatureSha256 = obj.SignatureSha256;
            this.InstallModes = obj.InstallModes;
            this.InstallerSwitches = obj.InstallerSwitches;
            this.InstallerSuccessCodes = obj.InstallerSuccessCodes;
            this.ExpectedReturnCodes = obj.ExpectedReturnCodes;
            this.UpgradeBehavior = obj.UpgradeBehavior;
            this.Commands = obj.Commands;
            this.Protocols = obj.Protocols;
            this.FileExtensions = obj.FileExtensions;
            this.Dependencies = obj.Dependencies;
            this.PackageFamilyName = obj.PackageFamilyName;
            this.ProductCode = obj.ProductCode;
            this.Capabilities = obj.Capabilities;
            this.RestrictedCapabilities = obj.RestrictedCapabilities;
            this.MSStoreProductIdentifier = obj.MSStoreProductIdentifier;
            this.InstallerAbortsTerminal = obj.InstallerAbortsTerminal;
            this.ReleaseDate = obj.ReleaseDate;
            this.InstallLocationRequired = obj.InstallLocationRequired;
            this.RequireExplicitUpgrade = obj.RequireExplicitUpgrade;
            this.ElevationRequirement = obj.ElevationRequirement;
            this.UnsupportedOSArchitectures = obj.UnsupportedOSArchitectures;
            this.AppsAndFeaturesEntries = obj.AppsAndFeaturesEntries;
            this.Markets = obj.Markets;
            this.NestedInstallerType = obj.NestedInstallerType;
            this.NestedInstallerFiles = obj.NestedInstallerFiles;
            this.DisplayInstallWarnings = obj.DisplayInstallWarnings;
            this.UnsupportedArguments = obj.UnsupportedArguments;
            this.InstallationMetadata = obj.InstallationMetadata;
            this.DownloadCommandProhibited = obj.DownloadCommandProhibited;
            this.RepairBehavior = obj.RepairBehavior;
            this.ArchiveBinariesDependOnPath = obj.ArchiveBinariesDependOnPath;
        }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            // InstallerUrl and InstallerSHA256 should be specified if InstallerType is not msstore.
            if (this.InstallerType != "msstore" && (string.IsNullOrEmpty(this.InstallerUrl) || string.IsNullOrEmpty(this.InstallerSha256)))
            {
                results.Add(new ValidationResult(
                    $"{nameof(this.InstallerUrl)} and {nameof(this.InstallerSha256)} in {validationContext.ObjectType} must not be null if Installer type is not msstore."));
            }

            // InstallerUrl and InstallerSHA256 should be specified if InstallerType is not msstore.
            if (this.InstallerType == "msstore" && string.IsNullOrEmpty(this.MSStoreProductIdentifier))
            {
                results.Add(new ValidationResult($"{nameof(this.MSStoreProductIdentifier)} must not be null if Installer type is msstore."));
            }

            // Optional Objects
            if (this.Platform != null)
            {
                ApiDataValidator.Validate(this.Platform, results);
            }

            if (this.InstallModes != null)
            {
                ApiDataValidator.Validate(this.InstallModes, results);
            }

            if (this.InstallerSwitches != null)
            {
                ApiDataValidator.Validate(this.InstallerSwitches, results);
            }

            if (this.InstallerSuccessCodes != null)
            {
                ApiDataValidator.Validate(this.InstallerSuccessCodes, results);
            }

            if (this.ExpectedReturnCodes != null)
            {
                ApiDataValidator.Validate(this.ExpectedReturnCodes, results);
            }

            if (this.Commands != null)
            {
                ApiDataValidator.Validate(this.Commands, results);
            }

            if (this.Protocols != null)
            {
                ApiDataValidator.Validate(this.Protocols, results);
            }

            if (this.FileExtensions != null)
            {
                ApiDataValidator.Validate(this.FileExtensions, results);
            }

            if (this.Dependencies != null)
            {
                ApiDataValidator.Validate(this.Dependencies, results);
            }

            if (this.Capabilities != null)
            {
                ApiDataValidator.Validate(this.Capabilities, results);
            }

            if (this.RestrictedCapabilities != null)
            {
                ApiDataValidator.Validate(this.RestrictedCapabilities, results);
            }

            if (this.UnsupportedOSArchitectures != null)
            {
                ApiDataValidator.Validate(this.UnsupportedOSArchitectures, results);
            }

            if (this.AppsAndFeaturesEntries != null)
            {
                ApiDataValidator.Validate(this.AppsAndFeaturesEntries, results);
            }

            if (this.Markets != null)
            {
                ApiDataValidator.Validate(this.Markets, results);
            }

            if (this.NestedInstallerFiles != null)
            {
                ApiDataValidator.Validate(this.NestedInstallerFiles, results);
            }

            if (this.UnsupportedArguments != null)
            {
                ApiDataValidator.Validate(this.UnsupportedArguments, results);
            }

            if (this.InstallationMetadata != null)
            {
                ApiDataValidator.Validate(this.InstallationMetadata, results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(Installer other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.InstallerIdentifier, other.InstallerIdentifier)
                   && Equals(this.InstallerSha256, other.InstallerSha256)
                   && Equals(this.InstallerUrl, other.InstallerUrl)
                   && Equals(this.Architecture, other.Architecture)
                   && Equals(this.InstallerLocale, other.InstallerLocale)
                   && Equals(this.Platform, other.Platform)
                   && Equals(this.MinimumOsVersion, other.MinimumOsVersion)
                   && Equals(this.InstallerType, other.InstallerType)
                   && Equals(this.Scope, other.Scope)
                   && Equals(this.SignatureSha256, other.SignatureSha256)
                   && Equals(this.InstallModes, other.InstallModes)
                   && Equals(this.InstallerSwitches, other.InstallerSwitches)
                   && Equals(this.InstallerSuccessCodes, other.InstallerSuccessCodes)
                   && Equals(this.ExpectedReturnCodes, other.ExpectedReturnCodes)
                   && Equals(this.UpgradeBehavior, other.UpgradeBehavior)
                   && Equals(this.Commands, other.Commands)
                   && Equals(this.Protocols, other.Protocols)
                   && Equals(this.FileExtensions, other.FileExtensions)
                   && Equals(this.Dependencies, other.Dependencies)
                   && Equals(this.PackageFamilyName, other.PackageFamilyName)
                   && Equals(this.ProductCode, other.ProductCode)
                   && Equals(this.Capabilities, other.Capabilities)
                   && Equals(this.RestrictedCapabilities, other.RestrictedCapabilities)
                   && Equals(this.MSStoreProductIdentifier, other.MSStoreProductIdentifier)
                   && Equals(this.InstallerAbortsTerminal, other.InstallerAbortsTerminal)
                   && Equals(this.ReleaseDate, other.ReleaseDate)
                   && Equals(this.InstallLocationRequired, other.InstallLocationRequired)
                   && Equals(this.RequireExplicitUpgrade, other.RequireExplicitUpgrade)
                   && Equals(this.ElevationRequirement, other.ElevationRequirement)
                   && Equals(this.UnsupportedOSArchitectures, other.UnsupportedOSArchitectures)
                   && Equals(this.AppsAndFeaturesEntries, other.AppsAndFeaturesEntries)
                   && Equals(this.Markets, other.Markets)
                   && Equals(this.NestedInstallerType, other.NestedInstallerType)
                   && Equals(this.NestedInstallerFiles, other.NestedInstallerFiles)
                   && Equals(this.DisplayInstallWarnings, other.DisplayInstallWarnings)
                   && Equals(this.UnsupportedArguments, other.UnsupportedArguments)
                   && Equals(this.InstallationMetadata, other.InstallationMetadata)
                   && Equals(this.DownloadCommandProhibited, other.DownloadCommandProhibited)
                   && Equals(this.RepairBehavior, other.RepairBehavior)
                   && Equals(this.ArchiveBinariesDependOnPath, other.ArchiveBinariesDependOnPath);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Installer installer && this.Equals(installer);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            HashCode hashCode = default;
            hashCode.Add(this.InstallerIdentifier);
            hashCode.Add(this.InstallerSha256);
            hashCode.Add(this.InstallerUrl);
            hashCode.Add(this.Architecture);
            hashCode.Add(this.InstallerLocale);
            hashCode.Add(this.Platform);
            hashCode.Add(this.MinimumOsVersion);
            hashCode.Add(this.InstallerType);
            hashCode.Add(this.Scope);
            hashCode.Add(this.SignatureSha256);
            hashCode.Add(this.InstallModes);
            hashCode.Add(this.InstallerSwitches);
            hashCode.Add(this.InstallerSuccessCodes);
            hashCode.Add(this.ExpectedReturnCodes);
            hashCode.Add(this.UpgradeBehavior);
            hashCode.Add(this.Commands);
            hashCode.Add(this.Protocols);
            hashCode.Add(this.FileExtensions);
            hashCode.Add(this.Dependencies);
            hashCode.Add(this.PackageFamilyName);
            hashCode.Add(this.ProductCode);
            hashCode.Add(this.Capabilities);
            hashCode.Add(this.RestrictedCapabilities);
            hashCode.Add(this.MSStoreProductIdentifier);
            hashCode.Add(this.InstallerAbortsTerminal);
            hashCode.Add(this.ReleaseDate);
            hashCode.Add(this.InstallLocationRequired);
            hashCode.Add(this.RequireExplicitUpgrade);
            hashCode.Add(this.ElevationRequirement);
            hashCode.Add(this.UnsupportedOSArchitectures);
            hashCode.Add(this.AppsAndFeaturesEntries);
            hashCode.Add(this.Markets);
            hashCode.Add(this.NestedInstallerType);
            hashCode.Add(this.NestedInstallerFiles);
            hashCode.Add(this.DisplayInstallWarnings);
            hashCode.Add(this.UnsupportedArguments);
            hashCode.Add(this.InstallationMetadata);
            hashCode.Add(this.DownloadCommandProhibited);
            hashCode.Add(this.RepairBehavior);
            hashCode.Add(this.ArchiveBinariesDependOnPath);
            return hashCode.ToHashCode();
        }
    }
}
