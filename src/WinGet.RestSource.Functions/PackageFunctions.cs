// -----------------------------------------------------------------------
// <copyright file="PackageFunctions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Models;
    using Microsoft.WinGet.RestSource.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Models.Schemas;

    /// <summary>
    /// This class contains the functions for interacting with packages.
    /// </summary>
    public class PackageFunctions
    {
        private readonly ICosmosDatabase cosmosDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageFunctions"/> class.
        /// </summary>
        /// <param name="cosmosDatabase">Cosmos Database.</param>
        public PackageFunctions(ICosmosDatabase cosmosDatabase)
        {
            this.cosmosDatabase = cosmosDatabase;
        }

        /// <summary>
        /// Package Post Function.
        /// This allows us to handle post requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackagePost)]
        public async Task<IActionResult> PackagesPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "packages")]
            HttpRequest req,
            ILogger log)
        {
            Package package = null;

            try
            {
                // Parse body as package
                package = await Parser.StreamParser<Package>(req.Body, log);
                ApiDataValidator.Validate<Package>(package);

                // Convert Package to Manifest for storage
                PackageManifest packageManifest = new PackageManifest(package);
                CosmosPackageManifest cosmosPackageManifest = new CosmosPackageManifest(packageManifest);

                // Create Document and add to cosmos.
                CosmosDocument<CosmosPackageManifest> cosmosDocument = new CosmosDocument<CosmosPackageManifest>
                {
                    Document = cosmosPackageManifest,
                };
                await this.cosmosDatabase.Add<CosmosPackageManifest>(cosmosDocument);
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.UnhandledError(e);
            }

            return new OkObjectResult(new ApiResponse<Package>(package));
        }

        /// <summary>
        /// Package Delete Function.
        /// This allows us to make delete requests for packages.
        /// This will delete all sub resources as well.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackageDelete)]
        public async Task<IActionResult> PackageDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionDelete, Route = "packages/{packageIdentifier}")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            try
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
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.UnhandledError(e);
            }

            return new NoContentResult();
        }

        /// <summary>
        /// Package Put Function.
        /// This allows us to make put requests for packages.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackagePut)]
        public async Task<IActionResult> PackagesPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPut, Route = "packages/{packageIdentifier}")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            Package package = null;

            try
            {
                // Parse body as package
                package = await Parser.StreamParser<Package>(req.Body, log);
                ApiDataValidator.Validate<Package>(package);

                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);

                // Update Package
                cosmosDocument.Document.Update(package);

                // Save Package
                await this.cosmosDatabase.Update<CosmosPackageManifest>(cosmosDocument);
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.UnhandledError(e);
            }

            return new OkObjectResult(new ApiResponse<Package>(package));
        }

        /// <summary>
        /// Manifest Get Function.
        /// This allows us to make Get requests for manifests.
        /// This also allows us to query manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackageGet)]
        public async Task<IActionResult> PackagesGetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, FunctionConstants.FunctionGet, Route = "packages/{packageIdentifier?}")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            List<Package> packages = new List<Package>();
            string continuationToken = null;

            try
            {
                if (string.IsNullOrWhiteSpace(packageIdentifier))
                {
                    // Parse Parameters
                    continuationToken = req.Query["ct"];
                    StringEncoder.DecodeContinuationToken(continuationToken);
                    continuationToken = StringEncoder.DecodeContinuationToken(continuationToken);

                    // Create feed options
                    // TODO: Expand Feed Options
                    FeedOptions feedOptions = new FeedOptions
                    {
                        EnableCrossPartitionQuery = true,
                        MaxItemCount = FunctionSettingsConstants.MaxResultsPerPage,
                        RequestContinuation = continuationToken,
                    };

                    // Get iQueryable
                    IQueryable<CosmosPackageManifest> query = this.cosmosDatabase.GetIQueryable<CosmosPackageManifest>(feedOptions);

                    // Apply query parameters to query
                    // TODO: Apply Query Parameters

                    // Finalize Query
                    IDocumentQuery<CosmosPackageManifest> documentQuery = query.AsDocumentQuery();

                    // Get results
                    CosmosPage<CosmosPackageManifest> cosmosPage = await this.cosmosDatabase.GetByDocumentQuery<CosmosPackageManifest>(documentQuery);
                    foreach (CosmosPackageManifest result in cosmosPage.Items)
                    {
                        packages.Add(result.ToPackage());
                    }

                    continuationToken = StringEncoder.EncodeContinuationToken(cosmosPage.ContinuationToken);
                }
                else
                {
                    // Fetch Current Package
                    CosmosDocument<CosmosPackageManifest> cosmosDocument = await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                    log.LogInformation(FormatJSON.Indented(cosmosDocument, log));
                    packages.Add(cosmosDocument.Document.ToPackage());
                    continuationToken = null;
                }
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.UnhandledError(e);
            }

            return packages.Count switch
            {
                0 => new NoContentResult(),
                1 => new OkObjectResult(new ApiResponse<Package>(packages.First(), continuationToken)),
                _ => new OkObjectResult(new ApiResponse<List<Package>>(packages, continuationToken)),
            };
        }
    }
}
