// -----------------------------------------------------------------------
// <copyright file="VersionFunctions.cs" company="Microsoft Corporation">
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
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Functions.Common;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Helpers.AppConfig;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;
    using Microsoft.WinGet.RestSource.Utils.Validators;
    using Version = Microsoft.WinGet.RestSource.Utils.Models.Schemas.Version;

    /// <summary>
    /// This class contains the functions for interacting with versions.
    /// </summary>
    public class VersionFunctions
    {
        private readonly IApiDataStore dataStore;
        private readonly IWinGetAppConfig appConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionFunctions"/> class.
        /// </summary>
        /// <param name="dataStore">Data Store.</param>
        /// <param name="appConfig">App Config.</param>
        public VersionFunctions(IApiDataStore dataStore, IWinGetAppConfig appConfig)
        {
            this.dataStore = dataStore;
            this.appConfig = appConfig;
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
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse body as Version
                version = await Parser.StreamParser<Version>(req.Body, log);
                ApiDataValidator.Validate(version);

                // Save Document
                await this.dataStore.AddVersion(packageIdentifier, version);
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
                        Geneva.ErrorMetrics.ServerInformationError,
                        FunctionConstants.VersionPost,
                        req.Path.Value,
                        headers,
                        version,
                        e,
                        log);
                }

                return ActionResultHelper.UnhandledError(e);
            }

            return new ApiObjectResult(new ApiResponse<Version>(version));
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
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);
                await this.dataStore.DeleteVersion(packageIdentifier, packageVersion);
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
                        Geneva.ErrorMetrics.ServerInformationError,
                        FunctionConstants.VersionDelete,
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
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse body as Version
                version = await Parser.StreamParser<Version>(req.Body, log);
                ApiDataValidator.Validate(version);

                // Validate Versions Match
                if (version.PackageVersion != packageVersion)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.VersionDoesNotMatchErrorCode,
                            ErrorConstants.VersionDoesNotMatchErrorMessage));
                }

                await this.dataStore.UpdateVersion(packageIdentifier, packageVersion, version);
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
                        Geneva.ErrorMetrics.ServerInformationError,
                        FunctionConstants.VersionPut,
                        req.Path.Value,
                        headers,
                        version,
                        e,
                        log);
                }

                return ActionResultHelper.UnhandledError(e);
            }

            return new ApiObjectResult(new ApiResponse<Version>(version));
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
            ApiDataPage<Version> versions;
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                versions = await this.dataStore.GetVersions(packageIdentifier, packageVersion);
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
                        Geneva.ErrorMetrics.ServerInformationError,
                        FunctionConstants.VersionGet,
                        req.Path.Value,
                        headers,
                        e,
                        log);
                }

                return ActionResultHelper.UnhandledError(e);
            }

            return versions.Items.Count switch
            {
                0 => new NoContentResult(),
                1 => new ApiObjectResult(new ApiResponse<Version>(versions.Items.First(), versions.ContinuationToken)),
                _ => new ApiObjectResult(new ApiResponse<List<Version>>(versions.Items.ToList(), versions.ContinuationToken)),
            };
        }
    }
}
