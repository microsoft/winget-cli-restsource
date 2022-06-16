// -----------------------------------------------------------------------
// <copyright file="TestUtils.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Moq;
    using Newtonsoft.Json;

    /// <summary>
    /// Test helper utilities.
    /// </summary>
    public static class TestUtils
    {
        /// <summary>
        /// Executes a foreach async operation on an enumerable source in which iterations are run in parallel.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in source.</typeparam>
        /// <param name="source">The enumerable that contains the original data source.</param>
        /// <param name="body">The async delegate that is invoked once per iteration.</param>
        /// <param name="maxDegreeOfParallelism">The maximum number of concurrent tasks.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task AsyncParallelForEach<TSource>(this IEnumerable<TSource> source, Func<TSource, Task> body, int maxDegreeOfParallelism)
        {
            var semaphore = new Semaphore(maxDegreeOfParallelism, maxDegreeOfParallelism);
            var tasks = new List<Task>();

            foreach (var item in source)
            {
                semaphore.WaitOne();
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        await body(item);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            // Wait until all are done
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Creates HttpRequest mocks. Serializes object that will be the body.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="headerDictionary">Optional header dictionary.</param>
        /// <param name="queryMap">Optional query map.</param>
        /// <returns>Mocked HttpRequest.</returns>
        public static Mock<HttpRequest> CreateMockHttpRequest<T>(
            T obj,
            HeaderDictionary headerDictionary = null,
            Dictionary<string, StringValues> queryMap = null)
        {
            return CreateMockHttpRequest(
                JsonConvert.SerializeObject(obj),
                headerDictionary,
                queryMap);
        }

        /// <summary>
        /// Creates HttpRequest mocks.
        /// </summary>
        /// <param name="body">Body.</param>
        /// <param name="headerDictionary">Optional header dictionary.</param>
        /// <param name="queryMap">Optional query map.</param>
        /// <returns>Mocked HttpRequest.</returns>
        public static Mock<HttpRequest> CreateMockHttpRequest(
            string body,
            HeaderDictionary headerDictionary = null,
            Dictionary<string, StringValues> queryMap = null)
        {
            var mockHttpRequest = new Mock<HttpRequest>();

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            sw.Write(body);
            sw.Flush();

            ms.Position = 0;

            mockHttpRequest.Setup(x => x.Body).Returns(ms);

            if (headerDictionary != null)
            {
                mockHttpRequest.Setup(x => x.Headers).Returns(headerDictionary);
            }

            if (queryMap != null)
            {
                mockHttpRequest.Setup(x => x.Query).Returns(new QueryCollection(queryMap));
            }

            return mockHttpRequest;
        }
    }
}
