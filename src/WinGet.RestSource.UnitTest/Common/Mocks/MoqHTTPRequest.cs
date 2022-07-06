// -----------------------------------------------------------------------
// <copyright file="MoqHTTPRequest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Common.Mocks
{
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Moq;
    using Newtonsoft.Json;

    /// <summary>
    /// Helper Methods for Mocking HTTPRequest.
    /// </summary>
    internal static class MoqHTTPRequest
    {
        /// <summary>
        /// Creates a Mock HTTP Request with a body that contains a JSON representation of the object.
        /// </summary>
        /// <param name="obj">Any JSON Serializable Object.</param>
        /// <returns>Mock HTTPRequest.</returns>
        public static Mock<HttpRequest> CreateMockRequest(object obj)
        {
            return CreateMockRequest(JsonConvert.SerializeObject(obj));
        }

        /// <summary>
        /// Creates a Mock HTTP Request with a body that contains the provided string.
        /// </summary>
        /// <param name="body">Any JSON Serializable Object.</param>
        /// <param name="headerDictionary">Optional headers.</param>
        /// <returns>Mock HTTPRequest.</returns>
        public static Mock<HttpRequest> CreateMockRequest(string body, HeaderDictionary headerDictionary = null)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            sw.Write(body);
            sw.Flush();

            ms.Position = 0;

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Body).Returns(ms);
            mockRequest.Setup(x => x.Headers).Returns(headerDictionary);
            return mockRequest;
        }

        /// <summary>
        /// Create Mock request for query string.
        /// </summary>
        /// <param name="queryMap">Map of request query string.</param>
        /// <returns>Mock HTTPRequest.</returns>
        public static Mock<HttpRequest> CreateMockRequest(Dictionary<string, StringValues> queryMap)
        {
            var mockRequest = new Mock<HttpRequest>();
            foreach (var queryPair in queryMap)
            {
                mockRequest.Setup(x => x.Query).Returns(new QueryCollection(queryMap));
            }

            return mockRequest;
        }
    }
}
