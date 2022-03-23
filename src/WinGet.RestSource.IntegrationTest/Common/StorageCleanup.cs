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
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Fixtures;
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

        /// <inheritdoc/>
        public async Task CleanupAsync(IntegrationTestFixture fixture)
        {
            foreach (EndPointRequest request in this.EndPointRequests)
            {
                string url = $"{fixture.RestSourceUrl}/{request.RelativeUrlPath}";
                var response = await url.SendAsync(HttpMethod.Get);
                if (response.StatusCode == (int)HttpStatusCode.OK)
                {
                    await url
                        .WithHeader("x-functions-key", fixture.FunctionsHostKey)
                        .SendAsync(HttpMethod.Delete);
                }
            }
        }

        /// <inheritdoc/>
        public void Deserialize(IXunitSerializationInfo info)
        {
            this.EndPointRequests = info.GetValue<EndPointRequest[]>(nameof(this.EndPointRequests));
        }

        /// <inheritdoc/>
        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(this.EndPointRequests), this.EndPointRequests, typeof(EndPointRequest[]));
        }
    }
}
