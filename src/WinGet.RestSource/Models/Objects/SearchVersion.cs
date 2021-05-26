// -----------------------------------------------------------------------
// <copyright file="SearchVersion.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// SearchVersion.
    /// </summary>
    public class SearchVersion : IApiObject<SearchVersion>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchVersion"/> class.
        /// </summary>
        public SearchVersion()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchVersion"/> class.
        /// </summary>
        /// <param name="searchVersion">SearchVersion.</param>
        public SearchVersion(SearchVersion searchVersion)
        {
            this.Update(searchVersion);
        }

        /// <summary>
        /// Gets or sets PackageVersion.
        /// </summary>
        [PackageVersionValidator]
        public string PackageVersion { get; set; }

        /// <summary>
        /// Gets or sets Channel.
        /// </summary>
        [ChannelValidator]
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets PackageFamilyNames.
        /// </summary>
        public PackageFamilyNames PackageFamilyNames { get; set; }

        /// <summary>
        /// Gets or sets ProductCodes.
        /// </summary>
        public ProductCodes ProductCodes { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(SearchVersion left, SearchVersion right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(SearchVersion left, SearchVersion right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(SearchVersion obj)
        {
            this.PackageVersion = obj.PackageVersion;
            this.Channel = obj.Channel;
            this.PackageFamilyNames = obj.PackageFamilyNames;
            this.ProductCodes = obj.ProductCodes;
        }

        /// <summary>
        /// This Merges a search version if package version and channel match.
        /// </summary>
        /// <param name="obj">Search Version.</param>
        public void Merge(SearchVersion obj)
        {
            if (this.PackageVersion == obj.PackageVersion && this.Channel == obj.Channel)
            {
                if (obj.PackageFamilyNames != null)
                {
                    this.PackageFamilyNames.AddRange(obj.PackageFamilyNames);
                    this.PackageFamilyNames.MakeDistinct();
                }

                if (obj.ProductCodes != null)
                {
                    this.ProductCodes.AddRange(obj.ProductCodes);
                    this.ProductCodes.MakeDistinct();
                }
            }
        }

        /// <summary>
        /// ConsolidationExpression Expression to be used for merging.
        /// </summary>
        /// <param name="x">Search Version.</param>
        /// <returns>Bool.</returns>
        public bool ConsolidationExpression(SearchVersion x) => this.PackageVersion.Equals(x.PackageVersion) && this.Channel.Equals(x.Channel);

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            // Optional Objects
            if (this.PackageFamilyNames != null)
            {
                ApiDataValidator.Validate(this.PackageFamilyNames, results);
            }

            if (this.ProductCodes != null)
            {
                ApiDataValidator.Validate(this.ProductCodes, results);
            }

            return results;
        }

        /// <inheritdoc />
        public bool Equals(SearchVersion other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.PackageVersion, other.PackageVersion) && Equals(this.Channel, other.Channel);
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

            return this.Equals((SearchVersion)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.PackageVersion != null ? this.PackageVersion.GetHashCode() : 0;
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Channel != null ? this.Channel.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.PackageFamilyNames != null ? this.PackageFamilyNames.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.ProductCodes != null ? this.ProductCodes.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
