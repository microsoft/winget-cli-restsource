// -----------------------------------------------------------------------
// <copyright file="InstallerType.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Enum
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// InstallerType.
    /// </summary>
    public class InstallerType : ApiEnum
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "msix",
            "msi",
            "appx",
            "exe",
            "zip",
            "inno",
            "nullsoft",
            "wix",
            "burn",
            "pwa",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerType"/> class.
        /// </summary>
        public InstallerType()
        {
            this.APIStringName = nameof(InstallerType);
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
