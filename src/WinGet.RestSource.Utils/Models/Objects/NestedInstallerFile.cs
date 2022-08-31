// -----------------------------------------------------------------------
// <copyright file="NestedInstallerFile.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// NestedInstallerFile.
    /// </summary>
    public class NestedInstallerFile : IApiObject<NestedInstallerFile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NestedInstallerFile"/> class.
        /// </summary>
        public NestedInstallerFile()
        {
        }

        /// <summary>
        /// Gets or sets RelativeFilePath.
        /// </summary>
        [NestedInstallerFileRelativeFilePathValidator]
        public string RelativeFilePath { get; set; }

        /// <summary>
        /// Gets or sets PortableCommandAlias.
        /// </summary>
        [PortableCommandAliasValidator]
        public string PortableCommandAlias { get; set; }

        /// <summary>
        /// Operator==.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator ==(NestedInstallerFile left, NestedInstallerFile right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator!=.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <returns>Bool.</returns>
        public static bool operator !=(NestedInstallerFile left, NestedInstallerFile right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// This updates the current NestedInstallerFile to match the other NestedInstallerFile.
        /// </summary>
        /// <param name="nestedInstallerFile">Package Dependency.</param>
        public void Update(NestedInstallerFile nestedInstallerFile)
        {
            this.RelativeFilePath = nestedInstallerFile.RelativeFilePath;
            this.PortableCommandAlias = nestedInstallerFile.PortableCommandAlias;
        }

        /// <inheritdoc />
        public bool Equals(NestedInstallerFile other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.RelativeFilePath, other.RelativeFilePath) && Equals(this.PortableCommandAlias, other.PortableCommandAlias);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is NestedInstallerFile nestedInstallerFile && this.Equals(nestedInstallerFile);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (this.RelativeFilePath, this.PortableCommandAlias).GetHashCode();
        }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
