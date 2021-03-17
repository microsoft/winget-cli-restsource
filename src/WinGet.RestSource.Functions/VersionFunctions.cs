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
    using Microsoft.WinGet.RestSource.Models.Errors;
    using Microsoft.WinGet.RestSource.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Models.Schemas;

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
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "packages/{id}/versions")]
            HttpRequest req,
            string id,
            ILogger log)
        {
            Models.Schemas.Version version = null;

            try
            {
                // Parse body as package
                version = await Parser.StreamParser<Models.Schemas.Version>(req.Body, log);
                ApiDataValidator.Validate<Models.Schemas.Version>(version);

                // Fetch Current Package
                CosmosDocument<CosmosManifest> cosmosDocument = await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosManifest>(id, id);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Create list if null
                cosmosDocument.Document.Versions ??= new VersionsExtended();

                // If does not exist add
                if (cosmosDocument.Document.Versions.All(nested => nested.PackageVersion != version.PackageVersion))
                {
                    cosmosDocument.Document.Versions.Add(new VersionExtended(version));
                }
                else
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionAlreadyExistsErrorCode,
                            ErrorConstants.VersionAlreadyExistsErrorMessage));
                }

                // Save Document
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

            return new OkObjectResult(FormatJSON.Indented(version, log));
        }

        /// <summary>
        /// Version Delete Function.
        /// This allows us to make Delete requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionDelete)]
        public async Task<IActionResult> VersionsDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionDelete, Route = "packages/{packageIdentifier}/versions/{packageVersion}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            ILogger log)
        {
            try
            {
                // Fetch Current Package
                CosmosDocument<CosmosManifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Validate Package Version is not null
                if (cosmosDocument.Document.Versions == null)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionsIsNullErrorCode,
                            ErrorConstants.VersionsIsNullErrorMessage));
                }

                // Validate Version exists
                if (cosmosDocument.Document.Versions.All(versionExtended => versionExtended.PackageVersion != packageVersion))
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionDoesNotExistErrorCode,
                            ErrorConstants.VersionDoesNotExistErrorMessage));
                }

                // Delete it
                cosmosDocument.Document.Versions = new VersionsExtended(cosmosDocument.Document.Versions.Where(versionExtended => versionExtended.PackageVersion != packageVersion));
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

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

            return new NoContentResult();
        }

        /// <summary>
        /// Version Put Function.
        /// This allows us to make put requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionPut)]
        public async Task<IActionResult> VersionsPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPut, Route = "packages/{packageIdentifier}/versions/{packageVersion}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            ILogger log)
        {
            Models.Schemas.Version version = null;

            try
            {
                // Parse body as package
                version = await Parser.StreamParser<Models.Schemas.Version>(req.Body, log);
                ApiDataValidator.Validate<Models.Schemas.Version>(version);

                if (version.PackageVersion != packageVersion)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionDoesNotMatchErrorCode,
                            ErrorConstants.VersionDoesNotMatchErrorMessage));
                }

                // Fetch Current Package
                CosmosDocument<CosmosManifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // If version does not exist, throw
                if (cosmosDocument.Document.Versions == null ||
                    cosmosDocument.Document.Versions.All(versionExtended => versionExtended.PackageVersion != packageVersion))
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionDoesNotExistErrorCode,
                            ErrorConstants.VersionDoesNotExistErrorMessage));
                }

                // Delete Current Version
                cosmosDocument.Document.Versions = new VersionsExtended(cosmosDocument.Document.Versions.Where(versionExtended => versionExtended.PackageVersion != packageVersion));
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Create list if null
                cosmosDocument.Document.Versions ??= new VersionsExtended();

                // Add Updated Version
                cosmosDocument.Document.Versions.Add(new VersionExtended(version));

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

            return new OkObjectResult(version);
        }

        /// <summary>
        /// Version Get Function.
        /// This allows us to make get requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionGet)]
        public async Task<IActionResult> VersionsGetAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionGet, Route = "packages/{packageIdentifier}/versions/{packageVersion?}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            ILogger log)
        {
            ApiResponse<Models.Schemas.Version> apiResponse = new ApiResponse<Models.Schemas.Version>();

            try
            {
                // Fetch Current Package
                CosmosDocument<CosmosManifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Validate Package Contains Versions
                if (cosmosDocument.Document.Versions == null)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionsIsNullErrorCode,
                            ErrorConstants.VersionsIsNullErrorMessage));
                }

                // Process Version Request.
                if (string.IsNullOrWhiteSpace(packageVersion))
                {
                    foreach (Models.Schemas.Version versionCore in cosmosDocument.Document.Versions.Select(
                        versionExtended => new Models.Schemas.Version(versionExtended)))
                    {
                        apiResponse.Data.Add(versionCore);
                    }
                }
                else
                {
                    // If version does not exist, throw an error
                    if (cosmosDocument.Document.Versions.All(versionExtended => versionExtended.PackageVersion != packageVersion))
                    {
                        throw new InvalidArgumentException(
                            new InternalRestError(
                                ErrorConstants.VersionDoesNotExistErrorCode,
                                ErrorConstants.VersionDoesNotExistErrorMessage));
                    }

                    IEnumerable<Models.Schemas.Version> enumerable = cosmosDocument.Document.Versions
                        .Where(versionExtended => versionExtended.PackageVersion == packageVersion)
                        .Select(versionExtended => new Models.Schemas.Version(versionExtended));
                    foreach (Models.Schemas.Version versionCore in enumerable)
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
                _ => new OkObjectResult(FormatJSON.Indented(apiResponse, log))
            };
        }
    }
}
