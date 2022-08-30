// -----------------------------------------------------------------------
// <copyright file="ManifestSearchResponse.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Utils.Models.Objects;
    using Microsoft.WinGet.RestSource.Utils.Validators;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

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
                    AppsAndFeaturesEntryVersions appsAndFeaturesEntryVersions = new AppsAndFeaturesEntryVersions();
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

                        if (installer.AppsAndFeaturesEntries != null)
                        {
                            foreach (AppsAndFeatures appsAndFeatures in installer.AppsAndFeaturesEntries)
                            {
                                if (!string.IsNullOrEmpty(appsAndFeatures.DisplayVersion) && !appsAndFeaturesEntryVersions.Contains(appsAndFeatures.DisplayVersion))
                                {
                                    appsAndFeaturesEntryVersions.Add(appsAndFeatures.DisplayVersion);
                                }
                            }
                        }
                    }

                    SearchVersion searchVersion = new SearchVersion
                    {
                        PackageVersion = extended.PackageVersion,
                        Channel = extended.Channel,
                        PackageFamilyNames = packageFamilyNames.Count > 0 ? packageFamilyNames : null,
                        ProductCodes = productCodes.Count > 0 ? productCodes : null,
                        AppsAndFeaturesEntryVersions = appsAndFeaturesEntryVersions.Count > 0 ? appsAndFeaturesEntryVersions : null,
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
                // If exists in results update otherwise add
                ManifestSearchResponse match = list.FirstOrDefault(x => x.PackageIdentifier == response.PackageIdentifier);
                if (match != null)
                {
                    // Verify versions exists
                    if (match.Versions == null && response.Versions != null)
                    {
                        match.Versions = new SearchVersions();
                    }

                    match.Versions.Merge(response.Versions);
                }
                else
                {
                    // Add
                    if (response.Versions != null)
                    {
                        response.Versions.MakeDistinct();
                    }

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
            if (other is null)
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
            return obj is ManifestSearchResponse manifestSearchResponse && this.Equals(manifestSearchResponse);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(this.PackageIdentifier, this.PackageName, this.Publisher, this.Versions);
        }
    }
}
