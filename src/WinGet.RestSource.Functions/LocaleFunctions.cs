// -----------------------------------------------------------------------
// <copyright file="LocaleFunctions.cs" company="Microsoft Corporation">
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
    using Microsoft.Azure.Functions.Worker;
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
    /// This class contains the functions for interacting with locales.
    /// </summary>
    public class LocaleFunctions
    {
        private readonly IApiDataStore dataStore;
        private readonly IWinGetAppConfig appConfig;
        private readonly ILogger<LocaleFunctions> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocaleFunctions"/> class.
        /// </summary>
        /// <param name="dataStore">Data Store.</param>
        /// <param name="appConfig">App Config.</param>
        /// <param name="logger">Logger.</param>
        public LocaleFunctions(IApiDataStore dataStore, IWinGetAppConfig appConfig, ILogger<LocaleFunctions> logger)
        {
            this.dataStore = dataStore;
            this.appConfig = appConfig;
            this.logger = logger;
        }

        /// <summary>
        /// Locale Post Function.
        /// This allows us to make post requests for locales.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <returns>IActionResult.</returns>
        [Function(FunctionConstants.LocalePost)]
        public async Task<IActionResult> LocalePostAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "packages/{packageIdentifier}/versions/{packageVersion}/locales")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion)
        {
            Locale locale = null;
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse body as locale
                locale = await Parser.StreamParser<Locale>(req.Body, this.logger);
                ApiDataValidator.Validate(locale);

                await this.dataStore.AddLocale(packageIdentifier, packageVersion, locale);
            }
            catch (DefaultException e)
            {
                this.logger.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                this.logger.LogError(e.ToString());

                if (await this.appConfig.IsEnabledAsync(FeatureFlag.GenevaLogging, null))
                {
                    Geneva.Metrics.EmitMetricForOperation(
                        Geneva.ErrorMetrics.DatabaseUpdateError,
                        FunctionConstants.LocalePost,
                        req.Path.Value,
                        headers,
                        locale,
                        e,
                        this.logger);
                }

                return ActionResultHelper.UnhandledError(e);
            }

            return new ApiObjectResult(new ApiResponse<Locale>(locale));
        }

        /// <summary>
        /// Locale Delete Function.
        /// This allows us to make delete requests for locales.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="packageLocale">Package locale.</param>
        /// <returns>IActionResult.</returns>
        [Function(FunctionConstants.LocaleDelete)]
        public async Task<IActionResult> LocaleDeleteAsync(
            [HttpTrigger(
                AuthorizationLevel.Function,
                FunctionConstants.FunctionDelete,
                Route = "packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            string packageLocale)
        {
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);
                await this.dataStore.DeleteLocale(packageIdentifier, packageVersion, packageLocale);
            }
            catch (DefaultException e)
            {
                this.logger.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                this.logger.LogError(e.ToString());

                if (await this.appConfig.IsEnabledAsync(FeatureFlag.GenevaLogging, null))
                {
                    Geneva.Metrics.EmitMetricForOperation(
                        Geneva.ErrorMetrics.DatabaseUpdateError,
                        FunctionConstants.LocaleDelete,
                        req.Path.Value,
                        headers,
                        e,
                        this.logger);
                }

                return ActionResultHelper.UnhandledError(e);
            }

            return new NoContentResult();
        }

        /// <summary>
        /// Locale Put Function.
        /// This allows us to make put requests for locale.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="packageLocale">Package locale.</param>
        /// <returns>IActionResult.</returns>
        [Function(FunctionConstants.LocalePut)]
        public async Task<IActionResult> LocalePutAsync(
            [HttpTrigger(
                AuthorizationLevel.Function,
                FunctionConstants.FunctionPut,
                Route = "packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            string packageLocale)
        {
            Locale locale = null;
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse body as package
                locale = await Parser.StreamParser<Locale>(req.Body, this.logger);
                ApiDataValidator.Validate(locale);

                if (locale.PackageLocale != packageLocale)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.LocaleDoesNotMatchErrorCode,
                            ErrorConstants.LocaleDoesNotMatchErrorMessage));
                }

                await this.dataStore.UpdateLocale(packageIdentifier, packageVersion, packageLocale, locale);
            }
            catch (DefaultException e)
            {
                this.logger.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                this.logger.LogError(e.ToString());

                if (await this.appConfig.IsEnabledAsync(FeatureFlag.GenevaLogging, null))
                {
                    Geneva.Metrics.EmitMetricForOperation(
                        Geneva.ErrorMetrics.DatabaseUpdateError,
                        FunctionConstants.LocalePut,
                        req.Path.Value,
                        headers,
                        locale,
                        e,
                        this.logger);
                }

                return ActionResultHelper.UnhandledError(e);
            }

            return new ApiObjectResult(new ApiResponse<Locale>(locale));
        }

        /// <summary>
        /// Locale Get Function.
        /// This allows us to make put requests for locales.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="packageLocale">Package locale.</param>
        /// <returns>IActionResult.</returns>
        [Function(FunctionConstants.LocaleGet)]
        public async Task<IActionResult> LocaleGetAsync(
            [HttpTrigger(
#pragma warning disable SA1114 // Parameter list should follow declaration
#if WINGET_REST_SOURCE_LEGACY_SUPPORT
                AuthorizationLevel.Anonymous,
#else
                AuthorizationLevel.Function,
#endif
#pragma warning restore SA1114 // Parameter list should follow declaration
                FunctionConstants.FunctionGet,
                Route = "packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale?}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            string packageLocale)
        {
            ApiDataPage<Locale> locales;
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                locales = await this.dataStore.GetLocales(packageIdentifier, packageVersion, packageLocale);
            }
            catch (DefaultException e)
            {
                this.logger.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                this.logger.LogError(e.ToString());

                if (await this.appConfig.IsEnabledAsync(FeatureFlag.GenevaLogging, null))
                {
                    Geneva.Metrics.EmitMetricForOperation(
                        Geneva.ErrorMetrics.DatabaseGetError,
                        FunctionConstants.LocaleGet,
                        req.Path.Value,
                        headers,
                        e,
                        this.logger);
                }

                return ActionResultHelper.UnhandledError(e);
            }

            return locales.Items.Count switch
            {
                0 => new NoContentResult(),
                1 => new ApiObjectResult(new ApiResponse<Locale>(locales.Items.First(), locales.ContinuationToken)),
                _ => new ApiObjectResult(new ApiResponse<List<Locale>>(locales.Items.ToList(), locales.ContinuationToken)),
            };
        }
    }
}
