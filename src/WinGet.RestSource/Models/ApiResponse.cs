// -----------------------------------------------------------------------
// <copyright file="ApiResponse.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// This will wrap API responses that need additional data.
    /// </summary>
    /// <typeparam name="T">Response Type.</typeparam>
    public class ApiResponse<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
        /// </summary>
        public ApiResponse()
        {
            this.Data = new List<T>();
        }

        /// <summary>
        /// Gets or sets items.
        /// </summary>
        [JsonProperty("data", Order = 1)]
        public IList<T> Data { get; set; }

        /// <summary>
        /// Gets or sets continuation Token.
        /// </summary>
        [JsonProperty("continuationToken", Order = 2)]
        public string ContinuationToken { get; set; }
    }
}