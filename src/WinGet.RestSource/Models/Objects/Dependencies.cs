// -----------------------------------------------------------------------
// <copyright file="Dependencies.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Dependencies.
    /// </summary>
    public class Dependencies : IApiObject<Dependencies>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dependencies"/> class.
        /// </summary>
        public Dependencies()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dependencies"/> class.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        public Dependencies(Dependencies dependencies)
        {
            this.Update(dependencies);
        }

        /// <summary>
        /// Gets or sets WindowsFeatures.
        /// </summary>
        public Arrays.Dependencies WindowsFeatures { get; set; }

        /// <summary>
        /// Gets or sets WindowsLibraries.
        /// </summary>
        public Arrays.Dependencies WindowsLibraries { get; set; }

        /// <summary>
        /// Gets or sets PackageDependencies.
        /// </summary>
        public Arrays.PackageDependency PackageDependencies { get; set; }

        /// <summary>
        /// Gets or sets ExternalDependencies.
        /// </summary>
        public Arrays.Dependencies ExternalDependencies { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(Dependencies left, Dependencies right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(Dependencies left, Dependencies right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(Dependencies obj)
        {
            this.WindowsFeatures = obj.WindowsFeatures;
            this.WindowsLibraries = obj.WindowsLibraries;
            this.PackageDependencies = obj.PackageDependencies;
            this.ExternalDependencies = obj.ExternalDependencies;
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            if (this.WindowsLibraries != null)
            {
                Validator.TryValidateObject(this.WindowsLibraries, new ValidationContext(this.WindowsLibraries, null, null), results);
            }

            if (this.WindowsFeatures != null)
            {
                Validator.TryValidateObject(this.WindowsFeatures, new ValidationContext(this.WindowsFeatures, null, null), results);
            }

            if (this.PackageDependencies != null)
            {
                Validator.TryValidateObject(this.PackageDependencies, new ValidationContext(this.PackageDependencies, null, null), results);
            }

            if (this.ExternalDependencies != null)
            {
                Validator.TryValidateObject(this.ExternalDependencies, new ValidationContext(this.ExternalDependencies, null, null), results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(Dependencies other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.WindowsFeatures, other.WindowsFeatures) && Equals(this.WindowsLibraries, other.WindowsLibraries) && Equals(this.PackageDependencies, other.PackageDependencies) && Equals(this.ExternalDependencies, other.ExternalDependencies);
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

            return this.Equals((Dependencies)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.WindowsFeatures != null ? this.WindowsFeatures.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this.WindowsLibraries != null ? this.WindowsLibraries.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.PackageDependencies != null ? this.PackageDependencies.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.ExternalDependencies != null ? this.ExternalDependencies.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
