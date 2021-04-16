// -----------------------------------------------------------------------
// <copyright file="Locale.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// Locale.
    /// </summary>
    public class Locale : IApiObject<Locale>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Locale"/> class.
        /// </summary>
        public Locale()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Locale"/> class.
        /// </summary>
        /// <param name="locale">Locale.</param>
        public Locale(Locale locale)
        {
            this.Update(locale);
        }

        /// <summary>
        /// Gets or sets PackageLocale.
        /// </summary>
        [LocaleValidator]
        public string PackageLocale { get; set; }

        /// <summary>
        /// Gets or sets Publisher.
        /// </summary>
        [PublisherValidator]
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets PublisherUrl.
        /// </summary>
        [UrlValidator]
        public string PublisherUrl { get; set; }

        /// <summary>
        /// Gets or sets PublisherSupportUrl.
        /// </summary>
        [UrlValidator]
        public string PublisherSupportUrl { get; set; }

        /// <summary>
        /// Gets or sets PrivacyUrl.
        /// </summary>
        [UrlValidator]
        public string PrivacyUrl { get; set; }

        /// <summary>
        /// Gets or sets Author.
        /// </summary>
        [AuthorValidator]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets PackageName.
        /// </summary>
        [PackageNameValidator]
        public string PackageName { get; set; }

        /// <summary>
        /// Gets or sets PackageUrl.
        /// </summary>
        [UrlValidator]
        public string PackageUrl { get; set; }

        /// <summary>
        /// Gets or sets License.
        /// </summary>
        [LicenseValidator]
        public string License { get; set; }

        /// <summary>
        /// Gets or sets LicenseUrl.
        /// </summary>
        [UrlValidator]
        public string LicenseUrl { get; set; }

        /// <summary>
        /// Gets or sets Copyright.
        /// </summary>
        [CopyrightValidator]
        public string Copyright { get; set; }

        /// <summary>
        /// Gets or sets CopyrightUrl.
        /// </summary>
        [UrlValidator]
        public string CopyrightUrl { get; set; }

        /// <summary>
        /// Gets or sets ShortDescription.
        /// </summary>
        [ShortDescriptionValidator]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        [DescriptionValidator]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Tags.
        /// </summary>
        public Tags Tags { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(Locale left, Locale right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(Locale left, Locale right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(Locale obj)
        {
            this.PackageLocale = obj.PackageLocale;
            this.Publisher = obj.Publisher;
            this.PublisherUrl = obj.PublisherUrl;
            this.PublisherSupportUrl = obj.PublisherSupportUrl;
            this.PrivacyUrl = obj.PrivacyUrl;
            this.Author = obj.Author;
            this.PackageName = obj.PackageName;
            this.PackageUrl = obj.PackageUrl;
            this.License = obj.License;
            this.LicenseUrl = obj.LicenseUrl;
            this.Copyright = obj.Copyright;
            this.CopyrightUrl = obj.CopyrightUrl;
            this.ShortDescription = obj.ShortDescription;
            this.Description = obj.Description;
            this.Tags = obj.Tags;
        }

        /// <inheritdoc />
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            if (this.Tags != null)
            {
                ApiDataValidator.Validate(this.Tags, results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(Locale other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.PackageLocale, other.PackageLocale)
                   && Equals(this.Publisher, other.Publisher)
                   && Equals(this.PublisherUrl, other.PublisherUrl)
                   && Equals(this.PublisherSupportUrl, other.PublisherSupportUrl)
                   && Equals(this.PrivacyUrl, other.PrivacyUrl)
                   && Equals(this.Author, other.Author)
                   && Equals(this.PackageName, other.PackageName)
                   && Equals(this.PackageUrl, other.PackageUrl)
                   && Equals(this.License, other.License)
                   && Equals(this.LicenseUrl, other.LicenseUrl)
                   && Equals(this.Copyright, other.Copyright)
                   && Equals(this.CopyrightUrl, other.CopyrightUrl)
                   && Equals(this.ShortDescription, other.ShortDescription)
                   && Equals(this.Description, other.Description)
                   && Equals(this.Tags, other.Tags);
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

            return this.Equals((Locale)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.PackageLocale != null ? this.PackageLocale.GetHashCode() : 0;
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Publisher != null ? this.Publisher.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.PublisherUrl != null ? this.PublisherUrl.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.PublisherSupportUrl != null ? this.PublisherSupportUrl.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.PrivacyUrl != null ? this.PrivacyUrl.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Author != null ? this.Author.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.PackageName != null ? this.PackageName.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.PackageUrl != null ? this.PackageUrl.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.License != null ? this.License.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.LicenseUrl != null ? this.LicenseUrl.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Copyright != null ? this.Copyright.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.CopyrightUrl != null ? this.CopyrightUrl.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.ShortDescription != null ? this.ShortDescription.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Description != null ? this.Description.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Tags != null ? this.Tags.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
