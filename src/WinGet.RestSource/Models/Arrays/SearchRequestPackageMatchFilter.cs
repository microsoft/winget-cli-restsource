// -----------------------------------------------------------------------
// <copyright file="SearchRequestPackageMatchFilter.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// SearchRequestPackageMatchFilter.
    /// </summary>
    public class SearchRequestPackageMatchFilter : ApiArray<Objects.SearchRequestPackageMatchFilter>
    {
        private const bool Nullable = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequestPackageMatchFilter"/> class.
        /// </summary>
        public SearchRequestPackageMatchFilter()
        {
            this.APIArrayName = nameof(SearchRequestPackageMatchFilter);
            this.AllowNull = Nullable;
        }
    }
}
