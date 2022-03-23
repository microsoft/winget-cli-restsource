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
        private IMessageSink diagnosticMessageSink;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestFixture"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">An object of type <see cref="IMessageSink"/>.</param>
        public IntegrationTestFixture(IMessageSink diagnosticMessageSink)
        {
            this.diagnosticMessageSink = diagnosticMessageSink;

            var configuration = new ConfigurationBuilder()

                // Defaults specified in the Test.runsettings.json
                .AddJsonFile("Test.runsettings.json", true)

                // But they can be overridden using environment variables
                .AddEnvironmentVariables()
                .Build();

            const string restSourceNameKey = "RestSourceName";
            const string restSourceUrlKey = "RestSourceUrl";
            const string addRestSourceKey = "AddRestSource";
            const string RunWriteTestsKey = "RunWriteTests";
            const string functionsHostKeyKey = "FunctionsHostKey";

            this.RestSourceName = configuration[restSourceNameKey] ?? throw new NullException(restSourceNameKey);

            this.RestSourceUrl = configuration[restSourceUrlKey] ?? throw new NullException(restSourceUrlKey);
            this.RestSourceUrl = this.RestSourceUrl.TrimEnd('/');

            this.FunctionsHostKey = configuration[functionsHostKeyKey] ?? throw new NullException(functionsHostKeyKey);
            this.AddRestSource = bool.TryParse(configuration[addRestSourceKey], out bool addRestSource) && addRestSource;
            this.RunWriteTests = bool.TryParse(configuration[RunWriteTestsKey], out bool runWriteTests) && runWriteTests;

            this.WriteMessage($"{restSourceNameKey}: {this.RestSourceName}");
            this.WriteMessage($"{restSourceUrlKey}: {this.RestSourceUrl}");
            this.WriteMessage($"{functionsHostKeyKey}: {this.FunctionsHostKey}");
            this.WriteMessage($"{addRestSourceKey}: {this.AddRestSource}");
            this.WriteMessage($"{RunWriteTestsKey}: {this.RunWriteTests}");

            if (this.RunWriteTests && string.IsNullOrEmpty(this.FunctionsHostKey))
            {
                throw new ArgumentException($"RunWriteTests is set to true, but FunctionsHostKey is not set");
            }

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
        /// Gets a value indicating whether or not to add the rest source to winget as part of the test execution.
        /// </summary>
        public bool AddRestSource { get; }

        /// <summary>
        /// Gets a value indicating whether or not to run tests which modify the repository.
        /// </summary>
        public bool RunWriteTests { get; }

        /// <summary>
        /// Gets the url for the rest source function app to use for integration tests.
        /// </summary>
        public string RestSourceUrl { get; }

        /// <summary>
        /// Gets the rest source name to use with the winget client.
        /// </summary>
        public string RestSourceName { get; }

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

        private void WriteMessage(string message)
        {
            var diagMessage = new DiagnosticMessage(message);
            this.diagnosticMessageSink.OnMessage(diagMessage);
        }
    }
}