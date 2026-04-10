// -----------------------------------------------------------------------
// <copyright file="SearchRequestPackageMatchFilter.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators;
    using Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators;

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
            return (this.RequestMatch, this.PackageMatchField) ==
                   (other.RequestMatch, other.PackageMatchField);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is SearchRequestPackageMatchFilter searchRequestPackageMatchFilter && this.Equals(searchRequestPackageMatchFilter);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.RequestMatch, this.PackageMatchField).GetHashCode();
        }
    }
}
