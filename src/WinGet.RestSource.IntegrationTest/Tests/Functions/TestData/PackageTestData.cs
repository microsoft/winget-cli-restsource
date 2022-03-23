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
    /// Package version test data.
    /// </summary>
    public class PackageTestData : BaseTestData
    {
        private const string PowerToysPackageIdentifier = "Microsoft.PowerToys";
        private const string PowerToysJsonFileName = "powertoys.json";

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageTestData"/> class.
        /// </summary>
        public PackageTestData()
        {
            this.Data = new List<object[]>()
            {
                new object[]
                {
                    new PackageVersionTestHelper()
                    {
                        PackageIdentifier = PowerToysPackageIdentifier,
                        ApiResponse = PackageVersionTestHelper.ApiResponseScenario.IdentifierFound,
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = $"packages/{PowerToysPackageIdentifier}",
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
                    },
                },
                new object[]
                {
                    new PackageVersionTestHelper()
                    {
                        PackageIdentifier = PowerToysPackageIdentifier,
                        ApiResponse = PackageVersionTestHelper.ApiResponseScenario.IdentifierNotFound,
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = $"packages/{PowerToysPackageIdentifier}",
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
                    },
                },
            };
        }
    }
}
