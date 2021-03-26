// -----------------------------------------------------------------------
// <copyright file="Locales.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Models.Errors;

    /// <summary>
    /// Locales.
    /// </summary>
    public class Locales : ApiArray<Locale>
    {
        private const bool Nullable = true;
        private const bool Unique = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Locales"/> class.
        /// </summary>
        public Locales()
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Locales"/> class.
        /// </summary>
        /// <param name="enumerable">Enumerable.</param>
        public Locales(IEnumerable<Locale> enumerable)
            : base(enumerable)
        {
            this.SetDefaults();
        }

        /// <summary>
        /// Add new Locale.
        /// </summary>
        /// <param name="obj">Locale to add.</param>
        public new void Add(Locale obj)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);

            // Verify locale does not exist
            this.AssertLocaleDoesNotExists(obj.PackageLocale);

            base.Add(obj);
        }

        /// <summary>
        /// Update an Locale.
        /// </summary>
        /// <param name="obj">New Locale.</param>
        public void Update(Locale obj)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(obj);

            // Verify locale exists
            this.AssertLocaleExists(obj.PackageLocale);

            // Update
            this[this.FindIndex(0, installer => installer.PackageLocale == obj.PackageLocale)].Update(obj);
        }

        /// <summary>
        /// Remove an Locale.
        /// </summary>
        /// <param name="packageLocale">Package Locale to remove.</param>
        public void Remove(string packageLocale)
        {
            // Verify Parameters not null
            ApiDataValidator.NotNull(packageLocale);

            // Verify locale exists
            this.AssertLocaleExists(packageLocale);

            this.RemoveAll(locale => locale.PackageLocale == packageLocale);
        }

        /// <summary>
        /// Get Installers.
        /// </summary>
        /// <param name="packageLocale">Package Locale to get.</param>
        /// <returns>Installers.</returns>
        public Locales Get(string packageLocale)
        {
            Locales locales = new Locales();

            if (string.IsNullOrWhiteSpace(packageLocale))
            {
                locales = this;
            }
            else
            {
                this.AssertLocaleExists(packageLocale);
                locales.Add(this[this.FindIndex(0, locale => locale.PackageLocale == packageLocale)]);
            }

            return locales;
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            // Base
            results.AddRange(base.Validate(validationContext));

            // Verify Unique version numbers
            if (this.Select(obj => obj.PackageLocale).Distinct().Count() < this.Count())
            {
                results.Add(new ValidationResult($"{this.APIArrayName} does not meet unique item requirement: {this}."));
            }

            return results;
        }

        private void SetDefaults()
        {
            this.APIArrayName = nameof(Locales);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
        }

        private bool LocaleExists(string packageLocale)
        {
            return this.Any(p => p.PackageLocale == packageLocale);
        }

        private void AssertLocaleExists(string installerIdentifier)
        {
            if (!this.LocaleExists(installerIdentifier))
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.LocaleDoesNotExistErrorCode,
                        ErrorConstants.LocaleDoesNotExistErrorMessage));
            }
        }

        private void AssertLocaleDoesNotExists(string installerIdentifier)
        {
            if (this.LocaleExists(installerIdentifier))
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.LocaleAlreadyExistsErrorCode,
                        ErrorConstants.LocaleAlreadyExistsErrorMessage));
            }
        }
    }
}
