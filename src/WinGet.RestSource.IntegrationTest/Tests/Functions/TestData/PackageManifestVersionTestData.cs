// -----------------------------------------------------------------------
// <copyright file="PackageManifestVersionTestData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Tests.Functions.TestData
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Helpers;

    /// <summary>
    /// Package manifest version test data.
    /// </summary>
    public class PackageManifestVersionTestData : BaseTestData
    {
        private const string PowerToysPackageIdentifier = "Microsoft.PowerToys";

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageManifestVersionTestData"/> class.
        /// </summary>
        public PackageManifestVersionTestData()
        {
            this.Data = new List<object[]>
            {
                new object[]
                {
                    new PackageVersionTestHelper()
                    {
                        TestId = "Version 0.31.4 found in repo",
                        PackageIdentifier = PowerToysPackageIdentifier,
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = $"/packageManifests/{PowerToysPackageIdentifier}?version=0.31.4",
                        },
                        ApiResponse = PackageVersionTestHelper.ApiResponseScenario.VersionFound,
                        ExpectedVersions = new string[] { "0.31.42345" },
                    },
                },
                new object[]
                {
                    new PackageVersionTestHelper()
                    {
                        TestId = "Version 0.31.42345 not found in repo",
                        PackageIdentifier = PowerToysPackageIdentifier,
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = $"/packageManifests/{PowerToysPackageIdentifier}?version=0.31.42345",
                        },
                        ApiResponse = PackageVersionTestHelper.ApiResponseScenario.NoApplicableVersion,
                    },
                },
                new object[]
                {
                    new PackageVersionTestHelper()
                    {
                        TestId = "Invalid package identifier",
                        PackageIdentifier = PowerToysPackageIdentifier,
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = $"/packageManifests/123456?version=0.31.42345",
                        },
                        ApiResponse = PackageVersionTestHelper.ApiResponseScenario.IdentifierNotFound,
                    },
                },
            };
        }
    }
}
