// -----------------------------------------------------------------------
// <copyright file="PackageFunctions.cs" company="Microsoft Corporation">
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
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGet.RestSource.Utils.Validators;

    /// <summary>
    /// This class contains the functions for interacting with packages.
    /// </summary>
    public class PackageFunctions
    {
        private readonly IApiDataStore dataStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageFunctions"/> class.
        /// </summary>
        /// <param name="dataStore">Data Store.</param>
        public PackageFunctions(IApiDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        /// <summary>
        /// Package Post Function.
        /// This allows us to handle post requests for manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackagePost)]
        public async Task<IActionResult> PackagesPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "packages")]
            HttpRequest req,
            ILogger log)
        {
            Package package = null;
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse body as package
                package = await Parser.StreamParser<Package>(req.Body, log);
                ApiDataValidator.Validate(package);

                // Add Package Store
                await this.dataStore.AddPackage(package);
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                Geneva.Metrics.EmitMetricForOperation(
                    Geneva.ErrorMetrics.DatabaseUpdateError,
                    FunctionConstants.PackagePost,
                    req.Path.Value,
                    headers,
                    package,
                    e,
                    log);
                return ActionResultHelper.UnhandledError(e);
            }

            return new ApiObjectResult(new ApiResponse<Package>(package));
        }

        /// <summary>
        /// Package Delete Function.
        /// This allows us to make delete requests for packages.
        /// This will delete all sub resources as well.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackageDelete)]
        public async Task<IActionResult> PackageDeleteAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionDelete, Route = "packages/{packageIdentifier}")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);
                await this.dataStore.DeletePackage(packageIdentifier);
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                Geneva.Metrics.EmitMetricForOperation(
                    Geneva.ErrorMetrics.DatabaseUpdateError,
                    FunctionConstants.PackageDelete,
                    req.Path.Value,
                    headers,
                    e,
                    log);
                return ActionResultHelper.UnhandledError(e);
            }

            return new NoContentResult();
        }

        /// <summary>
        /// Package Put Function.
        /// This allows us to make put requests for packages.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackagePut)]
        public async Task<IActionResult> PackagesPutAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPut, Route = "packages/{packageIdentifier}")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            Package package = null;
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                // Parse body as package
                package = await Parser.StreamParser<Package>(req.Body, log);
                ApiDataValidator.Validate(package);

                // Validate Versions Match
                if (package.PackageIdentifier != packageIdentifier)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.PackageDoesNotMatchErrorCode,
                            ErrorConstants.PackageDoesNotMatchErrorMessage));
                }

                await this.dataStore.UpdatePackage(packageIdentifier, package);
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                Geneva.Metrics.EmitMetricForOperation(
                    Geneva.ErrorMetrics.DatabaseUpdateError,
                    FunctionConstants.PackagePut,
                    req.Path.Value,
                    headers,
                    package,
                    e,
                    log);
                return ActionResultHelper.UnhandledError(e);
            }

            return new ApiObjectResult(new ApiResponse<Package>(package));
        }

        /// <summary>
        /// Manifest Get Function.
        /// This allows us to make Get requests for manifests.
        /// This also allows us to query manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="packageIdentifier">Package ID.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.PackageGet)]
        public async Task<IActionResult> PackagesGetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, FunctionConstants.FunctionGet, Route = "packages/{packageIdentifier?}")]
            HttpRequest req,
            string packageIdentifier,
            ILogger log)
        {
            ApiDataPage<Package> packages;
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);

                // Fetch Results
                packages = await this.dataStore.GetPackages(packageIdentifier, headers.GetValueOrDefault(QueryConstants.ContinuationToken));
            }
            catch (DefaultException e)
            {
                log.LogError(e.ToString());
                return ActionResultHelper.ProcessError(e.InternalRestError);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                Geneva.Metrics.EmitMetricForOperation(
                    Geneva.ErrorMetrics.DatabaseGetError,
                    FunctionConstants.PackageGet,
                    req.Path.Value,
                    headers,
                    e,
                    log);
                return ActionResultHelper.UnhandledError(e);
            }

            return packages.Items.Count switch
            {
                0 => new NoContentResult(),
                1 => new ApiObjectResult(new ApiResponse<Package>(packages.Items.First(), packages.ContinuationToken)),
                _ => new ApiObjectResult(new ApiResponse<List<Package>>(packages.Items.ToList(), packages.ContinuationToken)),
            };
        }
    }
}
