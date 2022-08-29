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
    public class InstallerTypeValidator : NestedInstallerTypeValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "zip",
            "pwa",
            "msstore",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerTypeValidator"/> class.
        /// </summary>
        public InstallerTypeValidator()
            : base()
        {
            this.Values.AddRange(this.enumList);
        }
    }
}
