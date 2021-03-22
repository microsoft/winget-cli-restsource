// -----------------------------------------------------------------------
// <copyright file="Capabilities.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Capabilities.
    /// </summary>
    public class Capabilities : ApiArray<Strings.Capabilities>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Capabilities"/> class.
        /// </summary>
        public Capabilities()
        {
            this.APIArrayName = nameof(Capabilities);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
