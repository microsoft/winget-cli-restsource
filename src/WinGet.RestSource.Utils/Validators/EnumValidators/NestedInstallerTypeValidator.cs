// -----------------------------------------------------------------------
// <copyright file="NestedInstallerTypeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// NestedInstallerType.
    /// </summary>
    public class NestedInstallerTypeValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "msix",
            "msi",
            "appx",
            "exe",
            "inno",
            "nullsoft",
            "wix",
            "burn",
            "portable",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedInstallerTypeValidator"/> class.
        /// </summary>
        public NestedInstallerTypeValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
