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
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
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
    /// This class contains the functions for interacting with installers.
    /// </summary>
    /// TODO: Refactor duplicate code to library.
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
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.InstallerPost)]
        public async Task<IActionResult> InstallerPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "packages/{id}/versions/{version}/installers")]
            HttpRequest req,
            string id,
            string version,
            ILogger log)
        {
            InstallerCore installerCore = null;

            try
            {
                // Parse body as package
                installerCore = await Parser.StreamParser<InstallerCore>(req.Body, log);

                // Validate Parsed Values
                // TODO: Validate Parsed Values

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

                // Get version
                VersionExtended versionToUpdate = new List<VersionExtended>(
                        cosmosDocument.Document.Versions.Where(versionExtended => versionExtended.Version == version))
                    .First();

                // Create list if null
                versionToUpdate.Installers ??= new List<InstallerCore>();

                // If does not exist add
                if (versionToUpdate.Installers.All(nested => nested.Sha256 != installerCore.Sha256))
                {
                    versionToUpdate.Installers.Add(installerCore);
                }
                else
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.InstallerAlreadyExistsErrorCode,
                            ErrorConstants.InstallerAlreadyExistsErrorMessage));
                }

                // Replace Version
                cosmosDocument.Document.Versions = new List<VersionExtended>(
                    cosmosDocument.Document.Versions.Where(versionExtended => versionExtended.Version != version));
                cosmosDocument.Document.Versions.Add(versionToUpdate);

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

            return new OkObjectResult(JsonConvert.SerializeObject(installerCore, Formatting.Indented));
        }

        /// <summary>
        /// Installer Delete Function.
        /// This allows us to make delete requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="sha256">SHA 256 for the installer.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.InstallerDelete)]
        public async Task<IActionResult> InstallerDeleteAsync(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "delete",
                Route = "packages/{id}/versions/{version}/installers/{sha256}")]
            HttpRequest req,
            string id,
            string version,
            string sha256,
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

                // Get version
                VersionExtended versionToUpdate = new List<VersionExtended>(
                        cosmosDocument.Document.Versions.Where(versionExtended => versionExtended.Version == version))
                    .First();

                // Validate Installer not null
                if (versionToUpdate.Installers == null)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.InstallerIsNullErrorCode,
                            ErrorConstants.InstallerIsNullErrorMessage));
                }

                // Verify Installer Exists
                if (versionToUpdate.Installers.All(installer => installer.Sha256 != sha256))
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.InstallerDoesNotExistErrorCode,
                            ErrorConstants.InstallerDoesNotExistErrorMessage));
                }

                // Remove installer
                versionToUpdate.Installers = new List<InstallerCore>(
                    versionToUpdate.Installers.Where(installer => installer.Sha256 != sha256));

                // Replace Version
                cosmosDocument.Document.Versions = new List<VersionExtended>(
                    cosmosDocument.Document.Versions.Where(versionExtended => versionExtended.Version != version));
                cosmosDocument.Document.Versions.Add(versionToUpdate);

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

            return new NoContentResult();
        }

        /// <summary>
        /// Installer Put Function.
        /// This allows us to make put requests for installers.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="sha256">SHA 256 for the installer.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.InstallerPut)]
        public async Task<IActionResult> InstallerPutAsync(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "put",
                Route = "packages/{id}/versions/{version}/installers/{sha256}")]
            HttpRequest req,
            string id,
            string version,
            string sha256,
            ILogger log)
        {
            InstallerCore installerCore = null;

            try
            {
                // Parse body as package
                installerCore = await Parser.StreamParser<InstallerCore>(req.Body, log);

                // Validate Parsed Values
                // TODO: Validate Parsed Values
                if (installerCore.Sha256 != sha256)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.InstallerDoesNotMatchErrorCode,
                            ErrorConstants.InstallerDoesNotMatchErrorMessage));
                }

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

                // Get version
                VersionExtended versionToUpdate = new List<VersionExtended>(
                        cosmosDocument.Document.Versions.Where(versionExtended => versionExtended.Version == version))
                    .First();

                // Validate Installer not null
                if (versionToUpdate.Installers == null)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.InstallerIsNullErrorCode,
                            ErrorConstants.InstallerIsNullErrorMessage));
                }

                // Verify Installer Exists
                if (versionToUpdate.Installers.All(installer => installer.Sha256 != sha256))
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.InstallerDoesNotExistErrorCode,
                            ErrorConstants.InstallerDoesNotExistErrorMessage));
                }

                // Replace installer
                versionToUpdate.Installers = new List<InstallerCore>(
                    versionToUpdate.Installers.Where(installer => installer.Sha256 != sha256));
                versionToUpdate.Installers.Add(installerCore);

                // Replace Version
                cosmosDocument.Document.Versions = new List<VersionExtended>(
                    cosmosDocument.Document.Versions.Where(versionExtended => versionExtended.Version != version));
                cosmosDocument.Document.Versions.Add(versionToUpdate);

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

            return new OkObjectResult(JsonConvert.SerializeObject(installerCore, Formatting.Indented));
        }

        /// <summary>
        /// Installer Put Function.
        /// This allows us to make put requests for installers.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="sha256">SHA 256 for the installer.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.InstallerGet)]
        public async Task<IActionResult> InstallerGetAsync(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "get",
                Route = "packages/{id}/versions/{version}/installers/{sha256?}")]
            HttpRequest req,
            string id,
            string version,
            string sha256,
            ILogger log)
        {
            ApiResponse<InstallerCore> apiResponse = new ApiResponse<InstallerCore>();

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

                // Get version
                VersionExtended versionToUpdate = new List<VersionExtended>(
                        cosmosDocument.Document.Versions.Where(versionExtended => versionExtended.Version == version))
                    .First();

                // Validate Installer not null
                if (versionToUpdate.Installers == null)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.InstallerIsNullErrorCode,
                            ErrorConstants.InstallerIsNullErrorMessage));
                }

                // Process Installers
                if (string.IsNullOrWhiteSpace(sha256))
                {
                    IEnumerable<InstallerCore> enumerable =
                        versionToUpdate.Installers.Select(installerCore => new InstallerCore(installerCore));
                    foreach (InstallerCore installerCore in enumerable)
                    {
                        apiResponse.Data.Add(installerCore);
                    }
                }
                else
                {
                    // Verify Installer Exists
                    if (versionToUpdate.Installers.All(installer => installer.Sha256 != sha256))
                    {
                        throw new InvalidArgumentException(
                            new InternalRestError(
                                ErrorConstants.InstallerDoesNotExistErrorCode,
                                ErrorConstants.InstallerDoesNotExistErrorMessage));
                    }

                    // Add Installer(s)
                    IEnumerable<InstallerCore> enumerable = versionToUpdate.Installers
                        .Where(installer => installer.Sha256 == sha256)
                        .Select(installer => new InstallerCore(installer));
                    foreach (InstallerCore installerCore in enumerable)
                    {
                        apiResponse.Data.Add(installerCore);
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