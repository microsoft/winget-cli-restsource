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
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageVersionTestData"/> class.
        /// </summary>
        public PackageVersionTestData()
        {
            this.Data = new List<object[]>
            {
                new object[]
                {
                    new PackageTestHelper()
                    {
                        PackageIdentifier = TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1,
                        ApiResponse = PackageTestHelper.ApiResponseScenario.IdentifierFound,
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = $"packages/{TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1}/versions",
                        },
                        StorageCleanup = new IStorageCleanup[]
                        {
                            new StorageCleanup()
                            {
                                EndPointRequests = new EndPointRequest[]
                                {
                                    new EndPointRequest()
                                    {
                                        RelativeUrlPath = $"packageManifests/{TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1}",
                                    },
                                },
                            },
                        },
                        StorageSetup = new IStorageSetup[]
                        {
                            new StorageSetup()
                            {
                                EndPointRequests = new EndPointRequest[]
                                {
                                    new EndPointRequest()
                                    {
                                        RelativeUrlPath = TestDataConstants.PackageManifestEndpointRelativePath,
                                        JsonFileName = TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item2,
                                    },
                                },
                            },
                        },
                        ExpectedVersions = new[] { "0.31.4", "0.32.4" },
                    },
                },
                new object[]
                {
                    new PackageTestHelper()
                    {
                        PackageIdentifier = TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1,
                        ApiResponse = PackageTestHelper.ApiResponseScenario.IdentifierFound,
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = $"packages/{TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1}/versions/0.31.4",
                        },
                        StorageCleanup = new IStorageCleanup[]
                        {
                            new StorageCleanup()
                            {
                                EndPointRequests = new EndPointRequest[]
                                {
                                    new EndPointRequest()
                                    {
                                        RelativeUrlPath = $"packageManifests/{TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1}",
                                    },
                                },
                            },
                        },
                        StorageSetup = new IStorageSetup[]
                        {
                            new StorageSetup()
                            {
                                EndPointRequests = new EndPointRequest[]
                                {
                                    new EndPointRequest()
                                    {
                                        RelativeUrlPath = TestDataConstants.PackageManifestEndpointRelativePath,
                                        JsonFileName = TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item2,
                                    },
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
