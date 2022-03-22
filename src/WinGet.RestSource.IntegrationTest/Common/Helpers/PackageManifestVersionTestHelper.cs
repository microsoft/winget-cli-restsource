// -----------------------------------------------------------------------
// <copyright file="PackageManifestVersionTestHelper.cs" company="Microsoft Corporation">
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
    /// helper class for package manifest version test data.
    /// </summary>
    public class PackageManifestVersionTestHelper : IXunitSerializable
    {
        /// <summary>
        /// Represents the expercted response scenario.
        /// </summary>
        public enum ApiResponseScenario
        {
            /// <summary>
            /// Query scenario for identifier not found.
            /// </summary>
            IdentifierNotFound,

            /// <summary>
            /// Query scenario for version not found.
            /// </summary>
            NoApplicableVersion,

            /// <summary>
            /// Version query succesful.
            /// </summary>
            VersionFound,
        }

        /// <summary>
        /// Gets or sets the Test Id.
        /// </summary>
        public string TestId { get; set; }

        /// <summary>
        /// Gets or sets the Package Identifier.
        /// </summary>
        public string PackageIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the Endpoint request.
        /// </summary>
        public EndPointRequest EndPointRequest { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether version is found or not.
        /// </summary>
        public string ExpectedVersion { get; set; }

        /// <summary>
        /// Gets or sets the api response scenario.
        /// </summary>
        public ApiResponseScenario ApiResponse { get; set; }

        /// <inheritdoc/>
        public void Deserialize(IXunitSerializationInfo info)
        {
            this.TestId = info.GetValue<string>(nameof(this.TestId));
            this.EndPointRequest = info.GetValue<EndPointRequest>(nameof(this.EndPointRequest));
            this.ExpectedVersion = info.GetValue<string>(nameof(this.ExpectedVersion));
            this.ApiResponse = info.GetValue<ApiResponseScenario>(nameof(this.ApiResponse));
        }

        /// <inheritdoc/>
        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(this.TestId), this.TestId, typeof(string));
            info.AddValue(nameof(this.EndPointRequest), this.EndPointRequest, typeof(EndPointRequest));
            info.AddValue(nameof(this.ExpectedVersion), this.ExpectedVersion, typeof(string));
            info.AddValue(nameof(this.ApiResponse), this.ApiResponse, typeof(ApiResponseScenario));
        }
    }
}
