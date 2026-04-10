// -----------------------------------------------------------------------
// <copyright file="IntegrationTestFixture.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common.Fixtures
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Flurl.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common;
    using Microsoft.WinGet.RestSource.IntegrationTest.Model;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Newtonsoft.Json;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    /// <summary>
    /// Base class used for all integration tests.
    /// </summary>
    public class IntegrationTestFixture
    {
        private readonly IMessageSink diagnosticMessageSink;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestFixture"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">An object of type <see cref="IMessageSink"/>.</param>
        public IntegrationTestFixture(IMessageSink diagnosticMessageSink)
        {
            this.diagnosticMessageSink = diagnosticMessageSink;

            ConfigSettings configSettings = ConfigSettings.Build();

            this.RestSourceUrl = configSettings.RestSourceUrl;
            this.FunctionsHostKey = configSettings.FunctionsHostKey;
            this.TestCollateral = new TestCollateral();
        }

        /// <summary>
        /// Gets the test collateral.
        /// </summary>
        public TestCollateral TestCollateral { get; }

        /// <summary>
        /// Gets the functions host key to use for tests that modify the repository.
        /// </summary>
        public string FunctionsHostKey { get; }

        /// <summary>
        /// Gets the url for the rest source function app to use for integration tests.
        /// </summary>
        public string RestSourceUrl { get; }

        /// <summary>
        /// Gets the logger to use for tests.
        /// </summary>
        public ITestOutputHelper Log { get; }

        /// <summary>
        /// Gets the Cosmos Data store.
        /// </summary>
        public CosmosDataStore CosmosDataStore { get; }

        /// <summary>
        /// Get Api Response.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Endpoint url.</param>
        /// <returns>An object of <see cref="ApiResponse{T}"/>.</returns>
        public static async Task<ApiResponse<List<T>>> GetConsistentApiResponse<T>(string url)
        {
            return await GetConsistentApiResponse<T>(new FlurlRequest(url));
        }

        /// <summary>
        /// Get Api Response.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">An object of type <see cref="IFlurlRequest"/>.</param>
        /// <returns>An object of <see cref="ApiResponse{T}"/>.</returns>
        public static async Task<ApiResponse<List<T>>> GetConsistentApiResponse<T>(IFlurlRequest url)
        {
            string json = await url.GetStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<ApiResponse<List<T>>>(json);
            }
            catch (JsonSerializationException)
            {
                var singleResult = JsonConvert.DeserializeObject<ApiResponse<T>>(json);
                return new ApiResponse<List<T>>(new List<T> { singleResult.Data });
            }
        }
    }
}