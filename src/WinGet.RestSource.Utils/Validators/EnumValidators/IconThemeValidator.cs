// -----------------------------------------------------------------------
// <copyright file="IconThemeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// IconThemeValidator.
    /// </summary>
    public class IconThemeValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "default",
            "light",
            "dark",
            "highContrast",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="IconThemeValidator"/> class.
        /// </summary>
        public IconThemeValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
