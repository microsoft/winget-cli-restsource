// -----------------------------------------------------------------------
// <copyright file="ManifestSearchFunctionDataFactory.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Functions.TestData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common;

    /// <summary>
    /// Manifest Search Function data factory.
    /// </summary>
    public class ManifestSearchFunctionDataFactory
    {
        private const string PowerToysPackageIdentifier = "RestIntegrationTest.PowerToys";
        private const string AzureCliPackageIdentifier = "RestIntegrationTest.AzureCLI";
        private const string VSCodePackageIdentifier = "RestIntegrationTest.VisualStudioCode";
        private const string CodeLitePackageIdentifier = "RestIntegrationTest.CodeLite";

        /// <summary>
        /// Get test setup data.
        /// </summary>
        /// <returns>An array of type <see cref="EndPointRequest"/>.</returns>
        public static EndPointRequest[] GetTestSetupData()
        {
            return new EndPointRequest[]
            {
                new EndPointRequest()
                {
                    RelativeUrlPath = TestDataConstants.PackageManifestEndpointRelativePath,
                    JsonFileName = "powertoys-search.json",
                    TestCollateralType = TestCollateral.TestCollateralType.Manifests,
                },
                new EndPointRequest()
                {
                    RelativeUrlPath = TestDataConstants.PackageManifestEndpointRelativePath,
                    JsonFileName = "azure-cli-search.json",
                    TestCollateralType = TestCollateral.TestCollateralType.Manifests,
                },
                new EndPointRequest()
                {
                    RelativeUrlPath = TestDataConstants.PackageManifestEndpointRelativePath,
                    JsonFileName = "code-lite-search.json",
                    TestCollateralType = TestCollateral.TestCollateralType.Manifests,
                },
                new EndPointRequest()
                {
                    RelativeUrlPath = TestDataConstants.PackageManifestEndpointRelativePath,
                    JsonFileName = "vs-code-search.json",
                    TestCollateralType = TestCollateral.TestCollateralType.Manifests,
                },
            };
        }

        /// <summary>
        /// Get test cleanup data.
        /// </summary>
        /// <returns>An array of type <see cref="EndPointRequest"/>.</returns>
        public static EndPointRequest[] GetTestCleanupData()
        {
            return new EndPointRequest[]
            {
                new EndPointRequest()
                {
                    RelativeUrlPath = $"packageManifests/{PowerToysPackageIdentifier}",
                },
                new EndPointRequest()
                {
                    RelativeUrlPath = $"packageManifests/{AzureCliPackageIdentifier}",
                },
                new EndPointRequest()
                {
                    RelativeUrlPath = $"packageManifests/{VSCodePackageIdentifier}",
                },
                new EndPointRequest()
                {
                    RelativeUrlPath = $"packageManifests/{CodeLitePackageIdentifier}",
                },
            };
        }

        /// <summary>
        /// Get Randome test endpoint request.
        /// </summary>
        /// <param name="identifier">Idenfier.</param>
        /// <param name="size">Amoutn of endpoint request generated, default is 21.</param>
        /// <returns>An array of type <see cref="EndPointRequest"/>.</returns>
        public static EndPointRequest[] GetRandomTestEndpointRequest(
            string identifier,
            int size = 21)
        {
            EndPointRequest[] endpoints = new EndPointRequest[size];
            AddRandomData(endpoints, identifier);
            return endpoints;
        }

        private static void AddRandomData(EndPointRequest[] endpoints, string prefix)
        {
            TestCollateral testCollateral = new TestCollateral();
            for (int i = 0; i < endpoints.Length; i++)
            {
                string endPointCollateralFileContent =
                    testCollateral.FetchTestCollateralContent(
                        "fake-manifest-placeholder.json",
                        TestCollateral.TestCollateralType.Manifests);

                string endPointFileName = $"{prefix}-{i}.json";
                endPointCollateralFileContent = endPointCollateralFileContent.Replace(
                    "[publisher-name-placeholder]", $"{prefix}-{i}");

                testCollateral.AddTestCollateral(
                    endPointCollateralFileContent,
                    endPointFileName,
                    TestCollateral.TestCollateralType.Manifests);
                endpoints[i] = new EndPointRequest()
                {
                    RelativeUrlPath = TestDataConstants.PackageManifestEndpointRelativePath,
                    JsonFileName = endPointFileName,
                    TestCollateralType = TestCollateral.TestCollateralType.Manifests,
                };
            }
        }
    }
}
