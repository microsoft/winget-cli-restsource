// -----------------------------------------------------------------------
// <copyright file="IconResolutionValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// IconResolutionValidator.
    /// </summary>
    public class IconResolutionValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "custom",
            "16x16",
            "20x20",
            "24x24",
            "30x30",
            "32x32",
            "36x36",
            "40x40",
            "48x48",
            "60x60",
            "64x64",
            "72x72",
            "80x80",
            "96x96",
            "256x256",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="IconResolutionValidator"/> class.
        /// </summary>
        public IconResolutionValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
