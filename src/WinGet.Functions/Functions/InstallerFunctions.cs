// -----------------------------------------------------------------------
// <copyright file="InstallerFunctions.cs" company="Microsoft Corporation">
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
    /// This class contains the functions for interacting with installers.
    /// </summary>
    public static class InstallerFunctions
    {
        /// <summary>
        /// Installer Post Function.
        /// This allows us to make post requests for installers.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName("InstallerPost")]
        public static async Task<IActionResult> InstallerPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "packages/{id}/versions/{version}/installers")] HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            string id,
            string version,
            ILogger log)
        {
            InstallerCore installerCore = null;

            try
            {
                // Parse body as package
                installerCore = await Parser.StreamParser<InstallerCore>(req.Body, log);

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

                // Get version
                VersionExtended versionToUpdate = new List<VersionExtended>(manifest.Versions.Where(versionExtended => versionExtended.Version == version)).First();

                // Create list if null
                if (versionToUpdate.Installers == null)
                {
                    versionToUpdate.Installers = new List<InstallerCore>();
                }

                // If does not exist add
                if (!versionToUpdate.Installers.Any(nested => nested.Sha256 == installerCore.Sha256))
                {
                    versionToUpdate.Installers.Add(installerCore);
                }
                else
                {
                    throw new Exception();
                }

                log.LogInformation(JsonConvert.SerializeObject(versionToUpdate, Formatting.Indented));

                // Replace Version
                manifest.Versions = new List<VersionExtended>(manifest.Versions.Where(versionExtended => versionExtended.Version != version));
                log.LogInformation(JsonConvert.SerializeObject(manifest, Formatting.Indented));

                if (manifest.Versions == null)
                {
                    manifest.Versions = new List<VersionExtended>();
                }

                manifest.Versions.Add(versionToUpdate);
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

            return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(installerCore, Formatting.Indented));
        }

        /// <summary>
        /// Installer Delete Function.
        /// This allows us to make delete requests for versions.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="sha256">SHA 256 for the installer.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName("InstallerDelete")]
        public static async Task<IActionResult> InstallerDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "packages/{id}/versions/{version}/installers/{sha256}")] HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            string id,
            string version,
            string sha256,
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

                // Get version
                VersionExtended versionToUpdate = new List<VersionExtended>(manifest.Versions.Where(versionExtended => versionExtended.Version == version)).First();

                // If installer does not exist, throw
                if (versionToUpdate.Installers == null || !versionToUpdate.Installers.Any(nested => nested.Sha256 == sha256))
                {
                    throw new Exception();
                }

                // Remove installer
                versionToUpdate.Installers = new List<InstallerCore>(versionToUpdate.Installers.Where(installer => installer.Sha256 != sha256));

                // Replace Version
                manifest.Versions = new List<VersionExtended>(manifest.Versions.Where(versionExtended => versionExtended.Version != version));
                log.LogInformation(JsonConvert.SerializeObject(manifest, Formatting.Indented));

                if (manifest.Versions == null)
                {
                    manifest.Versions = new List<VersionExtended>();
                }

                manifest.Versions.Add(versionToUpdate);
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

            return (ActionResult)new OkObjectResult("Deleted");
        }

        /// <summary>
        /// Installer Put Function.
        /// This allows us to make put requests for installers.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="sha256">SHA 256 for the installer.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName("InstallerPut")]
        public static async Task<IActionResult> InstallerPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "packages/{id}/versions/{version}/installers/{sha256}")] HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
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
                installerCore.Sha256 = sha256;

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

                // Get version
                VersionExtended versionToUpdate = new List<VersionExtended>(manifest.Versions.Where(versionExtended => versionExtended.Version == version)).First();

                // If installer does not exist throw
                if (!versionToUpdate.Installers.Any(nested => nested.Sha256 == installerCore.Sha256))
                {
                    throw new Exception();
                }

                // Remove installer
                versionToUpdate.Installers = new List<InstallerCore>(versionToUpdate.Installers.Where(installer => installer.Sha256 != sha256));

                // Create list if null
                if (versionToUpdate.Installers == null)
                {
                    versionToUpdate.Installers = new List<InstallerCore>();
                }

                // Add Version
                versionToUpdate.Installers.Add(installerCore);

                // Replace Version
                manifest.Versions = new List<VersionExtended>(manifest.Versions.Where(versionExtended => versionExtended.Version != version));

                if (manifest.Versions == null)
                {
                    manifest.Versions = new List<VersionExtended>();
                }

                manifest.Versions.Add(versionToUpdate);

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

            return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(installerCore, Formatting.Indented));
        }

        /// <summary>
        /// Installer Put Function.
        /// This allows us to make put requests for installers.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="client">CosmosDB DocumentClient.</param>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Version ID.</param>
        /// <param name="sha256">SHA 256 for the installer.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName("InstallerGet")]
        public static async Task<IActionResult> InstallerGetAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "packages/{id}/versions/{version}/installers/{sha256?}")] HttpRequest req,
            [CosmosDB(
                databaseName: CosmosConnectionConstants.DatabaseName,
                collectionName: CosmosConnectionConstants.CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            string id,
            string version,
            string sha256,
            ILogger log)
        {
            List<InstallerCore> installerCores = new List<InstallerCore>();

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

                // Get version
                VersionExtended versionToUpdate = new List<VersionExtended>(manifest.Versions.Where(versionExtended => versionExtended.Version == version)).First();

                // Check for versions
                if (versionToUpdate.Installers == null)
                {
                    throw new Exception();
                }

                if (sha256 == null)
                {
                    installerCores.AddRange(versionToUpdate.Installers.Select(installerCore => new InstallerCore(installerCore)));
                }
                else
                {
                    if (!versionToUpdate.Installers.Any(installer => installer.Sha256 == sha256))
                    {
                        throw new Exception();
                    }

                    installerCores.AddRange(from installer in versionToUpdate.Installers where installer.Sha256 == sha256 select new InstallerCore(installer));
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

            return installerCores.Count switch
            {
                0 => new NoContentResult(),
                1 => new OkObjectResult(JsonConvert.SerializeObject(installerCores.First(), Formatting.Indented)),
                _ => new OkObjectResult(JsonConvert.SerializeObject(installerCores, Formatting.Indented))
            };
        }
    }
}