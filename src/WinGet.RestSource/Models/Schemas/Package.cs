// -----------------------------------------------------------------------
// <copyright file="Package.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// PackageCore.
    /// </summary>
    public class Package : IApiObject<Package>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Package"/> class.
        /// </summary>
        public Package()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Package"/> class.
        /// </summary>
        /// <param name="package">PackageCore.</param>
        public Package(Package package)
        {
            this.Update(package);
        }

        /// <summary>
        /// Gets or sets PackageIdentifier.
        /// </summary>
        [PackageIdentifierValidator]
        public string PackageIdentifier { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(Package left, Package right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(Package left, Package right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(Package package)
        {
            this.PackageIdentifier = package.PackageIdentifier;
        }

        /// <inheritdoc/>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        /// <inheritdoc />
        public bool Equals(Package other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.PackageIdentifier, other.PackageIdentifier);
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

            return this.Equals((Package)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.PackageIdentifier != null ? this.PackageIdentifier.GetHashCode() : 0;
        }
    }
}
