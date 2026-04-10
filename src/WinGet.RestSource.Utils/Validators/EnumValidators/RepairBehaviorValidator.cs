// -----------------------------------------------------------------------
// <copyright file="RepairBehaviorValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// RepairBehaviorValidator.
    /// </summary>
    public class RepairBehaviorValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "modify",
            "uninstaller",
            "installer",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="RepairBehaviorValidator"/> class.
        /// </summary>
        public RepairBehaviorValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
