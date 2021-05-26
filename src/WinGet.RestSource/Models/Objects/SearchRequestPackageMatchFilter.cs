// -----------------------------------------------------------------------
// <copyright file="SearchRequestPackageMatchFilter.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators;
    using Microsoft.WinGet.RestSource.Validators.EnumValidators;

    /// <summary>
    /// SearchRequestPackageMatchFilter.
    /// </summary>
    public class SearchRequestPackageMatchFilter : IApiObject<SearchRequestPackageMatchFilter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequestPackageMatchFilter"/> class.
        /// </summary>
        public SearchRequestPackageMatchFilter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequestPackageMatchFilter"/> class.
        /// </summary>
        /// <param name="searchRequestPackageMatchFilter">searchRequestPackageMatchFilter.</param>
        public SearchRequestPackageMatchFilter(SearchRequestPackageMatchFilter searchRequestPackageMatchFilter)
        {
            this.Update(searchRequestPackageMatchFilter);
        }

        /// <summary>
        /// Gets or sets PackageMatchField.
        /// </summary>
        [PackageMatchFieldValidator]
        public string PackageMatchField { get; set; }

        /// <summary>
        /// Gets or sets SearchRequestMatch.
        /// </summary>
        public SearchRequestMatch RequestMatch { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(SearchRequestPackageMatchFilter left, SearchRequestPackageMatchFilter right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(SearchRequestPackageMatchFilter left, SearchRequestPackageMatchFilter right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This updates the current package core to match another public core.
        /// </summary>
        /// <param name="searchRequestPackageMatchFilter">Package Dependency.</param>
        public void Update(SearchRequestPackageMatchFilter searchRequestPackageMatchFilter)
        {
            this.RequestMatch = searchRequestPackageMatchFilter.RequestMatch;
            this.PackageMatchField = searchRequestPackageMatchFilter.PackageMatchField;
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            // Validate Required Members
            if (this.RequestMatch == null)
            {
                results.Add(new ValidationResult($"{nameof(this.RequestMatch)} in {validationContext.ObjectType} must not be null."));
            }
            else
            {
                ApiDataValidator.Validate(this.RequestMatch, results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(SearchRequestPackageMatchFilter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.RequestMatch, other.RequestMatch) && Equals(this.PackageMatchField, other.PackageMatchField);
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

            return this.Equals((SearchRequestPackageMatchFilter)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.RequestMatch != null ? this.RequestMatch.GetHashCode() : 0) * ApiConstants.HashCodeConstant) ^ (this.PackageMatchField != null ? this.PackageMatchField.GetHashCode() : 0);
            }
        }
    }
}
