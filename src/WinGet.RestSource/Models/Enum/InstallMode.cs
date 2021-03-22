// -----------------------------------------------------------------------
// <copyright file="InstallMode.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Enum
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// InstallMode.
    /// </summary>
    public class InstallMode : ApiEnum
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "interactive",
            "silent",
            "silentWithProgress",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallMode"/> class.
        /// </summary>
        public InstallMode()
        {
            this.APIStringName = nameof(InstallMode);
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
