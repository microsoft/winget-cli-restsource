// -----------------------------------------------------------------------
// <copyright file="ManifestSearchFunctionTestData.cs" company="Microsoft Corporation">
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
    /// Manifest search function test data.
    /// </summary>
    public class ManifestSearchFunctionTestData : BaseTestData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchFunctionTestData"/> class.
        /// </summary>
        public ManifestSearchFunctionTestData()
        {
            this.Data = new List<object[]>()
            {
                new object[]
                {
                    new ManifestSearchTestHelper()
                    {
                        StorageCleanup = new IStorageCleanup[]
                        {
                            new StorageCleanup()
                            {
                                EndPointRequests = ManifestSearchFunctionDataFactory.GetTestCleanupData(),
                            },
                        },
                        StorageSetup = new IStorageSetup[]
                        {
                            new StorageSetup()
                            {
                                EndPointRequests = ManifestSearchFunctionDataFactory.GetTestSetupData(),
                            },
                        },
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = "manifestSearch",
                            JsonFileName = "match-type-substring.json",
                            TestCollateralType = TestCollateral.TestCollateralType.RequestFiles,
                        },
                        SearchResponses = new SearchResponseTestHelper[]
                        {
                            new SearchResponseTestHelper()
                            {
                                PackageIdentifier = "RestIntegrationTest.CodeLite",
                                Publisher = "CodeLite",
                                ExpectedVersions = new string[] { "14.0.0" },
                            },
                            new SearchResponseTestHelper()
                            {
                                PackageIdentifier = "RestIntegrationTest.VisualStudioCode",
                                Publisher = "Microsoft Corporation",
                                ExpectedVersions = new string[] { "1.65.2", "1.65.3" },
                            },
                        },
                        MaximumAllowedResponseTime = 3,
                    },
                },
                new object[]
                {
                    new ManifestSearchTestHelper()
                    {
                        StorageCleanup = new IStorageCleanup[]
                        {
                            new StorageCleanup()
                            {
                                EndPointRequests = ManifestSearchFunctionDataFactory.GetTestCleanupData(),
                            },
                        },
                        StorageSetup = new IStorageSetup[]
                        {
                            new StorageSetup()
                            {
                                EndPointRequests = ManifestSearchFunctionDataFactory.GetTestSetupData(),
                            },
                        },
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = "manifestSearch",
                            JsonFileName = "match-type-startswith.json",
                            TestCollateralType = TestCollateral.TestCollateralType.RequestFiles,
                        },
                        SearchResponses = new SearchResponseTestHelper[]
                        {
                            new SearchResponseTestHelper()
                            {
                                PackageIdentifier = "RestIntegrationTest.PowerToys",
                                Publisher = "Microsoft Corporation",
                                ExpectedVersions = new string[] { "0.31.4", "0.32.4" },
                            },
                            new SearchResponseTestHelper()
                            {
                                PackageIdentifier = "RestIntegrationTest.VisualStudioCode",
                                Publisher = "Microsoft Corporation",
                                ExpectedVersions = new string[] { "1.65.2", "1.65.3" },
                            },
                            new SearchResponseTestHelper()
                            {
                                PackageIdentifier = "RestIntegrationTest.AzureCLI",
                                Publisher = "Microsoft Corporation",
                                ExpectedVersions = new string[] { "2.32.0", "2.33.0" },
                            },
                        },
                        MaximumAllowedResponseTime = 3,
                    },
                },
                new object[]
                {
                    new ManifestSearchTestHelper()
                    {
                        StorageCleanup = new IStorageCleanup[]
                        {
                            new StorageCleanup()
                            {
                                EndPointRequests = ManifestSearchFunctionDataFactory.GetTestCleanupData(),
                            },
                        },
                        StorageSetup = new IStorageSetup[]
                        {
                            new StorageSetup()
                            {
                                EndPointRequests = ManifestSearchFunctionDataFactory.GetTestSetupData(),
                            },
                        },
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = "manifestSearch",
                            JsonFileName = "match-type-exact.json",
                            TestCollateralType = TestCollateral.TestCollateralType.RequestFiles,
                        },
                        SearchResponses = new SearchResponseTestHelper[]
                        {
                            new SearchResponseTestHelper()
                            {
                                PackageIdentifier = "RestIntegrationTest.AzureCLI",
                                Publisher = "Microsoft Corporation",
                                ExpectedVersions = new string[] { "2.32.0", "2.33.0" },
                            },
                        },
                        MaximumAllowedResponseTime = 3,
                    },
                },
                new object[]
                {
                    new ManifestSearchTestHelper()
                    {
                        StorageCleanup = new IStorageCleanup[]
                        {
                            new StorageCleanup()
                            {
                                EndPointRequests = ManifestSearchFunctionDataFactory.GetTestCleanupData(),
                            },
                        },
                        StorageSetup = new IStorageSetup[]
                        {
                            new StorageSetup()
                            {
                                EndPointRequests = ManifestSearchFunctionDataFactory.GetTestSetupData(),
                            },
                        },
                        EndPointRequest = new EndPointRequest()
                        {
                            RelativeUrlPath = "manifestSearch",
                            JsonFileName = "match-type-non-existent.json",
                            TestCollateralType = TestCollateral.TestCollateralType.RequestFiles,
                        },
                        SearchResponses = new SearchResponseTestHelper[]
                        {
                        },
                        MaximumAllowedResponseTime = 3,
                    },
                },
            };
        }
    }
}
