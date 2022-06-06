﻿// -----------------------------------------------------------------------
// <copyright file="StorageSetup.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Flurl.Http;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Fixtures;
    using Xunit.Abstractions;

    /// <summary>
    /// Represents the storage setup.
    /// </summary>
    public class StorageSetup : IStorageSetup, IXunitSerializable
    {
        /// <summary>
        /// Gets or sets the EndpointRequest.
        /// </summary>
        public EndPointRequest[] EndPointRequests { get; set; }

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

        /// <inheritdoc/>
        public async Task SetupAsync(
            IntegrationTestFixture fixture,
            bool allowConflict = true)
        {
            foreach (EndPointRequest request in this.EndPointRequests)
            {
                HttpContent content = new StringContent(
                    fixture.TestCollateral.FetchTestCollateralContent(request.JsonFileName, request.TestCollateralType));
                string url = $"{fixture.RestSourceUrl}/{request.RelativeUrlPath}";
                try
                {
                    await url
                        .WithHeader("x-functions-key", fixture.FunctionsHostKey)
                        .SendAsync(HttpMethod.Post, content);
                }
                catch (FlurlHttpException e)
                {
                    const int coflictStatusCode = 409;
                    if (allowConflict && e.StatusCode == coflictStatusCode)
                    {
                        continue;
                    }

                    throw;
                }
            }
        }
    }
}
