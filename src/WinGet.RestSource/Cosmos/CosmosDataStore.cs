// -----------------------------------------------------------------------
// <copyright file="CosmosDataStore.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Cosmos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using LinqKit;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Constants.Enumerations;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Models.Errors;
    using Microsoft.WinGet.RestSource.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Models.Objects;
    using Microsoft.WinGet.RestSource.Models.Schemas;
    using Microsoft.WinGet.RestSource.Validators;
    using Version = Microsoft.WinGet.RestSource.Models.Schemas.Version;

    /// <summary>
    /// Cosmos Data Store.
    /// </summary>
    public class CosmosDataStore : IApiDataStore
    {
        private const string ShortDescription = "ShortDescription";

        private const int AllElements = -1;

        private readonly ICosmosDatabase cosmosDatabase;
        private readonly ILogger<CosmosDataStore> log;

        private readonly List<string> queryList = new List<string>()
        {
            PackageMatchFields.PackageIdentifier,
            PackageMatchFields.PackageName,
            PackageMatchFields.Moniker,
            PackageMatchFields.Command,
            PackageMatchFields.Tag,
            PackageMatchFields.PackageFamilyName,
            PackageMatchFields.ProductCode,
            PackageMatchFields.NormalizedPackageNameAndPublisher,
            ShortDescription,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDataStore"/> class.
        /// </summary>
        /// <param name="log">Log.</param>
        public CosmosDataStore(ILogger<CosmosDataStore> log)
        {
            Uri endpoint = new Uri(
                Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosAccountEndpointSetting) ??
                throw new System.IO.InvalidDataException());
            CosmosDatabase database = new CosmosDatabase(
                endpoint,
                Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosAccountKeySetting),
                Environment.GetEnvironmentVariable(CosmosConnectionConstants.DatabaseNameSetting),
                Environment.GetEnvironmentVariable(CosmosConnectionConstants.CollectionNameSetting));

            this.cosmosDatabase = database;
            this.log = log;
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
            await this.cosmosDatabase.Add<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc/>
        public async Task DeletePackage(string packageIdentifier)
        {
            CosmosDocument<CosmosPackageManifest> cosmosDocument = new CosmosDocument<CosmosPackageManifest>
            {
                Id = packageIdentifier,
                PartitionKey = packageIdentifier,
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
        public async Task<ApiDataPage<Package>> GetPackages(string packageIdentifier, IQueryCollection queryParameters)
        {
            // Process Continuation token
            string continuationToken = null;
            if (queryParameters != null)
            {
                continuationToken = queryParameters[QueryConstants.ContinuationToken];
            }

            continuationToken = continuationToken != null ? StringEncoder.DecodeContinuationToken(continuationToken) : null;

            // Create feed options
            FeedOptions feedOptions = new FeedOptions
            {
                ResponseContinuationTokenLimitInKb = CosmosConnectionConstants.ResponseContinuationTokenLimitInKb,
                EnableCrossPartitionQuery = true,
                MaxItemCount = FunctionSettingsConstants.MaxResultsPerPage,
                RequestContinuation = continuationToken,
            };

            // Get iQueryable
            IQueryable<CosmosPackageManifest> query = this.cosmosDatabase.GetIQueryable<CosmosPackageManifest>(feedOptions);

            if (!string.IsNullOrWhiteSpace(packageIdentifier))
            {
                query = query.Where(package => package.PackageIdentifier.Equals(packageIdentifier));
            }

            // Finalize Query
            IDocumentQuery<CosmosPackageManifest> documentQuery = query.AsDocumentQuery();

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
        public async Task<ApiDataPage<Version>> GetVersions(string packageIdentifier, string packageVersion, IQueryCollection queryParameters)
        {
            // Process Continuation token
            string continuationToken = null;
            if (queryParameters != null)
            {
                continuationToken = queryParameters[QueryConstants.ContinuationToken];
            }

            continuationToken = continuationToken != null ? StringEncoder.DecodeContinuationToken(continuationToken) : null;

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
        public async Task<ApiDataPage<Installer>> GetInstallers(string packageIdentifier, string packageVersion, string installerIdentifier, IQueryCollection queryParameters)
        {
            // Process Continuation token
            string continuationToken = null;
            if (queryParameters != null)
            {
                continuationToken = queryParameters[QueryConstants.ContinuationToken];
            }

            continuationToken = continuationToken != null ? StringEncoder.DecodeContinuationToken(continuationToken) : null;

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
        public async Task<ApiDataPage<Locale>> GetLocales(string packageIdentifier, string packageVersion, string packageLocale, IQueryCollection queryParameters)
        {
            // Process Continuation token
            string continuationToken = null;
            if (queryParameters != null)
            {
                continuationToken = queryParameters[QueryConstants.ContinuationToken];
            }

            continuationToken = continuationToken != null ? StringEncoder.DecodeContinuationToken(continuationToken) : null;

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
            // Parse Variables
            CosmosDocument<CosmosPackageManifest> cosmosDocument = new CosmosDocument<CosmosPackageManifest>
            {
                Id = packageIdentifier,
                PartitionKey = packageIdentifier,
            };

            // Delete Document
            await this.cosmosDatabase.Delete<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task UpdatePackageManifest(string packageIdentifier, PackageManifest packageManifest)
        {
            CosmosPackageManifest cosmosPackageManifest = new CosmosPackageManifest(packageManifest);
            CosmosDocument<CosmosPackageManifest> cosmosDocument = new CosmosDocument<CosmosPackageManifest>
            {
                Document = cosmosPackageManifest,
                Id = packageIdentifier,
                PartitionKey = packageIdentifier,
            };
            await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
        }

        /// <inheritdoc />
        public async Task<ApiDataPage<PackageManifest>> GetPackageManifests(string packageIdentifier, IQueryCollection queryParameters)
        {
            // Note: GetPackageManifests should use query parameters as predicates when querying all package manifests. Currently, query parameters
            // are only exposed for GetPackageManifests with a PackageIdentifier input. Whenever query parameters are exposed to querying all
            // package manifests, this method should utilize search predicates to filter on query parameters.

            // Process Continuation token
            string continuationToken = null;
            string versionFilter = null;
            string channelFilter = null;
            string marketFilter = null;
            if (queryParameters != null)
            {
                continuationToken = queryParameters[QueryConstants.ContinuationToken];
                versionFilter = queryParameters[QueryConstants.Version];
                channelFilter = queryParameters[QueryConstants.Channel];
                marketFilter = queryParameters[QueryConstants.Market];
            }

            continuationToken = continuationToken != null ? StringEncoder.DecodeContinuationToken(continuationToken) : null;

            // Create feed options
            FeedOptions feedOptions = new FeedOptions
            {
                ResponseContinuationTokenLimitInKb = CosmosConnectionConstants.ResponseContinuationTokenLimitInKb,
                EnableCrossPartitionQuery = true,
                MaxItemCount = FunctionSettingsConstants.MaxResultsPerPage,
                RequestContinuation = continuationToken,
            };

            // Get iQueryable
            IQueryable<PackageManifest> query = this.cosmosDatabase.GetIQueryable<PackageManifest>(feedOptions);

            if (!string.IsNullOrWhiteSpace(packageIdentifier))
            {
                query = query.Where(package => package.PackageIdentifier.Equals(packageIdentifier));
            }

            // Finalize Query
            IDocumentQuery<PackageManifest> documentQuery = query.AsDocumentQuery();

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
                foreach (PackageManifest pm in apiDataDocument.Items)
                {
                    if (pm.Versions != null)
                    {
                        pm.Versions = new VersionsExtended(pm.Versions.Where(extended => extended.Channel != null && extended.Channel.Equals(channelFilter)));
                    }
                }
            }

            // Apply Market Filter. Return only those results that have Market value in installers that match the Market filter.
            // If Markets object is null or markets do not match filter, exclude them from results.
            if (!string.IsNullOrEmpty(marketFilter))
            {
                this.ApplyMarketFilter(apiDataDocument.Items, marketFilter);
            }

            return apiDataDocument;
        }

        /// <inheritdoc />
        public async Task<ApiDataPage<ManifestSearchResponse>> SearchPackageManifests(ManifestSearchRequest manifestSearchRequest, Dictionary<string, string> headers, IQueryCollection queryParameters)
        {
            // Create Working Set and return
            ApiDataPage<ManifestSearchResponse> apiDataPage = new ApiDataPage<ManifestSearchResponse>();
            List<PackageManifest> manifests = new List<PackageManifest>();
            List<ManifestSearchResponse> manifestSearchResponse = new List<ManifestSearchResponse>();

            // Create feed options for inclusion search: -1 so we can get all matches in inclusion, then filter down.
            FeedOptions feedOptions = new FeedOptions
            {
                ResponseContinuationTokenLimitInKb = CosmosConnectionConstants.ResponseContinuationTokenLimitInKb,
                EnableCrossPartitionQuery = true,
                MaxItemCount = AllElements,
                RequestContinuation = null,
            };

            if (manifestSearchRequest.FetchAllManifests || (manifestSearchRequest.Inclusions == null && manifestSearchRequest.Query == null))
            {
                IQueryable<CosmosPackageManifest> query = this.cosmosDatabase.GetIQueryable<CosmosPackageManifest>(feedOptions);
                IDocumentQuery<CosmosPackageManifest> documentQuery = query.AsDocumentQuery();
                ApiDataPage<CosmosPackageManifest> apiDataDocument = await this.cosmosDatabase.GetByDocumentQuery<CosmosPackageManifest>(documentQuery);
                manifests.AddRange(apiDataDocument.Items);
            }
            else
            {
                // Process Inclusions
                Models.Arrays.SearchRequestPackageMatchFilter inclusions = new Models.Arrays.SearchRequestPackageMatchFilter();
                if (manifestSearchRequest.Inclusions != null)
                {
                    inclusions.AddRange(manifestSearchRequest.Inclusions);
                }

                // Convert Query to inclusions to submit to cosmos
                if (manifestSearchRequest.Query != null)
                {
                    inclusions.AddRange(this.queryList.Select(q => new Models.Objects.SearchRequestPackageMatchFilter()
                    {
                        PackageMatchField = q,
                        RequestMatch = manifestSearchRequest.Query,
                    }));
                }

                // Submit Inclusions to Cosmos
                // Due to join limitation on Cosmos - we are submitting each predicate separately.
                // TODO: Create a more efficient search - but this will suffice for now for a light weight reference.
                if (inclusions.Count > 0)
                {
                    List<Task<ApiDataPage<PackageManifest>>> taskSet = new List<Task<ApiDataPage<PackageManifest>>>();
                    foreach (string packageMatchField in inclusions.Select(inc => inc.PackageMatchField).Distinct())
                    {
                        // Create Predicate for search
                        ExpressionStarter<PackageManifest> inclusionPredicate = PredicateBuilder.New<PackageManifest>();
                        foreach (SearchRequestPackageMatchFilter matchFilter in inclusions.Where(inc => inc.PackageMatchField.Equals(packageMatchField)))
                        {
                            // Some package match fields or types might not be supported by current version of search.
                            // So we will check the supported list before adding any predicates.
                            if (this.IsPackageMatchFieldSupported(matchFilter.PackageMatchField) &&
                                this.IsMatchTypeSupported(matchFilter.RequestMatch.MatchType))
                            {
                                inclusionPredicate.Or(this.QueryPredicate(matchFilter.PackageMatchField, matchFilter.RequestMatch));
                            }
                        }

                        // Create Document Query
                        IQueryable<PackageManifest> query = this.cosmosDatabase.GetIQueryable<PackageManifest>(feedOptions);
                        query = query.Where(inclusionPredicate);
                        IDocumentQuery<PackageManifest> documentQuery = query.AsDocumentQuery();

                        // Submit Query to Cosmos
                        taskSet.Add(Task.Run(() => this.cosmosDatabase.GetByDocumentQuery(documentQuery)));
                    }

                    // Wait for Cosmos Queries to complete
                    await Task.WhenAll(taskSet.ToArray());

                    // Process Manifests from Cosmos
                    foreach (Task<ApiDataPage<PackageManifest>> task in taskSet)
                    {
                        manifests.AddRange(task.Result.Items);
                    }

                    manifests = manifests.Distinct().ToList();
                }
            }

            // Process Filters
            if (manifestSearchRequest.Filters != null)
            {
                ExpressionStarter<PackageManifest> filterPredicate = PredicateBuilder.New<PackageManifest>();
                foreach (SearchRequestPackageMatchFilter matchFilter in manifestSearchRequest.Filters)
                {
                    if (this.IsPackageMatchFieldSupported(matchFilter.PackageMatchField) &&
                        this.IsMatchTypeSupported(matchFilter.RequestMatch.MatchType))
                    {
                        filterPredicate.Or(this.QueryPredicate(matchFilter.PackageMatchField, matchFilter.RequestMatch));
                    }
                }

                manifests = manifests.Where(filterPredicate).ToList();
            }

            foreach (PackageManifest manifest in manifests)
            {
                foreach (ManifestSearchResponse response in ManifestSearchResponse.GetSearchVersions(manifest))
                {
                    manifestSearchResponse.Add(response);
                }
            }

            // Consolidate Results
            manifestSearchResponse = ManifestSearchResponse.Consolidate(manifestSearchResponse).OrderBy(manifest => manifest.PackageIdentifier).ToList();

            // Process results
            if (manifestSearchResponse.Count > manifestSearchRequest.MaximumResults && manifestSearchRequest.MaximumResults > 0)
            {
                manifestSearchResponse = manifestSearchResponse.GetRange(0, manifestSearchRequest.MaximumResults);
            }

            int maxPageCount = manifestSearchRequest.MaximumResults < FunctionSettingsConstants.MaxResultsPerPage && manifestSearchRequest.MaximumResults > 0
                ? manifestSearchRequest.MaximumResults
                : FunctionSettingsConstants.MaxResultsPerPage;

            int totalResults = manifestSearchResponse.Count;
            if (totalResults > maxPageCount)
            {
                // Process Continuation Token
                ApiContinuationToken token = null;
                if (headers.ContainsKey(HeaderConstants.ContinuationToken))
                {
                    token = Parser.StringParser<ApiContinuationToken>(StringEncoder.DecodeContinuationToken(headers[HeaderConstants.ContinuationToken]));
                }
                else
                {
                    token = new ApiContinuationToken()
                    {
                        Index = 0,
                        MaxPageSize = maxPageCount,
                    };
                }

                // If index miss-match dump results and return no content.
                if (token.Index > manifestSearchResponse.Count - 1)
                {
                    manifestSearchResponse = new List<ManifestSearchResponse>();
                    token = null;
                }
                else
                {
                    int elementsRemaining = totalResults - token.Index;
                    int elements = elementsRemaining < token.MaxPageSize ? elementsRemaining : token.MaxPageSize;
                    manifestSearchResponse = manifestSearchResponse.GetRange(token.Index, elements);

                    token.Index += elements;
                    if (token.Index == totalResults)
                    {
                        token = null;
                    }
                }

                apiDataPage.ContinuationToken = token != null ? StringEncoder.EncodeContinuationToken(FormatJSON.None(token)) : null;
            }

            apiDataPage.Items = ManifestSearchResponse.Consolidate(manifestSearchResponse.ToList());

            return apiDataPage;
        }

        /// <summary>
        /// Method to apply market filter and return results.
        /// </summary>
        /// <param name="packageManifests">Package manifests on which filter must be applied.</param>
        /// <param name="marketFilter">Market filter value.</param>
        internal void ApplyMarketFilter(IList<PackageManifest> packageManifests, string marketFilter)
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

        private bool IsPackageMatchFieldSupported(string packageMatchField)
        {
            bool isPackageMatchFieldSupported = false;

            switch (packageMatchField)
            {
                case PackageMatchFields.PackageIdentifier:
                case PackageMatchFields.PackageName:
                case PackageMatchFields.PackageFamilyName:
                case PackageMatchFields.ProductCode:
                case PackageMatchFields.Tag:
                case PackageMatchFields.Command:
                case PackageMatchFields.Moniker:
                case PackageMatchFields.Market:
                    isPackageMatchFieldSupported = true;
                    break;
            }

            return isPackageMatchFieldSupported;
        }

        private bool IsMatchTypeSupported(string matchType)
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

        private Expression<Func<PackageManifest, bool>> QueryPredicate(string packageMatchField, SearchRequestMatch requestMatch)
        {
            return (packageMatchField, requestMatch.MatchType) switch
            {
                (PackageMatchFields.PackageIdentifier, MatchType.Exact) =>
                    manifest => manifest.PackageIdentifier.Equals(requestMatch.KeyWord),

                (PackageMatchFields.PackageIdentifier, MatchType.CaseInsensitive) =>
                    manifest => manifest.PackageIdentifier.ToLower().Equals(requestMatch.KeyWord.ToLower()),

                (PackageMatchFields.PackageIdentifier, MatchType.StartsWith) =>
                    manifest => manifest.PackageIdentifier.StartsWith(requestMatch.KeyWord),

                (PackageMatchFields.PackageIdentifier, MatchType.Substring) =>
                    manifest => manifest.PackageIdentifier.Contains(requestMatch.KeyWord),

                (PackageMatchFields.PackageName, MatchType.Exact) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.DefaultLocale.PackageName.Equals(requestMatch.KeyWord)),

                (PackageMatchFields.PackageName, MatchType.CaseInsensitive) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.DefaultLocale.PackageName.ToLower().Equals(requestMatch.KeyWord.ToLower())),

                (PackageMatchFields.PackageName, MatchType.StartsWith) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.DefaultLocale.PackageName.StartsWith(requestMatch.KeyWord)),

                (PackageMatchFields.PackageName, MatchType.Substring) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.DefaultLocale.PackageName.Contains(requestMatch.KeyWord)),

                (PackageMatchFields.Moniker, MatchType.Exact) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.DefaultLocale.Moniker != null && extended.DefaultLocale.Moniker.Equals(requestMatch.KeyWord)),

                (PackageMatchFields.Moniker, MatchType.CaseInsensitive) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.DefaultLocale.Moniker != null && extended.DefaultLocale.Moniker.ToLower().Equals(requestMatch.KeyWord.ToLower())),

                (PackageMatchFields.Moniker, MatchType.StartsWith) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.DefaultLocale.Moniker != null && extended.DefaultLocale.Moniker.StartsWith(requestMatch.KeyWord)),

                (PackageMatchFields.Moniker, MatchType.Substring) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.DefaultLocale.Moniker != null && extended.DefaultLocale.Moniker.Contains(requestMatch.KeyWord)),

                (PackageMatchFields.Command, MatchType.Exact) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Commands != null && installer.Commands.Any(command => command.Equals(requestMatch.KeyWord)))),

                (PackageMatchFields.Command, MatchType.CaseInsensitive) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Commands != null && installer.Commands.Any(command => command.ToLower().Equals(requestMatch.KeyWord.ToLower())))),

                (PackageMatchFields.Command, MatchType.StartsWith) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Commands != null && installer.Commands.Any(command => command.StartsWith(requestMatch.KeyWord)))),

                (PackageMatchFields.Command, MatchType.Substring) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Commands != null && installer.Commands.Any(command => command.Contains(requestMatch.KeyWord)))),

                (PackageMatchFields.Tag, MatchType.Exact) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.DefaultLocale.Tags != null && extended.DefaultLocale.Tags.Any(tag => tag.Equals(requestMatch.KeyWord))) || manifest.Versions.Any(extended => extended.Locales != null && extended.Locales.Any(locale => locale.Tags != null && locale.Tags.Any(tag => tag.Equals(requestMatch.KeyWord))))),

                (PackageMatchFields.Tag, MatchType.CaseInsensitive) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.DefaultLocale.Tags != null && extended.DefaultLocale.Tags.Any(tag => tag.ToLower().Equals(requestMatch.KeyWord.ToLower()))) || manifest.Versions.Any(extended => extended.Locales != null && extended.Locales.Any(locale => locale.Tags != null && locale.Tags.Any(tag => tag.ToLower().Equals(requestMatch.KeyWord.ToLower()))))),

                (PackageMatchFields.Tag, MatchType.StartsWith) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.DefaultLocale.Tags != null && extended.DefaultLocale.Tags.Any(tag => tag.StartsWith(requestMatch.KeyWord))) || manifest.Versions.Any(extended => extended.Locales != null && extended.Locales.Any(locale => locale.Tags != null && locale.Tags.Any(tag => tag.StartsWith(requestMatch.KeyWord))))),

                (PackageMatchFields.Tag, MatchType.Substring) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.DefaultLocale.Tags != null && extended.DefaultLocale.Tags.Any(tag => tag.Contains(requestMatch.KeyWord))) || manifest.Versions.Any(extended => extended.Locales != null && extended.Locales.Any(locale => locale.Tags != null && locale.Tags.Any(tag => tag.Contains(requestMatch.KeyWord))))),

                (PackageMatchFields.PackageFamilyName, MatchType.Exact) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.PackageFamilyName != null && installer.PackageFamilyName.Equals(requestMatch.KeyWord))),

                (PackageMatchFields.PackageFamilyName, MatchType.CaseInsensitive) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.PackageFamilyName != null && installer.PackageFamilyName.ToLower().Equals(requestMatch.KeyWord.ToLower()))),

                (PackageMatchFields.PackageFamilyName, MatchType.StartsWith) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.PackageFamilyName != null && installer.PackageFamilyName.StartsWith(requestMatch.KeyWord))),

                (PackageMatchFields.PackageFamilyName, MatchType.Substring) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.PackageFamilyName != null && installer.PackageFamilyName.Contains(requestMatch.KeyWord))),

                (PackageMatchFields.ProductCode, MatchType.Exact) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.ProductCode != null && installer.ProductCode.Equals(requestMatch.KeyWord))),

                (PackageMatchFields.ProductCode, MatchType.CaseInsensitive) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.ProductCode != null && installer.ProductCode.ToLower().Equals(requestMatch.KeyWord.ToLower()))),

                (PackageMatchFields.ProductCode, MatchType.StartsWith) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.ProductCode != null && installer.ProductCode.StartsWith(requestMatch.KeyWord))),

                (PackageMatchFields.ProductCode, MatchType.Substring) =>
                    manifest => manifest.Versions != null && manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.ProductCode != null && installer.ProductCode.Contains(requestMatch.KeyWord))),

                (ShortDescription, MatchType.Exact) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.DefaultLocale.ShortDescription.Equals(requestMatch.KeyWord)) || manifest.Versions.Any(extended => extended.Locales != null && extended.Locales.Any(locale => locale.ShortDescription != null && locale.ShortDescription.Equals(requestMatch.KeyWord)))),

                (ShortDescription, MatchType.CaseInsensitive) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.DefaultLocale.ShortDescription.ToLower().Equals(requestMatch.KeyWord.ToLower())) || manifest.Versions.Any(extended => extended.Locales != null && extended.Locales.Any(locale => locale.ShortDescription != null && locale.ShortDescription.ToLower().Equals(requestMatch.KeyWord.ToLower())))),

                (ShortDescription, MatchType.StartsWith) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.DefaultLocale.ShortDescription.StartsWith(requestMatch.KeyWord)) || manifest.Versions.Any(extended => extended.Locales != null && extended.Locales.Any(locale => locale.ShortDescription != null && locale.ShortDescription.StartsWith(requestMatch.KeyWord)))),

                (ShortDescription, MatchType.Substring) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.DefaultLocale.ShortDescription.Contains(requestMatch.KeyWord)) || manifest.Versions.Any(extended => extended.Locales != null && extended.Locales.Any(locale => locale.ShortDescription != null && locale.ShortDescription.Contains(requestMatch.KeyWord)))),

                (PackageMatchFields.Market, MatchType.Exact) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Markets != null && installer.Markets.AllowedMarkets != null && installer.Markets.AllowedMarkets.Any(market => market.Equals(requestMatch.KeyWord)))) || manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Markets != null && installer.Markets.ExcludedMarkets != null && installer.Markets.ExcludedMarkets.Any(market => !market.Equals(requestMatch.KeyWord))))),

                (PackageMatchFields.Market, MatchType.CaseInsensitive) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Markets != null && installer.Markets.AllowedMarkets != null && installer.Markets.AllowedMarkets.Any(market => market.ToLower().Equals(requestMatch.KeyWord.ToLower())))) || manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Markets != null && installer.Markets.ExcludedMarkets != null && installer.Markets.ExcludedMarkets.Any(market => !market.ToLower().Equals(requestMatch.KeyWord.ToLower()))))),

                (PackageMatchFields.Market, MatchType.StartsWith) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Markets != null && installer.Markets.AllowedMarkets != null && installer.Markets.AllowedMarkets.Any(market => market.StartsWith(requestMatch.KeyWord)))) || manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Markets != null && installer.Markets.ExcludedMarkets != null && installer.Markets.ExcludedMarkets.Any(market => !market.StartsWith(requestMatch.KeyWord))))),

                (PackageMatchFields.Market, MatchType.Substring) =>
                    manifest => manifest.Versions != null && (manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Markets != null && installer.Markets.AllowedMarkets != null && installer.Markets.AllowedMarkets.Any(market => market.Contains(requestMatch.KeyWord)))) || manifest.Versions.Any(extended => extended.Installers != null && extended.Installers.Any(installer => installer.Markets != null && installer.Markets.ExcludedMarkets != null && installer.Markets.ExcludedMarkets.Any(market => !market.Contains(requestMatch.KeyWord))))),

                _ => throw new InvalidArgumentException(new InternalRestError(ErrorConstants.ValidationFailureErrorCode, ErrorConstants.ValidationFailureErrorMessage)),
            };
        }
    }
}
