// -----------------------------------------------------------------------
// <copyright file="SourceFunctionsTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.AzFunctions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.DurableTask;
    using Microsoft.DurableTask.Client;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WindowsPackageManager.Rest.Models;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Functions;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Functions.Geneva;
    using Microsoft.WinGet.RestSource.Interfaces;
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
                ConnectionString = $"InstrumentationKey={Guid.NewGuid()}",
            };

            this.mockHttpClientFactory
                .Setup(m => m.CreateClient(It.IsAny<string>()))
                .Returns(this.mockHttpClient.Object);
        }

        /// <summary>
        /// Tests the rebuild HTTP entry point.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RebuildPostAsync_Test()
        {
            SourceFunctions sourceFunctions = this.CreateSourceFunctions();
            var input = new ContextAndReferenceInput("operationId", "sasReference", ReferenceType.Add);
            HttpRequestData request = CreateHttpRequestData(FunctionConstants.RebuildPost, input);
            Mock<DurableTaskClient> durableClient = CreateDurableTaskClient(
                FunctionConstants.RebuildOrchestrator,
                input);

            HttpResponseData response = await sourceFunctions.RebuildPostAsync(request, durableClient.Object);

            durableClient.Verify();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        /// <summary>
        /// Tests the update HTTP entry point.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdatePostAsync_Test()
        {
            SourceFunctions sourceFunctions = this.CreateSourceFunctions();
            var input = new CommitContextAndReferenceInput("operationId", "sasReference", "commit", ReferenceType.Add);
            HttpRequestData request = CreateHttpRequestData(FunctionConstants.UpdatePost, input);
            Mock<DurableTaskClient> durableClient = CreateDurableTaskClient(
                FunctionConstants.UpdateOrchestrator,
                input);

            HttpResponseData response = await sourceFunctions.UpdatePostAsync(request, durableClient.Object);

            durableClient.Verify();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        /// <summary>
        /// Tests that HTTP entry point failures return a bad request.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SourceEntryPointHelperAsync_Test_Throws()
        {
            SourceFunctions sourceFunctions = this.CreateSourceFunctions();
            var input = new ContextAndReferenceInput("operationId", "sasReference", ReferenceType.Add);
            HttpRequestData request = CreateHttpRequestData(FunctionConstants.RebuildPost, input);
            var durableClient = new Mock<DurableTaskClient>("client");
            durableClient
                .Setup(m => m.ScheduleNewOrchestrationInstanceAsync(
                    It.IsAny<TaskName>(),
                    It.IsAny<object>(),
                    It.IsAny<StartOrchestrationOptions>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception())
                .Verifiable();

            HttpResponseData response = await sourceFunctions.SourceEntryPointHelperAsync<ContextAndReferenceInput>(
                request,
                durableClient.Object,
                FunctionConstants.RebuildOrchestrator,
                FunctionConstants.RebuildPost);

            durableClient.Verify();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Tests the rebuild orchestrator.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RebuildOrchestratorAsync_Test()
        {
            SourceFunctions sourceFunctions = this.CreateSourceFunctions();
            var input = new ContextAndReferenceInput("operationId", "sasReference", ReferenceType.Add);
            var expected = new SourceResultOutputHelper(SourceResultType.Success);
            Mock<TaskOrchestrationContext> context = this.CreateOrchestrationContext(
                FunctionConstants.RebuildOrchestrator,
                input);

            context
                .Setup(m => m.CallActivityAsync<SourceResultOutputHelper>(
                    FunctionConstants.RebuildActivity,
                    It.Is<ContextAndReferenceInput>(value => value.OperationId == input.OperationId),
                    It.IsAny<TaskOptions>()))
                .ReturnsAsync(expected)
                .Verifiable();

            SourceResultOutputHelper result = await sourceFunctions.RebuildOrchestratorAsync(context.Object);

            context.Verify();
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the update orchestrator.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateOrchestratorAsync_Test()
        {
            SourceFunctions sourceFunctions = this.CreateSourceFunctions();
            var input = new CommitContextAndReferenceInput("operationId", "sasReference", "commit", ReferenceType.Add);
            var expected = new SourceResultOutputHelper(SourceResultType.Success);
            Mock<TaskOrchestrationContext> context = this.CreateOrchestrationContext(
                FunctionConstants.UpdateOrchestrator,
                input);

            context
                .Setup(m => m.CallActivityAsync<SourceResultOutputHelper>(
                    FunctionConstants.UpdateActivity,
                    It.Is<CommitContextAndReferenceInput>(value => value.Commit == input.Commit),
                    It.IsAny<TaskOptions>()))
                .ReturnsAsync(expected)
                .Verifiable();

            SourceResultOutputHelper result = await sourceFunctions.UpdateOrchestratorAsync(context.Object);

            context.Verify();
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that orchestration failures return an error result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SourceOrchestratorHelperAsync_Throws()
        {
            SourceFunctions sourceFunctions = this.CreateSourceFunctions();
            var input = new ContextAndReferenceInput("operationId", "sasReference", ReferenceType.Add);
            Mock<TaskOrchestrationContext> context = this.CreateOrchestrationContext("orchestrator", input);

            context
                .Setup(m => m.CallActivityAsync<SourceResultOutputHelper>(
                    It.IsAny<TaskName>(),
                    It.IsAny<object>(),
                    It.IsAny<TaskOptions>()))
                .ThrowsAsync(new Exception())
                .Verifiable();

            SourceResultOutputHelper result = await sourceFunctions.SourceOrchestratorHelperAsync<ContextAndReferenceInput>(
                context.Object,
                "activity",
                "orchestrator",
                ErrorMetrics.SourceUpdateError);

            context.Verify();
            Assert.Equal(SourceResultType.Error, result.OverallResult);
        }

        /// <summary>
        /// Tests a successful rebuild activity.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RebuildActivityAsync_Test()
        {
            var rebuild = new Mock<IRebuild>();
            rebuild
                .Setup(m => m.ProcessRebuildRequestAsync(
                    It.IsAny<HttpClient>(),
                    "operationId",
                    "sasReference",
                    ReferenceType.Add,
                    It.IsAny<IRestSourceTriggerFunction>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            SourceFunctions sourceFunctions = this.CreateSourceFunctions(rebuild: rebuild.Object);
            var input = new ContextAndReferenceInput("operationId", "sasReference", ReferenceType.Add);

            SourceResultOutputHelper result = await sourceFunctions.RebuildActivityAsync(
                input,
                CreateFunctionContext(FunctionConstants.RebuildActivity));

            rebuild.Verify();
            Assert.Equal(SourceResultType.Success, result.OverallResult);
        }

        /// <summary>
        /// Tests that rebuild failures return an error result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RebuildActivityAsync_Throws()
        {
            var rebuild = new Mock<IRebuild>();
            rebuild
                .Setup(m => m.ProcessRebuildRequestAsync(
                    It.IsAny<HttpClient>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<ReferenceType>(),
                    It.IsAny<IRestSourceTriggerFunction>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<LoggingContext>()))
                .ThrowsAsync(new Exception());

            SourceFunctions sourceFunctions = this.CreateSourceFunctions(rebuild: rebuild.Object);
            var input = new ContextAndReferenceInput("operationId", "sasReference", ReferenceType.Add);

            SourceResultOutputHelper result = await sourceFunctions.RebuildActivityAsync(
                input,
                CreateFunctionContext(FunctionConstants.RebuildActivity));

            Assert.Equal(SourceResultType.Error, result.OverallResult);
        }

        /// <summary>
        /// Tests a successful update activity.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateActivityAsync_Test()
        {
            var update = new Mock<IUpdate>();
            update
                .Setup(m => m.ProcessUpdateRequestAsync(
                    It.IsAny<HttpClient>(),
                    "operationId",
                    "commit",
                    "sasReference",
                    ReferenceType.Add,
                    It.IsAny<IRestSourceTriggerFunction>(),
                    It.IsAny<string>(),
                    It.IsAny<LoggingContext>()))
                .Verifiable();

            SourceFunctions sourceFunctions = this.CreateSourceFunctions(update: update.Object);
            var input = new CommitContextAndReferenceInput("operationId", "sasReference", "commit", ReferenceType.Add);

            SourceResultOutputHelper result = await sourceFunctions.UpdateActivityAsync(
                input,
                CreateFunctionContext(FunctionConstants.UpdateActivity));

            update.Verify();
            Assert.Equal(SourceResultType.Success, result.OverallResult);
        }

        /// <summary>
        /// Tests that expected update failures return a failure result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateActivityAsync_ThrowsExpected()
        {
            var update = new Mock<IUpdate>();
            update
                .Setup(m => m.ProcessUpdateRequestAsync(
                    It.IsAny<HttpClient>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<ReferenceType>(),
                    It.IsAny<IRestSourceTriggerFunction>(),
                    It.IsAny<string>(),
                    It.IsAny<LoggingContext>()))
                .ThrowsAsync(new RestSourceCallException("message"));

            SourceFunctions sourceFunctions = this.CreateSourceFunctions(update: update.Object);
            var input = new CommitContextAndReferenceInput("operationId", "sasReference", "commit", ReferenceType.Add);

            SourceResultOutputHelper result = await sourceFunctions.UpdateActivityAsync(
                input,
                CreateFunctionContext(FunctionConstants.UpdateActivity));

            Assert.Equal(SourceResultType.Failure, result.OverallResult);
        }

        /// <summary>
        /// Tests that unexpected update failures are propagated.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateActivityAsync_Throws()
        {
            var update = new Mock<IUpdate>();
            update
                .Setup(m => m.ProcessUpdateRequestAsync(
                    It.IsAny<HttpClient>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<ReferenceType>(),
                    It.IsAny<IRestSourceTriggerFunction>(),
                    It.IsAny<string>(),
                    It.IsAny<LoggingContext>()))
                .ThrowsAsync(new Exception());

            SourceFunctions sourceFunctions = this.CreateSourceFunctions(update: update.Object);
            var input = new CommitContextAndReferenceInput("operationId", "sasReference", "commit", ReferenceType.Add);

            await Assert.ThrowsAsync<Exception>(
                () => sourceFunctions.UpdateActivityAsync(
                    input,
                    CreateFunctionContext(FunctionConstants.UpdateActivity)));
        }

        /// <summary>
        /// Tests that activity helper failures are propagated.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task SourceActivityHelperAsync_Throws()
        {
            SourceFunctions sourceFunctions = this.CreateSourceFunctions();
            var input = new ContextAndReferenceInput("operationId", "sasReference", ReferenceType.Add);

            Task<SourceResultOutputHelper> WorkAsync(ContextAndReferenceInput inputHelper, LoggingContext loggingContext)
            {
                throw new NotImplementedException();
            }

            await Assert.ThrowsAsync<NotImplementedException>(
                () => sourceFunctions.SourceActivityHelperAsync(
                    input,
                    CreateFunctionContext(FunctionConstants.RebuildActivity),
                    WorkAsync,
                    FunctionConstants.RebuildActivity));
        }

        private static Mock<DurableTaskClient> CreateDurableTaskClient<TInput>(
            string orchestratorName,
            TInput expectedInput)
        {
            var durableClient = new Mock<DurableTaskClient>("client");
            durableClient
                .Setup(m => m.ScheduleNewOrchestrationInstanceAsync(
                    It.Is<TaskName>(name => name.Name == orchestratorName),
                    It.Is<object>(input =>
                        Newtonsoft.Json.JsonConvert.SerializeObject(input) ==
                        Newtonsoft.Json.JsonConvert.SerializeObject(expectedInput)),
                    It.IsAny<StartOrchestrationOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync("instanceId")
                .Verifiable();
            return durableClient;
        }

        private static HttpRequestData CreateHttpRequestData<TInput>(string functionName, TInput input)
        {
            FunctionContext context = CreateFunctionContext(functionName);
            var response = new Mock<HttpResponseData>(context);
            response.SetupProperty(m => m.StatusCode);
            response.SetupProperty(m => m.Headers, new HttpHeadersCollection());
            response.SetupProperty(m => m.Body, new MemoryStream());
            response.SetupGet(m => m.Cookies).Returns(Mock.Of<HttpCookies>());

            var request = new Mock<HttpRequestData>(context);
            request.SetupGet(m => m.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(input))));
            request.SetupGet(m => m.Headers).Returns(new HttpHeadersCollection());
            request.SetupGet(m => m.Cookies).Returns(Mock.Of<IReadOnlyCollection<IHttpCookie>>());
            request.SetupGet(m => m.Identities).Returns(Array.Empty<ClaimsIdentity>());
            request.SetupGet(m => m.Method).Returns("POST");
            request.SetupGet(m => m.Url).Returns(new Uri("https://localhost/"));
            request.Setup(m => m.CreateResponse()).Returns(response.Object);
            return request.Object;
        }

        private static FunctionContext CreateFunctionContext(string functionName)
        {
            var definition = new Mock<FunctionDefinition>();
            definition.SetupGet(m => m.Name).Returns(functionName);

            var services = new ServiceCollection();
            services.AddOptions<WorkerOptions>().Configure(options =>
            {
                options.Serializer = new global::Azure.Core.Serialization.JsonObjectSerializer();
            });

            var context = new Mock<FunctionContext>();
            context.SetupGet(m => m.FunctionDefinition).Returns(definition.Object);
            context.SetupGet(m => m.InvocationId).Returns(Guid.NewGuid().ToString());
            context.SetupProperty(m => m.InstanceServices, services.BuildServiceProvider());
            return context.Object;
        }

        private SourceFunctions CreateSourceFunctions(IRebuild rebuild = null, IUpdate update = null)
        {
            return new SourceFunctions(
                this.mockHttpClientFactory.Object,
                this.telemetryConfiguration,
                rebuild ?? Mock.Of<IRebuild>(),
                update ?? Mock.Of<IUpdate>(),
                Mock.Of<IRestSourceTriggerFunction>(),
                Mock.Of<ILogger<SourceFunctions>>());
        }

        private Mock<TaskOrchestrationContext> CreateOrchestrationContext<TInput>(
            string functionName,
            TInput input)
        {
            var context = new Mock<TaskOrchestrationContext>();
            context.SetupGet(m => m.Name).Returns(functionName);
            context.SetupGet(m => m.InstanceId).Returns(Guid.NewGuid().ToString());
            context.SetupGet(m => m.CurrentUtcDateTime).Returns(DateTime.UtcNow);
            context.Setup(m => m.GetInput<TInput>()).Returns(input);
            context.Setup(m => m.CreateReplaySafeLogger<SourceFunctions>()).Returns(this.mockLogger.Object);
            return context;
        }
    }
}
