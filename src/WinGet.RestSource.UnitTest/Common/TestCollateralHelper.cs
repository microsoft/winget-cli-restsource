// -----------------------------------------------------------------------
// <copyright file="TestCollateralHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Common
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Test collateral helper.
    /// </summary>
    public static class TestCollateralHelper
    {
        /// <summary>
        /// Type of collateral.
        /// </summary>
        public enum TestCollateral
        {
            /// <summary>
            /// Cosmos package manifests.
            /// </summary>
            CosmosPackageManifests,

            /// <summary>
            /// Multi manifest.
            /// </summary>
            TestMultiManifest,

            /// <summary>
            /// Test manifest.
            /// </summary>
            TestManifests,

            /// <summary>
            /// Database
            /// </summary>
            Database,
        }

        /// <summary>
        /// Gets the path of test collateral.
        /// </summary>
        /// <param name="testCollateral">Test collateral.</param>
        /// <returns>Test collateral path.</returns>
        public static string GetTestCollateralPath(TestCollateral testCollateral)
        {
            string codeBasePath = Assembly.GetExecutingAssembly().Location;
            return Path.Combine(
                Path.GetDirectoryName(codeBasePath),
                "TestCollateral",
                testCollateral.ToString());
        }

        /// <summary>
        /// Gets a test collateral file. Throws if file doesn't exist.
        /// </summary>
        /// <param name="testCollateral">Test collateral.</param>
        /// <param name="file">File.</param>
        /// <returns>Test collateral file path.</returns>
        public static string GetTestCollateralFilePath(TestCollateral testCollateral, string file)
        {
            string filePath = Path.Combine(GetTestCollateralPath(testCollateral), file);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(file);
            }

            return filePath;
        }
    }
}
