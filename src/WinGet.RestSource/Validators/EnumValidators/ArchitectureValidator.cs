// -----------------------------------------------------------------------
// <copyright file="ArchitectureValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// Architecture.
    /// </summary>
    public class ArchitectureValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "x86",
            "x64",
            "arm",
            "arm64",
            "neutral",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchitectureValidator"/> class.
        /// </summary>
        public ArchitectureValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
