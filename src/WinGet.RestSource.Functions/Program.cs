// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions
{
    using System;
    using System.IO;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WinGet.RestSource.AppConfig;
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.Factories;
    using Microsoft.WinGet.RestSource.Helpers;
    using Microsoft.WinGet.RestSource.Interfaces;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Azure Functions isolated worker entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Configures and starts the Functions worker.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            FunctionsApplicationBuilder builder = FunctionsApplication.CreateBuilder(args);
            builder.ConfigureFunctionsWebApplication();

            builder.Services
                .AddApplicationInsightsTelemetryWorkerService()
                .ConfigureFunctionsApplicationInsights();

            DiagnosticsHelper.ConfigureGenevaLogging(builder.Logging);

            builder.Services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            ConfigureServices(builder.Services);
            builder.Build().Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            string endpoint = Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosAccountEndpointSetting) ?? throw new InvalidDataException();
            string databaseId = Environment.GetEnvironmentVariable(CosmosConnectionConstants.DatabaseNameSetting) ?? throw new InvalidDataException();
            string containerId = Environment.GetEnvironmentVariable(CosmosConnectionConstants.ContainerNameSetting) ?? throw new InvalidDataException();

#if WINGET_REST_SOURCE_LEGACY_SUPPORT
            string readOnlyKey = Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosReadOnlyKeySetting) ?? throw new InvalidDataException();
            string readWriteKey = Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosReadWriteKeySetting) ?? throw new InvalidDataException();
#endif

            services.AddSingleton<IWinGetAppConfig>(_ => WinGetAppConfig.Instance);

#if WINGET_REST_SOURCE_LEGACY_SUPPORT
            services.AddSingleton<IApiDataStore, CosmosDataStore>(
                serviceProvider => new CosmosDataStore(
                    serviceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CosmosDataStore>>(),
                    endpoint,
                    databaseId,
                    containerId,
                    readOnlyKey,
                    readWriteKey));
#else
            services.AddSingleton<IApiDataStore, CosmosDataStore>(
                serviceProvider => new CosmosDataStore(
                    serviceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CosmosDataStore>>(),
                    endpoint,
                    databaseId,
                    containerId));
#endif

            services.AddSingleton<IRebuild>(_ => RebuildFactory.InitializeRebuildInstance());
            services.AddSingleton<IUpdate>(_ => UpdateFactory.InitializeUpdateInstance());
            services.AddSingleton<IRestSourceTriggerFunction>(_ => new RestSourceTriggerFunctions(
                ApiConstants.AzFuncRestSourceEndpoint));
        }
    }
}
