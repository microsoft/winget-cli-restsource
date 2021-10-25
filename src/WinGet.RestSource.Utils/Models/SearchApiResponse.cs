// -----------------------------------------------------------------------
// <copyright file="SearchApiResponse.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Common
{
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Newtonsoft.Json;

    /// <summary>
    /// This will wrap API responses that need additional data.
    /// </summary>
    /// <typeparam name="T">Response Type.</typeparam>
    public class SearchApiResponse<T> : ApiResponse<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchApiResponse{T}"/> class.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="continuationToken">Continuation Token.</param>
        public SearchApiResponse(T data, string continuationToken = null)
            : base(data, continuationToken)
        {
        }

        /// <summary>
        /// Gets or sets UnsupportedPackageMatchFields.
        /// </summary>
        [JsonProperty(Order = 2)]
        public PackageMatchFields UnsupportedPackageMatchFields { get; set; }

        /// <summary>
        /// Gets or sets RequiredPackageMatchFields.
        /// </summary>
        [JsonProperty(Order = 3)]
        public PackageMatchFields RequiredPackageMatchFields { get; set; }
    }
}
