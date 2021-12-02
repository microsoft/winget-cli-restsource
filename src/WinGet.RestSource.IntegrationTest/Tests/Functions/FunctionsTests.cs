// -----------------------------------------------------------------------
// <copyright file="FunctionsTests.cs" company="Microsoft Corporation">
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
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Constants.Enumerations;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// CosmosDataStore Tests.
    /// </summary>
    public class FunctionsTests : TestsBase, IAsyncLifetime
    {
        private const int MaxResultsPerPage = 20;
        private readonly string packagesUrl;
        private readonly string packageManifestsUrl;
        private readonly string powerToysPackageManifestUrl;
        private readonly string powerToysPackageUrl;
        private string powerToysManifestJson;
        private bool modifiedManifest;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionsTests"/> class.
        /// </summary>
        /// <param name="log">ITestOutputHelper.</param>
        public FunctionsTests(ITestOutputHelper log)
            : base(log)
        {
            this.packagesUrl = this.RestSourceUrl.AppendPathSegment("packages");
            this.packageManifestsUrl = this.RestSourceUrl.AppendPathSegment("packageManifests");
            this.powerToysPackageManifestUrl = this.packageManifestsUrl.AppendPathSegment(PowerToysPackageIdentifier);
            this.powerToysPackageUrl = this.packagesUrl.AppendPathSegment(PowerToysPackageIdentifier);
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            // Store PowerToys manifest so we can ensure it's re-added at the end.
            var packageManifests = await GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            if (packageManifests == null)
            {
                throw new System.IO.InvalidDataException($"{PowerToysPackageIdentifier} isn't present in REST source. Populate source prior to running tests");
            }
            else
            {
                var powerToysManifest = packageManifests.Data.SingleOrDefault();
                this.powerToysManifestJson = JsonConvert.SerializeObject(powerToysManifest);
            }
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
            // Only run this test if the setting is explicitly set.
            Skip.IfNot(this.RunWriteTests);

            var packageManifestsResult = await GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            PackageManifest addedManifest = packageManifestsResult.Data.Single();

            // Start by deleting the manifest from the source and verify it's gone.
            await this.RunAndTestProtectedApi(this.powerToysPackageManifestUrl, HttpMethod.Delete);
            this.modifiedManifest = true;
            packageManifestsResult = await GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            Assert.Null(packageManifestsResult);

            // Now add it and verify it's present.
            await this.RunAndTestProtectedApi(this.packageManifestsUrl, HttpMethod.Post, addedManifest);
            packageManifestsResult = await GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            Assert.Equal(PowerToysPackageIdentifier, packageManifestsResult.Data.Single().PackageIdentifier);

            // Now update it and verify that it got updated.
            string updatedName = "BOGUS_NAME";
            addedManifest.Versions.First().DefaultLocale.PackageName = updatedName;
            await this.RunAndTestProtectedApi(this.powerToysPackageManifestUrl, HttpMethod.Put, addedManifest);
            packageManifestsResult = await GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            Assert.Equal(updatedName, packageManifestsResult.Data.Single().Versions.First().DefaultLocale.PackageName);

            // Now delete it, and verify it's gone.
            await this.RunAndTestProtectedApi(this.powerToysPackageManifestUrl, HttpMethod.Delete);
            packageManifestsResult = await GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            Assert.Null(packageManifestsResult);

            // Now add just the package, and verify it's present.
            await this.RunAndTestProtectedApi(this.packagesUrl, HttpMethod.Post, addedManifest);
            var packageResult = await GetConsistentApiResponse<PackageManifest>(this.powerToysPackageUrl);
            Assert.Equal(PowerToysPackageIdentifier, packageResult.Data.Single().PackageIdentifier);

            // Now add a version, installer, and locale to the package.
            VersionExtended addedVersion = addedManifest.Versions.First();
            string versionsUrl = this.powerToysPackageUrl.AppendPathSegment("versions");
            await this.RunAndTestProtectedApi(versionsUrl, HttpMethod.Post, addedVersion);
            string versionUrl = versionsUrl.AppendPathSegment(addedVersion.PackageVersion);
            await this.RunAndTestProtectedApi(versionUrl.AppendPathSegment("installers"), HttpMethod.Post, addedVersion.Installers.First());
            await this.RunAndTestProtectedApi(versionUrl.AppendPathSegment("locales"), HttpMethod.Post, addedVersion.DefaultLocale);

            // Verify that everything got added correctly.
            packageManifestsResult = await GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
            var resultVersion = packageManifestsResult.Data.First().Versions.First();
            Assert.Equal(addedVersion.PackageVersion, resultVersion.PackageVersion);
            Assert.Equal(addedVersion.DefaultLocale.PackageName, resultVersion.DefaultLocale.PackageName);
            Assert.Equal(addedVersion.Installers.First().InstallerUrl, resultVersion.Installers.First().InstallerUrl);

            // Delete the package using the packages API, and verify it's gone.
            await this.RunAndTestProtectedApi(this.powerToysPackageUrl, HttpMethod.Delete);
            packageResult = await GetConsistentApiResponse<PackageManifest>(this.powerToysPackageUrl);
            Assert.Null(packageResult);

            // Lastly, re-add the original copy.
            await this.RunAndTestProtectedApi(this.packageManifestsUrl, HttpMethod.Post, this.powerToysManifestJson);
            this.modifiedManifest = false;
        }

        /// <summary>
        /// Verifies the GetPackage* APIs.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetPackages()
        {
            this.Log.WriteLine("Tests that GetPackages returns the expected package.");
            {
                var packages = await GetConsistentApiResponse<Package>(this.powerToysPackageUrl);
                Assert.NotEmpty(packages.Data);
                Assert.Equal(PowerToysPackageIdentifier, packages.Data.First().PackageIdentifier);
            }

            this.Log.WriteLine("Tests that ContinuationToken has an effect for GetPackages.");
            {
                var firstPackageSet = await GetConsistentApiResponse<Package>(this.packagesUrl);
                Assert.Equal(MaxResultsPerPage, firstPackageSet.Data.Count);

                var continuedPackageSet = await GetConsistentApiResponse<Package>(this.packagesUrl.WithHeader(HeaderConstants.ContinuationToken, firstPackageSet.ContinuationToken));
                Assert.Equal(MaxResultsPerPage, continuedPackageSet.Data.Count);
                Assert.False(firstPackageSet.Data.Intersect(continuedPackageSet.Data).Any());
            }

            this.Log.WriteLine("Tests that GetPackageManifests returns the expected package.");
            {
                var packageManifests = await GetConsistentApiResponse<PackageManifest>(this.powerToysPackageManifestUrl);
                Assert.NotEmpty(packageManifests.Data);
                Assert.Equal(PowerToysPackageIdentifier, packageManifests.Data.First().PackageIdentifier);
            }

            this.Log.WriteLine("Tests that GetPackageManifests returns the expected package and version.");
            {
                const string version = "0.37.0";
                var packageManifests = await GetConsistentApiResponse<PackageManifest>(
                    this.RestSourceUrl
                    .AppendPathSegment("packageManifests")
                    .AppendPathSegment(PowerToysPackageIdentifier)
                    .SetQueryParam("Version", version));
                Assert.NotEmpty(packageManifests.Data);
                Assert.Equal(PowerToysPackageIdentifier, packageManifests.Data.First().PackageIdentifier);
                Assert.Equal(version, packageManifests.Data.First().Versions.Single().PackageVersion);
            }

            this.Log.WriteLine("Tests that ContinuationToken has an effect for GetPackageManifests.");
            {
                var firstPackageManifestSet = await GetConsistentApiResponse<Package>(this.packageManifestsUrl);
                Assert.Equal(MaxResultsPerPage, firstPackageManifestSet.Data.Count);

                var continuedPackageManifestSet = await GetConsistentApiResponse<Package>(this.packageManifestsUrl.WithHeader(HeaderConstants.ContinuationToken, firstPackageManifestSet.ContinuationToken));
                Assert.Equal(MaxResultsPerPage, continuedPackageManifestSet.Data.Count);
                Assert.False(firstPackageManifestSet.Data.Intersect(continuedPackageManifestSet.Data).Any());
            }
        }

        /// <summary>
        /// Verifies that the search API correctly handles a search using a search query term.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SearchUsingQuery()
        {
            this.Log.WriteLine("Tests that SearchPackageManifests returns the expected results when using the Query property.");
            {
                await this.TestSearchQuery("PowerToys", MatchType.Exact, PowerToysPackageIdentifier);
                await this.TestSearchQuery("powertoys", MatchType.CaseInsensitive, PowerToysPackageIdentifier);
                await this.TestSearchQuery("PowerT", MatchType.StartsWith, PowerToysPackageIdentifier);
                await this.TestSearchQuery("owertoy", MatchType.Substring, PowerToysPackageIdentifier);
                await this.TestSearchQuery("nonexistentpackage", MatchType.Substring);
            }

            this.Log.WriteLine("Tests that using ContinuationToken with SearchPackageManifests allows us to retrieve all manifests.");
            {
                /* TODO: Test writing to the repository between reads on the continuation token. */

                var allResults = new HashSet<ManifestSearchResponse>();
                string continuationToken = null;
                do
                {
                    var manifestSearchRequest = new ManifestSearchRequest();
                    var result = await this.GetSearchResults(manifestSearchRequest, continuationToken);
                    allResults.UnionWith(result.Data);
                    continuationToken = result.ContinuationToken;
                }
                while (continuationToken != null);
                Assert.True(allResults.Count > MaxResultsPerPage);
            }
        }

        /// <summary>
        /// Verifies that the API correctly handles a search using filters.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SearchUsingFilter()
        {
            await this.TestSearchFilter(PackageMatchFields.PackageName, "PowerToys", MatchType.Exact, PowerToysPackageIdentifier);
            await this.TestSearchFilter(PackageMatchFields.PackageName, "powertoys", MatchType.CaseInsensitive, PowerToysPackageIdentifier);
            await this.TestSearchFilter(PackageMatchFields.PackageName, "PowerT", MatchType.StartsWith, PowerToysPackageIdentifier);
            await this.TestSearchFilter(PackageMatchFields.PackageName, "owertoy", MatchType.Substring, PowerToysPackageIdentifier);
            await this.TestSearchFilter(PackageMatchFields.PackageName, "nonexistentpackage", MatchType.Substring);
        }

        private static void VerifyResult(SearchApiResponse<List<ManifestSearchResponse>> results, params string[] expectedPackageIdentifiers)
        {
            Assert.Equal(expectedPackageIdentifiers.Length, results.Data.Count);
            var packageIdentifierResults = results.Data.Select(i => i.PackageIdentifier).ToList();
            foreach (var expectedPackageIdentifier in expectedPackageIdentifiers)
            {
                Assert.Contains(expectedPackageIdentifier, packageIdentifierResults);
            }
        }

        private static async Task<ApiResponse<List<T>>> GetConsistentApiResponse<T>(string url)
        {
            return await GetConsistentApiResponse<T>(new FlurlRequest(url));
        }

        private static async Task<ApiResponse<List<T>>> GetConsistentApiResponse<T>(IFlurlRequest url)
        {
            string json = await url.GetStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<ApiResponse<List<T>>>(json);
            }
            catch (JsonSerializationException)
            {
                var singleResult = JsonConvert.DeserializeObject<ApiResponse<T>>(json);
                return new ApiResponse<List<T>>(new List<T> { singleResult.Data });
            }
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
            await method(url.SetQueryParam("code", this.FunctionsHostKey));
        }

        private async Task<SearchApiResponse<List<ManifestSearchResponse>>> GetSearchResults(ManifestSearchRequest manifestSearchRequest, string continuationToken = null)
        {
            string url = this.RestSourceUrl.AppendPathSegment("manifestSearch");

            IFlurlResponse response;
            if (continuationToken != null)
            {
                response = await url.WithHeader(HeaderConstants.ContinuationToken, continuationToken).PostJsonAsync(manifestSearchRequest);
            }
            else
            {
                response = await url.PostJsonAsync(manifestSearchRequest);
            }

            return await response.GetJsonAsync<SearchApiResponse<List<ManifestSearchResponse>>>();
        }

        private async Task TestSearchQuery(string value, string matchType, params string[] expectedPackageIdentifiers)
        {
            var manifestSearchRequest = new ManifestSearchRequest();
            manifestSearchRequest.Query = new RestSource.Utils.Models.Objects.SearchRequestMatch();
            manifestSearchRequest.Query.KeyWord = value;
            manifestSearchRequest.Query.MatchType = matchType;
            var results = await this.GetSearchResults(manifestSearchRequest);
            VerifyResult(results, expectedPackageIdentifiers);
        }

        private async Task TestSearchFilter(string packageMatchField, string value, string matchType, params string[] expectedPackageIdentifiers)
        {
            var manifestSearchRequest = new ManifestSearchRequest();
            manifestSearchRequest.Filters = new RestSource.Utils.Models.Arrays.SearchRequestPackageMatchFilter
            {
                new RestSource.Utils.Models.Objects.SearchRequestPackageMatchFilter { PackageMatchField = packageMatchField, RequestMatch = new RestSource.Utils.Models.Objects.SearchRequestMatch { KeyWord = value, MatchType = matchType } },
            };

            var results = await this.GetSearchResults(manifestSearchRequest);
            VerifyResult(results, expectedPackageIdentifiers);
        }
    }
}
