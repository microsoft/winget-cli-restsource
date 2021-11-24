// -----------------------------------------------------------------------
// <copyright file="TestsBase.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public abstract class TestsBase
    {
        protected const string PowerToysPackageIdentifier = "Microsoft.PowerToys";
        protected readonly ITestOutputHelper log;
        protected readonly string restSourceName;
        protected readonly string restSourceUrl;
        protected readonly string functionsMasterKey;
        protected readonly bool addRestSource;
        protected readonly bool RunWriteTests;

        public TestsBase(ITestOutputHelper log)
        {
            this.log = log;

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
            const string functionsMasterKeyKey = "FunctionsMasterKey";
            this.restSourceName = configuration[restSourceNameKey] ?? throw new NullException(restSourceNameKey);
            this.restSourceUrl = configuration[restSourceUrlKey] ?? throw new NullException(restSourceUrlKey);
            this.functionsMasterKey = configuration[functionsMasterKeyKey] ?? throw new NullException(functionsMasterKeyKey);
            this.addRestSource = bool.TryParse(configuration[addRestSourceKey], out bool addRestSource) && addRestSource;
            this.RunWriteTests = bool.TryParse(configuration[RunWriteTestsKey], out bool RunWriteTests) && RunWriteTests;

            this.log.WriteLine($"{restSourceNameKey}: {this.restSourceName}");
            this.log.WriteLine($"{restSourceUrlKey}: {this.restSourceUrl}");
            this.log.WriteLine($"{functionsMasterKeyKey}: {this.functionsMasterKey}");
            this.log.WriteLine($"{addRestSourceKey}: {this.addRestSource}");
            this.log.WriteLine($"{RunWriteTestsKey}: {this.RunWriteTests}");

            if (this.RunWriteTests && string.IsNullOrEmpty(this.functionsMasterKey))
            {
                throw new ArgumentException($"RunWriteTests is set to true, but FunctionsMasterKey is not set");
            }
        }
    }
}