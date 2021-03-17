// -----------------------------------------------------------------------
// <copyright file="InstallerSwitches.cs" company="Microsoft Corporation">
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
    /// InstallerSwitches.
    /// </summary>
    public class InstallerSwitches : IApiObject<InstallerSwitches>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerSwitches"/> class.
        /// </summary>
        public InstallerSwitches()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerSwitches"/> class.
        /// </summary>
        /// <param name="installerSwitches">InstallerSwitches.</param>
        public InstallerSwitches(InstallerSwitches installerSwitches)
        {
            this.Update(installerSwitches);
        }

        /// <summary>
        /// Gets or sets Silent.
        /// </summary>
        public Switch Silent { get; set; }

        /// <summary>
        /// Gets or sets SilentWithProgress.
        /// </summary>
        public Switch SilentWithProgress { get; set; }

        /// <summary>
        /// Gets or sets Interactive.
        /// </summary>
        public Switch Interactive { get; set; }

        /// <summary>
        /// Gets or sets InstallLocation.
        /// </summary>
        public Switch InstallLocation { get; set; }

        /// <summary>
        /// Gets or sets Log.
        /// </summary>
        public Switch Log { get; set; }

        /// <summary>
        /// Gets or sets Upgrade.
        /// </summary>
        public Switch Upgrade { get; set; }

        /// <summary>
        /// Gets or sets Custom.
        /// </summary>
        public Switch Custom { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(InstallerSwitches left, InstallerSwitches right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(InstallerSwitches left, InstallerSwitches right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public void Update(InstallerSwitches obj)
        {
            this.Silent = obj.Silent;
            this.SilentWithProgress = obj.SilentWithProgress;
            this.Interactive = obj.Interactive;
            this.InstallLocation = obj.InstallLocation;
            this.Log = obj.Log;
            this.Upgrade = obj.Upgrade;
            this.Custom = obj.Custom;
        }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Create Validation Results
            var results = new List<ValidationResult>();

            // Validate Optional Members
            if (this.Silent != null)
            {
                Validator.TryValidateObject(this.Silent, new ValidationContext(this.Silent, null, null), results);
            }

            if (this.SilentWithProgress != null)
            {
                Validator.TryValidateObject(this.SilentWithProgress, new ValidationContext(this.SilentWithProgress, null, null), results);
            }

            if (this.Interactive != null)
            {
                Validator.TryValidateObject(this.Interactive, new ValidationContext(this.Interactive, null, null), results);
            }

            if (this.InstallLocation != null)
            {
                Validator.TryValidateObject(this.InstallLocation, new ValidationContext(this.InstallLocation, null, null), results);
            }

            if (this.Log != null)
            {
                Validator.TryValidateObject(this.Log, new ValidationContext(this.Log, null, null), results);
            }

            if (this.Upgrade != null)
            {
                Validator.TryValidateObject(this.Upgrade, new ValidationContext(this.Upgrade, null, null), results);
            }

            if (this.Custom != null)
            {
                Validator.TryValidateObject(this.Custom, new ValidationContext(this.Custom, null, null), results);
            }

            // Return Results
            return results;
        }

        /// <inheritdoc />
        public bool Equals(InstallerSwitches other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.Silent, other.Silent) &&
                   Equals(this.SilentWithProgress, other.SilentWithProgress) &&
                   Equals(this.Interactive, other.Interactive) &&
                   Equals(this.InstallLocation, other.InstallLocation) &&
                   Equals(this.Log, other.Log) &&
                   Equals(this.Upgrade, other.Upgrade) &&
                   Equals(this.Custom, other.Custom);
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

            return this.Equals((InstallerSwitches)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.Silent != null ? this.Silent.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this.SilentWithProgress != null ? this.SilentWithProgress.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Interactive != null ? this.Interactive.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.InstallLocation != null ? this.InstallLocation.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Log != null ? this.Log.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Upgrade != null ? this.Upgrade.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Custom != null ? this.Custom.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
