// -----------------------------------------------------------------------
// <copyright file="SearchVersions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Models.Objects;

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

        /// <summary>
        /// Merge in a Search Version.
        /// </summary>
        /// <param name="searchVersions">Search Versions.</param>
        public void Merge(SearchVersions searchVersions)
        {
            foreach (SearchVersion searchVersion in searchVersions)
            {
                if (this.Exists(searchVersion.ConsolidationExpression))
                {
                    int ind = this.FindIndex(searchVersion.ConsolidationExpression);
                    this[ind].Merge(searchVersion);
                }
                else
                {
                    this.Add(searchVersion);
                }
            }
        }
    }
}
