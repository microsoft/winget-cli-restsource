// -----------------------------------------------------------------------
// <copyright file="ServerFunctions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.Functions.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;

    /// <summary>
    /// This class contains the functions for interacting with packages.
    /// </summary>
    public class ServerFunctions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerFunctions"/> class.
        /// </summary>
        public ServerFunctions()
        {
        }

        /// <summary>
        /// Server Information Get Function.
        /// This allows us to make Get Server Information.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="log">ILogger.</param>
        /// <returns>IActionResult.</returns>
        [FunctionName(FunctionConstants.InformationGet)]
        public IActionResult InformationGetAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, FunctionConstants.FunctionGet, Route = "information")]
            HttpRequest req,
            ILogger log)
        {
            Information information;
            try
            {
                information = new Information();
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

            return new ApiObjectResult(new ApiResponse<Information>(information));
        }
    }
}
