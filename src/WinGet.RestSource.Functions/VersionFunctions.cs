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
    using Version = Microsoft.WinGet.RestSource.Models.Schemas.Version;

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
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.VersionPost)]
        public async Task<IActionResult> VersionsPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "packages/{packageIdentifier}/versions")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            Version version = null;

            try
            {
                // Parse body as Version
                version = await Parser.StreamParser<Version>(req.Body, log);
                ApiDataValidator.Validate<Version>(version);

                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument = await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Add Version
                cosmosDocument.Document.AddVersion(version);

                // Save Document
                ApiDataValidator.Validate<PackageManifest>(cosmosDocument.Document);
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

            return new OkObjectResult(new ApiResponse<Version>(version));
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
                CosmosDocument<CosmosPackageManifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Remove Version
                cosmosDocument.Document.RemoveVersion(packageVersion);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Save Package
                ApiDataValidator.Validate<PackageManifest>(cosmosDocument.Document);
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
            Version version = null;

            try
            {
                // Parse body as Version
                version = await Parser.StreamParser<Version>(req.Body, log);
                ApiDataValidator.Validate<Version>(version);

                // Validate Versions Match
                if (version.PackageVersion != packageVersion)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionDoesNotMatchErrorCode,
                            ErrorConstants.VersionDoesNotMatchErrorMessage));
                }

                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument = await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Update
                cosmosDocument.Document.Versions.Update(version);

                // Save Package
                ApiDataValidator.Validate<PackageManifest>(cosmosDocument.Document);
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

            return new OkObjectResult(new ApiResponse<Version>(version));
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
            [HttpTrigger(AuthorizationLevel.Anonymous, FunctionConstants.FunctionGet, Route = "packages/{packageIdentifier}/versions/{packageVersion?}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            ILogger log)
        {
            List<Version> versions = new List<Version>();

            try
            {
                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument = await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Get Versions and convert.
                VersionsExtended extended = cosmosDocument.Document.GetVersion(packageVersion);
                versions.AddRange(extended.Select(ver => new Version(ver)));
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

            return versions.Count switch
            {
                0 => new NoContentResult(),
                1 => new OkObjectResult(new ApiResponse<Version>(versions.First())),
                _ => new OkObjectResult(new ApiResponse<List<Version>>(versions)),
            };
        }
    }
}
