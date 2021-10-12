// -----------------------------------------------------------------------
// <copyright file="Markets.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators;
    using Microsoft.WinGet.RestSource.Validators.EnumValidators;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// Markets.
    /// </summary>
    public class Markets : IApiObject<Markets>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Markets"/> class.
        /// </summary>
        public Markets()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Markets"/> class.
        /// </summary>
        /// <param name="markets">Markets.</param>
        public Markets(Markets markets)
        {
            this.Update(markets);
        }

        /// <summary>
        /// Gets or sets AllowedMarkets.
        /// </summary>
        public Arrays.Markets AllowedMarkets { get; set; }

        /// <summary>
        /// Gets or sets ExcludedMarkets.
        /// </summary>
        public Arrays.Markets ExcludedMarkets { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(Markets left, Markets right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(Markets left, Markets right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(Markets obj)
        {
            this.AllowedMarkets = obj.AllowedMarkets;
            this.ExcludedMarkets = obj.ExcludedMarkets;
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            if (this.AllowedMarkets != null)
            {
                ApiDataValidator.Validate(this.AllowedMarkets, results);
            }

            if (this.ExcludedMarkets != null)
            {
                ApiDataValidator.Validate(this.ExcludedMarkets, results);
            }

            if (this.AllowedMarkets != null && this.ExcludedMarkets != null)
            {
                results.Add(new ValidationResult($"Only one of {nameof(this.AllowedMarkets)} and {nameof(this.ExcludedMarkets)} must be specified."));
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(Markets other)
        {
            return (this.AllowedMarkets, this.ExcludedMarkets) ==
                   (other.AllowedMarkets, other.ExcludedMarkets);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Markets markets && this.Equals(markets);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return System.HashCode.Combine(this.AllowedMarkets, this.ExcludedMarkets);
        }
    }
}
