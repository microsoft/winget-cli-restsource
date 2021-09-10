// -----------------------------------------------------------------------
// <copyright file="AppsAndFeatures.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators.EnumValidators;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// AppsAndFeatures.
    /// </summary>
    public class AppsAndFeatures : IApiObject<AppsAndFeatures>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppsAndFeatures"/> class.
        /// </summary>
        public AppsAndFeatures()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppsAndFeatures"/> class.
        /// </summary>
        /// <param name="appsAndFeatures">AppsAndFeatures.</param>
        public AppsAndFeatures(AppsAndFeatures appsAndFeatures)
        {
            this.Update(appsAndFeatures);
        }

        /// <summary>
        /// Gets or sets DisplayName.
        /// </summary>
        [DisplayAndPublisherNameValidator]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets Publisher.
        /// </summary>
        [DisplayAndPublisherNameValidator]
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets DisplayVersion.
        /// </summary>
        [DisplayVersionValidator]
        public string DisplayVersion { get; set; }

        /// <summary>
        /// Gets or sets ProductCode.
        /// </summary>
        [ProductCodeValidator]
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets or sets UpgradeCode.
        /// </summary>
        [ProductCodeValidator]
        public string UpgradeCode { get; set; }

        /// <summary>
        /// Gets or sets InstallerType.
        /// </summary>
        [InstallerTypeValidator]
        public string InstallerType { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(AppsAndFeatures left, AppsAndFeatures right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(AppsAndFeatures left, AppsAndFeatures right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(AppsAndFeatures obj)
        {
            this.DisplayName = obj.DisplayName;
            this.Publisher = obj.Publisher;
            this.DisplayVersion = obj.DisplayVersion;
            this.ProductCode = obj.ProductCode;
            this.UpgradeCode = obj.UpgradeCode;
            this.InstallerType = obj.InstallerType;
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Return Results
            return new List<ValidationResult>();
        }

        /// <inheritdoc />
        public bool Equals(AppsAndFeatures other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.DisplayName, other.DisplayName)
                && Equals(this.Publisher, other.Publisher)
                && Equals(this.DisplayVersion, other.DisplayVersion)
                && Equals(this.ProductCode, other.ProductCode)
                && Equals(this.UpgradeCode, other.UpgradeCode)
                && Equals(this.InstallerType, other.InstallerType);
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

            return this.Equals((AppsAndFeatures)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.DisplayName != null ? this.DisplayName.GetHashCode() : 0;
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.Publisher != null ? this.Publisher.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.DisplayVersion != null ? this.DisplayVersion.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.ProductCode != null ? this.ProductCode.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.UpgradeCode != null ? this.UpgradeCode.GetHashCode() : 0);
                hashCode = (hashCode * ApiConstants.HashCodeConstant) ^ (this.InstallerType != null ? this.InstallerType.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
