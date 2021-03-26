// -----------------------------------------------------------------------
// <copyright file="InstallerFunctions.cs" company="Microsoft Corporation">
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
    using Microsoft.WinGet.RestSource.Functions.Common;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Models;
    using Microsoft.WinGet.RestSource.Models.Errors;
    using Microsoft.WinGet.RestSource.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Models.Schemas;

    /// <summary>
    /// This class contains the functions for interacting with installers.
    /// </summary>
    public class InstallerFunctions
    {
        private readonly ICosmosDatabase cosmosDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerFunctions"/> class.
        /// </summary>
        /// <param name="cosmosDatabase">Cosmos Database.</param>
        public InstallerFunctions(ICosmosDatabase cosmosDatabase)
        {
            this.cosmosDatabase = cosmosDatabase;
        }

        /// <summary>
        /// Installer Post Function.
        /// This allows us to make post requests for installers.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.InstallerPost)]
        public async Task<IActionResult> InstallerPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "packages/{packageIdentifier}/versions/{packageVersion}/installers")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            ILogger log)
        {
            Installer installer = null;

            try
            {
                // Parse body as installer
                installer = await Parser.StreamParser<Installer>(req.Body, log);
                ApiDataValidator.Validate<Installer>(installer);

                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument = await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Add Installer
                cosmosDocument.Document.AddInstaller(installer, packageVersion);

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

            return new ApiObjectResult(new ApiResponse<Installer>(installer));
        }

        /// <summary>
        /// Installer Delete Function.
        /// This allows us to make delete requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="installerIdentifier">Installer Identifier for the installer.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.InstallerDelete)]
        public async Task<IActionResult> InstallerDeleteAsync(
            [HttpTrigger(
                AuthorizationLevel.Function,
                FunctionConstants.FunctionDelete,
                Route = "packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            string installerIdentifier,
            ILogger log)
        {
            try
            {
                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument = await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Remove Installer
                cosmosDocument.Document.RemoveInstaller(installerIdentifier, packageVersion);

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

            return new NoContentResult();
        }

        /// <summary>
        /// Installer Put Function.
        /// This allows us to make put requests for installers.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="installerIdentifier">Installer Identifier for the installer.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.InstallerPut)]
        public async Task<IActionResult> InstallerPutAsync(
            [HttpTrigger(
                AuthorizationLevel.Function,
                FunctionConstants.FunctionPut,
                Route = "packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            string installerIdentifier,
            ILogger log)
        {
            Installer installer = null;

            try
            {
                // Parse body as package
                installer = await Parser.StreamParser<Installer>(req.Body, log);
                ApiDataValidator.Validate<Installer>(installer);

                if (installer.InstallerIdentifier != installerIdentifier)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.InstallerDoesNotMatchErrorCode,
                            ErrorConstants.InstallerDoesNotMatchErrorMessage));
                }

                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Update Installer
                cosmosDocument.Document.UpdateInstaller(installer, packageVersion);

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

            return new ApiObjectResult(new ApiResponse<Installer>(installer));
        }

        /// <summary>
        /// Installer Get Function.
        /// This allows us to make get requests for installers.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="installerIdentifier">Installer Identifier for the installer.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.InstallerGet)]
        public async Task<IActionResult> InstallerGetAsync(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                FunctionConstants.FunctionGet,
                Route = "packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier?}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            string installerIdentifier,
            ILogger log)
        {
            List<Installer> installers = new List<Installer>();

            try
            {
                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                Installers cosmosInstallers = cosmosDocument.Document.GetInstaller(installerIdentifier, packageVersion);
                installers.AddRange(cosmosInstallers.Select(installer => new Installer(installer)));
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

            return installers.Count switch
            {
                0 => new NoContentResult(),
                1 => new ApiObjectResult(new ApiResponse<Installer>(installers.First())),
                _ => new ApiObjectResult(new ApiResponse<List<Installer>>(installers)),
            };
        }
    }
}
