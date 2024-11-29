// -----------------------------------------------------------------------
// <copyright file="CosmosDataStore.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Cosmos
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LinqKit;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Linq;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Interfaces;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Constants.Enumerations;
    using Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Utils.Models.Objects;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGet.RestSource.Utils.Validators;

    /// <summary>
    /// Cosmos Data Store.
    /// </summary>
    public class CosmosDataStore : IApiDataStore
    {
        private const int AllElements = -1;

        private readonly ICosmosDatabase cosmosDatabase;
        private readonly ILogger<CosmosDataStore> log;

        private readonly List<string> queryList = new List<string>()
        {
            PackageMatchFields.PackageIdentifier,
            PackageMatchFields.PackageName,
            PackageMatchFields.Publisher,
            PackageMatchFields.Moniker,
            PackageMatchFields.Command,
            PackageMatchFields.Tag,
            PackageMatchFields.PackageFamilyName,
            PackageMatchFields.ProductCode,
            PackageMatchFields.UpgradeCode,

            /*********************************
             * These are currently unsupported
             *********************************
            PackageMatchFields.NormalizedPackageNameAndPublisher,
            */
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDataStore"/> class with default credentials.
        /// </summary>
        /// <param name="log">Log.</param>
        /// <param name="serviceEndpoint">Service Endpoint.</param>
        /// <param name="databaseId">Database.</param>
        /// <param name="containerId">Database container.</param>
        public CosmosDataStore(ILogger<CosmosDataStore> log, string serviceEndpoint, string databaseId, string containerId)
        {
            this.cosmosDatabase = new CosmosDatabase(serviceEndpoint, databaseId, containerId);
            this.log = log;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDataStore"/> class.
        /// </summary>
        /// <param name="log">Log.</param>
        /// <param name="serviceEndpoint">Service Endpoint.</param>
        /// <param name="databaseId">Database.</param>
        /// <param name="containerId">Database container.</param>
        /// <param name="readOnlyKey">Authorization Key with read-only permissions.</param>
        /// <param name="readWriteKey">Authorization Key with read-write permissions.</param>
        public CosmosDataStore(ILogger<CosmosDataStore> log, string serviceEndpoint, string databaseId, string containerId, string readOnlyKey, string readWriteKey)
        {
            this.cosmosDatabase = new CosmosDatabase(serviceEndpoint, databaseId, containerId, readOnlyKey, readWriteKey);
            this.log = log;
        }

        /// <summary>
        /// Check if a container exists, and if it doesn't, create it. This will make a read operation, and if the
        /// container is not found it will do a create operation.
        /// </summary>
        /// <param name="throughput">(Optional) The throughput provisioned for a container in measurement of
        /// Request Units per second in the Azure Cosmos DB service.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CreateContainer(int? throughput = null)
        {
            await this.cosmosDatabase.CreateContainer(throughput);
        }

        /// <summary>
        /// Deletes the Cosmos DB container.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteContainer()
        {
            await this.cosmosDatabase.DeleteContainer();
        }

        /// <inheritdoc />
        public async Task<int> Count()
        {
            return await this.cosmosDatabase.Count<CosmosPackageManifest>();
        }

        /// <inheritdoc />
        public async Task AddPackage(Package package)
        {
            // Convert Package to Manifest for storage
            PackageManifest packageManifest = new PackageManifest(package);
            CosmosPackageManifest cosmosPackageManifest = new CosmosPackageManifest(packageManifest);

            // Create Document and add to cosmos.
            CosmosDocument<CosmosPackageManifest> cosmosDocument = new CosmosDocument<CosmosPackageManifest>
            {
                Document = cosmosPackageManifest,
            };

            ApiDataValidator.Validate(cosmosDocument.Document);
            await this.cosmosDatabase.Add(cosmosDocument);
        }

        /// <inheritdoc/>
        public async Task DeletePackage(string packageIdentifier)
        {
            CosmosDocument<CosmosPackageManifest> cosmosDocument = new CosmosDocument<CosmosPackageManifest>
            {
                Id = packageIdentifier,
            };

            // Delete Document
            await this.cosmosDatabase.Delete<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task UpdatePackage(string packageIdentifier, Package package)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Update Package
            cosmosDocument.Document.Update(package);

            // Save Package
            ApiDataValidator.Validate(cosmosDocument.Document);
            await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task<ApiDataPage<Package>> GetPackages(string packageIdentifier, string continuationToken = null)
        {
            continuationToken = continuationToken != null ? StringEncoder.DecodeContinuationToken(continuationToken) : null;

            // Create query options
            QueryRequestOptions queryRequestOptions = new QueryRequestOptions
            {
                ResponseContinuationTokenLimitInKb = CosmosConnectionConstants.ResponseContinuationTokenLimitInKb,
                MaxItemCount = FunctionSettingsConstants.MaxResultsPerPage,
            };

            // Get iQueryable
            IQueryable<CosmosPackageManifest> query = this.cosmosDatabase.GetIQueryable<CosmosPackageManifest>(queryRequestOptions, continuationToken);

            if (!string.IsNullOrWhiteSpace(packageIdentifier))
            {
                query = query.Where(package => package.PackageIdentifier.Equals(packageIdentifier));
            }

            // Finalize Query
            FeedIterator<CosmosPackageManifest> documentQuery = query.ToFeedIterator();

            // Fetch Current Package
            ApiDataPage<CosmosPackageManifest> apiDataDocument =
                await this.cosmosDatabase.GetByDocumentQuery<CosmosPackageManifest>(documentQuery);

            // Get Versions and convert.
            ApiDataPage<Package> packages = new ApiDataPage<Package>();
            foreach (CosmosPackageManifest result in apiDataDocument.Items)
            {
                packages.Items.Add(result.ToPackage());
            }

            packages.ContinuationToken = apiDataDocument.ContinuationToken != null ? StringEncoder.EncodeContinuationToken(apiDataDocument.ContinuationToken) : null;
            return packages;
        }

        /// <inheritdoc />
        public async Task AddVersion(string packageIdentifier, Version version)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Add Version
            cosmosDocument.Document.AddVersion(version);

            // Save Document
            ApiDataValidator.Validate(cosmosDocument.Document);
            await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task DeleteVersion(string packageIdentifier, string packageVersion)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Remove Version
            cosmosDocument.Document.RemoveVersion(packageVersion);

            // Save Package
            ApiDataValidator.Validate(cosmosDocument.Document);
            await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task UpdateVersion(string packageIdentifier, string packageVersion, Version version)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Update
            cosmosDocument.Document.Versions.Update(version);

            // Save Package
            ApiDataValidator.Validate(cosmosDocument.Document);
            await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc/>
        public async Task<ApiDataPage<Version>> GetVersions(string packageIdentifier, string packageVersion)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Get Versions and convert.
            ApiDataPage<Version> versions = new ApiDataPage<Version>();
            versions.Items = cosmosDocument.Document.GetVersion(packageVersion).Select(ver => new Version(ver)).ToList();
            versions.ContinuationToken = null;
            return versions;
        }

        /// <inheritdoc />
        public async Task AddInstaller(string packageIdentifier, string packageVersion, Installer installer)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Add Installer
            cosmosDocument.Document.AddInstaller(installer, packageVersion);

            // Save Document
            ApiDataValidator.Validate(cosmosDocument.Document);
            await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task DeleteInstaller(string packageIdentifier, string packageVersion, string installerIdentifier)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Remove Installer
            cosmosDocument.Document.RemoveInstaller(installerIdentifier, packageVersion);

            // Save Document
            ApiDataValidator.Validate(cosmosDocument.Document);
            await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task UpdateInstaller(string packageIdentifier, string packageVersion, string installerIdentifier, Installer installer)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Update Installer
            cosmosDocument.Document.UpdateInstaller(installer, packageVersion);

            // Save Document
            ApiDataValidator.Validate(cosmosDocument.Document);
            await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task<ApiDataPage<Installer>> GetInstallers(string packageIdentifier, string packageVersion, string installerIdentifier)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Get Versions and convert.
            ApiDataPage<Installer> installers = new ApiDataPage<Installer>();
            installers.Items = cosmosDocument.Document.GetInstaller(installerIdentifier, packageVersion).Select(installer => new Installer(installer)).ToList();
            installers.ContinuationToken = null;
            return installers;
        }

        /// <inheritdoc />
        public async Task AddLocale(string packageIdentifier, string packageVersion, Locale locale)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Add Locale
            cosmosDocument.Document.AddLocale(locale, packageVersion);

            // Save Document
            ApiDataValidator.Validate(cosmosDocument.Document);
            await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task DeleteLocale(string packageIdentifier, string packageVersion, string packageLocale)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Remove locale
            cosmosDocument.Document.RemoveLocale(packageLocale, packageVersion);

            // Save Document
            ApiDataValidator.Validate(cosmosDocument.Document);
            await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task UpdateLocale(string packageIdentifier, string packageVersion, string packageLocale, Locale locale)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Update locale
            cosmosDocument.Document.UpdateLocale(locale, packageVersion);

            // Save Document
            ApiDataValidator.Validate(cosmosDocument.Document);
            await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task<ApiDataPage<Locale>> GetLocales(string packageIdentifier, string packageVersion, string packageLocale)
        {
            // Fetch Current Package
            CosmosDocument<CosmosPackageManifest> cosmosDocument =
                await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

            // Get Versions and convert.
            ApiDataPage<Locale> locales = new ApiDataPage<Locale>();
            locales.Items = cosmosDocument.Document.GetLocale(packageLocale, packageVersion).Select(locale => new Locale(locale)).ToList();
            locales.ContinuationToken = null;
            return locales;
        }

        /// <inheritdoc />
        public async Task AddPackageManifest(PackageManifest packageManifest)
        {
            // Create Document and add to cosmos.
            CosmosPackageManifest cosmosPackageManifest = new CosmosPackageManifest(packageManifest);
            CosmosDocument<CosmosPackageManifest> cosmosDocument = new CosmosDocument<CosmosPackageManifest>
            {
                Document = cosmosPackageManifest,
            };
            await this.cosmosDatabase.Add<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task DeletePackageManifest(string packageIdentifier)
        {
            await this.DeletePackage(packageIdentifier);
        }

        /// <inheritdoc />
        public async Task UpdatePackageManifest(string packageIdentifier, PackageManifest packageManifest)
        {
            CosmosPackageManifest cosmosPackageManifest = new CosmosPackageManifest(packageManifest);
            CosmosDocument<CosmosPackageManifest> cosmosDocument = new CosmosDocument<CosmosPackageManifest>
            {
                Document = cosmosPackageManifest,
                Id = packageIdentifier,
            };
            await this.cosmosDatabase.Update(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task<ApiDataPage<PackageManifest>> GetPackageManifests(string packageIdentifier, string continuationToken = null, string versionFilter = null, string channelFilter = null, string marketFilter = null)
        {
            // Note: GetPackageManifests should use query parameters as predicates when querying all package manifests. Currently, query parameters
            // are only exposed for GetPackageManifests with a PackageIdentifier input. Whenever query parameters are exposed to querying all
            // package manifests, this method should utilize search predicates to filter on query parameters.
            continuationToken = continuationToken != null ? StringEncoder.DecodeContinuationToken(continuationToken) : null;

            // Create feed options
            QueryRequestOptions feedOptions = new QueryRequestOptions
            {
                ResponseContinuationTokenLimitInKb = CosmosConnectionConstants.ResponseContinuationTokenLimitInKb,
                MaxItemCount = FunctionSettingsConstants.MaxResultsPerPage,
            };

            // Get iQueryable
            IQueryable<PackageManifest> query = this.cosmosDatabase.GetIQueryable<PackageManifest>(feedOptions, continuationToken);

            if (!string.IsNullOrWhiteSpace(packageIdentifier))
            {
                query = query.Where(package => package.PackageIdentifier.Equals(packageIdentifier));
            }

            // Finalize Query
            FeedIterator<PackageManifest> documentQuery = query.ToFeedIterator();

            // Fetch Current Package
            ApiDataPage<PackageManifest> apiDataDocument =
                await this.cosmosDatabase.GetByDocumentQuery<PackageManifest>(documentQuery);

            apiDataDocument.ContinuationToken = apiDataDocument.ContinuationToken != null ? StringEncoder.EncodeContinuationToken(apiDataDocument.ContinuationToken) : null;

            // Apply Version Filter
            if (!string.IsNullOrEmpty(versionFilter))
            {
                foreach (PackageManifest packageManifest in apiDataDocument.Items)
                {
                    if (packageManifest.Versions != null)
                    {
                        packageManifest.Versions = new VersionsExtended(packageManifest.Versions.Where(extended => extended.PackageVersion.Equals(versionFilter)));
                    }
                }
            }

            // Apply Channel Filter
            if (!string.IsNullOrEmpty(channelFilter))
            {
                foreach (PackageManifest packageManifest in apiDataDocument.Items)
                {
                    if (packageManifest.Versions != null)
                    {
                        packageManifest.Versions = new VersionsExtended(packageManifest.Versions.Where(extended => extended.Channel != null && extended.Channel.Equals(channelFilter)));
                    }
                }
            }

            // Apply Market Filter. Return only those results that have Market value in installers that match the Market filter.
            // If Markets object is null or markets do not match filter, exclude them from results.
            if (!string.IsNullOrEmpty(marketFilter))
            {
                ApplyMarketFilter(apiDataDocument.Items, marketFilter);
            }

            return apiDataDocument;
        }

        /// <inheritdoc />
        public async Task<ApiDataPage<ManifestSearchResponse>> SearchPackageManifests(ManifestSearchRequest manifestSearchRequest, string continuationToken = null)
        {
            continuationToken = continuationToken != null ? StringEncoder.DecodeContinuationToken(continuationToken) : null;

            // Create feed options for inclusion search: -1 so we can get all matches in inclusion, then filter down.
            QueryRequestOptions feedOptions = new QueryRequestOptions
            {
                ResponseContinuationTokenLimitInKb = CosmosConnectionConstants.ResponseContinuationTokenLimitInKb,
                MaxItemCount = AllElements,
            };

            manifestSearchRequest.Inclusions ??= new Utils.Models.Arrays.SearchRequestPackageMatchFilter();
            manifestSearchRequest.Filters ??= new Utils.Models.Arrays.SearchRequestPackageMatchFilter();

            // Convert Query to inclusions to submit to cosmos
            if (manifestSearchRequest.Query != null)
            {
                manifestSearchRequest.Inclusions.AddRange(this.queryList.Select(q => new SearchRequestPackageMatchFilter()
                {
                    PackageMatchField = q,
                    RequestMatch = manifestSearchRequest.Query,
                }));
            }

            // Process inclusions
            var inclusionsPredicate = new PredicateGenerator();
            manifestSearchRequest.Inclusions.ForEach(inclusion => AddConditionIfValid(inclusionsPredicate, inclusion, PredicateOperator.Or));

            // Process filters
            var filtersPredicate = new PredicateGenerator();
            manifestSearchRequest.Filters.ForEach(filter => AddConditionIfValid(filtersPredicate, filter, PredicateOperator.And));

            IQueryable<CosmosPackageManifest> query = this.cosmosDatabase.GetIQueryable<CosmosPackageManifest>();

            if (inclusionsPredicate.IsStarted() || filtersPredicate.IsStarted())
            {
                query = query.AsExpandable();

                if (inclusionsPredicate.IsStarted())
                {
                    query = query.Where(inclusionsPredicate.Generate(PredicateOperator.Or));
                }

                if (filtersPredicate.IsStarted())
                {
                    query = query.Where(filtersPredicate.Generate(PredicateOperator.And));
                }
            }
            else
            {
                query = query.Where(Utils.Common.PredicateBuilder.True<CosmosPackageManifest>());
            }

            // Submit Query to Cosmos
            var results = await this.cosmosDatabase.GetByDocumentQuery(query, feedOptions, continuationToken);
            this.log.LogTrace($"Query used {results.RequestCharge} RUs query: {query}");

            List<ManifestSearchResponse> manifestSearchResponse = results.Items.Distinct().SelectMany(m => ManifestSearchResponse.GetSearchVersions(m)).ToList();

            // Consolidate Results
            manifestSearchResponse = ManifestSearchResponse.Consolidate(manifestSearchResponse).OrderBy(manifest => manifest.PackageIdentifier).ToList();

            // Create Working Set and return
            ApiDataPage<ManifestSearchResponse> apiDataPage = new ApiDataPage<ManifestSearchResponse>();
            apiDataPage.ContinuationToken = results.ContinuationToken != null ? StringEncoder.EncodeContinuationToken(results.ContinuationToken) : null;
            apiDataPage.Items = manifestSearchResponse;

            return apiDataPage;
        }

        private static void AddConditionIfValid(PredicateGenerator predicate, SearchRequestPackageMatchFilter condition, PredicateOperator predicateOperator)
        {
            // Some package match fields or types might not be supported by current version of search.
            // So we will check the supported list before adding any predicates.
            if (IsPackageMatchFieldSupported(condition.PackageMatchField) &&
                IsMatchTypeSupported(condition.RequestMatch.MatchType))
            {
                predicate.AddCondition(condition, predicateOperator);
            }
        }

        /// <summary>
        /// Method to apply market filter and return results.
        /// </summary>
        /// <param name="packageManifests">Package manifests on which filter must be applied.</param>
        /// <param name="marketFilter">Market filter value.</param>
        private static void ApplyMarketFilter(IList<PackageManifest> packageManifests, string marketFilter)
        {
            if (!string.IsNullOrEmpty(marketFilter))
            {
                foreach (PackageManifest packageManifest in packageManifests)
                {
                    if (packageManifest.Versions != null)
                    {
                        HashSet<string> versionsWithoutInstallers = new HashSet<string>();
                        foreach (VersionExtended version in packageManifest.Versions)
                        {
                            if (version.Installers != null)
                            {
                                HashSet<string> installersNotMatchingFilter = new HashSet<string>();
                                foreach (var installer in version.Installers)
                                {
                                    if (installer.Markets == null
                                        || (installer.Markets.AllowedMarkets == null && installer.Markets.ExcludedMarkets == null)
                                        || (installer.Markets.AllowedMarkets != null && !installer.Markets.AllowedMarkets.Contains(marketFilter))
                                        || (installer.Markets.ExcludedMarkets != null && installer.Markets.ExcludedMarkets.Contains(marketFilter)))
                                    {
                                        installersNotMatchingFilter.Add(installer.InstallerIdentifier);
                                    }
                                }

                                foreach (var installer in installersNotMatchingFilter)
                                {
                                    version.RemoveInstaller(installer);
                                }
                            }

                            if (version.Installers == null || version.Installers.Count() == 0)
                            {
                                versionsWithoutInstallers.Add(version.PackageVersion);
                            }
                        }

                        foreach (var version in versionsWithoutInstallers)
                        {
                            packageManifest.RemoveVersion(version);
                        }
                    }
                }
            }
        }

        private static bool IsPackageMatchFieldSupported(string packageMatchField)
        {
            bool isPackageMatchFieldSupported = false;

            switch (packageMatchField)
            {
                case PackageMatchFields.PackageIdentifier:
                case PackageMatchFields.PackageName:
                case PackageMatchFields.Publisher:
                case PackageMatchFields.PackageFamilyName:
                case PackageMatchFields.ProductCode:
                case PackageMatchFields.UpgradeCode:
                case PackageMatchFields.Tag:
                case PackageMatchFields.Command:
                case PackageMatchFields.Moniker:
                case PackageMatchFields.HasInstallerType:
                    isPackageMatchFieldSupported = true;
                    break;
            }

            return isPackageMatchFieldSupported;
        }

        private static bool IsMatchTypeSupported(string matchType)
        {
            bool isMatchTypeSupported = false;

            switch (matchType)
            {
                case MatchType.Exact:
                case MatchType.CaseInsensitive:
                case MatchType.StartsWith:
                case MatchType.Substring:
                    isMatchTypeSupported = true;
                    break;
            }

            return isMatchTypeSupported;
        }
    }
}
