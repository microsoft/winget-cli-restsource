// -----------------------------------------------------------------------
// <copyright file="SqlReaderTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.RestSource.Sql
{
    using Microsoft.WinGet.RestSource.Sql;
    using Microsoft.Winget.RestSource.UnitTest.Common;
    using Xunit;
    using Xunit.Abstractions;
    using static Microsoft.Winget.RestSource.UnitTest.Common.TestCollateralHelper;

    /// <summary>
    /// Tests SqlReader.
    /// </summary>
    public class SqlReaderTests
    {
        private readonly ITestOutputHelper log;
        private readonly string testIndexPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlReaderTests"/> class.
        /// </summary>
        /// <param name="log">log.</param>
        public SqlReaderTests(ITestOutputHelper log)
        {
            this.log = log;
            this.testIndexPath = TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.Database, "testIndex.db");
        }

        /// <summary>
        /// Test get packages.
        /// </summary>
        [Fact]
        public void GetPackages_Test()
        {
            using var sqlReader = new SqlReader(this.testIndexPath);
            var sqlPacakges = sqlReader.GetPackages();

            Assert.Equal(4, sqlPacakges.Count);

            Assert.Equal("Rtest.test", sqlPacakges[0].Id);
            Assert.Equal(2, sqlPacakges[0].Versions.Count);

            Assert.Equal("Etest.test", sqlPacakges[1].Id);
            Assert.Equal(1, sqlPacakges[1].Versions.Count);

            Assert.Equal("Dtest.test", sqlPacakges[2].Id);
            Assert.Equal(1, sqlPacakges[2].Versions.Count);

            Assert.Equal("Atest.test", sqlPacakges[3].Id);
            Assert.Equal(1, sqlPacakges[3].Versions.Count);
        }

        /// <summary>
        /// Test GetVersions.
        /// </summary>
        [Fact]
        public void GetVersions_Test()
        {
            using var sqlReader = new SqlReader(this.testIndexPath);
            var sqlVersions = sqlReader.GetVersions("1");

            Assert.Equal(2, sqlVersions.Count);

            Assert.Equal("0.0.0.2", sqlVersions[0].Version);
            Assert.Equal(@"manifests\r\Rtest\test\0.0.0.2\8e25-Rtest.test.yaml", sqlVersions[0].PathPart);

            Assert.Equal("0.0.0.1", sqlVersions[1].Version);
            Assert.Equal(@"manifests\r\Rtest\test\0.0.0.1\8e25-Rtest.test.yaml", sqlVersions[1].PathPart);
        }

        /// <summary>
        /// Test GetVersion.
        /// </summary>
        [Fact]
        public void GetVersion_Test()
        {
            using var sqlReader = new SqlReader(this.testIndexPath);
            var version = sqlReader.GetVersion("1");

            Assert.Equal("0.0.0.2", version);
        }

        /// <summary>
        /// Test ReadPathPartFromId.
        /// </summary>
        [Fact]
        public void ReadPathPartFromId_Test()
        {
            using var sqlReader = new SqlReader(this.testIndexPath);
            var path = sqlReader.ReadPathPartFromId("22");

            Assert.Equal(@"manifests\a\Atest\test\0.0.0.1", path);
        }
    }
}
