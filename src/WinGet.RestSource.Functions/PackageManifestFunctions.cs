// -----------------------------------------------------------------------
// <copyright file="PackageManifestFunctions.cs" company="Microsoft Corporation">
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
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGet.RestSource.Utils.Validators;

    /// <summary>
    /// This class contains the functions for interacting with manifests.
    /// </summary>
    public class PackageManifestFunctions
    {
        private readonly IApiDataStore dataStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageManifestFunctions"/> class.
        /// </summary>
        /// <param name="dataStore">Data Store.</param>
        public PackageManifestFunctions(IApiDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        /// <summary>
        /// Manifest Post Function.
        /// This allows us to handle post requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestPost)]
        public async Task<IActionResult> ManifestPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "packageManifests")]
            HttpRequest req,
            ILogger log)
        {
            PackageManifest packageManifest;
            try
            {
                // Parse Headers
                Dictionary<string, string> headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse Stream
                packageManifest = await Parser.StreamParser<PackageManifest>(req.Body, log);
                ApiDataValidator.Validate(packageManifest);

                await this.dataStore.AddPackageManifest(packageManifest);
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

            return new ApiObjectResult(new ApiResponse<PackageManifest>(packageManifest));
        }

        /// <summary>
        /// Manifest Delete Function.
        /// This allows us to make delete requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestDelete)]
        public async Task<IActionResult> ManifestDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionDelete, Route = "packageManifests/{packageIdentifier}")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            try
            {
                // Parse Headers
                Dictionary<string, string> headers = HeaderProcessor.ToDictionary(req.Headers);
                await this.dataStore.DeletePackageManifest(packageIdentifier);
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
        /// Manifest Put Function.
        /// This allows us to make put requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Manifest ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestPut)]
        public async Task<IActionResult> ManifestPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPut, Route = "packageManifests/{packageIdentifier}")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            PackageManifest packageManifest;
            try
            {
                // Parse Headers
                Dictionary<string, string> headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse Stream
                packageManifest = await Parser.StreamParser<PackageManifest>(req.Body, log);
                ApiDataValidator.Validate(packageManifest);

                // Validate Versions Match
                if (packageManifest.PackageIdentifier != packageIdentifier)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.PackageDoesNotMatchErrorCode,
                            ErrorConstants.PackageDoesNotMatchErrorMessage));
                }

                await this.dataStore.UpdatePackageManifest(packageIdentifier, packageManifest);
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

            return new ApiObjectResult(new ApiResponse<PackageManifest>(packageManifest));
        }

        /// <summary>
        /// Manifest Get Function.
        /// This allows us to make Get requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestGet)]
        public async Task<IActionResult> ManifestGetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, FunctionConstants.FunctionGet, Route = "packageManifests/{packageIdentifier?}")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            ApiDataPage<PackageManifest> manifests;
            QueryParameters unsupportedQueryParameters;
            QueryParameters requiredQueryParameters;

            try
            {
                // Parse Headers
                Dictionary<string, string> headers = HeaderProcessor.ToDictionary(req.Headers);
                string continuationToken = headers.GetValueOrDefault(QueryConstants.ContinuationToken);

                string versionFilter = null;
                string channelFilter = null;
                string marketFilter = null;

                // Schema supports query parameters only when PackageIdentifier is specified.
                if (!string.IsNullOrWhiteSpace(packageIdentifier))
                {
                    versionFilter = req.Query[QueryConstants.Version];
                    channelFilter = req.Query[QueryConstants.Channel];
                    marketFilter = req.Query[QueryConstants.Market];
                }

                manifests = await this.dataStore.GetPackageManifests(packageIdentifier, continuationToken, versionFilter, channelFilter, marketFilter);
                unsupportedQueryParameters = UnsupportedAndRequiredFieldsHelper.GetUnsupportedQueryParametersFromRequest(req.Query, ApiConstants.UnsupportedQueryParameters);
                requiredQueryParameters = UnsupportedAndRequiredFieldsHelper.GetRequiredQueryParametersFromRequest(req.Query, ApiConstants.RequiredQueryParameters);
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

            return manifests.Items.Count switch
            {
                0 => new NoContentResult(),
                1 => new ApiObjectResult(new GetPackageManifestApiResponse<PackageManifest>(manifests.Items.First(), manifests.ContinuationToken)
                {
                    UnsupportedQueryParameters = unsupportedQueryParameters,
                    RequiredQueryParameters = requiredQueryParameters,
                }),
                _ => new ApiObjectResult(new GetPackageManifestApiResponse<List<PackageManifest>>(manifests.Items.ToList(), manifests.ContinuationToken)
                {
                    UnsupportedQueryParameters = unsupportedQueryParameters,
                    RequiredQueryParameters = requiredQueryParameters,
                }),
            };
        }
    }
}
