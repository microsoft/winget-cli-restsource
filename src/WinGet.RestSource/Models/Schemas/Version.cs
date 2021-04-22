// -----------------------------------------------------------------------
// <copyright file="Version.cs" company="Microsoft Corporation">
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
    /// This is the core version model.
    /// </summary>
    public class Version : IApiObject<Version>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        public Version()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        /// <param name="version">Version Core Model.</param>
        public Version(Version version)
        {
            this.Update(version);
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
        /// Gets or sets DefaultLocale.
        /// </summary>
        public DefaultLocale DefaultLocale { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(Version left, Version right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(Version left, Version right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(Version obj)
        {
            this.PackageVersion = obj.PackageVersion;
            this.Channel = obj.Channel;
            this.DefaultLocale = obj.DefaultLocale;
        }

        /// <inheritdoc />
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            // Verify Required Fields
            if (this.DefaultLocale == null)
            {
                results.Add(new ValidationResult($"{nameof(this.DefaultLocale)} in {validationContext.ObjectType} must not be null."));
            }
            else
            {
                ApiDataValidator.Validate(this.DefaultLocale, results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(Version other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.PackageVersion, other.PackageVersion)
                   && Equals(this.Channel, other.Channel)
                   && Equals(this.DefaultLocale, other.DefaultLocale);
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

            return this.Equals((Version)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.PackageVersion != null ? this.PackageVersion.GetHashCode() : 0;
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Channel != null ? this.Channel.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.DefaultLocale != null ? this.DefaultLocale.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
