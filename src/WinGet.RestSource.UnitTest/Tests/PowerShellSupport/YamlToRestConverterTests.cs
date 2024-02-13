// -----------------------------------------------------------------------
// <copyright file="YamlToRestConverterTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.PowerShellSupport
{
    using System.Linq;
    using Microsoft.WinGet.RestSource.PowershellSupport;
    using Microsoft.Winget.RestSource.UnitTest.Common;
    using Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;
    using static Microsoft.Winget.RestSource.UnitTest.Common.TestCollateralHelper;

    /// <summary>
    /// PowerShell support tests.
    /// </summary>
    public class YamlToRestConverterTests
    {
        private readonly ITestOutputHelper log;

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlToRestConverterTests"/> class.
        /// </summary>
        /// <param name="log">ITestOutputHelper.</param>
        public YamlToRestConverterTests(ITestOutputHelper log)
        {
            this.log = log;
        }

        /// <summary>
        /// Sanity tests for AddManifestToPackageManifest helper function.
        /// </summary>
        [Fact]
        public void CreateMergedManifest()
        {
            string testPath = TestCollateralHelper.GetTestCollateralPath(TestCollateral.TestMultiManifest);

            string json = YamlToRestConverter.AddManifestToPackageManifest(testPath, string.Empty);
            var manifest = JsonConvert.DeserializeObject<PackageManifest>(json);
            VersionExtended version = manifest.Versions.Single();

            Assert.Equal("/silent", version.Installers[0].InstallerSwitches.Silent);

            // Installers have have some root level values set, and Installers[0] overrides some of them
            Assert.Equal("de-DE", version.Installers[0].InstallerLocale);
            Assert.Equal("en-US", version.Installers[1].InstallerLocale);
            Assert.Equal(new[] { "dep3", "dep4" }, version.Installers[0].Dependencies.WindowsFeatures);
            Assert.Equal(new[] { "dep1", "dep2" }, version.Installers[1].Dependencies.WindowsFeatures);
            Assert.Equal(new long[] { 3, 4 }, version.Installers[0].InstallerSuccessCodes);
            Assert.Equal(new long[] { 1, 2 }, version.Installers[1].InstallerSuccessCodes);
        }
    }
}
