// -----------------------------------------------------------------------
// <copyright file="TestDataConstants.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Tests.Functions.TestData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Test data constants.
    /// </summary>
    public class TestDataConstants
    {
        /// <summary>
        /// Path to package manifest endpoint, relative to api base uri.
        /// </summary>
        public const string PackageManifestEndpointRelativePath = "packageManifests";

        private static Dictionary<TestData, Tuple<string, string>> testDataMap =
            new Dictionary<TestData, Tuple<string, string>>()
            {
                [TestData.PowerToys] = new Tuple<string, string>("RestIntegrationTest.PowerToys", "powertoys.json"),
            };

        /// <summary>
        /// Represents a test data.
        /// </summary>
        public enum TestData
        {
            /// <summary>
            /// Powertoys test data.
            /// </summary>
            PowerToys,
        }

        /// <summary>
        /// Get test data.
        /// </summary>
        /// <param name="testData">An enum of type <see cref="TestData"/>.</param>
        /// <returns>A tuple mapping of package identifier to file name.</returns>
        public static Tuple<string, string> GetTestData(TestData testData)
        {
            return testDataMap[testData];
        }
    }
}
