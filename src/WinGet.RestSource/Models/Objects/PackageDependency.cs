// -----------------------------------------------------------------------
// <copyright file="PackageDependency.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Models.Strings;

    /// <summary>
    /// PackageDependency.
    /// </summary>
    public class PackageDependency : IApiObject<PackageDependency>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageDependency"/> class.
        /// </summary>
        public PackageDependency()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageDependency"/> class.
        /// </summary>
        /// <param name="packageDependency">PackageDependency.</param>
        public PackageDependency(PackageDependency packageDependency)
        {
            this.Update(packageDependency);
        }

        /// <summary>
        /// Gets or sets PackageIdentifier.
        /// </summary>
        public PackageIdentifier PackageIdentifier { get; set; }

        /// <summary>
        /// Gets or sets MinimumVersion.
        /// </summary>
        public PackageVersion MinimumVersion { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(PackageDependency left, PackageDependency right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(PackageDependency left, PackageDependency right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This updates the current package core to match another public core.
        /// </summary>
        /// <param name="packageDependency">Package Dependency.</param>
        public void Update(PackageDependency packageDependency)
        {
            this.PackageIdentifier = packageDependency.PackageIdentifier;
            this.MinimumVersion = packageDependency.MinimumVersion;
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            // Validate Required Members
            Validator.TryValidateObject(this.PackageIdentifier, new ValidationContext(this.PackageIdentifier, null, null), results);

            // Validate Optional Members
            if (this.MinimumVersion != null)
            {
                Validator.TryValidateObject(this.MinimumVersion, new ValidationContext(this.MinimumVersion, null, null), results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(PackageDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.PackageIdentifier, other.PackageIdentifier) && Equals(this.MinimumVersion, other.MinimumVersion);
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

            return this.Equals((PackageDependency)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.PackageIdentifier != null ? this.PackageIdentifier.GetHashCode() : 0) * 397) ^ (this.MinimumVersion != null ? this.MinimumVersion.GetHashCode() : 0);
            }
        }
    }
}
