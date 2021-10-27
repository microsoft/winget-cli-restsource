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
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Functions.Common;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="LocaleFunctions"/> class.
        /// </summary>
        /// <param name="dataStore">Data Store.</param>
        public LocaleFunctions(IApiDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        /// <summary>
        /// Locale Post Function.
        /// This allows us to make post requests for locales.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.LocalePost)]
        public async Task<IActionResult> LocalePostAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "packages/{packageIdentifier}/versions/{packageVersion}/locales")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            ILogger log)
        {
            Locale locale;
            try
            {
                // Parse Headers
                Dictionary<string, string> headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse body as locale
                locale = await Parser.StreamParser<Locale>(req.Body, log);
                ApiDataValidator.Validate(locale);

                await this.dataStore.AddLocale(packageIdentifier, packageVersion, locale);
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
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.LocaleDelete)]
        public async Task<IActionResult> LocaleDeleteAsync(
            [HttpTrigger(
                AuthorizationLevel.Function,
                FunctionConstants.FunctionDelete,
                Route = "packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            string packageLocale,
            ILogger log)
        {
            try
            {
                // Parse Headers
                Dictionary<string, string> headers = HeaderProcessor.ToDictionary(req.Headers);
                await this.dataStore.DeleteLocale(packageIdentifier, packageVersion, packageLocale);
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
        /// Locale Put Function.
        /// This allows us to make put requests for locale.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="packageVersion">Version ID.</param>
        /// <param name="packageLocale">Package locale.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.LocalePut)]
        public async Task<IActionResult> LocalePutAsync(
            [HttpTrigger(
                AuthorizationLevel.Function,
                FunctionConstants.FunctionPut,
                Route = "packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            string packageLocale,
            ILogger log)
        {
            Locale locale;
            try
            {
                // Parse Headers
                Dictionary<string, string> headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse body as package
                locale = await Parser.StreamParser<Locale>(req.Body, log);
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
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
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
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.LocaleGet)]
        public async Task<IActionResult> LocaleGetAsync(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                FunctionConstants.FunctionGet,
                Route = "packages/{packageIdentifier}/versions/{packageVersion}/locales/{packageLocale?}")]
            HttpRequest req,
            string packageIdentifier,
            string packageVersion,
            string packageLocale,
            ILogger log)
        {
            ApiDataPage<Locale> locales;

            try
            {
                // Parse Headers
                Dictionary<string, string> headers = HeaderProcessor.ToDictionary(req.Headers);

                locales = await this.dataStore.GetLocales(packageIdentifier, packageVersion, packageLocale);
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

            return locales.Items.Count switch
            {
                0 => new NoContentResult(),
                1 => new ApiObjectResult(new ApiResponse<Locale>(locales.Items.First(), locales.ContinuationToken)),
                _ => new ApiObjectResult(new ApiResponse<List<Locale>>(locales.Items.ToList(), locales.ContinuationToken)),
            };
        }
    }
}
