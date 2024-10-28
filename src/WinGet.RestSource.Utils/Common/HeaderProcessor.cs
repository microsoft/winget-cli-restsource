// -----------------------------------------------------------------------
// <copyright file="HeaderProcessor.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Microsoft.WinGet.RestSource.Utils.Models.Errors;

    /// <summary>
    /// Function to assist with processing Headers.
    /// </summary>
    public static class HeaderProcessor
    {
        /// <summary>
        /// This function processes a set of headers to a standard dictionary.
        /// It will throw an error if it detects an unsupported behavior.
        /// </summary>
        /// <returns>Dictionary.</returns>
        /// <param name="headerDictionary">Header Dictionary.</param>
        public static Dictionary<string, string> ToDictionary(IHeaderDictionary headerDictionary)
        {
            if (headerDictionary == null)
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.HeadersAreNullErrorCode,
                        ErrorConstants.HeadersAreNullErrorMessage));
            }

            Dictionary<string, string> headers = new Dictionary<string, string>();

            // Process Version
            ProcessVersion(headers, headerDictionary[HeaderConstants.Version]);

            // Process ContinuationToken
            ProcessContinuationToken(headers, headerDictionary[HeaderConstants.ContinuationToken]);

            // Return Dictionary
            return headers;
        }

        private static void ProcessVersion(Dictionary<string, string> headers, StringValues versions)
        {
            if (versions.Count == 0)
            {
                headers.Add(HeaderConstants.Version, ApiConstants.ServerSupportedVersions.OrderBy(System.Version.Parse).ToList().Last());
            }
            else
            {
                List<string> joinedVersions = ApiConstants.ServerSupportedVersions.Where(version => versions.Contains(version)).OrderBy(System.Version.Parse).ToList();
                if (joinedVersions.Count == 0)
                {
                    throw new InvalidArgumentException(
                        new InternalRestError(
                            ErrorConstants.ServerVersionNotSupportedErrorCode,
                            ErrorConstants.ServerVersionNotSupportedErrorMessage));
                }

                headers.Add(HeaderConstants.Version, joinedVersions.Last());
            }
        }

        private static void ProcessContinuationToken(Dictionary<string, string> headers, StringValues continuationTokens)
        {
            if (continuationTokens.Count == 0)
            {
                return;
            }

            if (continuationTokens.Count > 1)
            {
                throw new InvalidArgumentException(
                    new InternalRestError(
                        ErrorConstants.TooManyContinuationTokensErrorCode,
                        ErrorConstants.ToManyContinuationTokensErrorMessage));
            }

            headers.Add(HeaderConstants.ContinuationToken, continuationTokens.First());
        }
    }
}
