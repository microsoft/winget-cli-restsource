// -----------------------------------------------------------------------
// <copyright file="PackageTestHelper.cs" company="Microsoft Corporation">
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
    /// Helper class for package manifest version test data.
    /// </summary>
    public class PackageTestHelper : IXunitSerializable
    {
        /// <summary>
        /// Represents the expected response scenario.
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
            /// Query scenario for identifier found.
            /// </summary>
            IdentifierFound,

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
        /// Gets or sets the storage cleanup.
        /// </summary>
        public IStorageCleanup[] StorageCleanup { get; set; }

        /// <summary>
        /// Gets or sets the storage setup.
        /// </summary>
        public IStorageSetup[] StorageSetup { get; set; }

        /// <summary>
        /// Gets or sets the expected versions.
        /// </summary>
        public string[] ExpectedVersions { get; set; }

        /// <summary>
        /// Gets or sets the api response scenario.
        /// </summary>
        public ApiResponseScenario ApiResponse { get; set; }

        /// <inheritdoc/>
        public void Deserialize(IXunitSerializationInfo info)
        {
            this.TestId = info.GetValue<string>(nameof(this.TestId));
            this.EndPointRequest = info.GetValue<EndPointRequest>(nameof(this.EndPointRequest));
            this.StorageCleanup = info.GetValue<IStorageCleanup[]>(nameof(this.StorageCleanup));
            this.StorageSetup = info.GetValue<IStorageSetup[]>(nameof(this.StorageSetup));
            this.ExpectedVersions = info.GetValue<string[]>(nameof(this.ExpectedVersions));
            this.PackageIdentifier = info.GetValue<string>(nameof(this.PackageIdentifier));
            this.ApiResponse = info.GetValue<ApiResponseScenario>(nameof(this.ApiResponse));
        }

        /// <inheritdoc/>
        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(this.TestId), this.TestId, typeof(string));
            info.AddValue(nameof(this.EndPointRequest), this.EndPointRequest, typeof(EndPointRequest));
            info.AddValue(nameof(this.StorageCleanup), this.StorageCleanup, typeof(IStorageCleanup[]));
            info.AddValue(nameof(this.StorageSetup), this.StorageSetup, typeof(IStorageSetup[]));
            info.AddValue(nameof(this.ExpectedVersions), this.ExpectedVersions, typeof(string[]));
            info.AddValue(nameof(this.ApiResponse), this.ApiResponse, typeof(ApiResponseScenario));
            info.AddValue(nameof(this.PackageIdentifier), this.PackageIdentifier, typeof(string));
        }
    }
}
