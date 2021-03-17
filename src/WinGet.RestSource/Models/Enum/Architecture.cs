// -----------------------------------------------------------------------
// <copyright file="Architecture.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Enum
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Architecture.
    /// </summary>
    public class Architecture : ApiEnum
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
        /// Initializes a new instance of the <see cref="Architecture"/> class.
        /// </summary>
        public Architecture()
        {
            this.APIStringName = nameof(Architecture);
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
