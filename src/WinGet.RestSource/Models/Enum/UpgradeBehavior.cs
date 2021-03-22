// -----------------------------------------------------------------------
// <copyright file="UpgradeBehavior.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Enum
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// UpgradeBehavior.
    /// </summary>
    public class UpgradeBehavior : ApiEnum
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "install",
            "uninstallPrevious",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeBehavior"/> class.
        /// </summary>
        public UpgradeBehavior()
        {
            this.APIStringName = nameof(UpgradeBehavior);
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
