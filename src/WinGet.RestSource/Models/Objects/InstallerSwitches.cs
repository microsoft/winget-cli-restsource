// -----------------------------------------------------------------------
// <copyright file="InstallerSwitches.cs" company="Microsoft Corporation">
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
        [SwitchValidator]
        public string Silent { get; set; }

        /// <summary>
        /// Gets or sets SilentWithProgress.
        /// </summary>
        [SwitchValidator]
        public string SilentWithProgress { get; set; }

        /// <summary>
        /// Gets or sets Interactive.
        /// </summary>
        [SwitchValidator]
        public string Interactive { get; set; }

        /// <summary>
        /// Gets or sets InstallLocation.
        /// </summary>
        [SwitchValidator]
        public string InstallLocation { get; set; }

        /// <summary>
        /// Gets or sets Log.
        /// </summary>
        [SwitchValidator]
        public string Log { get; set; }

        /// <summary>
        /// Gets or sets Upgrade.
        /// </summary>
        [SwitchValidator]
        public string Upgrade { get; set; }

        /// <summary>
        /// Gets or sets Custom.
        /// </summary>
        [CustomSwitchValidator]
        public string Custom { get; set; }

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
            return new List<ValidationResult>();
        }

        /// <inheritdoc />
        public bool Equals(InstallerSwitches other)
        {
            if (other is null)
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
            return obj is InstallerSwitches installerSwitches && this.Equals(installerSwitches);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return System.HashCode.Combine(this.Silent, this.SilentWithProgress, this.Interactive, this.InstallLocation, this.Log, this.Upgrade, this.Custom);
        }
    }
}
