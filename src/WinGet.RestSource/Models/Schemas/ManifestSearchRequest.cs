// -----------------------------------------------------------------------
// <copyright file="ManifestSearchRequest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// ManifestSearchRequest.
    /// </summary>
    public class ManifestSearchRequest : IApiObject<ManifestSearchRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchRequest"/> class.
        /// </summary>
        public ManifestSearchRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchRequest"/> class.
        /// </summary>
        /// <param name="manifestSearchRequest">ManifestSearchRequest.</param>
        public ManifestSearchRequest(ManifestSearchRequest manifestSearchRequest)
        {
            this.Update(manifestSearchRequest);
        }

        /// <summary>
        /// Gets or sets MaximumResults.
        /// </summary>
        public int MaximumResults { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to FetchAllManifests.
        /// </summary>
        public bool FetchAllManifests { get; set; }

        /// <summary>
        /// Gets or sets Query.
        /// </summary>
        public Objects.SearchRequestMatch Query { get; set; }

        /// <summary>
        /// Gets or sets Query.
        /// </summary>
        public Arrays.SearchRequestPackageMatchFilter Inclusions { get; set; }

        /// <summary>
        /// Gets or sets Query.
        /// </summary>
        public Arrays.SearchRequestPackageMatchFilter Filters { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(ManifestSearchRequest left, ManifestSearchRequest right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(ManifestSearchRequest left, ManifestSearchRequest right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(ManifestSearchRequest obj)
        {
            this.MaximumResults = obj.MaximumResults;
            this.FetchAllManifests = obj.FetchAllManifests;
            this.Query = obj.Query;
            this.Inclusions = obj.Inclusions;
            this.Filters = obj.Filters;
        }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            // Verify Optional Fields
            if (this.Query != null)
            {
                ApiDataValidator.Validate(this.Query, results);
            }

            if (this.Inclusions != null)
            {
                ApiDataValidator.Validate(this.Inclusions, results);
            }

            if (this.Filters != null)
            {
                ApiDataValidator.Validate(this.Filters, results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(ManifestSearchRequest other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.MaximumResults, other.MaximumResults)
                   && Equals(this.FetchAllManifests, other.FetchAllManifests)
                   && Equals(this.Query, other.Query)
                   && Equals(this.Inclusions, other.Inclusions)
                   && Equals(this.Filters, other.Filters);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is ManifestSearchRequest manifestSearchRequest && this.Equals(manifestSearchRequest);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return System.HashCode.Combine(this.MaximumResults, this.FetchAllManifests, this.Query, this.Inclusions, this.Filters);
        }
    }
}
