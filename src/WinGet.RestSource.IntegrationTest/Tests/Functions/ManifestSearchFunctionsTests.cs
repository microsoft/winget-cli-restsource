// -----------------------------------------------------------------------
// <copyright file="ManifestSearchFunctionsTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Tests.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Flurl;
    using Flurl.Http;
    using Microsoft.Azure.Cosmos.Serialization.HybridRow;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Fixtures;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Helpers;
    using Microsoft.WinGet.RestSource.IntegrationTest.Tests.Functions.TestData;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Constants.Enumerations;
    using Microsoft.WinGet.RestSource.Utils.Models.Objects;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Manifest search functions tests.
    /// </summary>
    [Collection("IntegrationTestCollection")]
    public class ManifestSearchFunctionsTests
    {
        private const int MaxResultsPerPage = 20;
        private const string PowerToysPackageIdentifier = "RestIntegrationTest.PowerToys";
        private ITestOutputHelper log;

        private IntegrationTestFixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchFunctionsTests"/> class.
        /// </summary>
        /// <param name="log">An object of type <see cref="ITestOutputHelper"/>.</param>
        /// <param name="fixture">An object <see cref="IntegrationTestFixture"/>.</param>
        public ManifestSearchFunctionsTests(ITestOutputHelper log, IntegrationTestFixture fixture)
        {
            this.log = log;
            this.fixture = fixture;
        }

        /// <summary>
        /// Manifest search function tests.
        /// </summary>
        /// <param name="helper">An object of type <see cref="ManifestSearchTestHelper"/>.</param>
        /// <returns>An object of <see cref="Task"/> representing an asynchronous operation.</returns>
        [Theory]
        [ClassData(typeof(ManifestSearchFunctionTestData))]
        public async Task ManifestSearchFunctionsTests_Request_Response_Cycle(ManifestSearchTestHelper helper)
        {
            Assert.NotNull(helper);
            Assert.NotNull(helper.EndPointRequest);
            Assert.NotNull(helper.SearchResponses);
            Assert.True(helper.MaximumAllowedResponseTime > 0);

            await this.InitializeAsync(helper);

            string requestBody = this.fixture.TestCollateral.FetchTestCollateralContent(
                helper.EndPointRequest.JsonFileName, Common.TestCollateral.TestCollateralType.RequestFiles);
            var manifestSearchRequest = JsonSerializer.Deserialize<ManifestSearchRequest>(requestBody);

            string url = this.fixture.RestSourceUrl.AppendPathSegment(helper.EndPointRequest.RelativeUrlPath);

            var searchResult = await this.GetSearchResultsAsync(
                url, manifestSearchRequest, maxAllowedTimeElapsed: helper.MaximumAllowedResponseTime);

            VerifyResult(searchResult, helper.SearchResponses);
        }

        /// <summary>
        /// Verifies that the search API correctly handles a search using a search query term.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SearchUsingQuery()
        {
            string url = this.fixture.RestSourceUrl.AppendPathSegment("manifestSearch");
            await this.SetUpRandomTestData(nameof(this.SearchUsingQuery));
            this.log.WriteLine("Tests that SearchPackageManifests returns the expected results when using the Query property.");
            {
                await this.TestSearchQuery("PowerToys", MatchType.Exact, PowerToysPackageIdentifier);
                await this.TestSearchQuery("powertoys", MatchType.CaseInsensitive, PowerToysPackageIdentifier);
                await this.TestSearchQuery("PowerT", MatchType.StartsWith, PowerToysPackageIdentifier);
                await this.TestSearchQuery("owertoy", MatchType.Substring, PowerToysPackageIdentifier);
                await this.TestSearchQuery("nonexistentpackage", MatchType.Substring);
            }

            this.log.WriteLine("Tests that using ContinuationToken with SearchPackageManifests allows us to retrieve all manifests.");
            {
                /* TODO: Test writing to the repository between reads on the continuation token. */

                var allResults = new HashSet<ManifestSearchResponse>();
                string continuationToken = null;
                do
                {
                    var manifestSearchRequest = new ManifestSearchRequest();
                    var result = await this.GetSearchResultsAsync(url, manifestSearchRequest, continuationToken);
                    allResults.UnionWith(result.Data);
                    continuationToken = result.ContinuationToken;
                }
                while (continuationToken != null);
                Assert.True(allResults.Count > MaxResultsPerPage);
            }
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

        private static void VerifyResult(
            SearchApiResponse<List<ManifestSearchResponse>> results,
            SearchResponseTestHelper[] expectedSearchResponses)
        {
            Assert.Equal(expectedSearchResponses.Count(), results.Data.Count);
            var expectedPackageIdentifiers = results.Data.ConvertAll<string>(i => i.PackageIdentifier).ToHashSet();
            var expectedPublishers = results.Data.ConvertAll<string>(i => i.Publisher).ToHashSet();
            Dictionary<string, ISet<string>> packageIdentifierToVersionsMap = new Dictionary<string, ISet<string>>();
            results.Data.ForEach(
                (i) =>
                {
                    packageIdentifierToVersionsMap[i.PackageIdentifier] =
                                i.Versions.ConvertAll<string>((i) => i.PackageVersion).ToHashSet();
                });

            foreach (var expectedSearchResponse in expectedSearchResponses)
            {
                Assert.Contains(expectedSearchResponse.PackageIdentifier, expectedPackageIdentifiers);
                Assert.Contains(expectedSearchResponse.Publisher, expectedPublishers);

                Assert.Equal(
                    expectedSearchResponse.ExpectedVersions.ToHashSet(),
                    packageIdentifierToVersionsMap[expectedSearchResponse.PackageIdentifier]);
            }
        }

        private async Task<SearchApiResponse<List<ManifestSearchResponse>>> GetSearchResultsAsync(
            string url,
            ManifestSearchRequest manifestSearchRequest,
            string continuationToken = null,
            int maxAllowedTimeElapsed = int.MaxValue)
        {
            IFlurlResponse response;

            DateTime startTime = DateTime.UtcNow;
            if (continuationToken != null)
            {
                response = await url.WithHeader(HeaderConstants.ContinuationToken, continuationToken).PostJsonAsync(manifestSearchRequest);
            }
            else
            {
                response = await url.PostJsonAsync(manifestSearchRequest);
            }

            DateTime endTime = DateTime.UtcNow;
            double timeElapsed = endTime.Subtract(startTime).TotalSeconds;
            Assert.True(timeElapsed < maxAllowedTimeElapsed, $"{timeElapsed} greater than maximum allowed time.");

            return await response.GetJsonAsync<SearchApiResponse<List<ManifestSearchResponse>>>();
        }

        private async Task TestSearchQuery(string value, string matchType, params string[] expectedPackageIdentifiers)
        {
            string url = this.fixture.RestSourceUrl.AppendPathSegment("manifestSearch");
            var manifestSearchRequest = new ManifestSearchRequest();
            manifestSearchRequest.Query = new RestSource.Utils.Models.Objects.SearchRequestMatch();
            manifestSearchRequest.Query.KeyWord = value;
            manifestSearchRequest.Query.MatchType = matchType;
            var results = await this.GetSearchResultsAsync(url, manifestSearchRequest);
            VerifyResult(results, expectedPackageIdentifiers);
        }

        private async Task SetUpRandomTestData(string identifier)
        {
            StorageCleanup storageCleanup = new StorageCleanup()
            {
                EndPointRequests = new EndPointRequest[]
                {
                    new EndPointRequest()
                    {
                        RelativeUrlPath = $"packageManifests/{PowerToysPackageIdentifier}",
                    },
                },
            };

            await storageCleanup.CleanupAsync(this.fixture);

            StorageSetup storageSetup = new StorageSetup()
            {
                EndPointRequests = new EndPointRequest[]
                {
                    new EndPointRequest()
                    {
                        RelativeUrlPath = TestDataConstants.PackageManifestEndpointRelativePath,
                        JsonFileName = "powertoys.json",
                        TestCollateralType = TestCollateral.TestCollateralType.Manifests,
                    },
                },
            };

            await storageSetup.SetupAsync(this.fixture);

            EndPointRequest[] requests = ManifestSearchFunctionDataFactory.GetRandomTestEndpointRequest(identifier, 100);

            StorageSetup randomDataStorageSetup = new StorageSetup()
            {
                EndPointRequests = requests,
            };

            await randomDataStorageSetup.SetupAsync(this.fixture, true);
        }

        private async Task InitializeAsync(ManifestSearchTestHelper helper)
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
