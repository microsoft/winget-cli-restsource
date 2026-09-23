// -----------------------------------------------------------------------
// <copyright file="SourceFunctions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.DurableTask;
    using Microsoft.DurableTask.Client;
    using Microsoft.Extensions.Logging;
    using Microsoft.Msix.Utils.Logger;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WindowsPackageManager.Rest.Models;
    using Microsoft.WindowsPackageManager.Rest.Utils;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Functions.Constants;
    using Microsoft.WinGet.RestSource.Functions.Geneva;
    using Microsoft.WinGet.RestSource.Interfaces;
    using Microsoft.WinGet.RestSource.Utils.Constants;

    /// <summary>
    /// This class contains the functions for uploads from and querying data from a repository.
    /// </summary>
    public sealed class SourceFunctions
    {
        private readonly HttpClient httpClient;
        private readonly TelemetryClient telemetryClient;
        private readonly IRebuild rebuildHandler;
        private readonly IUpdate updateHandler;
        private readonly IRestSourceTriggerFunction restSourceTriggerFunction;
        private readonly ILogger<SourceFunctions> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceFunctions"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Http client factory.</param>
        /// <param name="telemetryConfiguration">AppInsights Telemetry Configuration.</param>
        /// <param name="rebuildHandler">An object of type <see cref="IRebuild"/>.</param>
        /// <param name="updateHandler">An object of type <see cref="IUpdate"/>.</param>
        /// <param name="restSourceTriggerFunction">An object of type <see cref="IRestSourceTriggerFunction"/>.</param>
        /// <param name="logger">Logger.</param>
        public SourceFunctions(
            IHttpClientFactory httpClientFactory,
            TelemetryConfiguration telemetryConfiguration,
            IRebuild rebuildHandler,
            IUpdate updateHandler,
            IRestSourceTriggerFunction restSourceTriggerFunction,
            ILogger<SourceFunctions> logger)
        {
            this.httpClient = httpClientFactory.CreateClient();
            this.telemetryClient = new TelemetryClient(telemetryConfiguration);
            this.rebuildHandler = rebuildHandler;
            this.updateHandler = updateHandler;
            this.restSourceTriggerFunction = restSourceTriggerFunction;
            this.logger = logger;
        }

        /// <summary>
        /// Azure function to dispatch source rebuild work.
        /// </summary>
        /// <param name="durableContext">Durable orchestration context.</param>
        /// <returns>SourceResultOutputHelper.</returns>
        [Function(FunctionConstants.RebuildOrchestrator)]
        public async Task<SourceResultOutputHelper> RebuildOrchestratorAsync(
            [OrchestrationTrigger] TaskOrchestrationContext durableContext)
        {
            return await this.SourceOrchestratorHelperAsync<ContextAndReferenceInput>(
                durableContext,
                FunctionConstants.RebuildActivity,
                FunctionConstants.RebuildOrchestrator,
                ErrorMetrics.SourceRebuildError);
        }

        /// <summary>
        /// This function provides an API call that will perform the source rebuild.
        /// </summary>
        /// <param name="input">Activity input.</param>
        /// <param name="executionContext">Function execution context.</param>
        /// <returns>IActionResult.</returns>
        [Function(FunctionConstants.RebuildActivity)]
        public async Task<SourceResultOutputHelper> RebuildActivityAsync(
            [ActivityTrigger] ContextAndReferenceInput input,
            FunctionContext executionContext)
        {
            async Task<SourceResultOutputHelper> WorkAsync(ContextAndReferenceInput inputHelper, LoggingContext loggingContext)
            {
                SourceResultOutputHelper taskResult = new SourceResultOutputHelper(SourceResultType.Error);
                try
                {
                    await this.rebuildHandler.ProcessRebuildRequestAsync(
                        this.httpClient,
                        inputHelper.OperationId,
                        inputHelper.SASReference,
                        inputHelper.ReferenceType,
                        this.restSourceTriggerFunction,
                        ApiConstants.ManifestCacheEndpoint,
                        ApiConstants.AzureFunctionHostKey,
                        loggingContext);

                    taskResult = new SourceResultOutputHelper(SourceResultType.Success);
                }
                catch (Exception e)
                {
                    Logger.Error($"{loggingContext}Error occurred during Rebuild: {e}");
                }

                return taskResult;
            }

            return await this.SourceActivityHelperAsync<ContextAndReferenceInput>(
                input,
                executionContext,
                WorkAsync,
                FunctionConstants.RebuildActivity);
        }

        /// <summary>
        /// Rebuild Post Function.
        /// This function supports analyzing an SQLite file and attempting to match the cosmos Db state to the file.
        /// This function involves doing multiple full passes of the database, thus will be very expensive. It should be used
        /// sparingly only for bootstrapping catalogs and recovering from significant failures.
        /// </summary>
        /// <param name="req">HttpRequestData.</param>
        /// <param name="durableClient">Durable client object.</param>
        /// <returns>HttpResponseData.</returns>
        [Function(FunctionConstants.RebuildPost)]
        public async Task<HttpResponseData> RebuildPostAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "rebuild")]
            HttpRequestData req,
            [DurableClient] DurableTaskClient durableClient)
        {
            return await this.SourceEntryPointHelperAsync<ContextAndReferenceInput>(
                req,
                durableClient,
                FunctionConstants.RebuildOrchestrator,
                FunctionConstants.RebuildPost);
        }

        /// <summary>
        /// Azure function to dispatch source update work.
        /// </summary>
        /// <param name="durableContext">Durable orchestration context.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Function(FunctionConstants.UpdateOrchestrator)]
        public async Task<SourceResultOutputHelper> UpdateOrchestratorAsync(
            [OrchestrationTrigger] TaskOrchestrationContext durableContext)
        {
            return await this.SourceOrchestratorHelperAsync<CommitContextAndReferenceInput>(
                durableContext,
                FunctionConstants.UpdateActivity,
                FunctionConstants.UpdateOrchestrator,
                ErrorMetrics.SourceUpdateError);
        }

        /// <summary>
        /// This function provides an API call that will perform the source update.
        /// </summary>
        /// <param name="input">Activity input.</param>
        /// <param name="executionContext">Function execution context.</param>
        /// <returns>IActionResult.</returns>
        [Function(FunctionConstants.UpdateActivity)]
        public async Task<SourceResultOutputHelper> UpdateActivityAsync(
            [ActivityTrigger] CommitContextAndReferenceInput input,
            FunctionContext executionContext)
        {
            async Task<SourceResultOutputHelper> WorkAsync(CommitContextAndReferenceInput inputHelper, LoggingContext loggingContext)
            {
                SourceResultOutputHelper taskResult = new SourceResultOutputHelper(SourceResultType.Error);
                try
                {
                    await this.updateHandler.ProcessUpdateRequestAsync(
                        this.httpClient,
                        inputHelper.OperationId,
                        inputHelper.Commit,
                        inputHelper.SASReference,
                        inputHelper.ReferenceType,
                        this.restSourceTriggerFunction,
                        ApiConstants.AzureFunctionHostKey,
                        loggingContext);

                    taskResult = new SourceResultOutputHelper(SourceResultType.Success);
                }
                catch (Exception e)
                    when (e is RestSourceCallException || e is InvalidOperationException)
                {
                    // This is mostly user error.
                    Logger.Warning($"{loggingContext}Expected error occurred during Update: {e}");
                    taskResult = new SourceResultOutputHelper(SourceResultType.Failure);
                }
                catch (Exception e)
                {
                    // TODO: Once we have more telemetry we can talk about returning FailureCanRetry here or catch
                    // more specific network exceptions.
                    Logger.Error($"{loggingContext}Unexpected exception during Update: {e}");
                    throw;
                }

                return taskResult;
            }

            return await this.SourceActivityHelperAsync<CommitContextAndReferenceInput>(
                input,
                executionContext,
                WorkAsync,
                FunctionConstants.UpdatePost);
        }

        /// <summary>
        /// Update Post Function.
        /// For each change, we trigger the corresponding operation against the rest source implementation to
        /// add, remove, or update an application.
        /// </summary>
        /// <param name="req">HttpRequestData.</param>
        /// <param name="durableClient">Durable client object.</param>
        /// <returns>HttpResponseData.</returns>
        [Function(FunctionConstants.UpdatePost)]
        public async Task<HttpResponseData> UpdatePostAsync(
            [HttpTrigger(AuthorizationLevel.Function, FunctionConstants.FunctionPost, Route = "update")]
            HttpRequestData req,
            [DurableClient] DurableTaskClient durableClient)
        {
            return await this.SourceEntryPointHelperAsync<CommitContextAndReferenceInput>(
                req,
                durableClient,
                FunctionConstants.UpdateOrchestrator,
                FunctionConstants.UpdateActivity);
        }

        /// <summary>
        /// Source orchestrator helper that does all boilerplate for logging.
        /// Currently it handles calling only one activity function and the only accepted input is ContextAndReferenceInput.
        /// If a a more complex orchestration is needed, this can be updated to take a lambda with the orchestrator work
        /// and use generics for input and output.
        /// Because this is executed in an Azure Orchestration Function there MUST NOT be awaitable calls that are
        /// not initiated by the durable orchestration context.
        /// </summary>
        /// <typeparam name="TFunctionInput">Generic function input.</typeparam>
        /// <param name="durableContext">Orchestration durable context.</param>
        /// <param name="activityFunction">Activity function to call.</param>
        /// <param name="orchestrationFunction">Name of orchestration function. Used for debugging.</param>
        /// <param name="errorMetric">Metric to emit for failures.</param>
        /// <returns>SourceResultOutputHelper.</returns>
        internal async Task<SourceResultOutputHelper> SourceOrchestratorHelperAsync<TFunctionInput>(
            TaskOrchestrationContext durableContext,
            string activityFunction,
            string orchestrationFunction,
            ErrorMetrics errorMetric)
            where TFunctionInput : ContextAndReferenceInput
        {
            LoggingContext loggingContext = new LoggingContext();
            SourceResultOutputHelper sourceResultOutput = new SourceResultOutputHelper(SourceResultType.Error);
            Dictionary<string, string> customDimensions = new Dictionary<string, string>();

            TFunctionInput inputHelper = null;
            ILogger replaySafeLogger = durableContext.CreateReplaySafeLogger<SourceFunctions>();
            try
            {
                inputHelper = durableContext.GetInput<TFunctionInput>();

                loggingContext = DiagnosticsHelper.Instance.GetLoggingContext(
                    durableContext.Name,
                    durableContext.InstanceId,
                    inputHelper.OperationId);

                customDimensions.Add("FunctionName", durableContext.Name);
                customDimensions.Add("OperationId", inputHelper.OperationId);

                replaySafeLogger.LogInformation($"{loggingContext}{orchestrationFunction} executed at: {durableContext.CurrentUtcDateTime}." +
                    $" Calling {activityFunction} activity function.");

                // Actual work.
                sourceResultOutput = await durableContext.CallActivityAsync<SourceResultOutputHelper>(
                    activityFunction,
                    inputHelper);

                replaySafeLogger.LogInformation($"{loggingContext}{activityFunction} activity function verification result {sourceResultOutput}.");
            }
            catch (Exception e)
            {
                replaySafeLogger.LogError($"{loggingContext}Error occurred in SourceOrchestratorHelperAsync {e}");
                customDimensions.Add("Exception", e.GetType().FullName);
                if (!durableContext.IsReplaying)
                {
                    Metrics.EmitMetric(errorMetric, customDimensions, replaySafeLogger);
                }
            }
            finally
            {
                if (inputHelper != null)
                {
                    replaySafeLogger.LogInformation($"{loggingContext}Task result: {sourceResultOutput}");
                }

                customDimensions.Add("Result", sourceResultOutput.ToString());
            }

            return sourceResultOutput;
        }

        /// <summary>
        /// Source activity function helper that does all boilerplate for logging.
        /// Currently it handles only returning SourceResultOutputHelper and its input must
        /// implement ContextAndReferenceInputHelper.
        /// The return type must be a SourceResultOutputHelper.
        /// </summary>
        /// <typeparam name="TFunctionInput">Generic function input.</typeparam>
        /// <param name="functionInput">Activity input.</param>
        /// <param name="executionContext">Execute context.</param>
        /// <param name="workAsync">Work to be done by the activity function.</param>
        /// <param name="activityFunction">Activity function that does the work. Used for logging.</param>
        /// <returns>SourceResultOutputHelper.</returns>
        internal async Task<SourceResultOutputHelper> SourceActivityHelperAsync<TFunctionInput>(
            TFunctionInput functionInput,
            FunctionContext executionContext,
            Func<TFunctionInput, LoggingContext, Task<SourceResultOutputHelper>> workAsync,
            string activityFunction)
            where TFunctionInput : ContextAndReferenceInput
        {
            LoggingContext loggingContext = new LoggingContext();
            Dictionary<string, string> customDimensions = new Dictionary<string, string>();
            SourceResultOutputHelper workResult = new SourceResultOutputHelper(SourceResultType.Error);

            try
            {
                DiagnosticsHelper.Instance.SetupAzureFunctionLoggerAndGenevaTelemetry(
                    this.logger,
                    setupGenevaTelemetry: false);

                loggingContext = DiagnosticsHelper.Instance.GetLoggingContext(
                    executionContext.FunctionDefinition.Name,
                    executionContext.InvocationId,
                    functionInput.OperationId);

                customDimensions.Add("FunctionName", executionContext.FunctionDefinition.Name);
                customDimensions.Add("OperationId", functionInput.OperationId);
                this.telemetryClient.TrackEvent("ExecutionStart", customDimensions);

                Logger.Info($"{loggingContext}Starting {activityFunction}. Received: {functionInput}");

                // Actual work.
                workResult = await workAsync(functionInput, loggingContext);

                Logger.Info($"{loggingContext}{activityFunction} result: {workResult}");
            }
            catch (Exception e)
            {
                Logger.Error($"{loggingContext}Error occurred : {e}");

                this.telemetryClient.TrackException(e, customDimensions);
                customDimensions.Add("Exception", e.GetType().FullName);
                throw;
            }
            finally
            {
                customDimensions.Add("Result", workResult.ToString());
                this.telemetryClient.TrackEvent("ExecutionCompleted", customDimensions);
            }

            return workResult;
        }

        /// <summary>
        /// Source entry point helper function that does all boilerplate for logging.
        /// Parse the request body and starts the given orchestrator. Currently it only supports ContextAndReferenceInputHelper.
        /// </summary>
        /// <typeparam name="TInput">Input.</typeparam>
        /// <param name="req">Request.</param>
        /// <param name="durableClient">Durable orchestration client.</param>
        /// <param name="orchestratorFunction">Orchestration function to start.</param>
        /// <param name="entryPointFunction">Az functions that was called. Used for logging.</param>
        /// <returns>HttpResponseData.</returns>
        internal async Task<HttpResponseData> SourceEntryPointHelperAsync<TInput>(
            HttpRequestData req,
            DurableTaskClient durableClient,
            string orchestratorFunction,
            string entryPointFunction)
            where TInput : ContextAndReferenceInput
        {
            LoggingContext loggingContext = new LoggingContext();
            Dictionary<string, string> customDimensions = new Dictionary<string, string>();
            string orchestrationInstanceId;

            try
            {
                DiagnosticsHelper.Instance.SetupAzureFunctionLoggerAndGenevaTelemetry(
                    this.logger,
                    setupGenevaTelemetry: false);

                TInput input = await RequestBodyHelper.GetRequestDataFromBody<TInput>(
                    req.Body,
                    true);

                loggingContext = DiagnosticsHelper.Instance.GetLoggingContext(
                    req.FunctionContext.FunctionDefinition.Name,
                    req.FunctionContext.InvocationId,
                    input.OperationId);

                customDimensions.Add("FunctionName", req.FunctionContext.FunctionDefinition.Name);
                customDimensions.Add("OperationId", input.OperationId);
                this.telemetryClient.TrackEvent("ExecutionStart", customDimensions);

                Logger.Info($"{loggingContext}Starting {entryPointFunction} processing. Received: {input}");

                orchestrationInstanceId = await durableClient.ScheduleNewOrchestrationInstanceAsync(
                    orchestratorFunction,
                    input);

                Logger.Info($"{loggingContext}{orchestratorFunction} " +
                    $"Orchestration instance id:  {orchestrationInstanceId}.");
            }
            catch (Exception e)
            {
                Logger.Error($"{loggingContext}Error occurred : {e}");
                customDimensions.Add("Exception", e.GetType().FullName);
                this.telemetryClient.TrackException(e, customDimensions);

                HttpResponseData badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteAsJsonAsync(new { Name = $"Error: {e}" });
                return badRequest;
            }
            finally
            {
                this.telemetryClient.TrackEvent("ExecutionCompleted", customDimensions);
            }

            // This returns information that the client can use to query the status of the running orchestration.
            // We expect the client to poll for results and pull the success/fail of the operation from the output of the status response.
            // We are leveraging the full durable function pre-built infrastructure to offer our API Async.
            return await durableClient.CreateCheckStatusResponseAsync(req, orchestrationInstanceId);
        }
    }
}
