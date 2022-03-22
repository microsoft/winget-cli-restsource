// -----------------------------------------------------------------------
// <copyright file="StorageCleanup.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Flurl.Http;

    /// <summary>
    /// Represents storage cleanup.
    /// </summary>
    public class StorageCleanup : IStorageCleanup
    {
        /// <summary>
        /// Gets or sets the EndpointRequest.
        /// </summary>
        public EndPointRequest[] EndPointRequests { get; set; }

        /// <summary>
        /// Gets or sets the EndpointRequest.
        /// </summary>
        public string FunctionKey { get; set; }

        /// <inheritdoc/>
        public async Task CleanupAsync()
        {
            foreach (EndPointRequest request in this.EndPointRequests)
            {
                var response = await request.Url.SendAsync(HttpMethod.Get);
                if (response.StatusCode == (int)HttpStatusCode.OK)
                {
                    await request
                        .Url
                        .WithHeader("x-functions-key", this.FunctionKey)
                        .SendAsync(HttpMethod.Delete);
                }
            }
        }
    }
}
