// -----------------------------------------------------------------------
// <copyright file="ManifestFunctions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Functions.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.Functions.Constants;
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Models;
    using Newtonsoft.Json;
    using Error = Microsoft.WinGet.RestSource.Models.Error;

    /// <summary>
    /// This class contains the functions for interacting with manifests.
    /// </summary>
    /// TODO: Create and switch to non-af binding DocumentClient.
    public static class ManifestFunctions
    {
        /// <summary>
        /// Manifest Post Function.
        /// This allows us to handle post requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName("ManifestPost")]
        public static async Task<IActionResult> ManifestPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "manifests")] HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            ILogger log)
        {
            Manifest manifest = null;
            try
            {
                manifest = await Parser.StreamParser<Manifest>(req.Body, log);
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(
                    CosmosConnectionConstants.DatabaseName,
                    CosmosConnectionConstants.CollectionName);
                await client.CreateDocumentAsync(collectionUri, manifest);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                Error error = new Error
                {
                    ErrorCode = ErrorConstants.UnhandledErrorCode,
                    ErrorMessage = ErrorConstants.UnhandledErrorMessage,
                };

                return new ObjectResult(JsonConvert.SerializeObject(error, Formatting.Indented))
                {
                    StatusCode = 500,
                };
            }

            return new OkObjectResult(JsonConvert.SerializeObject(manifest, Formatting.Indented));
        }

        /// <summary>
        /// Manifest Delete Function.
        /// This allows us to make delete requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Manifest ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName("ManifestDelete")]
        public static async Task<IActionResult> ManifestDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "manifests/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            string id,
            ILogger log)
        {
            try
            {
                Uri documentUri = UriFactory.CreateDocumentUri(CosmosConnectionConstants.DatabaseName, CosmosConnectionConstants.CollectionName, id);
                await client.DeleteDocumentAsync(documentUri, new RequestOptions
                {
                    PartitionKey = new PartitionKey(id),
                });
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());

                Error error = new Error
                {
                    ErrorCode = ErrorConstants.UnhandledErrorCode,
                    ErrorMessage = ErrorConstants.UnhandledErrorMessage,
                };

                return new ObjectResult(JsonConvert.SerializeObject(error, Formatting.Indented))
                {
                    StatusCode = 500,
                };
            }

            return new OkObjectResult("Deleted");
        }

        /// <summary>
        /// Manifest Put Function.
        /// This allows us to make put requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Manifest ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName("ManifestPut")]
        public static async Task<IActionResult> ManifestPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "manifests/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: Constants.CosmosConnectionConstants.DatabaseName,
                collectionName: Constants.CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            string id,
            ILogger log)
        {
            Manifest manifest = null;

            try
            {
                manifest = await Parser.StreamParser<Manifest>(req.Body, log);
                Uri documentLink = UriFactory.CreateDocumentUri(CosmosConnectionConstants.DatabaseName, CosmosConnectionConstants.CollectionName, id);
                await client.ReplaceDocumentAsync(documentLink, manifest, null);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());

                Error error = new Error
                {
                    ErrorCode = ErrorConstants.UnhandledErrorCode,
                    ErrorMessage = ErrorConstants.UnhandledErrorMessage,
                };

                return new ObjectResult(JsonConvert.SerializeObject(error, Formatting.Indented))
                {
                    StatusCode = 500,
                };
            }

            return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(manifest, Formatting.Indented));
        }

        /// <summary>
        /// Manifest Get Function.
        /// This allows us to make Get requests for manifests.
        /// This also allows us to query manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName("ManifestGet")]
        public static async Task<IActionResult> ManifestGetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "manifests")] HttpRequest req,
            [CosmosDB(
                databaseName: Constants.CosmosConnectionConstants.DatabaseName,
                collectionName: Constants.CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            ILogger log)
        {
            List<Manifest> manifests = new List<Manifest>();
            try
            {
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(Constants.CosmosConnectionConstants.DatabaseName, Constants.CosmosConnectionConstants.CollectionName);
                IQueryable<Manifest> iQueryable = client.CreateDocumentQuery<Manifest>(collectionUri, new FeedOptions { EnableCrossPartitionQuery = true });

                // TODO: Apply Query Parameters

                // Finalize query
                IDocumentQuery<Manifest> query = iQueryable.AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    foreach (Manifest result in await query.ExecuteNextAsync())
                    {
                        manifests.Add(result);
                    }
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());

                Error error = new Error
                {
                    ErrorCode = ErrorConstants.UnhandledErrorCode,
                    ErrorMessage = ErrorConstants.UnhandledErrorMessage,
                };

                return new ObjectResult(JsonConvert.SerializeObject(error, Formatting.Indented))
                {
                    StatusCode = 500,
                };
            }

            return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(manifests, Formatting.Indented));
        }
    }
}