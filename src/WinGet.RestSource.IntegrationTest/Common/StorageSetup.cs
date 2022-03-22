// -----------------------------------------------------------------------
// <copyright file="StorageSetup.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Flurl.Http;
    using Xunit.Abstractions;

    /// <summary>
    /// Represents the storage setup.
    /// </summary>
    public class StorageSetup : IXunitSerializable, IStorageSetup
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

        /// <inheritdoc/>
        public async Task SetupAsync(TestCollateral testCollateral)
        {
            foreach (EndPointRequest request in this.EndPointRequests)
            {
                HttpContent content = new StringContent(
                    testCollateral.FetchTestCollateralContent(request.JsonFileName));
                await request
                        .Url
                        .WithHeader("x-functions-key", this.FunctionKey)
                        .SendAsync(HttpMethod.Post, content);
            }
        }
    }
}
