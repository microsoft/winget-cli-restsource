// -----------------------------------------------------------------------
// <copyright file="ManifestSearchFunctions.cs" company="Microsoft Corporation">
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
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Models;
    using Microsoft.WinGet.RestSource.Models.Schemas;
    using Newtonsoft.Json;

    /// <summary>
    /// This class contains the functions for searching manifests.
    /// </summary>
    /// TODO: Refactor duplicate code to library.
    public class ManifestSearchFunctions
    {
        private readonly ICosmosDatabase cosmosDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchFunctions"/> class.
        /// </summary>
        /// <param name="cosmosDatabase">Cosmos Database.</param>
        public ManifestSearchFunctions(ICosmosDatabase cosmosDatabase)
        {
            this.cosmosDatabase = cosmosDatabase;
        }

        /// <summary>
        /// Manifest Search Post Function.
        /// This also allows us to query manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestSearchPost)]
        public async Task<IActionResult> ManifestSearchPostAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, FunctionConstants.FunctionPost, Route = "manifestSearch")]
            HttpRequest req,
            ILogger log)
        {
            ApiResponse<Manifest> apiResponse = new ApiResponse<Manifest>();
            ManifestSearch manifestSearch = null;
            try
            {
                manifestSearch = await Parser.StreamParser<ManifestSearch>(req.Body, log);

                // Create feed options
                int maxItemCount = manifestSearch.MaximumResults < FunctionSettingsConstants.MaxResultsPerPage
                    ? manifestSearch.MaximumResults
                    : FunctionSettingsConstants.MaxResultsPerPage;

                // TODO: Expand Feed Options
                FeedOptions feedOptions = new FeedOptions
                {
                    EnableCrossPartitionQuery = true,
                    MaxItemCount = maxItemCount,
                    RequestContinuation = StringEncoder.DecodeContinuationToken(manifestSearch.ContinuationToken),
                };

                // Get iQueryable
                IQueryable<Manifest> query = this.cosmosDatabase.GetIQueryable<Manifest>(feedOptions);

                // Apply query parameters to query
                // TODO: Apply Query Parameters

                // Finalize Query
                IDocumentQuery<Manifest> documentQuery = query.AsDocumentQuery();

                // Get results
                CosmosPage<Manifest> cosmosPage =
                    await this.cosmosDatabase.GetByDocumentQuery<Manifest>(documentQuery);
                apiResponse.Data = cosmosPage.Items.ToList();
                apiResponse.ContinuationToken = StringEncoder.EncodeContinuationToken(cosmosPage.ContinuationToken);
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
