// -----------------------------------------------------------------------
// <copyright file="InstallModeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// InstallModeValidator.
    /// </summary>
    public class InstallModeValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "interactive",
            "silent",
            "silentWithProgress",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallModeValidator"/> class.
        /// </summary>
        public InstallModeValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
