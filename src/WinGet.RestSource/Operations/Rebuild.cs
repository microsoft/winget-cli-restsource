// -----------------------------------------------------------------------
// <copyright file="Rebuild.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Operations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Msix.Utils.Logger;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WindowsPackageManager.Rest.Models;
    using Microsoft.WindowsPackageManager.Rest.Utils;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Interfaces;
    using Microsoft.WinGet.RestSource.PowershellSupport.Helpers;
    using Microsoft.WinGet.RestSource.Sql;
    using Microsoft.WinGet.RestSource.Utils.Extensions;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGetUtil.Models.V1;

    /// <summary>
    /// Class that contains rebuild operations.
    /// Rebuild reconstructs the rest source back end data to align with the provided sqlite index file.
    /// These are relatively expensive operation that should be used sparingly.
    /// </summary>
    public class Rebuild : IRebuild
    {
        private const int RetryWaitTime = 30000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rebuild"/> class.
        /// </summary>
        internal Rebuild()
        {
        }

        /// <inheritdoc/>
        public async Task ProcessRebuildRequestAsync(
            HttpClient httpClient,
            string operationId,
            string sasReference,
            ReferenceType type,
            IRestSourceTriggerFunction restSourceTriggerFunction,
            string manifestCacheEndpoint,
            LoggingContext loggingContext)
        {
            // Get packages from sql source.
            string databasePath = await httpClient.DownloadFileAsync(sasReference, loggingContext);
            var sqlPackages = this.GetPackagesFromDatabase(databasePath);

            // Get packages from rest source.
            var restPackages = await restSourceTriggerFunction.GetAllPackagesAsync(
                    httpClient,
                    loggingContext);
            var restPackagesSet = new HashSet<string>(restPackages.Select(p => p.PackageIdentifier));

            Logger.Info($"{loggingContext}Number of packages in database {sqlPackages.Count}");
            Logger.Info($"{loggingContext}Number of packages in rest {restPackages.Count}");
            await this.ProcessRebuildRequestInternalAsync(
                httpClient,
                operationId,
                sqlPackages,
                restPackagesSet,
                restSourceTriggerFunction,
                manifestCacheEndpoint,
                loggingContext);
        }

        /// <summary>
        /// Processes a rebuild request from database path.
        /// </summary>
        /// <param name="httpClient">The function's http client.</param>
        /// <param name="operationId">Operation id.</param>
        /// <param name="sqlPackages">Sql packages.</param>
        /// <param name="restPackages">Rest packages.</param>
        /// <param name="restSourceTriggerFunction">IRestSourceTriggerFunction.</param>
        /// <param name="manifestCacheEndpoint">Manifest cache endpoint.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal async Task ProcessRebuildRequestInternalAsync(
            HttpClient httpClient,
            string operationId,
            IReadOnlyList<SqlPackage> sqlPackages,
            HashSet<string> restPackages,
            IRestSourceTriggerFunction restSourceTriggerFunction,
            string manifestCacheEndpoint,
            LoggingContext loggingContext)
        {
            Logger.Info($"{loggingContext}Starting to process rebuild request.");

            // Process all packages in sql.
            foreach (var sqlPackage in sqlPackages)
            {
                Logger.Info($"{loggingContext}Processing {sqlPackage.Id}");

                // Create the PackageManifest with all version on it.
                PackageManifest packageManifest = null;
                foreach (var sqlVersion in sqlPackage.Versions)
                {
                    Logger.Info($"{loggingContext}Processing {sqlPackage.Id} Version '{sqlVersion.Version}' Path '{sqlVersion.PathPart}'");

                    // Download merged manifest from CDN.
                    string endpoint = Path.Combine(manifestCacheEndpoint, Uri.EscapeDataString(sqlVersion.PathPart));
                    string manifestData = await httpClient.GetStringWithRetryAsync(
                        endpoint,
                        endpoint,
                        loggingContext);
                    Manifest manifest = Manifest.CreateManifestFromString(manifestData);
                    packageManifest = PackageManifestUtils.AddManifestToPackageManifest(manifest, packageManifest);
                }

                // We already got all the package identifiers from the rest source. If it exists then we do a put
                // to override it, otherwise post.
                try
                {
                    if (restPackages.Contains(sqlPackage.Id))
                    {
                        Logger.Info($"{loggingContext}{sqlPackage.Id} exists in both sources");
                        await RetryHelper.RunAndRetryWithExceptionAsync<HttpRequestException>(
                            async () =>
                            {
                                await restSourceTriggerFunction.PutPackageManifestAsync(httpClient, packageManifest, loggingContext);
                            },
                            sqlPackage.Id,
                            loggingContext,
                            waitTime: RetryWaitTime);

                        restPackages.Remove(sqlPackage.Id);
                    }
                    else
                    {
                        Logger.Info($"{loggingContext}{sqlPackage.Id} does not exists in rest");
                        await RetryHelper.RunAndRetryWithExceptionAsync<HttpRequestException>(
                            async () =>
                            {
                                await restSourceTriggerFunction.PostPackageManifestAsync(httpClient, packageManifest, loggingContext);
                            },
                            sqlPackage.Id,
                            loggingContext,
                            waitTime: RetryWaitTime);
                    }
                }
                catch (RestSourceCallException e)
                {
                    Logger.Error($"{loggingContext} Failed processing {sqlPackage.Id} {e}");
                    throw;
                }

                Logger.Info($"{loggingContext}Succeeded processing {sqlPackage.Id}");
            }

            // Successfully added all the packages from sql, but there might still be remnants packages in rest. Delete them.
            foreach (var restPackage in restPackages)
            {
                Logger.Info($"{loggingContext}Deleting from rest source {restPackage}");
                await RetryHelper.RunAndRetryWithExceptionAsync<Exception>(
                    async () =>
                    {
                        await restSourceTriggerFunction.DeletePackageAsync(httpClient, restPackage, loggingContext);
                    },
                    restPackage,
                    loggingContext,
                    waitTime: RetryWaitTime);
            }

            Logger.Info($"{loggingContext}Finish Rebuild");
        }

        private IReadOnlyList<SqlPackage> GetPackagesFromDatabase(string databasePath)
        {
            using var sqlReader = new SqlReader(databasePath);
            return sqlReader.GetPackages();
        }
    }
}
