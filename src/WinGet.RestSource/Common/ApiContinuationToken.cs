// -----------------------------------------------------------------------
// <copyright file="ApiContinuationToken.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Common
{
    /// <summary>
    /// This is a template continuation token for if a system token is not provided.
    /// </summary>
    public class ApiContinuationToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContinuationToken"/> class.
        /// </summary>
        public ApiContinuationToken()
        {
        }

        /// <summary>
        /// Gets or sets MaxResults.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Gets or sets Index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets Data.
        /// </summary>
        public string Data { get; set; }
    }
}
