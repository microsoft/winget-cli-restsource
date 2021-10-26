// -----------------------------------------------------------------------
// <copyright file="CosmosDataStoreTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.RestSource.Cosmos
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.Winget.RestSource.UnitTest.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Constants.Enumerations;
    using Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;
    using Arrays = Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Objects = Microsoft.WinGet.RestSource.Utils.Models.Objects;

    /// <summary>
    /// CosmosDataStore Tests.
    /// </summary>
    public class CosmosDataStoreTests : IAsyncLifetime
    {
        private const string ManifestsPath = @"Tests\RestSource\Cosmos\manifests.json";
        private const int ManifestCount = 500;
        private const int TestDatabaseRUs = 4000;
        private const string PowerToysPackageIdentifier = "Microsoft.PowerToys";

        private readonly ITestOutputHelper log;
        private readonly IConfigurationRoot configuration;
        private readonly CosmosDataStore cosmosDataStore;
        private IList<CosmosPackageManifest> allTestManifests;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDataStoreTests"/> class.
        /// </summary>
        /// <param name="log">ITestOutputHelper.</param>
        public CosmosDataStoreTests(ITestOutputHelper log)
        {
            this.log = log;

            this.configuration = new ConfigurationBuilder()
                .AddJsonFile("test.runsettings.json")
                .Build();

            string endpoint = this.configuration[CosmosConnectionConstants.CosmosAccountEndpointSetting] ?? throw new System.IO.InvalidDataException();
            string readOnlyKey = this.configuration[CosmosConnectionConstants.CosmosReadOnlyKeySetting] ?? throw new System.IO.InvalidDataException();
            string readWriteKey = this.configuration[CosmosConnectionConstants.CosmosReadWriteKeySetting] ?? throw new System.IO.InvalidDataException();
            string databaseId = this.configuration[CosmosConnectionConstants.DatabaseNameSetting] ?? throw new System.IO.InvalidDataException();
            string containerId = this.configuration[CosmosConnectionConstants.ContainerNameSetting] ?? throw new System.IO.InvalidDataException();

            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<CosmosDataStore>();
            this.cosmosDataStore = new CosmosDataStore(logger, endpoint, readWriteKey, readOnlyKey, databaseId, containerId);
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            var sw = Stopwatch.StartNew();

            string json = System.IO.File.ReadAllText(ManifestsPath);
            this.allTestManifests = JsonConvert.DeserializeObject<List<CosmosPackageManifest>>(json);

            // Ensure container exists prior to getting count
            await this.cosmosDataStore.CreateContainer(TestDatabaseRUs);

            int itemCount = await this.cosmosDataStore.Count();
            if (itemCount == ManifestCount)
            {
                this.log.WriteLine($"Test container already contains the expected number of items ({ManifestCount}), no need to re-initialize");
            }
            else
            {
                this.log.WriteLine($"Test container does not contain the expected number of items, re-initializing. Expected: {ManifestCount}, Actual: {itemCount}");
                await this.cosmosDataStore.DeleteContainer();
                await this.cosmosDataStore.CreateContainer(TestDatabaseRUs);

                // Only add the first 500 manifests to the database.
                await this.allTestManifests.Take(ManifestCount).AsyncParallelForEach(manifest => this.cosmosDataStore.AddPackageManifest(manifest), 30);
            }

            this.log.WriteLine($"Initialization completed in {sw.Elapsed}");
        }

        /// <inheritdoc/>
        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Verifies the various CRUD operations exposed by the CosmosDataStore.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CreateUpdateReadDeleteTest()
        {
            var addedManifest = this.allTestManifests.Skip(ManifestCount).First();

            var packageManifestsResult = await this.cosmosDataStore.GetPackageManifests(addedManifest.PackageIdentifier);
            Assert.Empty(packageManifestsResult.Items);

            await this.cosmosDataStore.AddPackageManifest(addedManifest);
            Assert.Equal(ManifestCount + 1, await this.cosmosDataStore.Count());

            packageManifestsResult = await this.cosmosDataStore.GetPackageManifests(addedManifest.PackageIdentifier);
            Assert.Equal(addedManifest.PackageIdentifier, packageManifestsResult.Items.Single().PackageIdentifier);

            string updatedName = "BOGUS_NAME";
            addedManifest.Versions.First().DefaultLocale.PackageName = updatedName;
            await this.cosmosDataStore.UpdatePackageManifest(addedManifest.PackageIdentifier, addedManifest);

            packageManifestsResult = await this.cosmosDataStore.GetPackageManifests(addedManifest.PackageIdentifier);
            Assert.Equal(updatedName, packageManifestsResult.Items.Single().Versions.First().DefaultLocale.PackageName);

            await this.cosmosDataStore.DeletePackageManifest(addedManifest.PackageIdentifier);
            Assert.Equal(ManifestCount, await this.cosmosDataStore.Count());

            await this.cosmosDataStore.AddPackage(addedManifest);
            Assert.Equal(ManifestCount + 1, await this.cosmosDataStore.Count());

            var packageResult = await this.cosmosDataStore.GetPackages(addedManifest.PackageIdentifier);
            Assert.Equal(addedManifest.PackageIdentifier, packageResult.Items.Single().PackageIdentifier);

            var addedVersion = addedManifest.Versions.First();
            await this.cosmosDataStore.AddVersion(addedManifest.PackageIdentifier, addedVersion);
            await this.cosmosDataStore.AddInstaller(addedManifest.PackageIdentifier, addedVersion.PackageVersion, addedVersion.Installers.First());
            await this.cosmosDataStore.AddLocale(addedManifest.PackageIdentifier, addedVersion.PackageVersion, addedVersion.DefaultLocale);

            packageManifestsResult = await this.cosmosDataStore.GetPackageManifests(addedManifest.PackageIdentifier);
            var resultVersion = packageManifestsResult.Items.First().Versions.First();
            Assert.Equal(addedVersion.PackageVersion, resultVersion.PackageVersion);
            Assert.Equal(addedVersion.DefaultLocale.PackageName, resultVersion.DefaultLocale.PackageName);
            Assert.Equal(addedVersion.Installers.First().InstallerUrl, resultVersion.Installers.First().InstallerUrl);

            await this.cosmosDataStore.DeletePackage(addedManifest.PackageIdentifier);
            Assert.Equal(ManifestCount, await this.cosmosDataStore.Count());
        }

        /// <summary>
        /// Verifies the GetPackage* APIs.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetPackages()
        {
            this.log.WriteLine("Tests that GetPackages returns the expected package.");
            {
                var packages = await this.cosmosDataStore.GetPackages(PowerToysPackageIdentifier, null);
                Assert.NotEqual(0, packages.Items.Count);
                Assert.Equal(PowerToysPackageIdentifier, packages.Items.First().PackageIdentifier);
            }

            this.log.WriteLine("Tests that ContinuationToken has an effect for GetPackages.");
            {
                var firstPackageSet = await this.cosmosDataStore.GetPackages(null);
                Assert.Equal(FunctionSettingsConstants.MaxResultsPerPage, firstPackageSet.Items.Count);

                var continuedPackageSet = await this.cosmosDataStore.GetPackages(null, firstPackageSet.ContinuationToken);
                Assert.Equal(FunctionSettingsConstants.MaxResultsPerPage, continuedPackageSet.Items.Count);
                Assert.False(firstPackageSet.Items.Intersect(continuedPackageSet.Items).Any());
            }

            this.log.WriteLine("Tests that GetPackageManifests returns the expected package.");
            {
                var packageManifests = await this.cosmosDataStore.GetPackageManifests(PowerToysPackageIdentifier);
                Assert.NotEqual(0, packageManifests.Items.Count);
                Assert.Equal(PowerToysPackageIdentifier, packageManifests.Items.First().PackageIdentifier);
            }

            this.log.WriteLine("Tests that GetPackageManifests returns the expected package and version.");
            {
                const string version = "0.37.0";
                var packageManifests = await this.cosmosDataStore.GetPackageManifests(PowerToysPackageIdentifier, null, version);
                Assert.NotEqual(0, packageManifests.Items.Count);
                Assert.Equal(PowerToysPackageIdentifier, packageManifests.Items.First().PackageIdentifier);
                Assert.Equal(version, packageManifests.Items.First().Versions.Single().PackageVersion);
            }

            this.log.WriteLine("Tests that ContinuationToken has an effect for GetPackageManifests.");
            {
                var firstPackageManifestSet = await this.cosmosDataStore.GetPackageManifests(null);
                Assert.Equal(FunctionSettingsConstants.MaxResultsPerPage, firstPackageManifestSet.Items.Count);

                var continuedPackageManifestSet = await this.cosmosDataStore.GetPackageManifests(null, firstPackageManifestSet.ContinuationToken);
                Assert.Equal(FunctionSettingsConstants.MaxResultsPerPage, continuedPackageManifestSet.Items.Count);
                Assert.False(firstPackageManifestSet.Items.Intersect(continuedPackageManifestSet.Items).Any());
            }
        }

        /// <summary>
        /// Verifies that the CosmosDataStore correctly handles a search using a search query term.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SearchUsingQuery()
        {
            this.log.WriteLine("Tests that SearchPackageManifests returns the expected results when using the Query property.");
            {
                await this.TestSearchQuery("PowerToys", MatchType.Exact, PowerToysPackageIdentifier);
                await this.TestSearchQuery("powertoys", MatchType.CaseInsensitive, PowerToysPackageIdentifier);
                await this.TestSearchQuery("PowerT", MatchType.StartsWith, PowerToysPackageIdentifier);
                await this.TestSearchQuery("owerToy", MatchType.Substring, PowerToysPackageIdentifier);
            }

            this.log.WriteLine("Tests that using ContinuationToken with SearchPackageManifests allows us to retrieve all manifests.");
            {
                var allResults = new HashSet<ManifestSearchResponse>();
                string continuationToken = null;
                do
                {
                    var manifestSearchRequest = new ManifestSearchRequest();
                    var result = await this.cosmosDataStore.SearchPackageManifests(manifestSearchRequest, continuationToken);
                    allResults.UnionWith(result.Items);
                    continuationToken = result.ContinuationToken;
                }
                while (continuationToken != null);
                Assert.Equal(ManifestCount, allResults.Count);
            }
        }

        /// <summary>
        /// Verifies that the CosmosDataStore correctly handles a search using filters.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SearchUsingFilter()
        {
            await this.TestSearchFilter(PackageMatchFields.PackageName, "PowerToys", MatchType.Exact, PowerToysPackageIdentifier);
            await this.TestSearchFilter(PackageMatchFields.PackageName, "powertoys", MatchType.CaseInsensitive, PowerToysPackageIdentifier);
            await this.TestSearchFilter(PackageMatchFields.PackageName, "PowerT", MatchType.StartsWith, PowerToysPackageIdentifier);
            await this.TestSearchFilter(PackageMatchFields.PackageName, "owerToy", MatchType.Substring, PowerToysPackageIdentifier);
        }

        private async Task TestSearchQuery(string value, string matchType, string expectedPackageIdentifier)
        {
            var manifestSearchRequest = new ManifestSearchRequest();
            manifestSearchRequest.Query = new Objects.SearchRequestMatch();
            manifestSearchRequest.Query.KeyWord = value;
            manifestSearchRequest.Query.MatchType = matchType;
            var results = await this.cosmosDataStore.SearchPackageManifests(manifestSearchRequest, null);
            Assert.NotEqual(0, results.Items.Count);
            Assert.Equal(expectedPackageIdentifier, results.Items.First().PackageIdentifier);
        }

        private async Task TestSearchFilter(string packageMatchField, string value, string matchType, string expectedPackageIdentifier)
        {
            var manifestSearchRequest = new ManifestSearchRequest();
            manifestSearchRequest.Filters = new Arrays.SearchRequestPackageMatchFilter
            {
                new Objects.SearchRequestPackageMatchFilter { PackageMatchField = packageMatchField, RequestMatch = new Objects.SearchRequestMatch { KeyWord = value, MatchType = matchType } },
            };

            var results = await this.cosmosDataStore.SearchPackageManifests(manifestSearchRequest, null);
            Assert.Equal(1, results.Items.Count);
            Assert.Equal(expectedPackageIdentifier, results.Items.Single().PackageIdentifier);
        }
    }
}
