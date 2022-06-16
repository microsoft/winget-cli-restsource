// -----------------------------------------------------------------------
// <copyright file="RestSourceTriggerFunctionsTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.RestSource.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WindowsPackageManager.Rest.Utils;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Helpers;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Moq;
    using Moq.Protected;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// RestSourceTriggerFunctions tests.
    /// </summary>
    public class RestSourceTriggerFunctionsTests
    {
        private const string AzFuncRestSourceEndpoint = "https://fake.azurewebsites.net/api/";
        private const string AzFuncHostKey = "1234567890";

        private readonly ITestOutputHelper log;
        private readonly LoggingContext loggingContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestSourceTriggerFunctionsTests"/> class.
        /// </summary>
        /// <param name="log">log.</param>
        public RestSourceTriggerFunctionsTests(ITestOutputHelper log)
        {
            this.log = log;
            this.loggingContext = new LoggingContext();
        }

        /// <summary>
        /// Tests GetPackageManifests.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetPackageManifests_Test()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            string packageIdentifier = "packageIdentifier";
            var apiResponse = new ApiResponse<PackageManifest>(new PackageManifest());
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packageManifests/{packageIdentifier}");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Get &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonHelper.SerializeObject(apiResponse)),
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var result = await restSourceTriggerFunctions.GetPackageManifestAsync(
                httpClient,
                packageIdentifier,
                this.loggingContext);

            mockHttpMessageHandler.Verify();
            Assert.NotNull(result);

            foreach (var x in mockHttpMessageHandler.Invocations)
            {
                this.log.WriteLine(x.Method.ToString());
            }
        }

        /// <summary>
        /// Tests GetPackageManifests result is no content.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetPackageManifests_NoContentTest()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            string packageIdentifier = "packageIdentifier";
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packageManifests/{packageIdentifier}");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Get &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Content = new StringContent(string.Empty),
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var result = await restSourceTriggerFunctions.GetPackageManifestAsync(
                httpClient,
                packageIdentifier,
                this.loggingContext);

            mockHttpMessageHandler.Verify();
            Assert.Null(result);
        }

        /// <summary>
        /// Tests GetPackageManifests when a failure status code is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetPackageManifests_FailureStatusCodeTest()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            string packageIdentifier = "packageIdentifier";
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packageManifests/{packageIdentifier}");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Get &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await Assert.ThrowsAsync<RestSourceCallException>(
                async () => await restSourceTriggerFunctions.GetPackageManifestAsync(
                    httpClient,
                    packageIdentifier,
                    this.loggingContext));

            mockHttpMessageHandler.Verify();
        }

        /// <summary>
        /// Test PostPackageManifestAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PostPackageManifestAsync_Test()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            var packageManifest = new PackageManifest();
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packageManifests");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Post &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await restSourceTriggerFunctions.PostPackageManifestAsync(
                httpClient,
                packageManifest,
                this.loggingContext);

            mockHttpMessageHandler.Verify();
        }

        /// <summary>
        /// Test PostPackageManifestAsync when a failure status code is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PostPackageManifestAsync_FailureStatusCodeTest()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            var packageManifest = new PackageManifest();
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packageManifests");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Post &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await Assert.ThrowsAsync<RestSourceCallException>(
                async () => await restSourceTriggerFunctions.PostPackageManifestAsync(
                    httpClient,
                    packageManifest,
                    this.loggingContext));

            mockHttpMessageHandler.Verify();
        }

        /// <summary>
        /// Test PutPackageManifestAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PutPackageManifestAsync_Test()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            var packageManifest = new PackageManifest();
            packageManifest.PackageIdentifier = "packageIdentifier";
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packageManifests/{packageManifest.PackageIdentifier}");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Put &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await restSourceTriggerFunctions.PutPackageManifestAsync(
                httpClient,
                packageManifest,
                this.loggingContext);

            mockHttpMessageHandler.Verify();
        }

        /// <summary>
        /// Test PutPackageManifestAsync when a failure status code is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PutPackageManifestAsync_FailureStatusCodeTest()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            var packageManifest = new PackageManifest();
            packageManifest.PackageIdentifier = "packageIdentifier";
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packageManifests/{packageManifest.PackageIdentifier}");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Put &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await Assert.ThrowsAsync<RestSourceCallException>(
                async () => await restSourceTriggerFunctions.PutPackageManifestAsync(
                    httpClient,
                    packageManifest,
                    this.loggingContext));

            mockHttpMessageHandler.Verify();
        }

        /// <summary>
        /// Test DeletePackageManifestAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task DeletePackageManifestAsync_Test()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            string packageIdentifier = "packageIdentifier";
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packageManifests/{packageIdentifier}");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Delete &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await restSourceTriggerFunctions.DeletePackageManifestAsync(
                httpClient,
                packageIdentifier,
                this.loggingContext);

            mockHttpMessageHandler.Verify();
        }

        /// <summary>
        /// Test DeletePackageManifestAsync when a failure status code is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task DeletePackageManifestAsync_FailureStatusCodeTest()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            string packageIdentifier = "packageIdentifier";
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packageManifests/{packageIdentifier}");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Delete &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await Assert.ThrowsAsync<RestSourceCallException>(
                async () => await restSourceTriggerFunctions.DeletePackageManifestAsync(
                    httpClient,
                    packageIdentifier,
                    this.loggingContext));

            mockHttpMessageHandler.Verify();
        }

        /// <summary>
        /// Test GetAllPackagesAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetAllPackagesAsync_Test()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packages");

            var firstApiResponse = new ApiResponse<List<Package>>(
                new List<Package>()
                {
                    new Package(),
                    new Package(),
                },
                "continuationToken");

            var secondApiResponse = new ApiResponse<List<Package>>(
                new List<Package>()
                {
                    new Package(),
                },
                null);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // First call won't have the continuation token. Return first response.
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Get &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri &&
                             !r.Headers.Contains(HeaderConstants.ContinuationToken)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonHelper.SerializeObject(firstApiResponse)),
                })
                .Verifiable();

            // Second call will have a continuation token, return second response with a null continuation token
            // to finish execution.
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Get &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri &&
                             r.Headers.Contains(HeaderConstants.ContinuationToken)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonHelper.SerializeObject(secondApiResponse)),
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var result = await restSourceTriggerFunctions.GetAllPackagesAsync(
                httpClient,
                this.loggingContext);

            mockHttpMessageHandler.Verify();
            Assert.Equal(3, result.Count);
        }

        /// <summary>
        /// Test GetPackagesAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetPackagesAsync_Test()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packages");

            var apiResponse = new ApiResponse<List<Package>>(
                new List<Package>()
                {
                    new Package(),
                    new Package(),
                });

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Get &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonHelper.SerializeObject(apiResponse)),
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var result = await restSourceTriggerFunctions.GetPackagesAsync(
                httpClient,
                this.loggingContext);

            mockHttpMessageHandler.Verify();
            Assert.Equal(2, result.Data.Count);
        }

        /// <summary>
        /// Test GetPackagesAsync no content.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetPackagesAsync_NoContentTest()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packages");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Get &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var result = await restSourceTriggerFunctions.GetPackagesAsync(
                httpClient,
                this.loggingContext);

            mockHttpMessageHandler.Verify();
            Assert.Null(result);
        }

        /// <summary>
        /// Test GetPackagesAsync when a failure status code is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetPackagesAsync_FailureStatusCodeTest()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packages");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Get &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await Assert.ThrowsAsync<RestSourceCallException>(
                async () => await restSourceTriggerFunctions.GetPackagesAsync(
                    httpClient,
                    this.loggingContext));

            mockHttpMessageHandler.Verify();
        }

        /// <summary>
        /// Test DeletePackageAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task DeletePackageAsync_Test()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            string packageIdentifier = "packageIdentifier";
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packages/{packageIdentifier}");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Delete &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await restSourceTriggerFunctions.DeletePackageAsync(
                httpClient,
                packageIdentifier,
                this.loggingContext);

            mockHttpMessageHandler.Verify();
        }

        /// <summary>
        /// Test DeletePackageAsync when a failure status code is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task DeletePackageAsync_FailureStatusCodeTest()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            string packageIdentifier = "packageIdentifier";
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packages/{packageIdentifier}");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Delete &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await Assert.ThrowsAsync<RestSourceCallException>(
                async () => await restSourceTriggerFunctions.DeletePackageAsync(
                    httpClient,
                    packageIdentifier,
                    this.loggingContext));

            mockHttpMessageHandler.Verify();
        }

        /// <summary>
        /// Test DeleteVersionAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task DeleteVersionAsync_Test()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            string packageIdentifier = "packageIdentifier";
            string version = "version";
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packages/{packageIdentifier}/versions/{version}");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Delete &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await restSourceTriggerFunctions.DeleteVersionAsync(
                httpClient,
                packageIdentifier,
                version,
                this.loggingContext);

            mockHttpMessageHandler.Verify();
        }

        /// <summary>
        /// Test DeleteVersionAsync when a failure status code is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task DeleteVersionAsync_FailureStatusCodeTest()
        {
            var restSourceTriggerFunctions = new RestSourceTriggerFunctions(AzFuncRestSourceEndpoint, AzFuncHostKey);

            string packageIdentifier = "packageIdentifier";
            string version = "version";
            Uri expectedUri = new Uri($"{AzFuncRestSourceEndpoint}packages/{packageIdentifier}/versions/{version}");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Delete &&
                             r.Headers.Contains("x-functions-key") &&
                             r.RequestUri.AbsoluteUri == expectedUri.AbsoluteUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            await Assert.ThrowsAsync<RestSourceCallException>(
                async () => await restSourceTriggerFunctions.DeleteVersionAsync(
                    httpClient,
                    packageIdentifier,
                    version,
                    this.loggingContext));

            mockHttpMessageHandler.Verify();
        }
    }
}
