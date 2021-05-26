// -----------------------------------------------------------------------
// <copyright file="PlatformValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// PlatformValidatorEnum.
    /// </summary>
    public class PlatformValidator : ApiEnumValidator
    {
        private List<string> enumList = new List<string>
        {
            "Windows.Desktop",
            "Windows.Universal",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformValidator"/> class.
        /// </summary>
        public PlatformValidator()
        {
            this.Values = this.enumList;
        }
    }
}
