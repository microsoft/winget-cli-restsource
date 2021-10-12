// -----------------------------------------------------------------------
// <copyright file="Locale.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
        /// Gets or sets ReleaseNotes.
        /// </summary>
        [ReleaseNotesValidator]
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// Gets or sets ReleaseNotesUrl.
        /// </summary>
        [UrlValidator]
        public string ReleaseNotesUrl { get; set; }

        /// <summary>
        /// Gets or sets Agreements.
        /// </summary>
        public Agreements Agreements { get; set; }

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
            this.ReleaseNotes = obj.ReleaseNotes;
            this.ReleaseNotesUrl = obj.ReleaseNotesUrl;
            this.Agreements = obj.Agreements;
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

            if (this.Agreements != null)
            {
                ApiDataValidator.Validate(this.Agreements, results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(Locale other)
        {
            if (other is null)
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
                   && Equals(this.Tags, other.Tags)
                   && Equals(this.ReleaseNotes, other.ReleaseNotes)
                   && Equals(this.ReleaseNotesUrl, other.ReleaseNotesUrl)
                   && Equals(this.Agreements, other.Agreements);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Locale locale && this.Equals(locale);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            System.HashCode hash = default;
            hash.Add(this.PackageLocale);
            hash.Add(this.Publisher);
            hash.Add(this.PublisherUrl);
            hash.Add(this.PublisherSupportUrl);
            hash.Add(this.PrivacyUrl);
            hash.Add(this.Author);
            hash.Add(this.PackageName);
            hash.Add(this.PackageUrl);
            hash.Add(this.License);
            hash.Add(this.LicenseUrl);
            hash.Add(this.Copyright);
            hash.Add(this.CopyrightUrl);
            hash.Add(this.ShortDescription);
            hash.Add(this.Description);
            hash.Add(this.Tags);
            hash.Add(this.ReleaseNotes);
            hash.Add(this.ReleaseNotesUrl);
            hash.Add(this.Agreements);
            return hash.ToHashCode();
        }
    }
}
