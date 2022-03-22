// -----------------------------------------------------------------------
// <copyright file="TestsBase.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    /// <summary>
    /// Base class used for all integration tests.
    /// </summary>
    public abstract class TestsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestsBase"/> class.
        /// </summary>
        /// <param name="log">Logger to use for tests.</param>
        public TestsBase(ITestOutputHelper log)
        {
            this.Log = log;

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
            this.FunctionsHostKey = configuration[functionsHostKeyKey] ?? throw new NullException(functionsHostKeyKey);
            this.AddRestSource = bool.TryParse(configuration[addRestSourceKey], out bool addRestSource) && addRestSource;
            this.RunWriteTests = bool.TryParse(configuration[RunWriteTestsKey], out bool runWriteTests) && runWriteTests;

            this.Log.WriteLine($"{restSourceNameKey}: {this.RestSourceName}");
            this.Log.WriteLine($"{restSourceUrlKey}: {this.RestSourceUrl}");
            this.Log.WriteLine($"{functionsHostKeyKey}: {this.FunctionsHostKey}");
            this.Log.WriteLine($"{addRestSourceKey}: {this.AddRestSource}");
            this.Log.WriteLine($"{RunWriteTestsKey}: {this.RunWriteTests}");

            if (this.RunWriteTests && string.IsNullOrEmpty(this.FunctionsHostKey))
            {
                throw new ArgumentException($"RunWriteTests is set to true, but FunctionsHostKey is not set");
            }

            this.TestCollateral = new TestCollateral();
        }

        /// <summary>
        /// Gets a value indicating whether or not to add the rest source to winget as part of the test execution.
        /// </summary>
        protected bool AddRestSource { get; }

        /// <summary>
        /// Gets a value indicating whether or not to run tests which modify the repository.
        /// </summary>
        protected bool RunWriteTests { get; }

        /// <summary>
        /// Gets the functions host key to use for tests that modify the repository.
        /// </summary>
        protected string FunctionsHostKey { get; }

        /// <summary>
        /// Gets the url for the rest source function app to use for integration tests.
        /// </summary>
        protected string RestSourceUrl { get; }

        /// <summary>
        /// Gets the rest source name to use with the winget client.
        /// </summary>
        protected string RestSourceName { get; }

        /// <summary>
        /// Gets the logger to use for tests.
        /// </summary>
        protected ITestOutputHelper Log { get; }

        /// <summary>
        /// Gets the Cosmos Data store.
        /// </summary>
        protected CosmosDataStore CosmosDataStore { get; }

        /// <summary>
        /// Gets the test collateral.
        /// </summary>
        protected TestCollateral TestCollateral { get; }
    }
}