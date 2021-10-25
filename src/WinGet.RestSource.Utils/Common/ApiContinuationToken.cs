// -----------------------------------------------------------------------
// <copyright file="ApiContinuationToken.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Common
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
        /// Gets or sets MaxPageSize.
        /// </summary>
        public int MaxPageSize { get; set; }

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
