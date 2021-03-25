// -----------------------------------------------------------------------
// <copyright file="PackageManifest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Models.Errors;
    using Microsoft.WinGet.RestSource.Models.ExtendedSchemas;
    using Newtonsoft.Json;

    /// <summary>
    /// This is a manifest, which is an extension of the package core model, and the extended version model.
    /// </summary>
    public class PackageManifest : Package, IApiObject<PackageManifest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageManifest"/> class.
        /// </summary>
        public PackageManifest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageManifest"/> class.
        /// </summary>
        /// <param name="packageManifest">Package Core.</param>
        public PackageManifest(PackageManifest packageManifest)
            : base(packageManifest)
        {
            this.Versions = packageManifest.Versions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageManifest"/> class.
        /// </summary>
        /// <param name="package">Package Core.</param>
        public PackageManifest(Package package)
            : base(package)
        {
            this.Versions = null;
        }

        /// <summary>
        /// Gets or sets versions.
        /// </summary>
        [JsonProperty(Order = 1)]
        public VersionsExtended Versions { get; set; }

        /// <summary>
        /// Converts to a Package Core.
        /// </summary>
        /// <returns>Package Core.</returns>
        public Package ToPackage()
        {
            Package pc = new Package(this);
            return pc;
        }

        /// <summary>
        /// This updates a Manifest to update match another Manifest.
        /// </summary>
        /// <param name="packageManifest">Manifest.</param>
        public void Update(PackageManifest packageManifest)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(packageManifest);

            this.Update(packageManifest.ToPackage());
        }

        /// <summary>
        /// Add Version.
        /// </summary>
        /// <param name="version">Version to add.</param>
        public void AddVersion(Version version)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(version);

            if (this.Versions == null)
            {
                this.Versions = new VersionsExtended();
            }

            this.Versions.Add(version);
        }

        /// <summary>
        /// Update a Version.
        /// </summary>
        /// <param name="version">Version to Update.</param>
        public void UpdateVersion(Version version)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(version);

            // Assert Versions not null
            this.AssertVersionsNotNull();

            this.Versions.Update(version);
        }

        /// <summary>
        /// Remove a Version.
        /// </summary>
        /// <param name="packageVersion">Version to Remove.</param>
        public void RemoveVersion(string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(packageVersion);

            // Assert Versions not null
            this.AssertVersionsNotNull();

            this.Versions.Remove(packageVersion);

            if (this.Versions.Count == 0)
            {
                this.Versions = null;
            }
        }

        /// <summary>
        /// Get Package Version.
        /// </summary>
        /// <param name="packageVersion">Package Version.</param>
        /// <returns>VersionsExtended.</returns>
        public VersionsExtended GetVersion(string packageVersion)
        {
            // Assert Versions not null
            this.AssertVersionsNotNull();

            return this.Versions.Get(packageVersion);
        }

        /// <summary>
        /// Add an Installer.
        /// </summary>
        /// <param name="installer">Installer to Update.</param>
        /// <param name="packageVersion">Package Version to update in.</param>
        public void AddInstaller(Installer installer, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(installer);
            ApiDataValidator.NotNull(packageVersion);

            // Instantiate if null
            if (this.Versions == null)
            {
                this.Versions = new VersionsExtended();
            }

            this.Versions.AddInstaller(installer, packageVersion);
        }

        /// <summary>
        /// Update an Installer.
        /// </summary>
        /// <param name="installer">Installer to Update.</param>
        /// <param name="packageVersion">Package Version to update in.</param>
        public void UpdateInstaller(Installer installer, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(installer);
            ApiDataValidator.NotNull(packageVersion);

            // Assert Versions not null
            this.AssertVersionsNotNull();

            this.Versions.UpdateInstaller(installer, packageVersion);
        }

        /// <summary>
        /// Remove an Installer.
        /// </summary>
        /// <param name="installerIdentifier">Installer to Update.</param>
        /// <param name="packageVersion">Package Version to update in.</param>
        public void RemoveInstaller(string installerIdentifier, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(installerIdentifier);
            ApiDataValidator.NotNull(packageVersion);

            // Assert Versions not null
            this.AssertVersionsNotNull();

            this.Versions.RemoveInstaller(installerIdentifier, packageVersion);
        }

        /// <summary>
        /// Get Installers.
        /// </summary>
        /// <param name="installerIdentifier">Installer to Get.</param>
        /// <param name="packageVersion">Package Version to get in.</param>
        /// <returns>Installers.</returns>
        public Installers GetInstaller(string installerIdentifier, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(packageVersion);

            // Assert Versions not null
            this.AssertVersionsNotNull();

            return this.Versions.GetInstaller(installerIdentifier, packageVersion);
        }

        /// <summary>
        /// Add an Locale.
        /// </summary>
        /// <param name="locale">Installer to Update.</param>
        /// <param name="packageVersion">Package Version to update in.</param>
        public void AddLocale(Locale locale, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(locale);
            ApiDataValidator.NotNull(packageVersion);

            // Instantiate if null
            if (this.Versions == null)
            {
                this.Versions = new VersionsExtended();
            }

            this.Versions.AddLocale(locale, packageVersion);
        }

        /// <summary>
        /// Update a Locale.
        /// </summary>
        /// <param name="locale">Locale to Update.</param>
        /// <param name="packageVersion">Package Version to update in.</param>
        public void UpdateLocale(Locale locale, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(locale);
            ApiDataValidator.NotNull(packageVersion);

            // Assert Versions not null
            this.AssertVersionsNotNull();

            this.Versions.UpdateLocale(locale, packageVersion);
        }

        /// <summary>
        /// Remove an Locale.
        /// </summary>
        /// <param name="packageLocale">Locale to Update.</param>
        /// <param name="packageVersion">Package Version to update in.</param>
        public void RemoveLocale(string packageLocale, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(packageLocale);
            ApiDataValidator.NotNull(packageVersion);

            // Assert Versions not null
            this.AssertVersionsNotNull();

            this.Versions.RemoveLocale(packageLocale, packageVersion);
        }

        /// <summary>
        /// Get Locale.
        /// </summary>
        /// <param name="packageLocale">Locale to Get.</param>
        /// <param name="packageVersion">Package Version to get in.</param>
        /// <returns>Locales.</returns>
        public Locales GetLocale(string packageLocale, string packageVersion)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(packageVersion);

            // Assert Versions not null
            this.AssertVersionsNotNull();

            return this.Versions.GetLocale(packageLocale, packageVersion);
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            // Base
            results.AddRange(base.Validate(validationContext));

            // Validate Optional Members
            if (this.Versions != null)
            {
                Validator.TryValidateObject(this.Versions, new ValidationContext(this.Versions, null, null), results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(PackageManifest other)
        {
            return base.Equals(other);
        }

        private void AssertVersionsNotNull()
        {
            if (this.Versions == null)
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.VersionsIsNullErrorCode,
                        ErrorConstants.VersionsIsNullErrorMessage));
            }
        }
    }
}
