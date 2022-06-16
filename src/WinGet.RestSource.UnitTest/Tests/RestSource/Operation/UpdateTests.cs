// -----------------------------------------------------------------------
// <copyright file="UpdateTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.RestSource.Operation
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WinGet.RestSource.Interfaces;
    using Microsoft.WinGet.RestSource.Operations;
    using Microsoft.WinGet.RestSource.PowershellSupport.Helpers;
    using Microsoft.Winget.RestSource.UnitTest.Common;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGetUtil.Models.V1;
    using Moq;
    using Xunit;
    using Xunit.Abstractions;
    using static Microsoft.Winget.RestSource.UnitTest.Common.TestCollateralHelper;

    /// <summary>
    /// Update tests.
    /// </summary>
    public class UpdateTests
    {
        private readonly ITestOutputHelper log;
        private readonly LoggingContext loggingContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTests"/> class.
        /// </summary>
        /// <param name="log">log.</param>
        public UpdateTests(ITestOutputHelper log)
        {
            this.log = log;
            this.loggingContext = new LoggingContext();
        }

        /// <summary>
        /// Test ProcessManifestAddAsync for a new package add.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessManifestAddAsync_NewPackageTest()
        {
            var inputManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.1.yaml"));
            PackageManifest packageManifest = null;

            var update = new Update();
            var mockHttpClient = new Mock<HttpClient>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            // Mock this is a new package.
            mockRestSourceTriggerFunction.Setup(
                m => m.GetPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    It.IsAny<LoggingContext>()))
                .ReturnsAsync(packageManifest)
                .Verifiable();

            mockRestSourceTriggerFunction.Setup(
                m => m.PostPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    It.Is<PackageManifest>(p => p.PackageIdentifier == inputManifest.Id),
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            await update.ProcessManifestAddAsync(
                mockHttpClient.Object,
                inputManifest,
                mockRestSourceTriggerFunction.Object,
                this.loggingContext);

            mockRestSourceTriggerFunction.Verify();
        }

        /// <summary>
        /// Test ProcessManifestAddAsync for a new package add.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessManifestAddAsync_NewVersionTest()
        {
            var inputManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.2.yaml"));
            var previousManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.1.yaml"));
            PackageManifest packageManifest = PackageManifestUtils.AddManifestToPackageManifest(previousManifest);

            var update = new Update();
            var mockHttpClient = new Mock<HttpClient>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            // Mock this is a new version.
            mockRestSourceTriggerFunction.Setup(
                m => m.GetPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    It.IsAny<LoggingContext>()))
                .ReturnsAsync(packageManifest)
                .Verifiable();

            mockRestSourceTriggerFunction.Setup(
                m => m.PutPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    It.Is<PackageManifest>(p => p.PackageIdentifier == inputManifest.Id && p.Versions.VersionExists(inputManifest.Version)),
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            await update.ProcessManifestAddAsync(
                mockHttpClient.Object,
                inputManifest,
                mockRestSourceTriggerFunction.Object,
                this.loggingContext);

            mockRestSourceTriggerFunction.Verify();
        }

        /// <summary>
        /// Test ProcessManifestAddAsync for a new package add.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessManifestAddAsync_VersionAlreadyExistsTest()
        {
            var inputManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.2.yaml"));
            PackageManifest packageManifest = PackageManifestUtils.AddManifestToPackageManifest(inputManifest);

            var update = new Update();
            var mockHttpClient = new Mock<HttpClient>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            mockRestSourceTriggerFunction.Setup(
                m => m.GetPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    It.IsAny<LoggingContext>()))
                .ReturnsAsync(packageManifest)
                .Verifiable();

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await update.ProcessManifestAddAsync(
                    mockHttpClient.Object,
                    inputManifest,
                    mockRestSourceTriggerFunction.Object,
                    this.loggingContext));

            mockRestSourceTriggerFunction.Verify();
        }

        /// <summary>
        /// Tests ProcessManifestModifyAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessManifestModifyAsync_Test()
        {
            var inputManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.2.yaml"));
            PackageManifest packageManifest = PackageManifestUtils.AddManifestToPackageManifest(inputManifest);

            var update = new Update();
            var mockHttpClient = new Mock<HttpClient>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            mockRestSourceTriggerFunction.Setup(
                m => m.GetPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    It.IsAny<LoggingContext>()))
                .ReturnsAsync(packageManifest)
                .Verifiable();

            mockRestSourceTriggerFunction.Setup(
                m => m.PutPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    It.Is<PackageManifest>(p => p.PackageIdentifier == inputManifest.Id),
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            await update.ProcessManifestModifyAsync(
                mockHttpClient.Object,
                inputManifest,
                mockRestSourceTriggerFunction.Object,
                this.loggingContext);

            mockRestSourceTriggerFunction.Verify();
        }

        /// <summary>
        /// Tests ProcessManifestModifyAsync package doesn't exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessManifestModifyAsync_NoPackageTest()
        {
            var inputManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.2.yaml"));
            PackageManifest packageManifest = null;

            var update = new Update();
            var mockHttpClient = new Mock<HttpClient>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            mockRestSourceTriggerFunction.Setup(
                m => m.GetPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    It.IsAny<LoggingContext>()))
                .ReturnsAsync(packageManifest)
                .Verifiable();

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await update.ProcessManifestModifyAsync(
                    mockHttpClient.Object,
                    inputManifest,
                    mockRestSourceTriggerFunction.Object,
                    this.loggingContext));

            mockRestSourceTriggerFunction.Verify();
        }

        /// <summary>
        /// Tests ProcessManifestModifyAsync package version doesn't exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessManifestModifyAsync_NoVersionTest()
        {
            var inputManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.2.yaml"));
            var previousManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.1.yaml"));
            PackageManifest packageManifest = PackageManifestUtils.AddManifestToPackageManifest(previousManifest);

            var update = new Update();
            var mockHttpClient = new Mock<HttpClient>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            mockRestSourceTriggerFunction.Setup(
                m => m.GetPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    It.IsAny<LoggingContext>()))
                .ReturnsAsync(packageManifest)
                .Verifiable();

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await update.ProcessManifestModifyAsync(
                    mockHttpClient.Object,
                    inputManifest,
                    mockRestSourceTriggerFunction.Object,
                    this.loggingContext));

            mockRestSourceTriggerFunction.Verify();
        }

        /// <summary>
        /// Tests ProcessManifestDeleteAsync. Package has one version.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessManifestDeleteAsync_OneVersionTest()
        {
            var inputManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.2.yaml"));
            PackageManifest packageManifest = PackageManifestUtils.AddManifestToPackageManifest(inputManifest);

            var update = new Update();
            var mockHttpClient = new Mock<HttpClient>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            mockRestSourceTriggerFunction.Setup(
                m => m.GetPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    It.IsAny<LoggingContext>()))
                .ReturnsAsync(packageManifest)
                .Verifiable();

            mockRestSourceTriggerFunction.Setup(
                m => m.DeletePackageAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            await update.ProcessManifestDeleteAsync(
                mockHttpClient.Object,
                inputManifest,
                mockRestSourceTriggerFunction.Object,
                this.loggingContext);

            mockRestSourceTriggerFunction.Verify();
        }

        /// <summary>
        /// Tests ProcessManifestDeleteAsync. Package has one version.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessManifestDeleteAsync_MultipleVersionTest()
        {
            var inputManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.2.yaml"));
            var priorManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.1.yaml"));

            // Combine manifests.
            PackageManifest packageManifest = PackageManifestUtils.AddManifestToPackageManifest(priorManifest);
            packageManifest = PackageManifestUtils.AddManifestToPackageManifest(inputManifest, packageManifest);

            var update = new Update();
            var mockHttpClient = new Mock<HttpClient>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            mockRestSourceTriggerFunction.Setup(
                m => m.GetPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    It.IsAny<LoggingContext>()))
                .ReturnsAsync(packageManifest)
                .Verifiable();

            mockRestSourceTriggerFunction.Setup(
                m => m.DeleteVersionAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    inputManifest.Version,
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            await update.ProcessManifestDeleteAsync(
                mockHttpClient.Object,
                inputManifest,
                mockRestSourceTriggerFunction.Object,
                this.loggingContext);

            mockRestSourceTriggerFunction.Verify();
        }

        /// <summary>
        /// Tests ProcessManifestDeleteAsync. Delete version that doesn't exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessManifestDeleteAsync_NoVersionTest()
        {
            var inputManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.2.yaml"));
            var priorManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.1.yaml"));
            PackageManifest packageManifest = PackageManifestUtils.AddManifestToPackageManifest(priorManifest);

            var update = new Update();
            var mockHttpClient = new Mock<HttpClient>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            mockRestSourceTriggerFunction.Setup(
                m => m.GetPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    It.IsAny<LoggingContext>()))
                .ReturnsAsync(packageManifest)
                .Verifiable();

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await update.ProcessManifestDeleteAsync(
                    mockHttpClient.Object,
                    inputManifest,
                    mockRestSourceTriggerFunction.Object,
                    this.loggingContext));

            mockRestSourceTriggerFunction.Verify();
        }

        /// <summary>
        /// Tests ProcessManifestDeleteAsync. Package doesn't exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessManifestDeleteAsync_NoPackageTest()
        {
            var inputManifest = Manifest.CreateManifestFromPath(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.2.yaml"));
            PackageManifest packageManifest = null;

            var update = new Update();
            var mockHttpClient = new Mock<HttpClient>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            mockRestSourceTriggerFunction.Setup(
                m => m.GetPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    inputManifest.Id,
                    It.IsAny<LoggingContext>()))
                .ReturnsAsync(packageManifest)
                .Verifiable();

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await update.ProcessManifestDeleteAsync(
                    mockHttpClient.Object,
                    inputManifest,
                    mockRestSourceTriggerFunction.Object,
                    this.loggingContext));

            mockRestSourceTriggerFunction.Verify();
        }
    }
}
