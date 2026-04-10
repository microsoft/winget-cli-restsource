// -----------------------------------------------------------------------
// <copyright file="ManifestSearchTestHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit.Abstractions;

    /// <summary>
    /// Manifest search test helper.
    /// </summary>
    public class ManifestSearchTestHelper : IXunitSerializable
    {
        /// <summary>
        /// Gets or sets the Storage setup data.
        /// </summary>
        public IStorageSetup[] StorageSetup { get; set; }

        /// <summary>
        /// Gets or sets the storage cleanup data.
        /// </summary>
        public IStorageCleanup[] StorageCleanup { get; set; }

        /// <summary>
        /// Gets or sets the Endpoint Request data.
        /// </summary>
        public EndPointRequest EndPointRequest { get; set; }

        /// <summary>
        /// Gets or sets the search response test helper.
        /// </summary>
        public SearchResponseTestHelper[] SearchResponses { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed response time.
        /// </summary>
        public int MaximumAllowedResponseTime { get; set; }

        /// <inheritdoc/>
        public void Deserialize(IXunitSerializationInfo info)
        {
            this.EndPointRequest = info.GetValue<EndPointRequest>(nameof(this.EndPointRequest));
            this.StorageSetup = info.GetValue<IStorageSetup[]>(nameof(this.StorageSetup));
            this.StorageCleanup = info.GetValue<IStorageCleanup[]>(nameof(this.StorageCleanup));
            this.SearchResponses = info.GetValue<SearchResponseTestHelper[]>(nameof(this.SearchResponses));
            this.MaximumAllowedResponseTime = info.GetValue<int>(nameof(this.MaximumAllowedResponseTime));
        }

        /// <inheritdoc/>
        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(this.EndPointRequest), this.EndPointRequest, typeof(EndPointRequest));
            info.AddValue(nameof(this.StorageSetup), this.StorageSetup, typeof(IStorageSetup[]));
            info.AddValue(nameof(this.StorageCleanup), this.StorageCleanup, typeof(IStorageCleanup[]));
            info.AddValue(nameof(this.SearchResponses), this.SearchResponses, typeof(SearchResponseTestHelper[]));
            info.AddValue(nameof(this.MaximumAllowedResponseTime), this.MaximumAllowedResponseTime, typeof(int));
        }
    }
}
