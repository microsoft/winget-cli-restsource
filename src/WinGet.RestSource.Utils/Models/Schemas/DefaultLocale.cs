// -----------------------------------------------------------------------
// <copyright file="DefaultLocale.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// Locale.
    /// </summary>
    public class DefaultLocale : Locale, IApiObject<DefaultLocale>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLocale"/> class.
        /// </summary>
        public DefaultLocale()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLocale"/> class.
        /// </summary>
        /// <param name="locale">Locale.</param>
        public DefaultLocale(DefaultLocale locale)
        {
            this.Update(locale);
        }

        /// <summary>
        /// Gets or sets Moniker.
        /// </summary>
        [TagValidator]
        public string Moniker { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(DefaultLocale left, DefaultLocale right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(DefaultLocale left, DefaultLocale right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(DefaultLocale obj)
        {
            base.Update(obj);
            this.Moniker = obj.Moniker;
        }

        /// <inheritdoc />
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            // Validate Required fields that are optional in base.
            if (string.IsNullOrEmpty(this.Publisher))
            {
                results.Add(new ValidationResult($"{nameof(this.Publisher)} in {validationContext.ObjectType} must not be null."));
            }

            if (string.IsNullOrEmpty(this.PackageName))
            {
                results.Add(new ValidationResult($"{nameof(this.PackageName)} in {validationContext.ObjectType} must not be null."));
            }

            if (string.IsNullOrEmpty(this.License))
            {
                results.Add(new ValidationResult($"{nameof(this.License)} in {validationContext.ObjectType} must not be null."));
            }

            if (string.IsNullOrEmpty(this.ShortDescription))
            {
                results.Add(new ValidationResult($"{nameof(this.ShortDescription)} in {validationContext.ObjectType} must not be null."));
            }

            // Validate Base
            results.AddRange(base.Validate(validationContext));

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(DefaultLocale other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this, other) && Equals(this.Moniker, other.Moniker);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is DefaultLocale defaultLocale && this.Equals(defaultLocale);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (base.GetHashCode(), this.Moniker).GetHashCode();
        }
    }
}
