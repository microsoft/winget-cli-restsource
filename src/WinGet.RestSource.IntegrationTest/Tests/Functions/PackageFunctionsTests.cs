// -----------------------------------------------------------------------
// <copyright file="PackageFunctionsTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Flurl;
    using Flurl.Http;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Fixtures;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Helpers;
    using Microsoft.WinGet.RestSource.IntegrationTest.Functions.TestData;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Package functions integration tests.
    /// </summary>
    [Collection("IntegrationTestCollection")]
    public class PackageFunctionsTests
    {
        private const int MaxResultsPerPage = 20;

        private readonly IntegrationTestFixture fixture;
        private readonly ITestOutputHelper log;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageFunctionsTests"/> class.
        /// </summary>
        /// <param name="fixture">An object of type <see cref="IntegrationTestFixture"/>.</param>
        /// <param name="log">An object of type <see cref="ITestOutputHelper"/>.</param>
        public PackageFunctionsTests(IntegrationTestFixture fixture, ITestOutputHelper log)
        {
            this.log = log;
            this.fixture = fixture;
        }

        /// <summary>
        /// Package function endpoint tests.
        /// </summary>
        /// <param name="helper">An object of type <see cref="PackageTestHelper"/>.</param>
        [Theory]
        [ClassData(typeof(PackageTestData))]
        public async void PackageFunctionEndpointTests(PackageTestHelper helper)
        {
            Assert.NotNull(helper);
            Assert.NotNull(helper.EndPointRequest);
            Assert.NotNull(helper.PackageIdentifier);

            await this.InitializeAsync(helper);

            string url = $"{this.fixture.RestSourceUrl}/{helper.EndPointRequest.RelativeUrlPath.TrimStart('/')}";
            var response = await IntegrationTestFixture.GetConsistentApiResponse<Package>(url);

            switch (helper.ApiResponse)
            {
                case PackageTestHelper.ApiResponseScenario.IdentifierNotFound:
                    Assert.Null(response);
                    break;
                case PackageTestHelper.ApiResponseScenario.IdentifierFound:
                    Assert.NotNull(response);
                    Assert.Equal(helper.PackageIdentifier, response.Data[0].PackageIdentifier);
                    break;
                default:
                    throw new ArgumentException($"Unknown Api response scenario {helper.ApiResponse}");
            }
        }

        /// <summary>
        /// Package function version endpoint tests.
        /// </summary>
        /// <param name="helper">An object of type <see cref="PackageTestHelper"/>.</param>
        [Theory]
        [ClassData(typeof(PackageVersionTestData))]
        public async void PackageFunctionVersionEndpointTests(PackageTestHelper helper)
        {
            Assert.NotNull(helper);
            Assert.NotNull(helper.EndPointRequest);
            Assert.NotNull(helper.PackageIdentifier);

            await this.InitializeAsync(helper);

            string url = $"{this.fixture.RestSourceUrl}/{helper.EndPointRequest.RelativeUrlPath.TrimStart('/')}";
            var response = await IntegrationTestFixture.GetConsistentApiResponse<VersionExtended>(url);

            if (helper.ApiResponse == PackageTestHelper.ApiResponseScenario.IdentifierFound)
            {
                Assert.NotNull(response);
                var actualVersions = response.Data.ConvertAll<string>((v) => v.PackageVersion).ToHashSet();
                foreach (var version in helper.ExpectedVersions)
                {
                    Assert.Contains(version, actualVersions);
                }
            }
        }

        /// <summary>
        /// Verifies the GetPackage* APIs.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetPackages()
        {
            string packageIdentifier = TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1;
            string packagesUrl = this.fixture.RestSourceUrl.AppendPathSegment("packages");
            string packageManifestsUrl = this.fixture.RestSourceUrl.AppendPathSegment(TestDataConstants.PackageManifestEndpointRelativePath);
            string powerToysPackageManifestUrl = packageManifestsUrl.AppendPathSegment(packageIdentifier);
            string powerToysPackageUrl = packagesUrl.AppendPathSegment(packageIdentifier);

            this.log.WriteLine("Tests that GetPackages returns the expected package.");
            {
                var packages = await IntegrationTestFixture.GetConsistentApiResponse<Package>(powerToysPackageUrl);
                Assert.NotEmpty(packages.Data);
                Assert.Equal(packageIdentifier, packages.Data.First().PackageIdentifier);
            }

            this.log.WriteLine("Tests that ContinuationToken has an effect for GetPackages.");
            {
                var firstPackageSet = await IntegrationTestFixture.GetConsistentApiResponse<Package>(packagesUrl);
                Assert.Equal(MaxResultsPerPage, firstPackageSet.Data.Count);

                var continuedPackageSet = await IntegrationTestFixture.GetConsistentApiResponse<Package>(packagesUrl.WithHeader(HeaderConstants.ContinuationToken, firstPackageSet.ContinuationToken));
                Assert.Equal(MaxResultsPerPage, continuedPackageSet.Data.Count);
                Assert.False(firstPackageSet.Data.Intersect(continuedPackageSet.Data).Any());
            }

            this.log.WriteLine("Tests that GetPackageManifests returns the expected package.");
            {
                var packageManifests = await IntegrationTestFixture.GetConsistentApiResponse<PackageManifest>(powerToysPackageManifestUrl);
                Assert.NotEmpty(packageManifests.Data);
                Assert.Equal(packageIdentifier, packageManifests.Data.First().PackageIdentifier);
            }

            this.log.WriteLine("Tests that ContinuationToken has an effect for GetPackageManifests.");
            {
                var firstPackageManifestSet = await IntegrationTestFixture.GetConsistentApiResponse<Package>(packageManifestsUrl);
                Assert.Equal(MaxResultsPerPage, firstPackageManifestSet.Data.Count);

                var continuedPackageManifestSet = await IntegrationTestFixture.GetConsistentApiResponse<Package>(packageManifestsUrl.WithHeader(HeaderConstants.ContinuationToken, firstPackageManifestSet.ContinuationToken));
                Assert.Equal(MaxResultsPerPage, continuedPackageManifestSet.Data.Count);
                Assert.False(firstPackageManifestSet.Data.Intersect(continuedPackageManifestSet.Data).Any());
            }
        }

        private async Task InitializeAsync(PackageTestHelper helper)
        {
            if (helper.StorageCleanup != null)
            {
                foreach (var cleanup in helper.StorageCleanup)
                {
                    await cleanup.CleanupAsync(this.fixture);
                }
            }

            if (helper.StorageSetup != null)
            {
                foreach (var setup in helper.StorageSetup)
                {
                    await setup.SetupAsync(this.fixture);
                }
            }
        }
    }
}
