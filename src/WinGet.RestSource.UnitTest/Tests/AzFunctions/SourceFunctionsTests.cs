// -----------------------------------------------------------------------
// <copyright file="SourceFunctionsTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.AzFunctions
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsPackageManager.Rest.Models;
    using Microsoft.WinGet.RestSource.Functions;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Interfaces;
    using Microsoft.Winget.RestSource.UnitTest.Common;
    using Moq;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Tests for SourceFunctions.
    /// </summary>
    public class SourceFunctionsTests
    {
        private readonly ITestOutputHelper log;
        private readonly TelemetryConfiguration telemetryConfiguration;
        private readonly Mock<ITelemetryChannel> mockTelemetryChannel = new Mock<ITelemetryChannel>();
        private readonly Mock<IHttpClientFactory> mockHttpClientFactory = new Mock<IHttpClientFactory>();
        private readonly Mock<HttpClient> mockHttpClient = new Mock<HttpClient>();
        private readonly Mock<ILogger> mockLogger = new Mock<ILogger>();
        private readonly Mock<ExecutionContext> mockExecutionContext = new Mock<ExecutionContext>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceFunctionsTests"/> class.
        /// </summary>
        /// <param name="log">Log.</param>
        public SourceFunctionsTests(ITestOutputHelper log)
        {
            this.log = log;

            this.telemetryConfiguration = new TelemetryConfiguration
            {
                TelemetryChannel = this.mockTelemetryChannel.Object,
                InstrumentationKey = Guid.NewGuid().ToString(),
            };

            this.mockHttpClientFactory.Setup(
                m => m.CreateClient(It.IsAny<string>()))
                .Returns(this.mockHttpClient.Object);
        }

        /// <summary>
        /// Test RebuildPostAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RebuildPostAsync_Test()
        {
            var mockRebuild = new Mock<IRebuild>();
            var mockUpdate = new Mock<IUpdate>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();
            var sourceFunctions = new SourceFunctions(
                this.mockHttpClientFactory.Object,
                this.telemetryConfiguration,
                mockRebuild.Object,
                mockUpdate.Object,
                mockRestSourceTriggerFunction.Object);

            string operationId = Guid.NewGuid().ToString();
            string sasReference = "sasReference";
            ReferenceType referenceType = ReferenceType.Add;
            var body = new ContextAndReferenceInput(
                operationId,
                sasReference,
                referenceType);
            var mockHttpRequest = TestUtils.CreateMockHttpRequest(body);

            string orchestrationInstanceId = Guid.NewGuid().ToString();
            var mockDurableOrchestrationClient = new Mock<IDurableOrchestrationClient>();
            mockDurableOrchestrationClient.Setup(
                m => m.StartNewAsync(
                    FunctionConstants.RebuildOrchestrator,
                    It.Is<ContextAndReferenceInput>(
                        c => c.OperationId == operationId && c.SASReference == sasReference && c.ReferenceType == referenceType)))
                .ReturnsAsync(orchestrationInstanceId)
                .Verifiable();
            mockDurableOrchestrationClient.Setup(
                m => m.CreateCheckStatusResponse(
                    mockHttpRequest.Object,
                    orchestrationInstanceId,
                    It.IsAny<bool>()))
                .Verifiable();

            _ = await sourceFunctions.RebuildPostAsync(
                mockHttpRequest.Object,
                mockDurableOrchestrationClient.Object,
                this.mockLogger.Object,
                this.mockExecutionContext.Object);
        }
    }
}
