// -----------------------------------------------------------------------
// <copyright file="FileTypeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// InstallationMetadataFileFileTypeValidator.
    /// </summary>
    public class FileTypeValidator : NestedInstallerTypeValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "launch",
            "uninstall",
            "other",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTypeValidator"/> class.
        /// </summary>
        public FileTypeValidator()
            : base()
        {
            this.Values.AddRange(this.enumList);
        }
    }
}
