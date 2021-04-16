// -----------------------------------------------------------------------
// <copyright file="ApiDataPage.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Common
{
    using System.Collections.Generic;

    /// <summary>
    /// This represents a cosmos page.
    /// It contains a list of elements, as well as associated data such as a continuation token.
    /// </summary>
    /// <typeparam name="T">This is the element type for cosmos.</typeparam>
    public class ApiDataPage<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDataPage{T}"/> class.
        /// </summary>
        public ApiDataPage()
        {
            this.Items = new List<T>();
            this.ContinuationToken = null;
        }

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
