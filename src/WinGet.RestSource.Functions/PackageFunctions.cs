// -----------------------------------------------------------------------
// <copyright file="PackageFunctions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions
{
    using System;
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
    /// TODO: Refactor duplicate code to library.
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
                Manifest manifest = new Manifest(package);
                CosmosManifest cosmosManifest = new CosmosManifest(manifest);

                // Create Document and add to cosmos.
                CosmosDocument<CosmosManifest> cosmosDocument = new CosmosDocument<CosmosManifest>
                {
                    Document = cosmosManifest,
                };
                await this.cosmosDatabase.Add<CosmosManifest>(cosmosDocument);
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

            return new OkObjectResult(FormatJSON.Indented(package, log));
        }

        /// <summary>
        /// Package Delete Function.
        /// This allows us to make delete requests for packages.
        /// This will delete all sub resources as well.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackageDelete)]
        public async Task<IActionResult> PackageDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionDelete, Route = "packages/{id}")]
            HttpRequest req,
            string id,
            ILogger log)
        {
            try
            {
                // Parse Variables
                CosmosDocument<CosmosManifest> cosmosDocument = new CosmosDocument<CosmosManifest>
                {
                    Id = id,
                    PartitionKey = id,
                };

                // Delete Document
                await this.cosmosDatabase.Delete<CosmosManifest>(cosmosDocument);
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
        /// <param name="id">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackagePut)]
        public async Task<IActionResult> PackagesPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPut, Route = "packages/{id}")]
            HttpRequest req,
            string id,
            ILogger log)
        {
            Package package = null;

            try
            {
                // Parse body as package
                package = await Parser.StreamParser<Package>(req.Body, log);
                ApiDataValidator.Validate<Package>(package);

                // Fetch Current Package
                CosmosDocument<CosmosManifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosManifest>(id, id);

                // Update Package
                cosmosDocument.Document.Update(package);

                // Save Package
                await this.cosmosDatabase.Update<CosmosManifest>(cosmosDocument);
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

            return new OkObjectResult(FormatJSON.Indented(package, log));
        }

        /// <summary>
        /// Manifest Get Function.
        /// This allows us to make Get requests for manifests.
        /// This also allows us to query manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackageGet)]
        public async Task<IActionResult> PackagesGetAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionGet, Route = "packages/{id?}")]
            HttpRequest req,
            string id,
            ILogger log)
        {
            ApiResponse<Package> apiResponse = new ApiResponse<Package>();

            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    // Parse Parameters
                    string continuationToken = req.Query["ct"];
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
                    IQueryable<CosmosManifest> query = this.cosmosDatabase.GetIQueryable<CosmosManifest>(feedOptions);

                    // Apply query parameters to query
                    // TODO: Apply Query Parameters

                    // Finalize Query
                    IDocumentQuery<CosmosManifest> documentQuery = query.AsDocumentQuery();

                    // Get results
                    CosmosPage<CosmosManifest> cosmosPage =
                        await this.cosmosDatabase.GetByDocumentQuery<CosmosManifest>(documentQuery);
                    foreach (CosmosManifest result in cosmosPage.Items)
                    {
                        apiResponse.Data.Add(result.ToPackage());
                    }

                    apiResponse.ContinuationToken = StringEncoder.EncodeContinuationToken(cosmosPage.ContinuationToken);
                }
                else
                {
                    // Fetch Current Package
                    CosmosDocument<CosmosManifest> cosmosDocument = await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosManifest>(id, id);
                    log.LogInformation(FormatJSON.Indented(cosmosDocument, log));
                    apiResponse.Data.Add(cosmosDocument.Document.ToPackage());
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

            return apiResponse.Data.Count switch
            {
                0 => new NoContentResult(),
                _ => new OkObjectResult(FormatJSON.Indented(apiResponse, log))
            };
        }
    }
}
