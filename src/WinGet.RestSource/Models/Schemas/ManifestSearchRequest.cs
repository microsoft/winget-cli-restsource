﻿// -----------------------------------------------------------------------
// <copyright file="ManifestSearchRequest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
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
            if (ReferenceEquals(null, other))
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

            return this.Equals((ManifestSearchRequest)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.MaximumResults;
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ this.FetchAllManifests.GetHashCode();
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Query != null ? this.Query.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Inclusions != null ? this.Inclusions.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Filters != null ? this.Filters.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
