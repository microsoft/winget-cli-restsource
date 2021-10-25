// -----------------------------------------------------------------------
// <copyright file="VersionsExtended.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGet.RestSource.Utils.Validators;

    /// <summary>
    /// VersionsExtended.
    /// </summary>
    public class VersionsExtended : ApiArray<VersionExtended>
    {
        private const bool Nullable = true;
        private const bool Unique = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionsExtended"/> class.
        /// </summary>
        public VersionsExtended()
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionsExtended"/> class.
        /// </summary>
        /// <param name="enumerable">enumerable.</param>
        public VersionsExtended(IEnumerable<VersionExtended> enumerable)
            : base(enumerable)
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Add a version.
        /// </summary>
        /// <param name="obj">Version to Add.</param>
        public new void Add(VersionExtended obj)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);

            // Verify Version does not exists
            this.AssertVersionDoesNotExists(obj.PackageVersion);

            // Add
            base.Add(obj);
        }

        /// <summary>
        /// Update a Version.
        /// </summary>
        /// <param name="obj">The new Version.</param>
        public void Update(VersionExtended obj)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);

            // Verify Version exists
            this.AssertVersionExists(obj.PackageVersion);

            // Update
            this[this.FindIndex(0, version => version.PackageVersion == obj.PackageVersion)].Update(obj);
        }

        /// <summary>
        /// Remove an Installer.
        /// </summary>
        /// <param name="packageVersion">Installer Identifier to remove.</param>
        public void Remove(string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(packageVersion);

            // Verify Version exists
            this.AssertVersionExists(packageVersion);

            // Remove
            this.RemoveAll(versionExtended => versionExtended.PackageVersion == packageVersion);
        }

        /// <summary>
        /// Add a version.
        /// </summary>
        /// <param name="obj">Version to Add.</param>
        public void Add(Version obj)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);

            // Verify Version does not exists
            this.AssertVersionDoesNotExists(obj.PackageVersion);

            // Add
            base.Add(new VersionExtended(obj));
        }

        /// <summary>
        /// Update a Version.
        /// </summary>
        /// <param name="obj">The new Version.</param>
        public void Update(Version obj)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);

            // Verify Version exists
            this.AssertVersionExists(obj.PackageVersion);

            // Update
            this[this.FindIndex(0, version => version.PackageVersion == obj.PackageVersion)].Update(obj);
        }

        /// <summary>
        /// Get Versions.
        /// </summary>
        /// <param name="packageVersion">Package Version.</param>
        /// <returns>VersionsExtended.</returns>
        public VersionsExtended Get(string packageVersion)
        {
            VersionsExtended extended = new VersionsExtended();

            if (string.IsNullOrWhiteSpace(packageVersion))
            {
                extended = this;
            }
            else
            {
                this.AssertVersionExists(packageVersion);
                extended.Add(this[this.FindIndex(0, version => version.PackageVersion == packageVersion)]);
            }

            return extended;
        }

        /// <summary>
        /// Add Installer.
        /// </summary>
        /// <param name="obj">Installer to Add.</param>
        /// <param name="packageVersion">Version to add to.</param>
        public void AddInstaller(Installer obj, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);
            ApiDataValidator.NotNull(packageVersion);

            // Verify Version exists
            this.AssertVersionExists(packageVersion);

            // Add
            this[this.FindIndex(0, version => version.PackageVersion == packageVersion)].AddInstaller(obj);
        }

        /// <summary>
        /// Update an Installer in a Version Extended.
        /// </summary>
        /// <param name="obj">Installer to Update.</param>
        /// <param name="packageVersion">Version to Update in.</param>
        public void UpdateInstaller(Installer obj, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);
            ApiDataValidator.NotNull(packageVersion);

            // Verify Version exists
            this.AssertVersionExists(packageVersion);

            // Update
            this[this.FindIndex(0, extended => extended.PackageVersion == packageVersion)].UpdateInstaller(obj);
        }

        /// <summary>
        /// Remove an installer.
        /// </summary>
        /// <param name="installerIdentifier">Installer Identity to remove.</param>
        /// <param name="packageVersion">Version to remove from.</param>
        public void RemoveInstaller(string installerIdentifier, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(installerIdentifier);
            ApiDataValidator.NotNull(packageVersion);

            // Verify Version exists
            this.AssertVersionExists(packageVersion);

            // Remove
            this[this.FindIndex(0, extended => extended.PackageVersion == packageVersion)].RemoveInstaller(installerIdentifier);
        }

        /// <summary>
        /// Get Installer.
        /// </summary>
        /// <param name="installerIdentifier">Installer Identity to get.</param>
        /// <param name="packageVersion">Version to get from.</param>
        /// <returns>Installers.</returns>
        public Installers GetInstaller(string installerIdentifier, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(packageVersion);

            // Verify Version exists
            this.AssertVersionExists(packageVersion);

            // Get
            return this[this.FindIndex(0, extended => extended.PackageVersion == packageVersion)].GetInstaller(installerIdentifier);
        }

        /// <summary>
        /// Add Locale.
        /// </summary>
        /// <param name="obj">Locale to Add.</param>
        /// <param name="packageVersion">Version to add to.</param>
        public void AddLocale(Locale obj, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);
            ApiDataValidator.NotNull(packageVersion);

            // Verify Version exists
            this.AssertVersionExists(packageVersion);

            // Add
            this[this.FindIndex(0, version => version.PackageVersion == packageVersion)].AddLocale(obj);
        }

        /// <summary>
        /// Update an Locale.
        /// </summary>
        /// <param name="obj">Locale to Update.</param>
        /// <param name="packageVersion">Version to Update in.</param>
        public void UpdateLocale(Locale obj, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);
            ApiDataValidator.NotNull(packageVersion);

            // Verify Version exists
            this.AssertVersionExists(packageVersion);

            // Update
            this[this.FindIndex(0, extended => extended.PackageVersion == packageVersion)].UpdateLocale(obj);
        }

        /// <summary>
        /// Remove an locale.
        /// </summary>
        /// <param name="packageLocale">Package Locale to remove.</param>
        /// <param name="packageVersion">Version to remove from.</param>
        public void RemoveLocale(string packageLocale, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(packageLocale);
            ApiDataValidator.NotNull(packageVersion);

            // Verify Version exists
            this.AssertVersionExists(packageVersion);

            // Remove
            this[this.FindIndex(0, extended => extended.PackageVersion == packageVersion)].RemoveLocale(packageLocale);
        }

        /// <summary>
        /// Get Locale.
        /// </summary>
        /// <param name="packageLocale">Package Locale to get.</param>
        /// <param name="packageVersion">Version to get from.</param>
        /// <returns>Installers.</returns>
        public Locales GetLocale(string packageLocale, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(packageVersion);

            // Verify Version exists
            this.AssertVersionExists(packageVersion);

            // Get
            return this[this.FindIndex(0, extended => extended.PackageVersion == packageVersion)].GetLocale(packageLocale);
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            // Base
            results.AddRange(base.Validate(validationContext));

            // Verify Unique version numbers
            if (this.Select(obj => obj.PackageVersion).Distinct().Count() < this.Count())
            {
                results.Add(new ValidationResult($"{validationContext.DisplayName} in {validationContext.ObjectType} does not meet unique item requirement."));
            }

            return results;
        }

        private void SetDefaults()
        {
            this.APIArrayName = nameof(VersionsExtended);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
        }

        private bool VersionExists(string version)
        {
            return this.Any(p => p.PackageVersion == version);
        }

        private void AssertVersionExists(string version)
        {
            if (!this.VersionExists(version))
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.VersionDoesNotExistErrorCode,
                        ErrorConstants.VersionDoesNotExistErrorMessage));
            }
        }

        private void AssertVersionDoesNotExists(string version)
        {
            if (this.VersionExists(version))
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.VersionAlreadyExistsErrorCode,
                        ErrorConstants.VersionAlreadyExistsErrorMessage));
            }
        }
    }
}
