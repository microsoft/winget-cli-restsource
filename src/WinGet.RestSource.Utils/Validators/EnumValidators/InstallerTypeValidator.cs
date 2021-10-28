// -----------------------------------------------------------------------
// <copyright file="InstallerTypeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// InstallerTypeValidator.
    /// </summary>
    public class InstallerTypeValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "msix",
            "msi",
            "appx",
            "exe",
            "zip",
            "inno",
            "nullsoft",
            "wix",
            "burn",
            "pwa",
            "msstore",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerTypeValidator"/> class.
        /// </summary>
        public InstallerTypeValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
