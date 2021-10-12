// -----------------------------------------------------------------------
// <copyright file="PackageDependency.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
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
        [PackageIdentifierValidator]
        public string PackageIdentifier { get; set; }

        /// <summary>
        /// Gets or sets MinimumVersion.
        /// </summary>
        [PackageVersionValidator]
        public string MinimumVersion { get; set; }

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
            return new List<ValidationResult>();
        }

        /// <inheritdoc />
        public bool Equals(PackageDependency other)
        {
            return (this.PackageIdentifier, this.MinimumVersion) ==
                   (other.PackageIdentifier, other.MinimumVersion);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is PackageDependency packageDependency && this.Equals(packageDependency);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return System.HashCode.Combine(this.PackageIdentifier, this.MinimumVersion);
        }
    }
}
