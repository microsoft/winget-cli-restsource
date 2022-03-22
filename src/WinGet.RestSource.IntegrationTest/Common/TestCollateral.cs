// -----------------------------------------------------------------------
// <copyright file="TestCollateral.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Represents the test collateral.
    /// </summary>
    public class TestCollateral
    {
        private const string TestCollateralDirectoryName = "TestCollateral";

        private string testCollateralDirectoryPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCollateral"/> class.
        /// </summary>
        public TestCollateral()
        {
            this.Initialize();
        }

        /// <summary>
        /// Represent the type of the test collateral.
        /// </summary>
        public enum TestCollateralType
        {
            /// <summary>
            /// Represents manifest test collateral type.
            /// Maps to manifest directory in test collateral.
            /// </summary>
            Manifests,
        }

        /// <summary>
        /// Fetches the test collateral.
        /// </summary>
        /// <param name="name">Test Collateral name.</param>
        /// <returns>The full path to the test collateral.</returns>
        public string FetchTestCollateralPath(string name)
        {
            string testCollateralPath = Path.Combine(
                this.testCollateralDirectoryPath, TestCollateralType.Manifests.ToString(), name);
            if (!File.Exists(testCollateralPath))
            {
                throw new FileNotFoundException($"{name} not found in test collateral directory.");
            }

            return testCollateralPath;
        }

        /// <summary>
        /// Fetches the test collateral.
        /// </summary>
        /// <param name="name">Test Collateral name.</param>
        /// <returns>The full path to the test collateral.</returns>
        public string FetchTestCollateralContent(string name)
        {
            return File.ReadAllText(this.FetchTestCollateralPath(name));
        }

        private void Initialize()
        {
            this.testCollateralDirectoryPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                TestCollateralDirectoryName);
        }
    }
}
