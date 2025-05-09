// -----------------------------------------------------------------------
// <copyright file="InstallerFunctions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
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
    using Microsoft.Azure.Cosmos.Linq;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.AppConfig;
    using Microsoft.WinGet.RestSource.Functions.Common;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGet.RestSource.Utils.Validators;

    /// <summary>
    /// This class contains the functions for interacting with installers.
    /// </summary>
    public class InstallerFunctions
    {
        private readonly IApiDataStore dataStore;
        private readonly IWinGetAppConfig appConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerFunctions"/> class.
        /// </summary>
        /// <param name="dataStore">Data Store.</param>
        /// <param name="appConfig">App Config.</param>
        public InstallerFunctions(IApiDataStore dataStore, IWinGetAppConfig appConfig)
        {
            this.dataStore = dataStore;
            this.appConfig = appConfig;
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
            [HttpTrigger(
                AuthorizationLevel.Function,
                FunctionConstants.FunctionPost,
                Route = "packages/{packageIdentifier}/versions/{packageVersion}/installers")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            ILogger log)
        {
            Installer installer = null;
            Dictionary<string, string> headers = null;
            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse body as installer
                installer = await Parser.StreamParser<Installer>(req.Body, log);
                ApiDataValidator.Validate(installer);

                await this.dataStore.AddInstaller(packageIdentifier, packageVersion, installer);
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());

                if (await this.appConfig.IsEnabledAsync(FeatureFlag.GenevaLogging, null))
                {
                    Geneva.Metrics.EmitMetricForOperation(
                        Geneva.ErrorMetrics.DatabaseUpdateError,
                        FunctionConstants.InstallerPost,
                        req.Path.Value,
                        headers,
                        installer,
                        e,
                        log);
                }

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
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);
                await this.dataStore.DeleteInstaller(packageIdentifier, packageVersion, installerIdentifier);
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());

                if (await this.appConfig.IsEnabledAsync(FeatureFlag.GenevaLogging, null))
                {
                    Geneva.Metrics.EmitMetricForOperation(
                        Geneva.ErrorMetrics.DatabaseUpdateError,
                        FunctionConstants.InstallerDelete,
                        req.Path.Value,
                        headers,
                        e,
                        log);
                }

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
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse body as package
                installer = await Parser.StreamParser<Installer>(req.Body, log);
                ApiDataValidator.Validate(installer);

                if (installer.InstallerIdentifier != installerIdentifier)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.InstallerDoesNotMatchErrorCode,
                            ErrorConstants.InstallerDoesNotMatchErrorMessage));
                }

                await this.dataStore.UpdateInstaller(packageIdentifier, packageVersion, installerIdentifier, installer);
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());

                if (await this.appConfig.IsEnabledAsync(FeatureFlag.GenevaLogging, null))
                {
                    Geneva.Metrics.EmitMetricForOperation(
                        Geneva.ErrorMetrics.DatabaseUpdateError,
                        FunctionConstants.InstallerPut,
                        req.Path.Value,
                        headers,
                        installer,
                        e,
                        log);
                }

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
#pragma warning disable SA1114 // Parameter list should follow declaration
#if WINGET_REST_SOURCE_LEGACY_SUPPORT
                AuthorizationLevel.Anonymous,
#else
                AuthorizationLevel.Function,
#endif
#pragma warning restore SA1114 // Parameter list should follow declaration
                FunctionConstants.FunctionGet,
                Route = "packages/{packageIdentifier}/versions/{packageVersion}/installers/{installerIdentifier?}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            string installerIdentifier,
            ILogger log)
        {
            ApiDataPage<Installer> installers;
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                installers = await this.dataStore.GetInstallers(packageIdentifier, packageVersion, installerIdentifier);
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());

                if (await this.appConfig.IsEnabledAsync(FeatureFlag.GenevaLogging, null))
                {
                    Geneva.Metrics.EmitMetricForOperation(
                        Geneva.ErrorMetrics.DatabaseGetError,
                        FunctionConstants.InstallerGet,
                        req.Path.Value,
                        headers,
                        e,
                        log);
                }

                return ActionResultHelper.UnhandledError(e);
            }

            return installers.Items.Count switch
            {
                0 => new NoContentResult(),
                1 => new ApiObjectResult(new ApiResponse<Installer>(installers.Items.First(), installers.ContinuationToken)),
                _ => new ApiObjectResult(new ApiResponse<List<Installer>>(installers.Items.ToList(), installers.ContinuationToken)),
            };
        }
    }
}
