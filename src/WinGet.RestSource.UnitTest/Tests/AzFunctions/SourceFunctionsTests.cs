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
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WindowsPackageManager.Rest.Models;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Functions;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Functions.Geneva;
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

            string operationId = "operationId";
            string sasReference = "sasReference";
            ReferenceType referenceType = ReferenceType.Add;
            var body = new ContextAndReferenceInput(
                operationId,
                sasReference,
                referenceType);
            var mockHttpRequest = TestUtils.CreateMockHttpRequest(body);

            string orchestrationInstanceId = "orchestrationInstanceId";
            var mockDurableOrchestrationClient = new Mock<IDurableOrchestrationClient>();
            mockDurableOrchestrationClient.Setup(
                m => m.StartNewAsync(
                    FunctionConstants.RebuildOrchestrator,
                    It.Is<ContextAndReferenceInput>(
                        c => c.OperationId == operationId &&
                             c.SASReference == sasReference &&
                             c.ReferenceType == referenceType)))
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

            mockDurableOrchestrationClient.Verify();
        }

        /// <summary>
        /// Test UpdatePostAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdatePostAsync_Test()
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

            string operationId = "operationId";
            string sasReference = "sasReference";
            string commit = "commit";
            ReferenceType referenceType = ReferenceType.Add;
            var body = new CommitContextAndReferenceInput(
                operationId,
                sasReference,
                commit,
                referenceType);
            var mockHttpRequest = TestUtils.CreateMockHttpRequest(body);

            string orchestrationInstanceId = "orchestrationInstanceId";
            var mockDurableOrchestrationClient = new Mock<IDurableOrchestrationClient>();
            mockDurableOrchestrationClient.Setup(
                m => m.StartNewAsync(
                    FunctionConstants.UpdateOrchestrator,
                    It.Is<CommitContextAndReferenceInput>(
                        c => c.OperationId == operationId &&
                             c.SASReference == sasReference &&
                             c.Commit == commit &&
                             c.ReferenceType == referenceType)))
                .ReturnsAsync(orchestrationInstanceId)
                .Verifiable();
            mockDurableOrchestrationClient.Setup(
                m => m.CreateCheckStatusResponse(
                    mockHttpRequest.Object,
                    orchestrationInstanceId,
                    It.IsAny<bool>()))
                .Verifiable();

            _ = await sourceFunctions.UpdatePostAsync(
                mockHttpRequest.Object,
                mockDurableOrchestrationClient.Object,
                this.mockLogger.Object,
                this.mockExecutionContext.Object);

            mockDurableOrchestrationClient.Verify();
        }

        /// <summary>
        /// Tests SourceEntryPointHelperAsync returns bad request object when there's an exception thrown.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SourceEntryPointHelperAsync_Test_Throws()
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

            string operationId = "operationId";
            string sasReference = "sasReference";
            ReferenceType referenceType = ReferenceType.Add;
            var body = new ContextAndReferenceInput(
                operationId,
                sasReference,
                referenceType);
            var mockHttpRequest = TestUtils.CreateMockHttpRequest(body);

            var mockDurableOrchestrationClient = new Mock<IDurableOrchestrationClient>();
            mockDurableOrchestrationClient.Setup(
                m => m.StartNewAsync(
                    It.IsAny<string>(),
                    It.Is<ContextAndReferenceInput>(
                        c => c.OperationId == operationId &&
                             c.SASReference == sasReference &&
                             c.ReferenceType == referenceType)))
                .Throws(new Exception())
                .Verifiable();

            var result = await sourceFunctions.SourceEntryPointHelperAsync<ContextAndReferenceInput>(
                mockHttpRequest.Object,
                mockDurableOrchestrationClient.Object,
                this.mockLogger.Object,
                this.mockExecutionContext.Object,
                "orchestartorFunction",
                "entryPointFunction");

            mockDurableOrchestrationClient.Verify();
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Tests RebuildOrchestratorAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RebuildOrchestratorAsync_Test()
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

            string operationId = "operationId";
            string sasReference = "sasReference";
            ReferenceType referenceType = ReferenceType.Add;
            var input = new ContextAndReferenceInput(
                operationId,
                sasReference,
                referenceType);

            var mockResult = new SourceResultOutputHelper(SourceResultType.Success);

            var mockDurableOrchestrationContext = new Mock<IDurableOrchestrationContext>();
            mockDurableOrchestrationContext.Setup(
                m => m.GetInput<ContextAndReferenceInput>())
                .Returns(input);
            mockDurableOrchestrationContext.Setup(
                m => m.CallActivityAsync<SourceResultOutputHelper>(
                    FunctionConstants.RebuildActivity,
                    It.Is<ContextAndReferenceInput>(
                        c => c.OperationId == operationId &&
                             c.SASReference == sasReference &&
                             c.ReferenceType == referenceType)))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var result = await sourceFunctions.RebuildOrchestratorAsync(
                mockDurableOrchestrationContext.Object,
                this.mockLogger.Object,
                this.mockExecutionContext.Object);

            mockDurableOrchestrationContext.Verify();
            Assert.Equal(mockResult, result);
        }

        /// <summary>
        /// Tests UpdateOrchestratorAsync.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateOrchestratorAsync_Test()
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

            string operationId = "operationId";
            string sasReference = "sasReference";
            string commit = "commit";
            ReferenceType referenceType = ReferenceType.Add;
            var input = new CommitContextAndReferenceInput(
                operationId,
                sasReference,
                commit,
                referenceType);

            var mockResult = new SourceResultOutputHelper(SourceResultType.Success);

            var mockDurableOrchestrationContext = new Mock<IDurableOrchestrationContext>();
            mockDurableOrchestrationContext.Setup(
                m => m.GetInput<CommitContextAndReferenceInput>())
                .Returns(input);
            mockDurableOrchestrationContext.Setup(
                m => m.CallActivityAsync<SourceResultOutputHelper>(
                    FunctionConstants.UpdateActivity,
                    It.Is<CommitContextAndReferenceInput>(
                        c => c.OperationId == operationId &&
                             c.SASReference == sasReference &&
                             c.ReferenceType == referenceType &&
                             c.Commit == commit)))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var result = await sourceFunctions.UpdateOrchestratorAsync(
                mockDurableOrchestrationContext.Object,
                this.mockLogger.Object,
                this.mockExecutionContext.Object);

            mockDurableOrchestrationContext.Verify();
            Assert.Equal(mockResult, result);
        }

        /// <summary>
        /// Tests SourceOrchestratorHelperAsync when activity functions fails.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SourceOrchestratorHelperAsync_Throws()
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

            string operationId = "operationId";
            string sasReference = "sasReference";
            ReferenceType referenceType = ReferenceType.Add;
            var input = new ContextAndReferenceInput(
                operationId,
                sasReference,
                referenceType);

            var mockDurableOrchestrationContext = new Mock<IDurableOrchestrationContext>();
            mockDurableOrchestrationContext.Setup(
                m => m.GetInput<ContextAndReferenceInput>())
                .Returns(input);
            mockDurableOrchestrationContext.Setup(
                m => m.CallActivityAsync<SourceResultOutputHelper>(
                    It.IsAny<string>(),
                    It.Is<ContextAndReferenceInput>(
                        c => c.OperationId == operationId &&
                             c.SASReference == sasReference &&
                             c.ReferenceType == referenceType)))
                .ThrowsAsync(new Exception())
                .Verifiable();

            var result = await sourceFunctions.SourceOrchestratorHelperAsync<ContextAndReferenceInput>(
                mockDurableOrchestrationContext.Object,
                this.mockLogger.Object,
                this.mockExecutionContext.Object,
                "activityFunction",
                "orchestrationFunction",
                ErrorMetrics.SourceUpdateError);

            mockDurableOrchestrationContext.Verify();
            Assert.Equal(SourceResultType.Error, result.OverallResult);
        }

        /// <summary>
        /// Tests RebuildActivityAsync good scenario.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RebuildActivityAsync_Test()
        {
            var mockUpdate = new Mock<IUpdate>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            string operationId = "operationId";
            string sasReference = "sasReference";
            ReferenceType referenceType = ReferenceType.Add;

            var mockRebuild = new Mock<IRebuild>();
            mockRebuild.Setup(
                m => m.ProcessRebuildRequestAsync(
                    It.IsAny<HttpClient>(),
                    operationId,
                    sasReference,
                    referenceType,
                    It.IsAny<IRestSourceTriggerFunction>(),
                    It.IsAny<string>(),
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            var sourceFunctions = new SourceFunctions(
                this.mockHttpClientFactory.Object,
                this.telemetryConfiguration,
                mockRebuild.Object,
                mockUpdate.Object,
                mockRestSourceTriggerFunction.Object);

            var input = new ContextAndReferenceInput(
                operationId,
                sasReference,
                referenceType);

            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            mockDurableActivityContext.Setup(
                m => m.GetInput<ContextAndReferenceInput>())
                .Returns(input)
                .Verifiable();

            var result = await sourceFunctions.RebuildActivityAsync(
                mockDurableActivityContext.Object,
                this.mockLogger.Object,
                this.mockExecutionContext.Object);

            mockDurableActivityContext.Verify();
            mockRebuild.Verify();
            Assert.Equal(SourceResultType.Success, result.OverallResult);
        }

        /// <summary>
        /// Tests RebuildActivityAsync when rebuild throws an exception.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RebuildActivityAsync_Throws()
        {
            var mockUpdate = new Mock<IUpdate>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            string operationId = "operationId";
            string sasReference = "sasReference";
            ReferenceType referenceType = ReferenceType.Add;

            var mockRebuild = new Mock<IRebuild>();
            mockRebuild.Setup(
                m => m.ProcessRebuildRequestAsync(
                    It.IsAny<HttpClient>(),
                    operationId,
                    sasReference,
                    referenceType,
                    It.IsAny<IRestSourceTriggerFunction>(),
                    It.IsAny<string>(),
                    It.IsAny<LoggingContext>()))
                .Throws(new Exception())
                .Verifiable();

            var sourceFunctions = new SourceFunctions(
                this.mockHttpClientFactory.Object,
                this.telemetryConfiguration,
                mockRebuild.Object,
                mockUpdate.Object,
                mockRestSourceTriggerFunction.Object);

            var input = new ContextAndReferenceInput(
                operationId,
                sasReference,
                referenceType);

            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            mockDurableActivityContext.Setup(
                m => m.GetInput<ContextAndReferenceInput>())
                .Returns(input)
                .Verifiable();

            var result = await sourceFunctions.RebuildActivityAsync(
                mockDurableActivityContext.Object,
                this.mockLogger.Object,
                this.mockExecutionContext.Object);

            mockDurableActivityContext.Verify();
            mockRebuild.Verify();
            Assert.Equal(SourceResultType.Error, result.OverallResult);
        }

        /// <summary>
        /// Tests UpdateActivityAsync normal scenario.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateActivityAsync_Test()
        {
            var mockRebuild = new Mock<IRebuild>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            string operationId = "operationId";
            string sasReference = "sasReference";
            string commit = "commit";
            ReferenceType referenceType = ReferenceType.Add;

            var mockUpdate = new Mock<IUpdate>();
            mockUpdate.Setup(
                m => m.ProcessUpdateRequestAsync(
                    It.IsAny<HttpClient>(),
                    operationId,
                    commit,
                    sasReference,
                    referenceType,
                    It.IsAny<IRestSourceTriggerFunction>(),
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            var sourceFunctions = new SourceFunctions(
                this.mockHttpClientFactory.Object,
                this.telemetryConfiguration,
                mockRebuild.Object,
                mockUpdate.Object,
                mockRestSourceTriggerFunction.Object);

            var input = new CommitContextAndReferenceInput(
                operationId,
                sasReference,
                commit,
                referenceType);

            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            mockDurableActivityContext.Setup(
                m => m.GetInput<CommitContextAndReferenceInput>())
                .Returns(input)
                .Verifiable();

            var result = await sourceFunctions.UpdateActivityAsync(
                mockDurableActivityContext.Object,
                this.mockLogger.Object,
                this.mockExecutionContext.Object);

            mockDurableActivityContext.Verify();
            mockUpdate.Verify();
            Assert.Equal(SourceResultType.Success, result.OverallResult);
        }

        /// <summary>
        /// Tests UpdateActivityAsync expected exception.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateActivityAsync_ThrowsExpected()
        {
            var mockRebuild = new Mock<IRebuild>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            string operationId = "operationId";
            string sasReference = "sasReference";
            string commit = "commit";
            ReferenceType referenceType = ReferenceType.Add;

            var mockUpdate = new Mock<IUpdate>();
            mockUpdate.Setup(
                m => m.ProcessUpdateRequestAsync(
                    It.IsAny<HttpClient>(),
                    operationId,
                    commit,
                    sasReference,
                    referenceType,
                    It.IsAny<IRestSourceTriggerFunction>(),
                    It.IsAny<LoggingContext>()))
                .Throws(new RestSourceCallException("message"))
                .Verifiable();

            var sourceFunctions = new SourceFunctions(
                this.mockHttpClientFactory.Object,
                this.telemetryConfiguration,
                mockRebuild.Object,
                mockUpdate.Object,
                mockRestSourceTriggerFunction.Object);

            var input = new CommitContextAndReferenceInput(
                operationId,
                sasReference,
                commit,
                referenceType);

            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            mockDurableActivityContext.Setup(
                m => m.GetInput<CommitContextAndReferenceInput>())
                .Returns(input)
                .Verifiable();

            var result = await sourceFunctions.UpdateActivityAsync(
                mockDurableActivityContext.Object,
                this.mockLogger.Object,
                this.mockExecutionContext.Object);

            mockDurableActivityContext.Verify();
            mockUpdate.Verify();
            Assert.Equal(SourceResultType.Failure, result.OverallResult);
        }

        /// <summary>
        /// Tests UpdateActivityAsync unexpected exception.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateActivityAsync_Throws()
        {
            var mockRebuild = new Mock<IRebuild>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            string operationId = "operationId";
            string sasReference = "sasReference";
            string commit = "commit";
            ReferenceType referenceType = ReferenceType.Add;

            var mockUpdate = new Mock<IUpdate>();
            mockUpdate.Setup(
                m => m.ProcessUpdateRequestAsync(
                    It.IsAny<HttpClient>(),
                    operationId,
                    commit,
                    sasReference,
                    referenceType,
                    It.IsAny<IRestSourceTriggerFunction>(),
                    It.IsAny<LoggingContext>()))
                .Throws(new Exception())
                .Verifiable();

            var sourceFunctions = new SourceFunctions(
                this.mockHttpClientFactory.Object,
                this.telemetryConfiguration,
                mockRebuild.Object,
                mockUpdate.Object,
                mockRestSourceTriggerFunction.Object);

            var input = new CommitContextAndReferenceInput(
                operationId,
                sasReference,
                commit,
                referenceType);

            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            mockDurableActivityContext.Setup(
                m => m.GetInput<CommitContextAndReferenceInput>())
                .Returns(input)
                .Verifiable();

            await Assert.ThrowsAsync<Exception>(
                async () => await sourceFunctions.UpdateActivityAsync(
                    mockDurableActivityContext.Object,
                    this.mockLogger.Object,
                    this.mockExecutionContext.Object));

            mockDurableActivityContext.Verify();
            mockUpdate.Verify();
        }

        /// <summary>
        /// Tests SourceEntryPointHelperAsync when lambda throws.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SourceEntryPointHelperAsync_Throws()
        {
            var mockUpdate = new Mock<IUpdate>();
            var mockRebuild = new Mock<IRebuild>();
            var mockRestSourceTriggerFunction = new Mock<IRestSourceTriggerFunction>();

            var sourceFunctions = new SourceFunctions(
                this.mockHttpClientFactory.Object,
                this.telemetryConfiguration,
                mockRebuild.Object,
                mockUpdate.Object,
                mockRestSourceTriggerFunction.Object);

            string operationId = "operationId";
            string sasReference = "sasReference";
            ReferenceType referenceType = ReferenceType.Add;
            var input = new ContextAndReferenceInput(
                operationId,
                sasReference,
                referenceType);

            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            mockDurableActivityContext.Setup(
                m => m.GetInput<ContextAndReferenceInput>())
                .Returns(input)
                .Verifiable();

            async Task<SourceResultOutputHelper> WorkAsync(ContextAndReferenceInput inputHelper, LoggingContext loggingContext)
            {
                await Task.CompletedTask;
                throw new NotImplementedException();
            }

            await Assert.ThrowsAsync<NotImplementedException>(
                async () => await sourceFunctions.SourceActivityHelperAsync<ContextAndReferenceInput>(
                    mockDurableActivityContext.Object,
                    this.mockLogger.Object,
                    this.mockExecutionContext.Object,
                    WorkAsync,
                    "activityFunction"));

            mockDurableActivityContext.Verify();
        }
    }
}
