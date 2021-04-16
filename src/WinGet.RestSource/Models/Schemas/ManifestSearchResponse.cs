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
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="channel">Channel.</param>
        /// <param name="packageName">Package Name.</param>
        /// <param name="publisher">Publisher.</param>
        /// <param name="packageFamilyName">Package Family Name.</param>
        /// <param name="productCode">Product Code.</param>
        public ManifestSearchResponse(
            string packageIdentifier = null,
            string packageVersion = null,
            string channel = null,
            string packageName = null,
            string publisher = null,
            string packageFamilyName = null,
            string productCode = null)
        {
            this.PackageIdentifier = packageIdentifier;
            this.PackageName = packageName;
            this.Publisher = publisher;
            this.PackageFamilyName = packageFamilyName;
            this.ProductCode = productCode;
            this.Versions = new SearchVersions()
            {
                new SearchVersion()
                {
                    PackageVersion = packageVersion,
                    Channel = channel,
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
        /// Gets or sets PackageFamilyName.
        /// </summary>
        [PackageFamilyNameValidator]
        public string PackageFamilyName { get; set; }

        /// <summary>
        /// Gets or sets ProductCode.
        /// </summary>
        [ProductCodeValidator]
        public string ProductCode { get; set; }

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
                response.AddRange(GetSearchVersions(manifest.PackageIdentifier, extended));
            }

            return response;
        }

        /// <summary>
        /// Creates a new set of Manifest Search Response.
        /// </summary>
        /// <param name="packageIdentifier">Package identifier.</param>
        /// <param name="extended">Version Extended.</param>
        /// <returns>Manifest Search Responses.</returns>
        public static List<ManifestSearchResponse> GetSearchVersions(string packageIdentifier, VersionExtended extended = null)
        {
            List<ManifestSearchResponse> response = new List<ManifestSearchResponse>();
            if (extended == null)
            {
                response.Add(new ManifestSearchResponse(packageIdentifier));
                return response;
            }

            if (extended.Installers == null)
            {
                response.Add(new ManifestSearchResponse(
                    packageIdentifier,
                    extended.PackageVersion,
                    extended.Channel,
                    extended.DefaultLocale.PackageName,
                    extended.DefaultLocale.Publisher));
                return response;
            }

            foreach (var installer in extended.Installers)
            {
                response.Add(new ManifestSearchResponse(
                    packageIdentifier,
                    extended.PackageVersion,
                    extended.Channel,
                    extended.DefaultLocale.PackageName,
                    extended.DefaultLocale.Publisher,
                    installer.PackageFamilyName,
                    installer.ProductCode));
            }

            return response;
        }

        /// <summary>
        /// Consolidates Manifest Search Responses.
        /// </summary>
        /// <param name="manifests">manifests.</param>
        /// <returns>Manifest Search Responses.</returns>
        public static List<ManifestSearchResponse> Consolidate(List<ManifestSearchResponse> manifests)
        {
            List<ManifestSearchResponse> list = new List<ManifestSearchResponse>();
            if (manifests == null)
            {
                return list;
            }

            foreach (ManifestSearchResponse response in manifests)
            {
                // Create Consolidation Predicate.
                Predicate<ManifestSearchResponse> consolidationExpression = x =>
                    Equals(response.PackageIdentifier, x.PackageIdentifier)
                    && Equals(response.PackageName, x.PackageName)
                    && Equals(response.Publisher, x.Publisher)
                    && Equals(response.PackageFamilyName, x.PackageFamilyName)
                    && Equals(response.ProductCode, x.ProductCode);

                // If exists in results update otherwise add
                if (list.Exists(consolidationExpression))
                {
                    // Get index
                    int index = list.FindIndex(consolidationExpression);

                    // Verify versions exists
                    if (list[index].Versions == null)
                    {
                        list[index].Versions = new SearchVersions();
                    }

                    // If response versions exists - add.
                    if (response.Versions != null)
                    {
                        foreach (SearchVersion searchVersion in response.Versions)
                        {
                            if (!list[index].Versions.Contains(searchVersion))
                            {
                                list[index].Versions.Add(searchVersion);
                            }
                        }
                    }
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
            this.PackageFamilyName = obj.PackageFamilyName;
            this.ProductCode = obj.ProductCode;
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
                   Equals(this.PackageFamilyName, other.PackageFamilyName) &&
                   Equals(this.ProductCode, other.ProductCode) &&
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
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.PackageFamilyName != null ? this.PackageFamilyName.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.ProductCode != null ? this.ProductCode.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Versions != null ? this.Versions.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
