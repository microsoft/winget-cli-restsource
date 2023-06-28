// -----------------------------------------------------------------------
// <copyright file="IconFileTypeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// IconFileTypeValidator.
    /// </summary>
    public class IconFileTypeValidator : ApiEnumValidator
    {
        private const bool Nullable = false;
        private List<string> enumList = new List<string>
        {
            "png",
            "jpeg",
            "ico",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="IconFileTypeValidator"/> class.
        /// </summary>
        public IconFileTypeValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
