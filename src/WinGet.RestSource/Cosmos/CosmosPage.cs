// -----------------------------------------------------------------------
// <copyright file="CosmosPage.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Cosmos
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Models;

    /// <summary>
    /// This represents a cosmos page.
    /// It contains a list of elements, as well as associated data such as a continuation token.
    /// </summary>
    /// <typeparam name="T">This is the element type for cosmos.</typeparam>
    public class CosmosPage<T>
        where T : class
    {
        /// <summary>
        /// Gets or sets items.
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// Gets or sets continuation Token.
        /// </summary>
        public string ContinuationToken { get; set; }
    }
}
