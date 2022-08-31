// -----------------------------------------------------------------------
// <copyright file="SearchVersion.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

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
        /// Gets or sets appsAndFeaturesEntryVersions.
        /// </summary>
        public AppsAndFeaturesEntryVersions AppsAndFeaturesEntryVersions { get; set; }

        /// <summary>
        /// Gets or sets UpgradeCodes.
        /// </summary>
        public ProductCodes UpgradeCodes { get; set; }

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
            this.AppsAndFeaturesEntryVersions = obj.AppsAndFeaturesEntryVersions;
            this.UpgradeCodes = obj.UpgradeCodes;
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

                if (obj.AppsAndFeaturesEntryVersions != null)
                {
                    this.AppsAndFeaturesEntryVersions.AddRange(obj.AppsAndFeaturesEntryVersions);
                    this.AppsAndFeaturesEntryVersions.MakeDistinct();
                }

                if (obj.UpgradeCodes != null)
                {
                    this.UpgradeCodes.AddRange(obj.UpgradeCodes);
                    this.UpgradeCodes.MakeDistinct();
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

            if (this.AppsAndFeaturesEntryVersions != null)
            {
                ApiDataValidator.Validate(this.AppsAndFeaturesEntryVersions, results);
            }

            if (this.UpgradeCodes != null)
            {
                ApiDataValidator.Validate(this.UpgradeCodes, results);
            }

            return results;
        }

        /// <inheritdoc />
        public bool Equals(SearchVersion other)
        {
            return (this.PackageVersion, this.Channel, this.PackageFamilyNames, this.ProductCodes, this.AppsAndFeaturesEntryVersions, this.UpgradeCodes) ==
                   (other.PackageVersion, other.Channel, other.PackageFamilyNames, other.ProductCodes, other.AppsAndFeaturesEntryVersions, other.UpgradeCodes);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is SearchVersion searchVersion && this.Equals(searchVersion);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.PackageVersion, this.Channel, this.PackageFamilyNames, this.ProductCodes, this.AppsAndFeaturesEntryVersions, this.UpgradeCodes).GetHashCode();
        }
    }
}
