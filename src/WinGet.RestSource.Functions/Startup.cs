// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.WinGet.RestSource.Functions.Constants;

[assembly:
    Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup(
        typeof(Microsoft.WinGet.RestSource.Functions.Startup))]

namespace Microsoft.WinGet.RestSource.Functions
{
    using System;
    using System.IO;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.AppConfig;
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.Factories;
    using Microsoft.WinGet.RestSource.Helpers;
    using Microsoft.WinGet.RestSource.Interfaces;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using YamlDotNet.Core.Tokens;

    /// <summary>
    /// Azure function startup class.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            string endpoint = Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosAccountEndpointSetting) ?? throw new InvalidDataException();
            string databaseId = Environment.GetEnvironmentVariable(CosmosConnectionConstants.DatabaseNameSetting) ?? throw new InvalidDataException();
            string containerId = Environment.GetEnvironmentVariable(CosmosConnectionConstants.ContainerNameSetting) ?? throw new InvalidDataException();

#if WINGET_REST_SOURCE_LEGACY_SUPPORT
            string readOnlyKey = Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosReadOnlyKeySetting) ?? throw new InvalidDataException();
            string readWriteKey = Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosReadWriteKeySetting) ?? throw new InvalidDataException();
#endif

            builder.Services.AddSingleton<IWinGetAppConfig>(sp => WinGetAppConfig.Instance);

#if WINGET_REST_SOURCE_LEGACY_SUPPORT
            builder.Services.AddSingleton<IApiDataStore, CosmosDataStore>(
                sp => new CosmosDataStore(
                    sp.GetRequiredService<ILogger<CosmosDataStore>>(),
                    endpoint,
                    databaseId,
                    containerId,
                    readOnlyKey,
                    readWriteKey));
#else
            builder.Services.AddSingleton<IApiDataStore, CosmosDataStore>(
                sp => new CosmosDataStore(
                    sp.GetRequiredService<ILogger<CosmosDataStore>>(),
                    endpoint,
                    databaseId,
                    containerId));
#endif

            builder.Services.AddSingleton<IRebuild>((s) => RebuildFactory.InitializeRebuildInstance());
            builder.Services.AddSingleton<IUpdate>((s) => UpdateFactory.InitializeUpdateInstance());
            builder.Services.AddSingleton<IRestSourceTriggerFunction>((s) => new RestSourceTriggerFunctions(
                ApiConstants.AzFuncRestSourceEndpoint));

            InjectTelemetryConfiguration(builder);
        }

        private static void InjectTelemetryConfiguration(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<TelemetryConfiguration>(sp =>
            {
                var key = AzureFunctionEnvironment.AppInsightsInstrumentationKey;
                var telemetryConfiguration = new TelemetryConfiguration();

                if (!string.IsNullOrWhiteSpace(key))
                {
                    telemetryConfiguration.ConnectionString = $"InstrumentationKey={key}";
                }

                return telemetryConfiguration;
            });
        }
    }
}
