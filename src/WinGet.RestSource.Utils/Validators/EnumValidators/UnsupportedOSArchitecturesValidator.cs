// -----------------------------------------------------------------------
// <copyright file="UnsupportedOSArchitecturesValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// UnsupportedOSArchitecturesValidator.
    /// </summary>
    public class UnsupportedOSArchitecturesValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "x86",
            "x64",
            "arm",
            "arm64",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedOSArchitecturesValidator"/> class.
        /// </summary>
        public UnsupportedOSArchitecturesValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
