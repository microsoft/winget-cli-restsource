// -----------------------------------------------------------------------
// <copyright file="Dependencies.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Dependencies.
    /// </summary>
    public class Dependencies : ApiArray<Strings.Dependencies>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dependencies"/> class.
        /// </summary>
        public Dependencies()
        {
            this.APIArrayName = nameof(Dependencies);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
