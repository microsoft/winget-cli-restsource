// -----------------------------------------------------------------------
// <copyright file="VersionFunctions.cs" company="Microsoft Corporation">
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
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Models;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Newtonsoft.Json;
    using Error = Microsoft.WinGet.RestSource.Models.Error;

    /// <summary>
    /// This class contains the functions for interacting with versions.
    /// </summary>
    /// TODO: Create and switch to non-af binding DocumentClient.
    /// TODO: Refactor duplicate code to library.
    public static class VersionFunctions
    {
        /// <summary>
        /// Version Post Function.
        /// This allows us to make post requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionPost)]
        public static async Task<IActionResult> VersionsPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "packages/{id}/versions")] HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = CosmosConnectionConstants.ConnectionStringSetting)] DocumentClient client,
            string id,
            ILogger log)
        {
            VersionCore versionCore = null;

            try
            {
                // Parse body as package
                versionCore = await Parser.StreamParser<VersionCore>(req.Body, log);

                // TODO: Validate Parsed Values

                // Fetch Current Document
                Uri documentLink = UriFactory.CreateDocumentUri(CosmosConnectionConstants.DatabaseName, CosmosConnectionConstants.CollectionName, id);
                DocumentResponse<Manifest> documentResponse = await client.ReadDocumentAsync<Manifest>(documentLink, new RequestOptions
                {
                    PartitionKey = new PartitionKey(id),
                });
                Manifest manifest = documentResponse.Document;

                // Create list if null
                if (manifest.Versions == null)
                {
                    manifest.Versions = new List<VersionExtended>();
                }

                // If does not exist add
                if (!manifest.Versions.Any(nested => nested.Version == versionCore.Version))
                {
                    manifest.Versions.Add(new VersionExtended(versionCore));
                }
                else
                {
                    throw new Exception();
                }

                // Save Document
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

            return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(versionCore, Formatting.Indented));
        }

        /// <summary>
        /// Version Delete Function.
        /// This allows us to make Delete requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionDelete)]
        public static async Task<IActionResult> VersionsDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "packages/{id}/versions/{version}")]
            HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = CosmosConnectionConstants.ConnectionStringSetting)] DocumentClient client,
            string id,
            string version,
            ILogger log)
        {
            try
            {
                // Fetch Current Document
                Uri documentLink = UriFactory.CreateDocumentUri(CosmosConnectionConstants.DatabaseName, CosmosConnectionConstants.CollectionName, id);
                DocumentResponse<Manifest> documentResponse = await client.ReadDocumentAsync<Manifest>(documentLink, new RequestOptions
                {
                    PartitionKey = new PartitionKey(id),
                });
                Manifest manifest = documentResponse.Document;

                // If version does not exist, throw
                if (manifest.Versions == null || !manifest.Versions.Any(versionExtended => versionExtended.Version == version))
                {
                    throw new Exception();
                }

                // Delete it
                manifest.Versions = new List<VersionExtended>(manifest.Versions.Where(versionExtended => versionExtended.Version != version));
                log.LogInformation(JsonConvert.SerializeObject(manifest, Formatting.Indented));

                // Save Document
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

            return new OkObjectResult("Deleted");
        }

        /// <summary>
        /// Version Put Function.
        /// This allows us to make put requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionPut)]
        public static async Task<IActionResult> VersionsPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "packages/{id}/versions/{version}")]
            HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = CosmosConnectionConstants.ConnectionStringSetting)] DocumentClient client,
            string id,
            string version,
            ILogger log)
        {
            VersionCore versionCore = null;

            try
            {
                // Parse body as package
                versionCore = await Parser.StreamParser<VersionCore>(req.Body, log);
                versionCore.Version = version;

                // TODO: Validate Parsed Values

                // Fetch Current Document
                Uri documentLink = UriFactory.CreateDocumentUri(CosmosConnectionConstants.DatabaseName, CosmosConnectionConstants.CollectionName, id);
                DocumentResponse<Manifest> documentResponse = await client.ReadDocumentAsync<Manifest>(documentLink, new RequestOptions
                {
                    PartitionKey = new PartitionKey(id),
                });
                Manifest manifest = documentResponse.Document;

                // If version does not exist, throw
                if (manifest.Versions == null || !manifest.Versions.Any(versionExtended => versionExtended.Version == version))
                {
                    throw new Exception();
                }

                // Delete Current Version
                manifest.Versions = new List<VersionExtended>(manifest.Versions.Where(versionExtended => versionExtended.Version != version));
                log.LogInformation(JsonConvert.SerializeObject(manifest, Formatting.Indented));

                // Create list if null
                if (manifest.Versions == null)
                {
                    manifest.Versions = new List<VersionExtended>();
                }

                // Add Updated Version
                manifest.Versions.Add(new VersionExtended(versionCore));

                // Save Document
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

            return new OkObjectResult(versionCore);
        }

        /// <summary>
        /// Version Get Function.
        /// This allows us to make get requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionGet)]
        public static async Task<IActionResult> VersionsGetAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "packages/{id}/versions/{version?}")] HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = CosmosConnectionConstants.ConnectionStringSetting)] DocumentClient client,
            string id,
            string version,
            ILogger log)
        {
            List<VersionCore> versionCores = new List<VersionCore>();

            try
            {
                // Fetch Current Package
                Uri documentLink = UriFactory.CreateDocumentUri(CosmosConnectionConstants.DatabaseName, CosmosConnectionConstants.CollectionName, id);
                DocumentResponse<Manifest> documentResponse = await client.ReadDocumentAsync<Manifest>(documentLink, new RequestOptions
                {
                    PartitionKey = new PartitionKey(id),
                });
                Manifest manifest = documentResponse.Document;

                if (manifest.Versions == null)
                {
                    throw new Exception();
                }

                if (string.IsNullOrWhiteSpace(version))
                {
                    versionCores.AddRange(manifest.Versions.Select(versionExtended => new VersionCore(versionExtended)));
                }
                else
                {
                    // If version does not exist, throw
                    if (!manifest.Versions.Any(versionExtended => versionExtended.Version == version))
                    {
                        throw new Exception();
                    }

                    versionCores.AddRange(from versionExtended in manifest.Versions where versionExtended.Version == version select new VersionCore(versionExtended));
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

            return versionCores.Count switch
            {
                0 => new NoContentResult(),
                1 => new OkObjectResult(JsonConvert.SerializeObject(versionCores.First(), Formatting.Indented)),
                _ => new OkObjectResult(JsonConvert.SerializeObject(versionCores, Formatting.Indented))
            };
        }
    }
}