// -----------------------------------------------------------------------
// <copyright file="ManifestFunctions.cs" company="Microsoft Corporation">
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
    using Newtonsoft.Json;

    /// <summary>
    /// This class contains the functions for interacting with manifests.
    /// </summary>
    /// TODO: Refactor duplicate code to library.
    public class ManifestFunctions
    {
        private readonly ICosmosDatabase cosmosDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestFunctions"/> class.
        /// </summary>
        /// <param name="cosmosDatabase">Cosmos Database.</param>
        public ManifestFunctions(ICosmosDatabase cosmosDatabase)
        {
            this.cosmosDatabase = cosmosDatabase;
        }

        /// <summary>
        /// Manifest Post Function.
        /// This allows us to handle post requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestPost)]
        public async Task<IActionResult> ManifestPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "manifests")]
            HttpRequest req,
            ILogger log)
        {
            Manifest manifest = null;
            try
            {
                // Parse Stream
                manifest = await Parser.StreamParser<Manifest>(req.Body, log);

                // Validate Parsed Values
                // TODO: Validate Parsed Values

                // Create Document and add to cosmos.
                CosmosDocument<Manifest> cosmosDocument = new CosmosDocument<Manifest>
                {
                    Document = manifest,
                };
                await this.cosmosDatabase.Add<Manifest>(cosmosDocument);
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

            return new OkObjectResult(JsonConvert.SerializeObject(manifest, Formatting.Indented));
        }

        /// <summary>
        /// Manifest Delete Function.
        /// This allows us to make delete requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Manifest ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestDelete)]
        public async Task<IActionResult> ManifestDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionDelete, Route = "manifests/{id}")]
            HttpRequest req,
            string id,
            ILogger log)
        {
            try
            {
                // Parse Variables
                CosmosDocument<Manifest> cosmosDocument = new CosmosDocument<Manifest>
                {
                    Id = id,
                    PartitionKey = id,
                };

                // Delete Document
                await this.cosmosDatabase.Delete<Manifest>(cosmosDocument);
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
        /// Manifest Put Function.
        /// This allows us to make put requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Manifest ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestPut)]
        public async Task<IActionResult> ManifestPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPut, Route = "manifests/{id}")]
            HttpRequest req,
            string id,
            ILogger log)
        {
            Manifest manifest = null;

            try
            {
                // Parse Stream
                manifest = await Parser.StreamParser<Manifest>(req.Body, log);

                // Validate Parsed Values
                // TODO: Validate Parsed Values
                if (manifest.Id != id)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.IdDoesNotMatchErrorCode,
                            ErrorConstants.IdDoesNotMatchErrorMessage));
                }

                // Create Document and add to cosmos.
                CosmosDocument<Manifest> cosmosDocument = new CosmosDocument<Manifest>
                {
                    Document = manifest,
                    Id = id,
                    PartitionKey = id,
                };
                await this.cosmosDatabase.Update<Manifest>(cosmosDocument);
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

            return new OkObjectResult(JsonConvert.SerializeObject(manifest, Formatting.Indented));
        }

        /// <summary>
        /// Manifest Get Function.
        /// This allows us to make Get requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestGet)]
        public async Task<IActionResult> ManifestGetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, FunctionConstants.FunctionGet, Route = "manifests/{id}")]
            HttpRequest req,
            string id,
            ILogger log)
        {
            ApiResponse<Manifest> apiResponse = new ApiResponse<Manifest>();
            try
            {
                // Fetch Current Package
                CosmosDocument<Manifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<Manifest>(id, id);
                log.LogInformation(JsonConvert.SerializeObject(cosmosDocument, Formatting.Indented));
                apiResponse.Data.Add(cosmosDocument.Document);
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
                _ => new OkObjectResult(JsonConvert.SerializeObject(apiResponse, Formatting.Indented))
            };
        }
    }
}
