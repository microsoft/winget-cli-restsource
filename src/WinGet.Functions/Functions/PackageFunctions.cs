// -----------------------------------------------------------------------
// <copyright file="PackageFunctions.cs" company="Microsoft Corporation">
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
    using Microsoft.WinGet.RestSource.Models.Core;
    using Newtonsoft.Json;
    using Error = Microsoft.WinGet.RestSource.Models.Error;

    /// <summary>
    /// This class contains the functions for interacting with packages.
    /// </summary>
    /// TODO: Create and switch to non-af binding DocumentClient.
    public static class PackageFunctions
    {
        /// <summary>
        /// Package Post Function.
        /// This allows us to handle post requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackagePost)]
        public static async Task<IActionResult> PackagesPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "packages")] HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            ILogger log)
        {
            PackageCore package = null;

            try
            {
                // Parse body as package
                package = await Parser.StreamParser<PackageCore>(req.Body, log);

                // Convert Package to Manifest for storage
                Manifest manifest = new Manifest(package);
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(CosmosConnectionConstants.DatabaseName, CosmosConnectionConstants.CollectionName);
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

            return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(package, Formatting.Indented));
        }

        /// <summary>
        /// Package Delete Function.
        /// This allows us to make delete requests for packages.
        /// This will delete all sub resources as well.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackageDelete)]
        public static async Task<IActionResult> PackageDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "packages/{id}")] HttpRequest req,
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
        /// Package Put Function.
        /// This allows us to make put requests for packages.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackagePut)]
        public static async Task<IActionResult> PackagesPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "packages/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            string id,
            ILogger log)
        {
            PackageCore package = null;

            try
            {
                // Parse body as package
                package = await Parser.StreamParser<PackageCore>(req.Body, log);
                package.Id = id;

                // Fetch Current Package
                Uri documentLink = UriFactory.CreateDocumentUri(CosmosConnectionConstants.DatabaseName, CosmosConnectionConstants.CollectionName, id);
                DocumentResponse<Manifest> documentResponse = await client.ReadDocumentAsync<Manifest>(documentLink, new RequestOptions
                {
                    PartitionKey = new PartitionKey(id),
                });
                Manifest manifest = documentResponse.Document;

                // Update Package
                manifest.Id = package.Id;
                manifest.DefaultLocale = package.DefaultLocale;

                // Save Package
                await client.ReplaceDocumentAsync(documentLink, manifest, null);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());

                Error error = new Error()
                {
                    ErrorCode = ErrorConstants.UnhandledErrorCode,
                    ErrorMessage = ErrorConstants.UnhandledErrorMessage,
                };

                return (ActionResult)new ObjectResult(JsonConvert.SerializeObject(error, Formatting.Indented))
                {
                    StatusCode = 500,
                };
            }

            return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(package, Formatting.Indented));
        }

        /// <summary>
        /// Manifest Get Function.
        /// This allows us to make Get requests for manifests.
        /// This also allows us to query manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackageGet)]
        public static async Task<IActionResult> PackagesGetAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "packages/{id?}")] HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            string id,
            ILogger log)
        {
            List<PackageCore> packages = new List<PackageCore>();

            try
            {
                if (id == null)
                {
                    Uri collectionUri = UriFactory.CreateDocumentCollectionUri(CosmosConnectionConstants.DatabaseName, CosmosConnectionConstants.CollectionName);
                    IQueryable<Manifest> iQueryable = client.CreateDocumentQuery<Manifest>(collectionUri, new FeedOptions { EnableCrossPartitionQuery = true });

                    // TODO: Apply Query Parameters

                    // Finalize query
                    IDocumentQuery<Manifest> query = iQueryable.AsDocumentQuery();

                    // Query Fetcher - Fetch Manifest as package
                    while (query.HasMoreResults)
                    {
                        foreach (PackageCore result in await query.ExecuteNextAsync())
                        {
                            packages.Add(result);
                        }
                    }
                }
                else
                {
                    // Fetch Current Package
                    Uri documentLink = UriFactory.CreateDocumentUri(CosmosConnectionConstants.DatabaseName, CosmosConnectionConstants.CollectionName, id);
                    DocumentResponse<PackageCore> documentResponse = await client.ReadDocumentAsync<PackageCore>(documentLink, new RequestOptions
                    {
                        PartitionKey = new PartitionKey(id),
                    });
                    packages.Add(documentResponse.Document);
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

            return packages.Count switch
            {
                0 => new NoContentResult(),
                1 => new OkObjectResult(JsonConvert.SerializeObject(packages.First(), Formatting.Indented)),
                _ => new OkObjectResult(JsonConvert.SerializeObject(packages, Formatting.Indented))
            };
        }
    }
}