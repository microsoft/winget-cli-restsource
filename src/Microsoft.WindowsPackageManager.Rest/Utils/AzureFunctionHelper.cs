// -----------------------------------------------------------------------
// <copyright file="AzureFunctionHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class that contains helper functions for azure functions.
    /// </summary>
    public class AzureFunctionHelper
    {
        private const string FunctionKeyHeader = "x-functions-key";
        private const string JsonMediaType = "application/json";

        /// <summary>
        /// Helper method to manually trigger an http function.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="httpMethod">HttpMethod.</param>
        /// <param name="azureFunctionURL">Azure function endpoint.</param>
        /// <param name="functionKey">Azure function key to access endpoint.</param>
        /// <param name="postRequestBody">Request body.</param>
        /// <param name="headers">Headers.</param>
        /// <returns>Http response message.</returns>
        public static async Task<HttpResponseMessage> TriggerAzureFunctionAsync(
            HttpClient httpClient,
            HttpMethod httpMethod,
            string azureFunctionURL,
            string functionKey,
            string postRequestBody = null,
            Dictionary<string, string> headers = null)
        {
            // Create Request.
            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod, azureFunctionURL);
            requestMessage.Headers.Add(FunctionKeyHeader, functionKey);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(postRequestBody))
            {
                requestMessage.Content = new StringContent(postRequestBody, Encoding.UTF8, JsonMediaType);
            }

            // Send Request.
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);
            return httpResponseMessage;
        }
    }
}
