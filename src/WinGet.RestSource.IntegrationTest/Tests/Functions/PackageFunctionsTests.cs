// -----------------------------------------------------------------------
// <copyright file="PackageFunctionsTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Tests.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Fixtures;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Helpers;
    using Microsoft.WinGet.RestSource.IntegrationTest.Tests.Functions.TestData;
    using Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Xunit;

    /// <summary>
    /// Package functions integration tests.
    /// </summary>
    [Collection("IntegrationTestCollection")]
    public class PackageFunctionsTests
    {
        private const string PowerToysPackageIdentifier = "Microsoft.PowerToys";
        private IntegrationTestFixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageFunctionsTests"/> class.
        /// </summary>
        /// <param name="fixture">An object of type <see cref="IntegrationTestFixture"/>.</param>
        public PackageFunctionsTests(IntegrationTestFixture fixture)
        {
            this.fixture = fixture;
        }

        /// <summary>
        /// Package function endpoint tests.
        /// </summary>
        /// <param name="helper">An object of type <see cref="PackageVersionTestHelper"/>.</param>
        [Theory]
        [ClassData(typeof(PackageTestData))]
        public async void PackageFunctionEndpointTests(PackageVersionTestHelper helper)
        {
            Assert.NotNull(helper);
            Assert.NotNull(helper.EndPointRequest);
            Assert.NotNull(helper.PackageIdentifier);

            await this.InitializeAsync(helper);

            string url = $"{this.fixture.RestSourceUrl}/{helper.EndPointRequest.RelativeUrlPath.TrimStart('/')}";
            var response = await IntegrationTestFixture.GetConsistentApiResponse<Package>(url);

            if (helper.ApiResponse == PackageVersionTestHelper.ApiResponseScenario.IdentifierNotFound)
            {
                Assert.Null(response);
            }

            if (helper.ApiResponse == PackageVersionTestHelper.ApiResponseScenario.IdentifierFound)
            {
                Assert.NotNull(response);
                Assert.Equal(helper.PackageIdentifier, response.Data[0].PackageIdentifier);
            }
        }

        /// <summary>
        /// Package function version endpoint tests.
        /// </summary>
        /// <param name="helper">An object of type <see cref="PackageVersionTestHelper"/>.</param>
        [Theory]
        [ClassData(typeof(PackageVersionTestData))]
        public async void PackageFunctionVersionEndpointTests(PackageVersionTestHelper helper)
        {
            Assert.NotNull(helper);
            Assert.NotNull(helper.EndPointRequest);
            Assert.NotNull(helper.PackageIdentifier);

            await this.InitializeAsync(helper);

            string url = $"{this.fixture.RestSourceUrl}/{helper.EndPointRequest.RelativeUrlPath.TrimStart('/')}";
            var response = await IntegrationTestFixture.GetConsistentApiResponse<VersionExtended>(url);

            if (helper.ApiResponse == PackageVersionTestHelper.ApiResponseScenario.IdentifierFound)
            {
                Assert.NotNull(response);
                var actualVersions = response.Data.ConvertAll<string>((v) => v.PackageVersion).ToHashSet();
                foreach (var version in helper.ExpectedVersions)
                {
                    Assert.Contains(version, actualVersions);
                }
            }
        }

        private async Task InitializeAsync(PackageVersionTestHelper helper)
        {
            if (helper.StorageCleanup != null)
            {
                await helper.StorageCleanup.CleanupAsync(this.fixture);
            }

            if (helper.StorageSetup != null)
            {
                await helper.StorageSetup.SetupAsync(this.fixture);
            }
        }
    }
}
