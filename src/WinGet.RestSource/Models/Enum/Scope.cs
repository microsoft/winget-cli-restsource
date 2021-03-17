// -----------------------------------------------------------------------
// <copyright file="Scope.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Enum
{
    using System;
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Scope.
    /// </summary>
    public class Scope : ApiEnum
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            "user",
            "machine",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Scope"/> class.
        /// </summary>
        public Scope()
        {
            this.APIStringName = nameof(Scope);
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
