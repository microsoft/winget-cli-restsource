// -----------------------------------------------------------------------
// <copyright file="ServerFunctions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using Microsoft.WinGet.RestSource.AppConfig;
    using Microsoft.WinGet.RestSource.Functions.Common;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;

    /// <summary>
    /// This class contains the functions for interacting with packages.
    /// </summary>
    public class ServerFunctions
    {
        private readonly IWinGetAppConfig appConfig;
        private readonly ILogger<ServerFunctions> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerFunctions"/> class.
        /// </summary>
        /// <param name="appConfig">App Config.</param>
        /// <param name="logger">Logger.</param>
        public ServerFunctions(IWinGetAppConfig appConfig, ILogger<ServerFunctions> logger)
        {
            this.appConfig = appConfig;
            this.logger = logger;
        }

        /// <summary>
        /// Server Information Get Function.
        /// This allows us to make Get Server Information.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <returns>IActionResult.</returns>
        [Function(FunctionConstants.InformationGet)]
        public IActionResult InformationGetAsync(
            [HttpTrigger(
#pragma warning disable SA1114 // Parameter list should follow declaration
#if WINGET_REST_SOURCE_LEGACY_SUPPORT
                AuthorizationLevel.Anonymous,
#else
                AuthorizationLevel.Function,
#endif
#pragma warning restore SA1114 // Parameter list should follow declaration
                FunctionConstants.FunctionGet,
                Route = "information")]
            HttpRequest req)
        {
            Information information = null;
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);
                information = new Information();
            }
            catch (DefaultException e)
            {
                this.logger.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                this.logger.LogError(e.ToString());

                if (this.appConfig.IsEnabled(FeatureFlag.GenevaLogging, null))
                {
                    Geneva.Metrics.EmitMetricForOperation(
                        Geneva.ErrorMetrics.ServerInformationError,
                        FunctionConstants.InformationGet,
                        req.Path.Value,
                        headers,
                        information,
                        e,
                        this.logger);
                }

                return ActionResultHelper.UnhandledError(e);
            }

            return new ApiObjectResult(new ApiResponse<Information>(information));
        }
    }
}
