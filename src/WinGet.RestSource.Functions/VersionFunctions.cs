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
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Models;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// This class contains the functions for interacting with versions.
    /// </summary>
    /// TODO: Refactor duplicate code to library.
    public class VersionFunctions
    {
        private readonly ICosmosDatabase cosmosDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionFunctions"/> class.
        /// </summary>
        /// <param name="cosmosDatabase">Cosmos Database.</param>
        public VersionFunctions(ICosmosDatabase cosmosDatabase)
        {
            this.cosmosDatabase = cosmosDatabase;
        }

        /// <summary>
        /// Version Post Function.
        /// This allows us to make post requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionPost)]
        public async Task<IActionResult> VersionsPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "packages/{id}/versions")]
            HttpRequest req,
            string id,
            ILogger log)
        {
            VersionCore versionCore = null;

            try
            {
                // Parse body as package
                versionCore = await Parser.StreamParser<VersionCore>(req.Body, log);

                // Validate Parsed Values
                // TODO: Validate Parsed Values

                // Fetch Current Package
                CosmosDocument<Manifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<Manifest>(id, id);
                log.LogInformation(JsonConvert.SerializeObject(cosmosDocument, Formatting.Indented));

                // Create list if null
                cosmosDocument.Document.Versions ??= new List<VersionExtended>();

                // If does not exist add
                if (cosmosDocument.Document.Versions.All(nested => nested.Version != versionCore.Version))
                {
                    cosmosDocument.Document.Versions.Add(new VersionExtended(versionCore));
                }
                else
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionAlreadyExistsErrorCode,
                            ErrorConstants.VersionAlreadyExistsErrorMessage));
                }

                // Save Document
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

            return new OkObjectResult(JsonConvert.SerializeObject(versionCore, Formatting.Indented));
        }

        /// <summary>
        /// Version Delete Function.
        /// This allows us to make Delete requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionDelete)]
        public async Task<IActionResult> VersionsDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "packages/{id}/versions/{version}")]
            HttpRequest req,
            string id,
            string version,
            ILogger log)
        {
            try
            {
                // Fetch Current Package
                CosmosDocument<Manifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<Manifest>(id, id);
                log.LogInformation(JsonConvert.SerializeObject(cosmosDocument, Formatting.Indented));

                // Validate Package Version is not null
                if (cosmosDocument.Document.Versions == null)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionsIsNullErrorCode,
                            ErrorConstants.VersionsIsNullErrorMessage));
                }

                // Validate Version exists
                if (cosmosDocument.Document.Versions.All(versionExtended => versionExtended.Version != version))
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionDoesNotExistErrorCode,
                            ErrorConstants.VersionDoesNotExistErrorMessage));
                }

                // Delete it
                cosmosDocument.Document.Versions = new List<VersionExtended>(
                    cosmosDocument.Document.Versions.Where(versionExtended => versionExtended.Version != version));
                log.LogInformation(JsonConvert.SerializeObject(cosmosDocument.Document, Formatting.Indented));

                // Save Package
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

            return new NoContentResult();
        }

        /// <summary>
        /// Version Put Function.
        /// This allows us to make put requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionPut)]
        public async Task<IActionResult> VersionsPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "packages/{id}/versions/{version}")]
            HttpRequest req,
            string id,
            string version,
            ILogger log)
        {
            VersionCore versionCore = null;

            try
            {
                // Parse body as package
                versionCore = await Parser.StreamParser<VersionCore>(req.Body, log);

                // Validate Parsed Values
                // TODO: Validate Parsed Values
                if (versionCore.Version != version)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionDoesNotMatchErrorCode,
                            ErrorConstants.VersionDoesNotMatchErrorMessage));
                }

                // Fetch Current Package
                CosmosDocument<Manifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<Manifest>(id, id);
                log.LogInformation(JsonConvert.SerializeObject(cosmosDocument, Formatting.Indented));

                // If version does not exist, throw
                if (cosmosDocument.Document.Versions == null ||
                    cosmosDocument.Document.Versions.All(versionExtended => versionExtended.Version != version))
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionDoesNotExistErrorCode,
                            ErrorConstants.VersionDoesNotExistErrorMessage));
                }

                // Delete Current Version
                cosmosDocument.Document.Versions = new List<VersionExtended>(
                    cosmosDocument.Document.Versions.Where(versionExtended => versionExtended.Version != version));
                log.LogInformation(JsonConvert.SerializeObject(cosmosDocument.Document, Formatting.Indented));

                // Create list if null
                cosmosDocument.Document.Versions ??= new List<VersionExtended>();

                // Add Updated Version
                cosmosDocument.Document.Versions.Add(new VersionExtended(versionCore));

                // Save Package
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

            return new OkObjectResult(versionCore);
        }

        /// <summary>
        /// Version Get Function.
        /// This allows us to make get requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionGet)]
        public async Task<IActionResult> VersionsGetAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "packages/{id}/versions/{version?}")]
            HttpRequest req,
            string id,
            string version,
            ILogger log)
        {
            ApiResponse<VersionCore> apiResponse = new ApiResponse<VersionCore>();

            try
            {
                // Fetch Current Package
                CosmosDocument<Manifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<Manifest>(id, id);
                log.LogInformation(JsonConvert.SerializeObject(cosmosDocument, Formatting.Indented));

                // Validate Package Contains Versions
                if (cosmosDocument.Document.Versions == null)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionsIsNullErrorCode,
                            ErrorConstants.VersionsIsNullErrorMessage));
                }

                // Process Version Request.
                if (string.IsNullOrWhiteSpace(version))
                {
                    foreach (VersionCore versionCore in cosmosDocument.Document.Versions.Select(
                        versionExtended => new VersionCore(versionExtended)))
                    {
                        apiResponse.Data.Add(versionCore);
                    }
                }
                else
                {
                    // If version does not exist, throw an error
                    if (cosmosDocument.Document.Versions.All(versionExtended => versionExtended.Version != version))
                    {
                        throw new InvalidArgumentException(
                            new InternalRestError(
                                ErrorConstants.VersionDoesNotExistErrorCode,
                                ErrorConstants.VersionDoesNotExistErrorMessage));
                    }

                    IEnumerable<VersionCore> enumerable = cosmosDocument.Document.Versions
                        .Where(versionExtended => versionExtended.Version == version)
                        .Select(versionExtended => new VersionCore(versionExtended));
                    foreach (VersionCore versionCore in enumerable)
                    {
                        apiResponse.Data.Add(versionCore);
                    }
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
                _ => new OkObjectResult(JsonConvert.SerializeObject(apiResponse, Formatting.Indented))
            };
        }
    }
}
