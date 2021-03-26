// -----------------------------------------------------------------------
// <copyright file="Installer.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Models.Enum;
    using Microsoft.WinGet.RestSource.Models.Objects;
    using Microsoft.WinGet.RestSource.Models.Strings;
    using Architecture = Microsoft.WinGet.RestSource.Models.Enum.Architecture;
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
        public InstallerIdentifier InstallerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets InstallerSha256.
        /// </summary>
        public InstallerSha256 InstallerSha256 { get; set; }

        /// <summary>
        /// Gets or sets InstallerUrl.
        /// </summary>
        public Url InstallerUrl { get; set; }

        /// <summary>
        /// Gets or sets Architecture.
        /// </summary>
        public Architecture Architecture { get; set; }

        /// <summary>
        /// Gets or sets InstallerLocale.
        /// </summary>
        public Locale InstallerLocale { get; set; }

        /// <summary>
        /// Gets or sets Platform.
        /// </summary>
        public Platform Platform { get; set; }

        /// <summary>
        /// Gets or sets MinimumOsVersion.
        /// </summary>
        public MinimumOSVersion MinimumOsVersion { get; set; }

        /// <summary>
        /// Gets or sets InstallerType.
        /// </summary>
        public InstallerType InstallerType { get; set; }

        /// <summary>
        /// Gets or sets Scope.
        /// </summary>
        public Scope Scope { get; set; }

        /// <summary>
        /// Gets or sets SignatureSha256.
        /// </summary>
        public SignatureSha256 SignatureSha256 { get; set; }

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
        public UpgradeBehavior UpgradeBehavior { get; set; }

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
        public PackageFamilyName PackageFamilyName { get; set; }

        /// <summary>
        /// Gets or sets ProductCode.
        /// </summary>
        public ProductCode ProductCode { get; set; }

        /// <summary>
        /// Gets or sets Capabilities.
        /// </summary>
        public Capabilities Capabilities { get; set; }

        /// <summary>
        /// Gets or sets RestrictedCapabilities.
        /// </summary>
        public RestrictedCapabilities RestrictedCapabilities { get; set; }

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
        }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            // Verify Required Fields
            if (this.InstallerIdentifier != null)
            {
                Validator.TryValidateObject(this.InstallerIdentifier, new ValidationContext(this.InstallerIdentifier, null, null), results);
            }

            if (this.InstallerSha256 == null)
            {
                results.Add(new ValidationResult($"InstallerSha256 must not be null."));
            }
            else
            {
                Validator.TryValidateObject(this.InstallerSha256, new ValidationContext(this.InstallerSha256, null, null), results);
            }

            if (this.InstallerUrl == null)
            {
                results.Add(new ValidationResult($"InstallerUrl must not be null."));
            }
            else
            {
                Validator.TryValidateObject(this.InstallerUrl, new ValidationContext(this.InstallerUrl, null, null), results);

                // URL Are generally nullable, but not in this instance.
                if (string.IsNullOrEmpty(this.InstallerUrl.APIString))
                {
                    results.Add(new ValidationResult($"{this.InstallerUrl.APIStringName} '{this.InstallerUrl.APIString}' must not be null."));
                }
            }

            if (this.Architecture == null)
            {
                results.Add(new ValidationResult($"Architecture must not be null."));
            }
            else
            {
                Validator.TryValidateObject(this.Architecture, new ValidationContext(this.Architecture, null, null), results);
            }

            if (this.InstallerType == null)
            {
                results.Add(new ValidationResult($"InstallerType must not be null."));
            }
            else
            {
                Validator.TryValidateObject(this.InstallerType, new ValidationContext(this.InstallerType, null, null), results);
            }

            // Validate Optional Members
            if (this.InstallerLocale != null)
            {
                Validator.TryValidateObject(this.InstallerLocale, new ValidationContext(this.InstallerLocale, null, null), results);
            }

            if (this.Platform != null)
            {
                Validator.TryValidateObject(this.Platform, new ValidationContext(this.Platform, null, null), results);
            }

            if (this.MinimumOsVersion != null)
            {
                Validator.TryValidateObject(this.MinimumOsVersion, new ValidationContext(this.MinimumOsVersion, null, null), results);
            }

            if (this.Scope != null)
            {
                Validator.TryValidateObject(this.Scope, new ValidationContext(this.Scope, null, null), results);
            }

            if (this.SignatureSha256 != null)
            {
                Validator.TryValidateObject(this.SignatureSha256, new ValidationContext(this.SignatureSha256, null, null), results);
            }

            if (this.InstallModes != null)
            {
                Validator.TryValidateObject(this.InstallModes, new ValidationContext(this.InstallModes, null, null), results);
            }

            if (this.InstallerSwitches != null)
            {
                Validator.TryValidateObject(this.InstallerSwitches, new ValidationContext(this.InstallerSwitches, null, null), results);
            }

            if (this.InstallerSuccessCodes != null)
            {
                Validator.TryValidateObject(this.InstallerSuccessCodes, new ValidationContext(this.InstallerSuccessCodes, null, null), results);
            }

            if (this.UpgradeBehavior != null)
            {
                Validator.TryValidateObject(this.UpgradeBehavior, new ValidationContext(this.UpgradeBehavior, null, null), results);
            }

            if (this.Commands != null)
            {
                Validator.TryValidateObject(this.Commands, new ValidationContext(this.Commands, null, null), results);
            }

            if (this.Protocols != null)
            {
                Validator.TryValidateObject(this.Protocols, new ValidationContext(this.Protocols, null, null), results);
            }

            if (this.FileExtensions != null)
            {
                Validator.TryValidateObject(this.FileExtensions, new ValidationContext(this.FileExtensions, null, null), results);
            }

            if (this.Dependencies != null)
            {
                Validator.TryValidateObject(this.Dependencies, new ValidationContext(this.Dependencies, null, null), results);
            }

            if (this.PackageFamilyName != null)
            {
                Validator.TryValidateObject(this.PackageFamilyName, new ValidationContext(this.PackageFamilyName, null, null), results);
            }

            if (this.ProductCode != null)
            {
                Validator.TryValidateObject(this.ProductCode, new ValidationContext(this.ProductCode, null, null), results);
            }

            if (this.Capabilities != null)
            {
                Validator.TryValidateObject(this.Capabilities, new ValidationContext(this.Capabilities, null, null), results);
            }

            if (this.RestrictedCapabilities != null)
            {
                Validator.TryValidateObject(this.RestrictedCapabilities, new ValidationContext(this.RestrictedCapabilities, null, null), results);
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
                   && Equals(this.RestrictedCapabilities, other.RestrictedCapabilities);
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
                return hashCode;
            }
        }
    }
}
