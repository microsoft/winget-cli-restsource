// -----------------------------------------------------------------------
// <copyright file="InstallationMetadataFile.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// InstallationMetadataFile.
    /// </summary>
    public class InstallationMetadataFile : IApiObject<InstallationMetadataFile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationMetadataFile"/> class.
        /// </summary>
        public InstallationMetadataFile()
        {
        }

        /// <summary>
        /// Gets or sets RelativeFilePath.
        /// </summary>
        [InstallationMetadataFileRelativeFilePathValidator]
        public string RelativeFilePath { get; set; }

        /// <summary>
        /// Gets or sets FileSha256.
        /// </summary>
        [Sha256Validator]
        public string FileSha256 { get; set; }

        /// <summary>
        /// Gets or sets FileType.
        /// </summary>
        [FileTypeValidator]
        public string FileType { get; set; }

        /// <summary>
        /// Gets or sets InvocationParameter.
        /// </summary>
        [InvocationParameterValidator]
        public string InvocationParameter { get; set; }

        /// <summary>
        /// Gets or sets DisplayName.
        /// </summary>
        [DisplayNameValidator]
        public string DisplayName { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(InstallationMetadataFile left, InstallationMetadataFile right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(InstallationMetadataFile left, InstallationMetadataFile right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This updates the current InstallationMetadataFile to match the other InstallationMetadataFile.
        /// </summary>
        /// <param name="installationMetadataFile">Package Dependency.</param>
        public void Update(InstallationMetadataFile installationMetadataFile)
        {
            this.RelativeFilePath = installationMetadataFile.RelativeFilePath;
            this.FileSha256 = installationMetadataFile.FileSha256;
            this.FileType = installationMetadataFile.FileType;
            this.InvocationParameter = installationMetadataFile.InvocationParameter;
            this.DisplayName = installationMetadataFile.DisplayName;
        }

        /// <inheritdoc />
        public bool Equals(InstallationMetadataFile other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.RelativeFilePath, other.RelativeFilePath)
                && Equals(this.FileSha256, other.FileSha256)
                && Equals(this.FileType, other.FileType)
                && Equals(this.InvocationParameter, other.InvocationParameter)
                && Equals(this.DisplayName, other.DisplayName);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is InstallationMetadataFile installationMetadataFile && this.Equals(installationMetadataFile);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.RelativeFilePath, this.FileSha256, this.FileType, this.InvocationParameter, this.DisplayName).GetHashCode();
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
