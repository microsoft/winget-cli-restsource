// -----------------------------------------------------------------------
// <copyright file="UpgradeBehaviorValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.EnumValidators
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
