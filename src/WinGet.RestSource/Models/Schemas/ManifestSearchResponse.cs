// -----------------------------------------------------------------------
// <copyright file="ManifestSearchResponse.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Models.Objects;
    using Microsoft.WinGet.RestSource.Validators;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// Manifest Search Response.
    /// </summary>
    public class ManifestSearchResponse : IApiObject<ManifestSearchResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchResponse"/> class.
        /// </summary>
        public ManifestSearchResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchResponse"/> class.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageName">Package Name.</param>
        /// <param name="publisher">Publisher.</param>
        /// <param name="searchVersions">Search Versions.</param>
        public ManifestSearchResponse(
            string packageIdentifier = null,
            string packageName = null,
            string publisher = null,
            SearchVersions searchVersions = null)
        {
            this.PackageIdentifier = packageIdentifier;
            this.PackageName = packageName;
            this.Publisher = publisher;
            this.Versions = searchVersions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchResponse"/> class.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageName">Package Name.</param>
        /// <param name="publisher">Publisher.</param>
        /// <param name="searchVersion">Search Version.</param>
        public ManifestSearchResponse(
            string packageIdentifier = null,
            string packageName = null,
            string publisher = null,
            SearchVersion searchVersion = null)
        {
            this.PackageIdentifier = packageIdentifier;
            this.PackageName = packageName;
            this.Publisher = publisher;
            this.Versions = new SearchVersions()
            {
                searchVersion,
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchResponse"/> class.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        public ManifestSearchResponse(
            string packageIdentifier = null)
        {
            this.PackageIdentifier = packageIdentifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchResponse"/> class.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageName">Package Name.</param>
        /// <param name="publisher">Publisher.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="channel">Channel.</param>
        /// <param name="packageFamilyName">Package Family Name.</param>
        /// <param name="productCode">Product Code.</param>
        public ManifestSearchResponse(
            string packageIdentifier = null,
            string packageName = null,
            string publisher = null,
            string packageVersion = null,
            string channel = null,
            string packageFamilyName = null,
            string productCode = null)
        {
            PackageFamilyNames pfn = null;
            if (!string.IsNullOrEmpty(packageFamilyName))
            {
                pfn = new PackageFamilyNames
                {
                    packageName,
                };
            }

            ProductCodes pc = null;
            if (!string.IsNullOrEmpty(productCode))
            {
                pc = new ProductCodes()
                {
                    productCode,
                };
            }

            this.PackageIdentifier = packageIdentifier;
            this.PackageName = packageName;
            this.Publisher = publisher;
            this.Versions = new SearchVersions()
            {
                new SearchVersion()
                {
                    PackageVersion = packageVersion,
                    Channel = channel,
                    PackageFamilyNames = pfn,
                    ProductCodes = pc,
                },
            };
        }

        /// <summary>
        /// Gets or sets PackageIdentifier.
        /// </summary>
        [PackageIdentifierValidator]
        public string PackageIdentifier { get; set; }

        /// <summary>
        /// Gets or sets PackageName.
        /// </summary>
        [PackageNameValidator]
        public string PackageName { get; set; }

        /// <summary>
        /// Gets or sets Publisher.
        /// </summary>
        [PublisherValidator]
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets SearchVersions.
        /// </summary>
        public SearchVersions Versions { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(ManifestSearchResponse left, ManifestSearchResponse right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(ManifestSearchResponse left, ManifestSearchResponse right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This gets.
        /// </summary>
        /// <param name="manifest">Package Manifest.</param>
        /// <returns>Manifest Search Responses.</returns>
        public static List<ManifestSearchResponse> GetSearchVersions(PackageManifest manifest)
        {
            List<ManifestSearchResponse> response = new List<ManifestSearchResponse>();
            if (manifest == null)
            {
                return response;
            }

            if (manifest.Versions == null)
            {
                response.Add(new ManifestSearchResponse(manifest.PackageIdentifier));
                return response;
            }

            foreach (VersionExtended extended in manifest.Versions)
            {
                if (extended.Installers == null)
                {
                    response.Add(new ManifestSearchResponse(
                        packageIdentifier: manifest.PackageIdentifier,
                        packageName: extended.DefaultLocale.PackageName,
                        publisher: extended.DefaultLocale.Publisher,
                        packageVersion: extended.PackageVersion,
                        channel: extended.Channel));
                }
                else
                {
                    PackageFamilyNames packageFamilyNames = new PackageFamilyNames();
                    ProductCodes productCodes = new ProductCodes();
                    foreach (Installer installer in extended.Installers)
                    {
                        if (!string.IsNullOrEmpty(installer.PackageFamilyName) && !packageFamilyNames.Contains(installer.PackageFamilyName))
                        {
                            packageFamilyNames.Add(installer.PackageFamilyName);
                        }

                        if (!string.IsNullOrEmpty(installer.ProductCode) && !productCodes.Contains(installer.ProductCode))
                        {
                            productCodes.Add(installer.ProductCode);
                        }
                    }

                    SearchVersion searchVersion = new SearchVersion
                    {
                        PackageVersion = extended.PackageVersion,
                        Channel = extended.Channel,
                        PackageFamilyNames = packageFamilyNames.Count > 0 ? packageFamilyNames : null,
                        ProductCodes = productCodes.Count > 0 ? productCodes : null,
                    };

                    response.Add(new ManifestSearchResponse(
                        packageIdentifier: manifest.PackageIdentifier,
                        packageName: extended.DefaultLocale.PackageName,
                        publisher: extended.DefaultLocale.Publisher,
                        searchVersion: searchVersion));
                }
            }

            return response;
        }

        /// <summary>
        /// Consolidates Manifest Search Responses.
        /// </summary>
        /// <param name="manifestSearchResponses">manifests.</param>
        /// <returns>Manifest Search Responses.</returns>
        public static List<ManifestSearchResponse> Consolidate(List<ManifestSearchResponse> manifestSearchResponses)
        {
            List<ManifestSearchResponse> list = new List<ManifestSearchResponse>();
            if (manifestSearchResponses == null)
            {
                return list;
            }

            foreach (ManifestSearchResponse response in manifestSearchResponses)
            {
                // Create Consolidation Predicate for search Response..
                bool MSRConsolidationExpression(ManifestSearchResponse x) =>
                    Equals(response.PackageIdentifier, x.PackageIdentifier)
                    && Equals(response.PackageName, x.PackageName)
                    && Equals(response.Publisher, x.Publisher);

                // If exists in results update otherwise add
                if (list.Exists(MSRConsolidationExpression))
                {
                    // Get index
                    int searchResponseIndex = list.FindIndex(MSRConsolidationExpression);

                    // Verify versions exists
                    if (list[searchResponseIndex].Versions == null && response.Versions != null)
                    {
                        list[searchResponseIndex].Versions = new SearchVersions();
                    }

                    list[searchResponseIndex].Versions.Merge(response.Versions);
                }
                else
                {
                    // Add
                    response.Versions.MakeDistinct();
                    list.Add(response);
                }
            }

            return list;
        }

        /// <inheritdoc />
        public void Update(ManifestSearchResponse obj)
        {
            this.PackageIdentifier = obj.PackageIdentifier;
            this.PackageName = obj.PackageName;
            this.Publisher = obj.Publisher;
            this.Versions = obj.Versions;
        }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            // Verify Optional Fields
            if (this.Versions != null)
            {
                ApiDataValidator.Validate(this.Versions, results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(ManifestSearchResponse other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.PackageIdentifier, other.PackageIdentifier) &&
                   Equals(this.PackageName, other.PackageName) &&
                   Equals(this.Publisher, other.Publisher) &&
                   Equals(this.Versions, other.Versions);
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

            return this.Equals((ManifestSearchResponse)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.PackageIdentifier != null ? this.PackageIdentifier.GetHashCode() : 0;
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.PackageName != null ? this.PackageName.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Publisher != null ? this.Publisher.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Versions != null ? this.Versions.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
