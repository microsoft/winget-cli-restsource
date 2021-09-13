// -----------------------------------------------------------------------
// <copyright file="QueryParameters.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// QueryParameters.
    /// </summary>
    public class QueryParameters : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameters"/> class.
        /// </summary>
        public QueryParameters()
        {
            this.APIArrayName = nameof(QueryParameters);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
        }
    }
}
