// -----------------------------------------------------------------------
// <copyright file="Installer.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Models.Objects;
    using Microsoft.WinGet.RestSource.Validators;
    using Microsoft.WinGet.RestSource.Validators.EnumValidators;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;
    using Capabilities = Microsoft.WinGet.RestSource.Models.Arrays.Capabilities;
    using Commands = Microsoft.WinGet.RestSource.Models.Arrays.Commands;
    using Dependencies = Microsoft.WinGet.RestSource.Models.Objects.Dependencies;
    using FileExtensions = Microsoft.WinGet.RestSource.Models.Arrays.FileExtensions;
    using Platform = Microsoft.WinGet.RestSource.Models.Arrays.Platform;
    using Protocols = Microsoft.WinGet.RestSource.Models.Arrays.Protocols;

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
        [InstallerSha256Validator]
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
        [SignatureSha256Validator]
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
        public bool InstallerAbortsTerminal { get; set; }

        /// <summary>
        /// Gets or sets ReleaseDate.
        /// </summary>
        [ReleaseDateValidator]
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the installer requires an install location provided.
        /// </summary>
        public bool InstallLocationRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the installer requires explicit upgrade.
        /// </summary>
        public bool RequireExplicitUpgrade { get; set; }

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
            this.UnsupportedOSArchitectures = obj.UnsupportedOSArchitectures;
            this.AppsAndFeaturesEntries = obj.AppsAndFeaturesEntries;
            this.Markets = obj.Markets;
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
                ApiDataValidator.Validate(this.InstallerSwitches, results);
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

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(Installer other)
        {
            if (ReferenceEquals(null, other))
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
                   && Equals(this.UnsupportedOSArchitectures, other.UnsupportedOSArchitectures)
                   && Equals(this.AppsAndFeaturesEntries, other.AppsAndFeaturesEntries)
                   && Equals(this.Markets, other.Markets);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Installer)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.InstallerIdentifier != null ? this.InstallerIdentifier.GetHashCode() : 0;
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.InstallerSha256 != null ? this.InstallerSha256.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.InstallerUrl != null ? this.InstallerUrl.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Architecture != null ? this.Architecture.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.InstallerLocale != null ? this.InstallerLocale.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Platform != null ? this.Platform.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.MinimumOsVersion != null ? this.MinimumOsVersion.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.InstallerType != null ? this.InstallerType.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Scope != null ? this.Scope.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.SignatureSha256 != null ? this.SignatureSha256.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.InstallModes != null ? this.InstallModes.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.InstallerSwitches != null ? this.InstallerSwitches.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.InstallerSuccessCodes != null ? this.InstallerSuccessCodes.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.UpgradeBehavior != null ? this.UpgradeBehavior.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Commands != null ? this.Commands.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Protocols != null ? this.Protocols.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.FileExtensions != null ? this.FileExtensions.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Dependencies != null ? this.Dependencies.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.PackageFamilyName != null ? this.PackageFamilyName.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.ProductCode != null ? this.ProductCode.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Capabilities != null ? this.Capabilities.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.RestrictedCapabilities != null ? this.RestrictedCapabilities.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.MSStoreProductIdentifier != null ? this.MSStoreProductIdentifier.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ this.InstallerAbortsTerminal.GetHashCode();
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.ReleaseDate != null ? this.ReleaseDate.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ this.InstallLocationRequired.GetHashCode();
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ this.RequireExplicitUpgrade.GetHashCode();
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.UnsupportedOSArchitectures != null ? this.UnsupportedOSArchitectures.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.AppsAndFeaturesEntries != null ? this.AppsAndFeaturesEntries.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Markets != null ? this.Markets.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
