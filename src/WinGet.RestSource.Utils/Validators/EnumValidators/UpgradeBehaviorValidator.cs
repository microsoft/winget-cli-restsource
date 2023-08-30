// -----------------------------------------------------------------------
// <copyright file="UpgradeBehaviorValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;

    /// <summary>
    /// UpgradeBehaviorValidator.
    /// </summary>
    public class UpgradeBehaviorValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "install",
            "uninstallPrevious",
            "deny",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeBehaviorValidator"/> class.
        /// </summary>
        public UpgradeBehaviorValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
