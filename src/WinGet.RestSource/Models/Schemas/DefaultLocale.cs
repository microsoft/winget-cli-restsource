﻿// -----------------------------------------------------------------------
// <copyright file="DefaultLocale.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Models.Strings;
    using PackageLocale = Microsoft.WinGet.RestSource.Models.Strings.Locale;

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
        public Tag Moniker { get; set; }

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
            if (this.Publisher == null)
            {
                results.Add(new ValidationResult($"Publisher must not be null."));
            }
            else
            {
                if (string.IsNullOrEmpty(this.Publisher.APIString))
                {
                    results.Add(new ValidationResult($"{this.Publisher.APIStringName} '{this.Publisher.APIString}' must not be null."));
                }
            }

            if (this.PackageName == null)
            {
                results.Add(new ValidationResult($"PackageName must not be null."));
            }
            else
            {
                if (string.IsNullOrEmpty(this.PackageName.APIString))
                {
                    results.Add(new ValidationResult($"{this.PackageName.APIStringName} '{this.PackageName.APIString}' must not be null."));
                }
            }

            if (this.License == null)
            {
                results.Add(new ValidationResult($"License must not be null."));
            }
            else
            {
                if (string.IsNullOrEmpty(this.License.APIString))
                {
                    results.Add(new ValidationResult($"{this.License.APIStringName} '{this.License.APIString}' must not be null."));
                }
            }

            if (this.ShortDescription == null)
            {
                results.Add(new ValidationResult($"ShortDescription must not be null."));
            }
            else
            {
                if (string.IsNullOrEmpty(this.ShortDescription.APIString))
                {
                    results.Add(new ValidationResult($"{this.ShortDescription.APIStringName} '{this.ShortDescription.APIString}' must not be null."));
                }
            }

            // Validate Base
            results.AddRange(base.Validate(validationContext));

            // Validate Optional Members
            if (this.Moniker != null)
            {
                Validator.TryValidateObject(this.Moniker, new ValidationContext(this.Moniker, null, null), results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(DefaultLocale other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this, other)
                   && Equals(this.Moniker, other.Moniker);
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

            return this.Equals((DefaultLocale)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Moniker != null ? this.Moniker.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
