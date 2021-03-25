// -----------------------------------------------------------------------
// <copyright file="PackageManifestFunctions.cs" company="Microsoft Corporation">
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
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Models;
    using Microsoft.WinGet.RestSource.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Models.Schemas;

    /// <summary>
    /// This class contains the functions for interacting with manifests.
    /// </summary>
    public class PackageManifestFunctions
    {
        private readonly ICosmosDatabase cosmosDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageManifestFunctions"/> class.
        /// </summary>
        /// <param name="cosmosDatabase">Cosmos Database.</param>
        public PackageManifestFunctions(ICosmosDatabase cosmosDatabase)
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
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "packageManifests")]
            HttpRequest req,
            ILogger log)
        {
            PackageManifest packageManifest = null;
            try
            {
                // Parse Stream
                packageManifest = await Parser.StreamParser<PackageManifest>(req.Body, log);
                ApiDataValidator.Validate<PackageManifest>(packageManifest);

                // Create Document and add to cosmos.
                CosmosPackageManifest cPackageManifest = new CosmosPackageManifest(packageManifest);
                CosmosDocument<CosmosPackageManifest> cosmosDocument = new CosmosDocument<CosmosPackageManifest>
                {
                    Document = cPackageManifest,
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

            return new OkObjectResult(new ApiResponse<PackageManifest>(packageManifest));
        }

        /// <summary>
        /// Manifest Delete Function.
        /// This allows us to make delete requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestDelete)]
        public async Task<IActionResult> ManifestDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionDelete, Route = "packageManifests/{packageIdentifier}")]
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
        /// Manifest Put Function.
        /// This allows us to make put requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Manifest ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestPut)]
        public async Task<IActionResult> ManifestPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPut, Route = "packageManifests/{packageIdentifier}")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            PackageManifest packageManifest = null;

            try
            {
                // Parse Stream
                packageManifest = await Parser.StreamParser<PackageManifest>(req.Body, log);
                ApiDataValidator.Validate<PackageManifest>(packageManifest);

                // Create Document and add to cosmos.
                CosmosPackageManifest cPackageManifest = new CosmosPackageManifest(packageManifest);
                CosmosDocument<CosmosPackageManifest> cosmosDocument = new CosmosDocument<CosmosPackageManifest>
                {
                    Document = cPackageManifest,
                    Id = packageIdentifier,
                    PartitionKey = packageIdentifier,
                };
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

            return new OkObjectResult(new ApiResponse<PackageManifest>(packageManifest));
        }

        /// <summary>
        /// Manifest Get Function.
        /// This allows us to make Get requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestGet)]
        public async Task<IActionResult> ManifestGetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, FunctionConstants.FunctionGet, Route = "packageManifests/{packageIdentifier}")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            List<PackageManifest> manifests = new List<PackageManifest>();
            try
            {
                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument = await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));
                manifests.Add(cosmosDocument.Document.ToManifest());
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

            return manifests.Count switch
            {
                0 => new NoContentResult(),
                _ => new OkObjectResult(new ApiResponse<PackageManifest>(manifests.First())),
            };
        }
    }
}
