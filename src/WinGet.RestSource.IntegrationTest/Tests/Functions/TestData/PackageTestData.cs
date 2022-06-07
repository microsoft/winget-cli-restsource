// -----------------------------------------------------------------------
// <copyright file="PackageTestData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Tests.Functions.TestData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Helpers;

    /// <summary>
    /// Package test data.
    /// </summary>
    public class PackageTestData : BaseTestData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageTestData"/> class.
        /// </summary>
        public PackageTestData()
        {
            this.Data = new List<object[]>()
            {
                new object[]
                {
                    new PackageTestHelper()
                    {
                        PackageIdentifier = TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1,
                        ApiResponse = PackageTestHelper.ApiResponseScenario.IdentifierFound,
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = $"packages/{TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1}",
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
                    },
                },
                new object[]
                {
                    new PackageTestHelper()
                    {
                        PackageIdentifier = TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1,
                        ApiResponse = PackageTestHelper.ApiResponseScenario.IdentifierNotFound,
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = $"packages/{TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1}",
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
                    },
                },
            };
        }
    }
}
