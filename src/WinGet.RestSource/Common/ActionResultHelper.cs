// -----------------------------------------------------------------------
// <copyright file="ActionResultHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Common
{
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// This Creates Action Results.
    /// </summary>
    public static class ActionResultHelper
    {
        /// <summary>
        /// This will process an Error, and create an appropriate response code.
        /// </summary>
        /// <param name="internalRestError">Error to process.</param>
        /// <returns>Object Result.</returns>
        public static ObjectResult ProcessError(InternalRestError internalRestError)
        {
            switch (internalRestError.ErrorCode)
            {
                case ErrorConstants.ResourceConflictErrorCode:
                case ErrorConstants.VersionAlreadyExistsErrorCode:
                case ErrorConstants.InstallerAlreadyExistsErrorCode:
                    return CreateJsonObjectResult((int)HttpStatusCode.Conflict, internalRestError);

                case ErrorConstants.ResourceNotFoundErrorCode:
                case ErrorConstants.VersionsIsNullErrorCode:
                case ErrorConstants.VersionDoesNotExistErrorCode:
                case ErrorConstants.InstallerIsNullErrorCode:
                case ErrorConstants.InstallerDoesNotExistErrorCode:
                    return CreateJsonObjectResult((int)HttpStatusCode.NotFound, internalRestError);

                case ErrorConstants.PreconditionFailedErrorCode:
                    return CreateJsonObjectResult((int)HttpStatusCode.PreconditionFailed, internalRestError);

                case ErrorConstants.IdDoesNotMatchErrorCode:
                case ErrorConstants.VersionDoesNotMatchErrorCode:
                case ErrorConstants.InstallerDoesNotMatchErrorCode:
                    return CreateJsonObjectResult((int)HttpStatusCode.BadRequest, internalRestError);

                case ErrorConstants.UnhandledErrorCode:
                default:
                    return CreateJsonObjectResult((int)HttpStatusCode.InternalServerError, internalRestError);
            }
        }

        /// <summary>
        /// This creates a default Unhandled error and returns the result from the process error function.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <returns>Object Result.</returns>
        public static ObjectResult UnhandledError(Exception exception = null)
        {
            InternalRestError internalRestError = new InternalRestError(
                ErrorConstants.UnhandledErrorCode,
                ErrorConstants.UnhandledErrorMessage,
                exception);
            return ProcessError(internalRestError);
        }

        /// <summary>
        /// This creates a JSON based Object Result.
        /// </summary>
        /// <param name="code">Return Code.</param>
        /// <param name="data">Data to Return.</param>
        /// <param name="formatting">Formatting to use. Default is indented.</param>
        /// <returns>Object Result.</returns>
        private static ObjectResult CreateJsonObjectResult(
            int code,
            object data,
            Formatting formatting = Formatting.Indented)
        {
            return new ObjectResult(JsonConvert.SerializeObject(data, formatting))
            {
                StatusCode = code,
            };
        }
    }
}
