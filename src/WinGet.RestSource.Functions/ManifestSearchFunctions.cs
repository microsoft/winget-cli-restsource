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
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Functions.Common;
    using Microsoft.WinGet.RestSource.Models;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Schemas;
    using Microsoft.WinGet.RestSource.Validators;

    /// <summary>
    /// This class contains the functions for searching manifests.
    /// </summary>
    public class ManifestSearchFunctions
    {
        private readonly IApiDataStore dataStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearchFunctions"/> class.
        /// </summary>
        /// <param name="dataStore">Data Store.</param>
        public ManifestSearchFunctions(IApiDataStore dataStore)
        {
            this.dataStore = dataStore;
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
            [HttpTrigger(AuthorizationLevel.Anonymous, FunctionConstants.FunctionPost, Route = "manifestSearch")]
            HttpRequest req,
            ILogger log)
        {
            ApiDataPage<ManifestSearchResponse> manifestSearchResponse;
            PackageMatchFields unsupportedFields;
            PackageMatchFields requiredFields;
            try
            {
                // Parse Headers
                Dictionary<string, string> headers = HeaderProcessor.ToDictionary(req.Headers);

                // Get Manifest Search Request and Validate.
                ManifestSearchRequest manifestSearch = await Parser.StreamParser<ManifestSearchRequest>(req.Body, log);
                ApiDataValidator.Validate(manifestSearch);

                manifestSearchResponse = await this.dataStore.SearchPackageManifests(manifestSearch, headers, req.Query);

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
