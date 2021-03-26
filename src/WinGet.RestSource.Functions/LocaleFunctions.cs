// -----------------------------------------------------------------------
// <copyright file="LocaleFunctions.cs" company="Microsoft Corporation">
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
    /// This class contains the functions for interacting with locales.
    /// </summary>
    public class LocaleFunctions
    {
        private readonly ICosmosDatabase cosmosDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocaleFunctions"/> class.
        /// </summary>
        /// <param name="cosmosDatabase">Cosmos Database.</param>
        public LocaleFunctions(ICosmosDatabase cosmosDatabase)
        {
            this.cosmosDatabase = cosmosDatabase;
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
            Locale locale = null;

            try
            {
                // Parse body as locale
                locale = await Parser.StreamParser<Locale>(req.Body, log);
                ApiDataValidator.Validate<Locale>(locale);

                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument = await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Add Locale
                cosmosDocument.Document.AddLocale(locale, packageVersion);

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
                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument = await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Remove locale
                cosmosDocument.Document.RemoveLocale(packageLocale, packageVersion);

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
            Locale locale = null;

            try
            {
                // Parse body as package
                locale = await Parser.StreamParser<Locale>(req.Body, log);
                ApiDataValidator.Validate<Locale>(locale);

                if (locale.PackageLocale != packageLocale)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.LocaleDoesNotMatchErrorCode,
                            ErrorConstants.LocaleDoesNotMatchErrorMessage));
                }

                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                // Update locale
                cosmosDocument.Document.UpdateLocale(locale, packageVersion);

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
            List<Locale> locales = new List<Locale>();

            try
            {
                // Fetch Current Package
                CosmosDocument<CosmosPackageManifest> cosmosDocument =
                    await this.cosmosDatabase.GetByIdAndPartitionKey<CosmosPackageManifest>(packageIdentifier, packageIdentifier);
                log.LogInformation(FormatJSON.Indented(cosmosDocument, log));

                Locales cosmosLocales = cosmosDocument.Document.GetLocale(packageLocale, packageVersion);
                locales.AddRange(cosmosLocales.Select(locale => new Locale(locale)));
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

            return locales.Count switch
            {
                0 => new NoContentResult(),
                1 => new ApiObjectResult(new ApiResponse<Locale>(locales.First())),
                _ => new ApiObjectResult(new ApiResponse<List<Locale>>(locales)),
            };
        }
    }
}
