// -----------------------------------------------------------------------
// <copyright file="InstallationMetadata.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// InstallationMetadata.
    /// </summary>
    public class InstallationMetadata : IApiObject<InstallationMetadata>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationMetadata"/> class.
        /// </summary>
        public InstallationMetadata()
        {
        }

        /// <summary>
        /// Gets or sets DefaultInstallLocation.
        /// </summary>
        [DefaultInstallLocationValidator]
        public string DefaultInstallLocation { get; set; }

        /// <summary>
        /// Gets or sets InstallationMetadataFiles.
        /// </summary>
        public InstallationMetadataFiles InstallationMetadataFiles { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(InstallationMetadata left, InstallationMetadata right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(InstallationMetadata left, InstallationMetadata right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This updates the current InstallationMetadata to match the other InstallationMetadata.
        /// </summary>
        /// <param name="installationMetadata">Package Dependency.</param>
        public void Update(InstallationMetadata installationMetadata)
        {
            this.DefaultInstallLocation = installationMetadata.DefaultInstallLocation;
            this.InstallationMetadataFiles = installationMetadata.InstallationMetadataFiles;
        }

        /// <inheritdoc />
        public bool Equals(InstallationMetadata other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.DefaultInstallLocation, other.DefaultInstallLocation)
                && Equals(this.InstallationMetadataFiles, other.InstallationMetadataFiles);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is InstallationMetadata installationMetadata && this.Equals(installationMetadata);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.DefaultInstallLocation, this.InstallationMetadataFiles).GetHashCode();
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
