// -----------------------------------------------------------------------
// <copyright file="SearchVersion.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Core;
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
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            return new List<ValidationResult>();
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
                return hashCode;
            }
        }
    }
}
