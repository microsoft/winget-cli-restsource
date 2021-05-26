// -----------------------------------------------------------------------
// <copyright file="ScopeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// ScopeValidator.
    /// </summary>
    public class ScopeValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "user",
            "machine",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeValidator"/> class.
        /// </summary>
        public ScopeValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
