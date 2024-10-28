// -----------------------------------------------------------------------
// <copyright file="ManifestSearchFunctions.cs" company="Microsoft Corporation">
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
    using Microsoft.WinGet.RestSource.AppConfig;
    using Microsoft.WinGet.RestSource.Functions.Common;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGet.RestSource.Utils.Validators;

    /// <summary>
    /// This class contains the functions for searching manifests.
    /// </summary>
    public class ManifestSearchFunctions
    {
        private readonly IApiDataStore dataStore;
        private readonly IWinGetAppConfig appConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchFunctions"/> class.
        /// </summary>
        /// <param name="dataStore">Data Store.</param>
        /// <param name="appConfig">App Config.</param>
        public ManifestSearchFunctions(IApiDataStore dataStore, IWinGetAppConfig appConfig)
        {
            this.dataStore = dataStore;
            this.appConfig = appConfig;
        }

        /// <summary>
        /// Manifest Search Post Function.
        /// This also allows us to query manifests.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.ManifestSearchPost)]
        public async Task<IActionResult> ManifestSearchPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "manifestSearch")]
            HttpRequest req,
            ILogger log)
        {
            ApiDataPage<ManifestSearchResponse> manifestSearchResponse;
            PackageMatchFields unsupportedFields;
            PackageMatchFields requiredFields;
            ManifestSearchRequest manifestSearch = null;
            Dictionary<string, string> headers = null;

            try
            {
                // Parse Headers
                headers = HeaderProcessor.ToDictionary(req.Headers);
                string continuationToken = headers.GetValueOrDefault(HeaderConstants.ContinuationToken);

                // Get Manifest Search Request and Validate.
                manifestSearch = await Parser.StreamParser<ManifestSearchRequest>(req.Body, log);
                ApiDataValidator.Validate(manifestSearch);

                manifestSearchResponse = await this.dataStore.SearchPackageManifests(manifestSearch, continuationToken);

                unsupportedFields = UnsupportedAndRequiredFieldsHelper.GetUnsupportedPackageMatchFieldsFromSearchRequest(manifestSearch, ApiConstants.UnsupportedPackageMatchFields);
                requiredFields = UnsupportedAndRequiredFieldsHelper.GetRequiredPackageMatchFieldsFromSearchRequest(manifestSearch, ApiConstants.RequiredPackageMatchFields);
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
                        FunctionConstants.ManifestSearchPost,
                        req.Path.Value,
                        headers,
                        manifestSearch,
                        e,
                        log);
                }

                return ActionResultHelper.UnhandledError(e);
            }

            return new ApiObjectResult(new SearchApiResponse<List<ManifestSearchResponse>>(manifestSearchResponse.Items?.ToList(), manifestSearchResponse.ContinuationToken)
            {
                UnsupportedPackageMatchFields = unsupportedFields,
                RequiredPackageMatchFields = requiredFields,
            });
        }
    }
}
