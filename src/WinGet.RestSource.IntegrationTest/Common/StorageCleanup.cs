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
    using Xunit.Abstractions;

    /// <summary>
    /// Represents storage cleanup.
    /// </summary>
    public class StorageCleanup : IStorageCleanup, IXunitSerializable
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
                var response = await request.RelativeUrlPath.SendAsync(HttpMethod.Get);
                if (response.StatusCode == (int)HttpStatusCode.OK)
                {
                    await request
                        .RelativeUrlPath
                        .WithHeader("x-functions-key", this.FunctionKey)
                        .SendAsync(HttpMethod.Delete);
                }
            }
        }

        /// <inheritdoc/>
        public void Deserialize(IXunitSerializationInfo info)
        {
            this.EndPointRequests = info.GetValue<EndPointRequest[]>(nameof(this.EndPointRequests));
            this.FunctionKey = info.GetValue<string>(nameof(this.FunctionKey));
        }

        /// <inheritdoc/>
        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(this.EndPointRequests), this.EndPointRequests, typeof(EndPointRequest[]));
            info.AddValue(nameof(this.FunctionKey), this.FunctionKey, typeof(string));
        }
    }
}
