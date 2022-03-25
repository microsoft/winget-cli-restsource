// -----------------------------------------------------------------------
// <copyright file="WingetTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Winget
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CliWrap;
    using CliWrap.Buffered;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Fixtures;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// E2E tests that use the winget client to test the REST service.
    /// </summary>
    [Collection("IntegrationTestCollection")]
    public class WingetTests : IAsyncLifetime
    {
        /// <summary>
        /// Package Identifier of app to use for testing, must be present in repository.
        /// </summary>
        private const string PowerToysPackageIdentifier = "Microsoft.PowerToys";

        private IntegrationTestFixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="WingetTests"/> class.
        /// </summary>
        /// <param name="log">ITestOutputHelper.</param>
        /// <param name="fixture">An object of type <see cref="IntegrationTestFixture"/>.</param>
        public WingetTests(ITestOutputHelper log, IntegrationTestFixture fixture)
        {
            this.fixture = fixture;
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            if (this.fixture.AddRestSource)
            {
                // Must be running as admin to succeed.
                await this.AddSource();
            }
        }

        /// <inheritdoc/>
        public async Task DisposeAsync()
        {
            if (this.fixture.AddRestSource)
            {
                // Must be running as admin to succeed.
                await this.RemoveSource();
            }
        }

        /// <summary>
        /// Tests winget's upgrade command against the REST api.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task WingetUpgrade()
        {
            await this.TestWingetQuery("upgrade Microsoft.PowerShell", output => output.Contains("No applicable update found."));
            await this.TestWingetQuery("upgrade Microsoft.PowerShelllll", output => output.Contains("No installed package found matching input criteria."));
        }

        /// <summary>
        /// Tests winget's list command against the REST api.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task WingetList()
        {
            await this.TestWingetQuery("list \"Windows Software Development Kit\"", output => output.Contains("Windows Software Development Kit"));
        }

        /// <summary>
        /// Tests winget's search command against the REST api.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task WingetSearch()
        {
            await this.TestWingetSearchQuery("PowerToys", PowerToysPackageIdentifier);
            await this.TestWingetSearchQuery("powertoys", PowerToysPackageIdentifier);
            await this.TestWingetSearchQuery("PowerT", PowerToysPackageIdentifier);
            await this.TestWingetSearchQuery("owertoy", PowerToysPackageIdentifier);
            await this.TestWingetSearchQuery("nonexistentpackage");

            await this.TestWingetSearchQuery("--name PowerToys", PowerToysPackageIdentifier);
            await this.TestWingetSearchQuery("--name powertoys", PowerToysPackageIdentifier);
            await this.TestWingetSearchQuery("--name PowerT", PowerToysPackageIdentifier);
            await this.TestWingetSearchQuery("--name owertoy", PowerToysPackageIdentifier);
            await this.TestWingetSearchQuery("--name nonexistentpackage");
        }

        private static IEnumerable<WingetApp> ParseWingetOutput(string output)
        {
            var rows = output.Split(Environment.NewLine);

            if (rows[0].Contains("No package found matching input criteria."))
            {
                return Enumerable.Empty<WingetApp>();
            }

            /* Output expected to be in this format:
                Name                              Id                                      Version          Match
                ----------------------------------------------------------------------------------------------------------
                PowerToys (Preview)               Microsoft.PowerToys                     0.51.0
                PowerShell Preview                Microsoft.PowerShell.Preview            7.2.0.10
                PowerShell                        Microsoft.PowerShell                    7.2.0.0
                Microsoft PowerApps CLI           Microsoft.PowerAppsCLI                  1.0
            */
            return rows
                .Skip(2)
                .Select(r =>
                {
                    var props = r.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    return props.Length == 3 ? new WingetApp(props[0], props[1], props[2]) : null;
                })
                .Where(r => r != null)
                .ToList();
        }

        private static async Task<string> RunWinget(string arguments, CommandResultValidation commandResultValidation = CommandResultValidation.None)
        {
            var result = await Cli.Wrap(@"winget")
                .WithArguments(arguments)
                .WithValidation(commandResultValidation)
                .ExecuteBufferedAsync();

            if (!string.IsNullOrWhiteSpace(result.StandardOutput))
            {
                return result.StandardOutput;
            }

            return result.StandardError;
        }

        private async Task TestWingetSearchQuery(string query, params string[] expectedPackageIdentifiers)
        {
            await this.TestWingetQuery($"search {query}", null, expectedPackageIdentifiers);
        }

        private async Task TestWingetQuery(string query, Func<string, bool> validator, params string[] expectedPackageIdentifiers)
        {
            string output = await RunWinget($"{query} -s {this.fixture.RestSourceName}");
            if (validator != null)
            {
                Assert.True(validator(output));
            }
            else
            {
                var results = ParseWingetOutput(output);
                var validators = expectedPackageIdentifiers.Select(id => (Action<WingetApp>)((WingetApp app) => Assert.Equal(id, app.Id))).ToArray();
                Assert.Collection(results, validators);
            }
        }

        private async Task AddSource()
        {
            await this.RemoveSource();
            await RunWinget($"source add -n {this.fixture.RestSourceName} -a {this.fixture.RestSourceUrl} -t \"Microsoft.Rest\"", CommandResultValidation.ZeroExitCode);
        }

        private async Task RemoveSource()
        {
            await RunWinget($"source remove {this.fixture.RestSourceName}");
        }

        private record WingetApp(string Name, string Id, string Version);
    }
}
