// -----------------------------------------------------------------------
// <copyright file="Update.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Operations
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Msix.Utils.Logger;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WindowsPackageManager.Rest.Models;
    using Microsoft.WindowsPackageManager.Rest.Utils;
    using Microsoft.WinGet.RestSource.Interfaces;
    using Microsoft.WinGet.RestSource.Utils;
    using Microsoft.WinGet.RestSource.Utils.Extensions;
    using Microsoft.WinGetUtil.Models.V1;

    /// <summary>
    /// Class that contains update operations.
    /// </summary>
    public class Update : IUpdate
    {
        private const int RetryWaitTime = 30000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Update"/> class.
        /// </summary>
        internal Update()
        {
        }

        /// <inheritdoc/>
        public async Task ProcessUpdateRequestAsync(
            HttpClient httpClient,
            string operationId,
            string commit,
            string sasManifestUrl,
            ReferenceType referenceType,
            IRestSourceTriggerFunction restSourceTriggerFunction,
            string azFuncHostKey,
            LoggingContext loggingContext)
        {
            Logger.Info($"{loggingContext}Processing commit {commit} from {operationId} for type {referenceType}");
            string logIdentifier = $"{operationId}_{commit}";

            var manifestData = await httpClient.GetStringWithRetryAsync(
                sasManifestUrl,
                nameof(this.ProcessUpdateRequestAsync),
                loggingContext);
            var manifest = Manifest.CreateManifestFromString(manifestData);

            if (referenceType == ReferenceType.Add)
            {
                await this.ProcessManifestAddAsync(
                    httpClient,
                    manifest,
                    restSourceTriggerFunction,
                    azFuncHostKey,
                    loggingContext);
            }
            else if (referenceType == ReferenceType.Modify)
            {
                await this.ProcessManifestModifyAsync(
                    httpClient,
                    manifest,
                    restSourceTriggerFunction,
                    azFuncHostKey,
                    loggingContext);
            }
            else if (referenceType == ReferenceType.Delete)
            {
                await this.ProcessManifestDeleteAsync(
                    httpClient,
                    manifest,
                    restSourceTriggerFunction,
                    azFuncHostKey,
                    loggingContext);
            }
            else
            {
                throw new InvalidOperationException(referenceType.ToString());
            }
        }

        /// <summary>
        /// Process an manifest add.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="manifest">Manifest.</param>
        /// <param name="restSourceTriggerFunction">Rest source trigger.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal async Task ProcessManifestAddAsync(
            HttpClient httpClient,
            Manifest manifest,
            IRestSourceTriggerFunction restSourceTriggerFunction,
            string azFuncHostKey,
            LoggingContext loggingContext)
        {
            // Check if the package already exists.
            var packageManifest = await restSourceTriggerFunction.GetPackageManifestAsync(
                httpClient,
                manifest.Id,
                azFuncHostKey,
                loggingContext);
            if (packageManifest == null)
            {
                Logger.Info($"{loggingContext}Package {manifest.Id} doesn't exists");
                packageManifest = PackageManifestUtils.AddManifestToPackageManifest(manifest);
                await RetryHelper.RunAndRetryWithExceptionAsync<HttpRequestException>(
                    async () =>
                    {
                        await restSourceTriggerFunction.PostPackageManifestAsync(
                            httpClient,
                            packageManifest,
                            azFuncHostKey,
                            loggingContext);
                    },
                    manifest.Id,
                    loggingContext,
                    waitTime: RetryWaitTime);
            }
            else
            {
                Logger.Info($"{loggingContext}Package {manifest.Id} exists");

                // The package exists, but because this is an add the version mustn't.
                bool versionExists = packageManifest.Versions.VersionExists(manifest.Version);
                if (versionExists)
                {
                    throw new InvalidOperationException($"Package {manifest.Id} {manifest.Version} already exists.");
                }

                packageManifest = PackageManifestUtils.AddManifestToPackageManifest(manifest, packageManifest);
                await RetryHelper.RunAndRetryWithExceptionAsync<HttpRequestException>(
                    async () =>
                    {
                        await restSourceTriggerFunction.PutPackageManifestAsync(
                            httpClient,
                            packageManifest,
                            azFuncHostKey,
                            loggingContext);
                    },
                    manifest.Id,
                    loggingContext,
                    waitTime: RetryWaitTime);
            }
        }

        /// <summary>
        /// Process an manifest modify.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="manifest">Manifest.</param>
        /// <param name="restSourceTriggerFunction">Rest source trigger.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal async Task ProcessManifestModifyAsync(
            HttpClient httpClient,
            Manifest manifest,
            IRestSourceTriggerFunction restSourceTriggerFunction,
            string azFuncHostKey,
            LoggingContext loggingContext)
        {
            // If this is a modify then it must be already be there.
            var packageManifest = await restSourceTriggerFunction.GetPackageManifestAsync(
                httpClient,
                manifest.Id,
                azFuncHostKey,
                loggingContext);
            if (packageManifest == null)
            {
                throw new InvalidOperationException($"Package {manifest.Id} does not exists.");
            }

            // The package exists, but because this is a modify the version must.
            Logger.Info($"{loggingContext}Package {manifest.Id} exists");
            bool versionExists = packageManifest.Versions.VersionExists(manifest.Version);
            if (!versionExists)
            {
                throw new InvalidOperationException($"Package {manifest.Id} {manifest.Version} doesn't exists.");
            }

            packageManifest = PackageManifestUtils.AddManifestToPackageManifest(manifest, packageManifest);
            await RetryHelper.RunAndRetryWithExceptionAsync<HttpRequestException>(
                async () =>
                {
                    await restSourceTriggerFunction.PutPackageManifestAsync(
                        httpClient,
                        packageManifest,
                        azFuncHostKey,
                        loggingContext);
                },
                manifest.Id,
                loggingContext,
                waitTime: RetryWaitTime);
        }

        /// <summary>
        /// Process an manifest delete.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="manifest">Manifest.</param>
        /// <param name="restSourceTriggerFunction">Rest source trigger.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal async Task ProcessManifestDeleteAsync(
            HttpClient httpClient,
            Manifest manifest,
            IRestSourceTriggerFunction restSourceTriggerFunction,
            string azFuncHostKey,
            LoggingContext loggingContext)
        {
            // If this is a delete then it must be already be there.
            var packageManifest = await restSourceTriggerFunction.GetPackageManifestAsync(
                httpClient,
                manifest.Id,
                azFuncHostKey,
                loggingContext);
            if (packageManifest == null)
            {
                throw new InvalidOperationException($"Package {manifest.Id} does not exists.");
            }

            if (!packageManifest.Versions.VersionExists(manifest.Version))
            {
                throw new InvalidOperationException($"Package {manifest.Id} {manifest.Version} doesn't exists.");
            }

            // If more versions exists then just delete this version. Otherwise delete the entire package.
            if (packageManifest.Versions.Count > 1)
            {
                await RetryHelper.RunAndRetryWithExceptionAsync<HttpRequestException>(
                    async () =>
                    {
                        await restSourceTriggerFunction.DeleteVersionAsync(
                            httpClient,
                            manifest.Id,
                            manifest.Version,
                            azFuncHostKey,
                            loggingContext);
                    },
                    manifest.Id,
                    loggingContext,
                    waitTime: RetryWaitTime);
            }
            else
            {
                await RetryHelper.RunAndRetryWithExceptionAsync<HttpRequestException>(
                    async () =>
                    {
                        await restSourceTriggerFunction.DeletePackageAsync(
                            httpClient,
                            manifest.Id,
                            azFuncHostKey,
                            loggingContext);
                    },
                    manifest.Id,
                    loggingContext,
                    waitTime: RetryWaitTime);
            }
        }
    }
}
