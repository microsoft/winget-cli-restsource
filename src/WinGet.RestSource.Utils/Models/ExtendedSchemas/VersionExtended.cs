// -----------------------------------------------------------------------
// <copyright file="VersionExtended.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGet.RestSource.Utils.Validators;
    using Newtonsoft.Json;

    /// <summary>
    /// This extends the core version model by nesting the installers model.
    /// </summary>
    public class VersionExtended : Version, IApiObject<VersionExtended>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionExtended"/> class.
        /// </summary>
        public VersionExtended()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionExtended"/> class.
        /// </summary>
        /// <param name="versionExtended">Version Core.</param>
        public VersionExtended(VersionExtended versionExtended)
            : base(versionExtended)
        {
            this.Installers = versionExtended.Installers;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionExtended"/> class.
        /// </summary>
        /// <param name="version">Version Core.</param>
        public VersionExtended(Version version)
            : base(version)
        {
            this.Installers = null;
        }

        /// <summary>
        /// Gets or sets Installers.
        /// </summary>
        [JsonProperty(Order = 1)]
        public Installers Installers { get; set; }

        /// <summary>
        /// Gets or sets Installers.
        /// </summary>
        [JsonProperty(Order = 2)]
        public Locales Locales { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(VersionExtended left, VersionExtended right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(VersionExtended left, VersionExtended right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This returns the Version representation of this object.
        /// </summary>
        /// <returns>Version Core Model.</returns>
        public Version GetVersion()
        {
            return new Version(this);
        }

        /// <inheritdoc />
        public void Update(VersionExtended obj)
        {
            base.Update(obj);
            this.Installers = obj.Installers;
            this.Locales = obj.Locales;
        }

        /// <summary>
        /// Add an Installer.
        /// </summary>
        /// <param name="installer">Installer to Add.</param>
        public void AddInstaller(Installer installer)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(installer);

            // If Installers are null instantiate.
            if (this.Installers == null)
            {
                this.Installers = new Installers();
            }

            this.Installers.Add(installer);
        }

        /// <summary>
        /// Update Installer.
        /// </summary>
        /// <param name="obj">Installer to Update.</param>
        public void UpdateInstaller(Installer obj)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);

            // Verify Installers is not null.
            this.AssertInstallersNotNull();

            this.Installers.Update(obj);
        }

        /// <summary>
        /// Remove Installer.
        /// </summary>
        /// <param name="installerIdentifier">Installer Identifier.</param>
        public void RemoveInstaller(string installerIdentifier)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(installerIdentifier);

            // Verify Installers is not null.
            this.AssertInstallersNotNull();

            this.Installers.Remove(installerIdentifier);

            if (this.Installers.Count == 0)
            {
                this.Installers = null;
            }
        }

        /// <summary>
        /// Get Installer.
        /// </summary>
        /// <param name="installerIdentifier">Installer Identifier.</param>
        /// <returns>Installers.</returns>
        public Installers GetInstaller(string installerIdentifier)
        {
            // Verify Installers is not null.
            this.AssertInstallersNotNull();

            return this.Installers.Get(installerIdentifier);
        }

        /// <summary>
        /// Add an Locale.
        /// </summary>
        /// <param name="locale">Locale to Add.</param>
        public void AddLocale(Locale locale)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(locale);

            // If Locales are null instantiate.
            if (this.Locales == null)
            {
                this.Locales = new Locales();
            }

            this.Locales.Add(locale);
        }

        /// <summary>
        /// Update Locale.
        /// </summary>
        /// <param name="obj">Locale to Update.</param>
        public void UpdateLocale(Locale obj)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);

            // Verify Locales is not null.
            this.AssertLocalesNotNull();

            this.Locales.Update(obj);
        }

        /// <summary>
        /// Remove Locale.
        /// </summary>
        /// <param name="packageLocale">Installer Identifier.</param>
        public void RemoveLocale(string packageLocale)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(packageLocale);

            // Verify Locale is not null.
            this.AssertLocalesNotNull();

            this.Locales.Remove(packageLocale);

            if (this.Locales.Count == 0)
            {
                this.Locales = null;
            }
        }

        /// <summary>
        /// Get Installer.
        /// </summary>
        /// <param name="packageLocale">Package Locale.</param>
        /// <returns>Installers.</returns>
        public Locales GetLocale(string packageLocale)
        {
            // Verify Installers is not null.
            this.AssertLocalesNotNull();

            return this.Locales.Get(packageLocale);
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            // Base
            results.AddRange(base.Validate(validationContext));

            // Validate Optional Members
            if (this.Installers != null)
            {
                ApiDataValidator.Validate(this.Installers, results);
            }

            if (this.Locales != null)
            {
                ApiDataValidator.Validate(this.Locales, results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(VersionExtended other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other)
                   && Equals(this.Installers, other.Installers)
                   && Equals(this.Locales, other.Locales);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is VersionExtended versionExtended && this.Equals(versionExtended);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return System.HashCode.Combine(base.GetHashCode(), this.Installers, this.Locales);
        }

        private void AssertInstallersNotNull()
        {
            if (this.Installers == null)
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.InstallerIsNullErrorCode,
                        ErrorConstants.InstallerIsNullErrorMessage));
            }
        }

        private void AssertLocalesNotNull()
        {
            // Verify Locales is not null.
            if (this.Locales == null)
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.LocaleIsNullErrorCode,
                        ErrorConstants.LocaleIsNullErrorMessage));
            }
        }
    }
}
