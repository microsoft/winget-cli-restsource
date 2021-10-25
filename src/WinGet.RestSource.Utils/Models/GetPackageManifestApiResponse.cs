// -----------------------------------------------------------------------
// <copyright file="GetPackageManifestApiResponse.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Common
{
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Newtonsoft.Json;

    /// <summary>
    /// This will wrap API responses that need additional data.
    /// </summary>
    /// <typeparam name="T">Response Type.</typeparam>
    public class GetPackageManifestApiResponse<T> : ApiResponse<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPackageManifestApiResponse{T}"/> class.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="continuationToken">Continuation Token.</param>
        public GetPackageManifestApiResponse(T data, string continuationToken = null)
            : base(data, continuationToken)
        {
        }

        /// <summary>
        /// Gets or sets UnsupportedQueryParameters.
        /// </summary>
        [JsonProperty(Order = 2)]
        public QueryParameters UnsupportedQueryParameters { get; set; }

        /// <summary>
        /// Gets or sets RequiredQueryParameters.
        /// </summary>
        [JsonProperty(Order = 3)]
        public QueryParameters RequiredQueryParameters { get; set; }
    }
}
