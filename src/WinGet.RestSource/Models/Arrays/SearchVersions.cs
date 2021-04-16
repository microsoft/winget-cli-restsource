// -----------------------------------------------------------------------
// <copyright file="SearchVersions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Models.Objects;

    /// <summary>
    /// SearchVersions.
    /// </summary>
    public class SearchVersions : ApiArray<SearchVersion>
    {
        private const bool Nullable = true;
        private const bool Unique = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchVersions"/> class.
        /// </summary>
        public SearchVersions()
        {
            this.APIArrayName = nameof(SearchVersions);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
        }
    }
}
