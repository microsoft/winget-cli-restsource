// -----------------------------------------------------------------------
// <copyright file="RebuildTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.RestSource.Operation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WinGet.RestSource.Interfaces;
    using Microsoft.WinGet.RestSource.Operations;
    using Microsoft.WinGet.RestSource.Sql;
    using Microsoft.Winget.RestSource.UnitTest.Common;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Moq;
    using Moq.Protected;
    using Xunit;
    using Xunit.Abstractions;
    using static Microsoft.Winget.RestSource.UnitTest.Common.TestCollateralHelper;

    /// <summary>
    /// Tests Rebuild.
    /// </summary>
    public class RebuildTests
    {
        private const string AzFuncRestSourceEndpoint = "https://fakestorage.blob.core.windows.net/cache/";
        private const string AzFuncHostKey = "1234567890";

        private readonly ITestOutputHelper log;
        private readonly LoggingContext loggingContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RebuildTests"/> class.
        /// </summary>
        /// <param name="log">log.</param>
        public RebuildTests(ITestOutputHelper log)
        {
            this.log = log;
            this.loggingContext = new LoggingContext();
        }

        /// <summary>
        /// Test ProcessRebuildRequestInternalAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task ProcessRebuildRequestInternalAsync_Test()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Package Rtest.test with two versions
            // V1
            var rtestv1 = new SqlVersion("0.0.0.1", @"manifests\r\Rtest\test\0.0.0.1\8e25-Rtest.test.yaml");
            var rtestv1Content = File.ReadAllText(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.1.yaml"));
            var rtestv1PathUri = new Uri($"{AzFuncRestSourceEndpoint}{Uri.EscapeDataString(rtestv1.PathPart)}");
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.RequestUri.AbsoluteUri == rtestv1PathUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(rtestv1Content),
                })
                .Verifiable();

            // V2
            var rtestv2 = new SqlVersion("0.0.0.2", @"manifests\r\Rtest\test\0.0.0.2\8e25-Rtest.test.yaml");
            var rtestv2Content = File.ReadAllText(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Rtest.test.0.0.0.2.yaml"));
            var rtestv2PathUri = new Uri($"{AzFuncRestSourceEndpoint}{Uri.EscapeDataString(rtestv2.PathPart)}");
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.RequestUri.AbsoluteUri == rtestv2PathUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(rtestv2Content),
                })
                .Verifiable();

            var rtestPackage = new SqlPackage("Rtest.test", new List<SqlVersion>() { rtestv1, rtestv2 });

            // Package Atest.test with one version.
            var atestv1 = new SqlVersion("0.0.0.1", @"manifests\a\Atest\test\0.0.0.1\8e25-Atest.test.yaml");
            var atestv1Content = File.ReadAllText(
                TestCollateralHelper.GetTestCollateralFilePath(TestCollateral.TestManifests, "Atest.test.0.0.0.1.yaml"));
            var atestv1PathUri = new Uri($"{AzFuncRestSourceEndpoint}{Uri.EscapeDataString(atestv1.PathPart)}");
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.RequestUri.AbsoluteUri == atestv1PathUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(atestv1Content),
                })
                .Verifiable();

            var atestPackage = new SqlPackage("Atest.test", new List<SqlVersion>() { atestv1 });

            // Sql has two packages. Rtest.test with two versions, Atest.test with one.
            var inputSqlPackages = new List<SqlPackage>() { rtestPackage, atestPackage };

            // Rest has two packages. Atest.test with one version, Etest.test (versions don't matter).
            string etestPackageId = "Etest.test";
            var inputRestPackages = new HashSet<string>() { atestPackage.Id, etestPackageId };

            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            // Rtest.test must be a post
            mockRestSourceTriggerFunction.Setup(
                m => m.PostPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    It.Is<PackageManifest>(p => p.PackageIdentifier == rtestPackage.Id),
                    AzFuncHostKey,
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            // Atest.test must be a put
            mockRestSourceTriggerFunction.Setup(
                m => m.PutPackageManifestAsync(
                    It.IsAny<HttpClient>(),
                    It.Is<PackageManifest>(p => p.PackageIdentifier == atestPackage.Id),
                    AzFuncHostKey,
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            // Etest.test must be a delete
            mockRestSourceTriggerFunction.Setup(
                m => m.DeletePackageAsync(
                    It.IsAny<HttpClient>(),
                    etestPackageId,
                    AzFuncHostKey,
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var rebuild = new Rebuild();

            await rebuild.ProcessRebuildRequestInternalAsync(
                httpClient,
                "operationId",
                inputSqlPackages,
                inputRestPackages,
                mockRestSourceTriggerFunction.Object,
                AzFuncRestSourceEndpoint,
                AzFuncHostKey,
                this.loggingContext);

            mockHttpMessageHandler.Verify();
            mockRestSourceTriggerFunction.Verify();
        }
    }
}
