// -----------------------------------------------------------------------
// <copyright file="RequestBodyHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Utils
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.WindowsPackageManager.Rest.Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// Class that contains parsing helpers for an input helper request body.
    /// </summary>
    public static class RequestBodyHelper
    {
        /// <summary>
        /// Method to get the context, reference, and repo request data from http request body.
        /// </summary>
        /// <param name="httpRequestBody">This is an http POST request body.</param>
        /// <param name="leaveStreamOpen">Bool indicating if the stream should be left open.</param>
        /// <typeparam name="TInputHelper">Type of input we are parsing from the Http Request.</typeparam>
        /// <returns>Pipeline data.</returns>
        public static async Task<TInputHelper> GetRequestDataFromBody<TInputHelper>(
            Stream httpRequestBody,
            bool leaveStreamOpen = false)
            where TInputHelper : IFunctionInput
        {
            string requestBody = await GetRequestBody(httpRequestBody, leaveStreamOpen);
            TInputHelper requestData = JsonConvert.DeserializeObject<TInputHelper>(requestBody);
            requestData.Validate();
            return requestData;
        }

        private static async Task<string> GetRequestBody(Stream httpRequestBody, bool leaveStreamOpen = false)
        {
            using var reader = leaveStreamOpen ?
                new StreamReader(
                    httpRequestBody,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: true,
                    bufferSize: 1024,
                    leaveOpen: true) :
                new StreamReader(httpRequestBody);

            string requestBody = await reader.ReadToEndAsync();
            if (leaveStreamOpen && httpRequestBody.CanSeek)
            {
                httpRequestBody.Position = 0;
            }

            return requestBody;
        }
    }
}
