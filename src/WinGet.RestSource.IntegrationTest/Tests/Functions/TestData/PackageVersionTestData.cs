// -----------------------------------------------------------------------
// <copyright file="PackageVersionTestData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Tests.Functions.TestData
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Helpers;

    /// <summary>
    /// Package Version Test Data.
    /// </summary>
    public class PackageVersionTestData : BaseTestData
    {
        private const string PowerToysPackageIdentifier = "Microsoft.PowerToys";
        private const string PowerToysJsonFileName = "powertoys.json";

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageVersionTestData"/> class.
        /// </summary>
        public PackageVersionTestData()
        {
            this.Data = new List<object[]>
            {
                new object[]
                {
                    new PackageVersionTestHelper()
                    {
                        PackageIdentifier = PowerToysPackageIdentifier,
                        ApiResponse = PackageVersionTestHelper.ApiResponseScenario.IdentifierFound,
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = $"packages/{PowerToysPackageIdentifier}/versions",
                        },
                        StorageCleanup = new StorageCleanup()
                        {
                            EndPointRequests = new EndPointRequest[]
                            {
                                new EndPointRequest()
                                {
                                    RelativeUrlPath = $"packageManifests/{PowerToysPackageIdentifier}",
                                },
                            },
                        },
                        StorageSetup = new StorageSetup()
                        {
                            EndPointRequests = new EndPointRequest[]
                            {
                                new EndPointRequest()
                                {
                                    RelativeUrlPath = $"packageManifests",
                                    JsonFileName = PowerToysJsonFileName,
                                },
                            },
                        },
                        ExpectedVersions = new[] { "0.31.4", "0.32.4" },
                    },
                },
                new object[]
                {
                    new PackageVersionTestHelper()
                    {
                        PackageIdentifier = PowerToysPackageIdentifier,
                        ApiResponse = PackageVersionTestHelper.ApiResponseScenario.IdentifierFound,
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = $"packages/{PowerToysPackageIdentifier}/versions/0.31.4",
                        },
                        StorageCleanup = new StorageCleanup()
                        {
                            EndPointRequests = new EndPointRequest[]
                            {
                                new EndPointRequest()
                                {
                                    RelativeUrlPath = $"packageManifests/{PowerToysPackageIdentifier}",
                                },
                            },
                        },
                        StorageSetup = new StorageSetup()
                        {
                            EndPointRequests = new EndPointRequest[]
                            {
                                new EndPointRequest()
                                {
                                    RelativeUrlPath = $"packageManifests",
                                    JsonFileName = PowerToysJsonFileName,
                                },
                            },
                        },
                        ExpectedVersions = new[] { "0.31.4" },
                    },
                },
            };
        }
    }
}
