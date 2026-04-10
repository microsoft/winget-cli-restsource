// -----------------------------------------------------------------------
// <copyright file="PackageManifestFunctionsTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Flurl;
    using Flurl.Http;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Fixtures;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Helpers;
    using Microsoft.WinGet.RestSource.IntegrationTest.Functions.TestData;
    using Microsoft.WinGet.RestSource.Utils.Constants.Enumerations;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Package manifest function Tests.
    /// </summary>
    [Collection("IntegrationTestCollection")]
    public class PackageManifestFunctionsTests : IAsyncLifetime
    {
        private const int MaxResultsPerPage = 20;
        private readonly string packagesUrl;
        private readonly string packageManifestsUrl;
        private readonly string powerToysPackageManifestUrl;
        private readonly string powerToysPackageUrl;
        private readonly IntegrationTestFixture fixture;
        private readonly ITestOutputHelper log;

        private string powerToysManifestJson;
        private bool modifiedManifest;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageManifestFunctionsTests"/> class.
        /// </summary>
        /// <param name="log">ITestOutputHelper.</param>
        /// <param name="fixture">An object of integration test fixture.</param>
        public PackageManifestFunctionsTests(ITestOutputHelper log, IntegrationTestFixture fixture)
        {
            string packageIdentifer = TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1;
            this.fixture = fixture;

            this.packagesUrl = this.fixture.RestSourceUrl.AppendPathSegment("packages");
            this.packageManifestsUrl = this.fixture.RestSourceUrl.AppendPathSegment(TestDataConstants.PackageManifestEndpointRelativePath);
            this.powerToysPackageManifestUrl = this.packageManifestsUrl.AppendPathSegment(packageIdentifer);
            this.powerToysPackageUrl = this.packagesUrl.AppendPathSegment(packageIdentifer);
            this.fixture = fixture;
            this.log = log;
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            await this.CleanSource();
            await this.PopulateSource();

            // TODO: remove this when tests move to data driven approach.
            var packageManifestsResult = await IntegrationTestFixture.GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);

            var powerToysManifest = packageManifestsResult.Data.SingleOrDefault();
            this.powerToysManifestJson = JsonConvert.SerializeObject(powerToysManifest);
        }

        /// <inheritdoc/>
        public async Task DisposeAsync()
        {
            // Restore the PowerToys manifest from backup if necessary.
            if (this.modifiedManifest)
            {
                await this.RunAndTestProtectedApi(this.powerToysPackageManifestUrl, HttpMethod.Delete);
                await this.RunAndTestProtectedApi(this.packageManifestsUrl, HttpMethod.Post, this.powerToysManifestJson);
            }
        }

        /// <summary>
        /// Verifies the various CRUD operations exposed by the API.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [SkippableFact]
        public async Task CreateUpdateReadDeleteTest()
        {
            string packageIdentifier = TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1;
            var packageManifestsResult = await IntegrationTestFixture.GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            PackageManifest addedManifest = packageManifestsResult.Data.Single();

            // Start by deleting the manifest from the source and verify it's gone.
            await this.RunAndTestProtectedApi(this.powerToysPackageManifestUrl, HttpMethod.Delete);
            this.modifiedManifest = true;
            packageManifestsResult = await IntegrationTestFixture.GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            Assert.Null(packageManifestsResult);

            // Now add it and verify it's present.
            await this.RunAndTestProtectedApi(this.packageManifestsUrl, HttpMethod.Post, addedManifest);
            packageManifestsResult = await IntegrationTestFixture.GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            Assert.Equal(packageIdentifier, packageManifestsResult.Data.Single().PackageIdentifier);

            // Now update it and verify that it got updated.
            string updatedName = "BOGUS_NAME";
            addedManifest.Versions.First().DefaultLocale.PackageName = updatedName;
            await this.RunAndTestProtectedApi(this.powerToysPackageManifestUrl, HttpMethod.Put, addedManifest);
            packageManifestsResult = await IntegrationTestFixture.GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            Assert.Equal(updatedName, packageManifestsResult.Data.Single().Versions.First().DefaultLocale.PackageName);

            // Now delete it, and verify it's gone.
            await this.RunAndTestProtectedApi(this.powerToysPackageManifestUrl, HttpMethod.Delete);
            packageManifestsResult = await IntegrationTestFixture.GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            Assert.Null(packageManifestsResult);

            // Now add just the package, and verify it's present.
            await this.RunAndTestProtectedApi(this.packagesUrl, HttpMethod.Post, addedManifest);
            var packageResult = await IntegrationTestFixture.GetConsistentApiResponse<PackageManifest>(this.powerToysPackageUrl);
            Assert.Equal(packageIdentifier, packageResult.Data.Single().PackageIdentifier);

            // Now add a version, installer, and locale to the package.
            VersionExtended addedVersion = addedManifest.Versions.First();
            string versionsUrl = this.powerToysPackageUrl.AppendPathSegment("versions");
            await this.RunAndTestProtectedApi(versionsUrl, HttpMethod.Post, addedVersion);
            string versionUrl = versionsUrl.AppendPathSegment(addedVersion.PackageVersion);
            await this.RunAndTestProtectedApi(versionUrl.AppendPathSegment("installers"), HttpMethod.Post, addedVersion.Installers.First());
            await this.RunAndTestProtectedApi(versionUrl.AppendPathSegment("locales"), HttpMethod.Post, addedVersion.DefaultLocale);

            // Verify that everything got added correctly.
            packageManifestsResult = await IntegrationTestFixture.GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            var resultVersion = packageManifestsResult.Data.First().Versions.First();
            Assert.Equal(addedVersion.PackageVersion, resultVersion.PackageVersion);
            Assert.Equal(addedVersion.DefaultLocale.PackageName, resultVersion.DefaultLocale.PackageName);
            Assert.Equal(addedVersion.Installers.First().InstallerUrl, resultVersion.Installers.First().InstallerUrl);

            // Delete the package using the packages API, and verify it's gone.
            await this.RunAndTestProtectedApi(this.powerToysPackageUrl, HttpMethod.Delete);
            packageResult = await IntegrationTestFixture.GetConsistentApiResponse<PackageManifest>(this.powerToysPackageUrl);
            Assert.Null(packageResult);

            // Lastly, re-add the original copy.
            await this.RunAndTestProtectedApi(this.packageManifestsUrl, HttpMethod.Post, this.powerToysManifestJson);
            this.modifiedManifest = false;
        }

        /// <summary>
        /// Represents test for package manifest versions.
        /// </summary>
        /// <param name="helper">An object of type <see cref="EndPointRequest"/>.</param>
        [Theory]
        [ClassData(typeof(PackageManifestVersionTestData))]
        public async void PackageManifestVersionsTest(PackageTestHelper helper)
        {
            Assert.NotNull(helper.TestId);
            Assert.NotNull(helper.PackageIdentifier);
            Assert.NotNull(helper.EndPointRequest);
            Assert.NotNull(helper.EndPointRequest.RelativeUrlPath);

            string url = $"{this.fixture.RestSourceUrl}/{helper.EndPointRequest.RelativeUrlPath.TrimStart('/')}";
            var response = await IntegrationTestFixture.GetConsistentApiResponse<PackageManifest>(url);

            if (helper.ApiResponse == PackageTestHelper.ApiResponseScenario.IdentifierNotFound)
            {
                Assert.Null(response);
            }

            if (helper.ApiResponse == PackageTestHelper.ApiResponseScenario.NoApplicableVersion)
            {
                int versionsCount = response.Data[0].Versions.Count();
                Assert.Equal(helper.PackageIdentifier, response.Data[0].PackageIdentifier);
                Assert.True(versionsCount == 0, $"Version count: {versionsCount} must have a value of 0 for NoApplicationVersion scenario");
            }

            if (helper.ApiResponse == PackageTestHelper.ApiResponseScenario.VersionFound)
            {
                int versionsCount = response.Data[0].Versions.Count();
                Assert.Equal(helper.PackageIdentifier, response.Data[0].PackageIdentifier);
                Assert.True(versionsCount == 1, $"Version count: {versionsCount} must have a value of 1 for VersionFound scenario");
                PackageManifest packageManifest = response.Data[0];
                Assert.Equal(helper.ExpectedVersions[0], packageManifest.Versions[0].PackageVersion);
            }
        }

        private async Task CleanSource()
        {
            string packageIdentifier = TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item1;
            var storageCleanup = new StorageCleanup()
            {
                EndPointRequests = new EndPointRequest[]
                {
                    new EndPointRequest()
                    {
                        RelativeUrlPath = $"packageManifests/{packageIdentifier}",
                    },
                },
            };

            await storageCleanup.CleanupAsync(this.fixture);
        }

        /// <summary>
        /// Populate source with manifests.
        /// </summary>
        private async Task PopulateSource()
        {
            var storageSetup = new StorageSetup()
            {
                EndPointRequests = new EndPointRequest[]
                {
                    new EndPointRequest()
                    {
                        RelativeUrlPath = TestDataConstants.PackageManifestEndpointRelativePath,
                        JsonFileName = TestDataConstants.GetTestData(TestDataConstants.TestData.PowerToys).Item2,
                    },
                },
            };

            await storageSetup.SetupAsync(this.fixture);
        }

        private async Task RunAndTestProtectedApi(string url, HttpMethod httpMethod, object data = null)
        {
            Func<string, Task> method = data == null ?
                (url => url.SendAsync(httpMethod)) :
                    data is string str ?
                        (url => url.SendStringAsync(httpMethod, str)) :
                        (url => url.SendJsonAsync(httpMethod, data));

            // Protected API should fail without an authorization code.
            await Assert.ThrowsAsync<FlurlHttpException>(() => method(url));

            // Should succeed once the authorization key is added.
            await method(url.SetQueryParam("code", this.fixture.FunctionsHostKey));
        }
    }
}
