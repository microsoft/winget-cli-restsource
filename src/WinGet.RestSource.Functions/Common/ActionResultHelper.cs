// -----------------------------------------------------------------------
// <copyright file="ActionResultHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions.Common
{
    using System;
    using System.Net;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;

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
        public static ApiObjectResult ProcessError(InternalRestError internalRestError)
        {
            switch (internalRestError.ErrorCode)
            {
                case ErrorConstants.ResourceConflictErrorCode:
                case ErrorConstants.VersionAlreadyExistsErrorCode:
                case ErrorConstants.InstallerAlreadyExistsErrorCode:
                case ErrorConstants.LocaleAlreadyExistsErrorCode:
                    return CreateObjectResult(internalRestError, (int)HttpStatusCode.Conflict);

                case ErrorConstants.ResourceNotFoundErrorCode:
                case ErrorConstants.VersionsIsNullErrorCode:
                case ErrorConstants.VersionDoesNotExistErrorCode:
                case ErrorConstants.InstallerIsNullErrorCode:
                case ErrorConstants.InstallerDoesNotExistErrorCode:
                case ErrorConstants.LocaleIsNullErrorCode:
                case ErrorConstants.LocaleDoesNotExistErrorCode:
                    return CreateObjectResult(internalRestError, (int)HttpStatusCode.NotFound);

                case ErrorConstants.PreconditionFailedErrorCode:
                    return CreateObjectResult(internalRestError, (int)HttpStatusCode.PreconditionFailed);

                case ErrorConstants.PackageDoesNotMatchErrorCode:
                case ErrorConstants.VersionDoesNotMatchErrorCode:
                case ErrorConstants.InstallerDoesNotMatchErrorCode:
                case ErrorConstants.LocaleDoesNotMatchErrorCode:
                case ErrorConstants.ValidationFailureErrorCode:
                case ErrorConstants.HeadersAreNullErrorCode:
                case ErrorConstants.ServerVersionNotSupportedErrorCode:
                case ErrorConstants.ToManyContinuationTokensErrorCode:
                    return CreateObjectResult(internalRestError, (int)HttpStatusCode.BadRequest);

                case ErrorConstants.HttpRequestExceptionErrorCode:
                case ErrorConstants.UnhandledErrorCode:
                default:
                    return CreateObjectResult(internalRestError, (int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// This creates a default Unhandled error and returns the result from the process error function.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <returns>Object Result.</returns>
        public static ApiObjectResult UnhandledError(Exception exception = null)
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
        /// <param name="data">Data to Return.</param>
        /// <param name="code">Return Code.</param>
        /// <returns>Object Result.</returns>
        private static ApiObjectResult CreateObjectResult(object data, int code)
        {
            return new ApiObjectResult(data, code);
        }
    }
}
