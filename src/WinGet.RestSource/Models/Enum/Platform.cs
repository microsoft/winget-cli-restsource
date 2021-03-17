// -----------------------------------------------------------------------
// <copyright file="Platform.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Enum
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// PlatformEnum.
    /// </summary>
    public class Platform : ApiEnum
    {
        private List<string> enumList = new List<string>
        {
            "Windows.Desktop",
            "Windows.Universal",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Platform"/> class.
        /// </summary>
        public Platform()
        {
            this.APIStringName = nameof(Platform);
            this.Values = this.enumList;
        }
    }
}
