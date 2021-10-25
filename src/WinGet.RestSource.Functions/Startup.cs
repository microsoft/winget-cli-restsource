// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

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
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;

    /// <summary>
    /// Azure function startup class.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string endpoint = Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosAccountEndpointSetting) ?? throw new InvalidDataException();
            string readOnlyKey = Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosReadWriteKeySetting) ?? throw new InvalidDataException();
            string readWriteKey = Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosReadWriteKeySetting) ?? throw new InvalidDataException();
            string databaseId = Environment.GetEnvironmentVariable(CosmosConnectionConstants.DatabaseNameSetting) ?? throw new InvalidDataException();
            string containerId = Environment.GetEnvironmentVariable(CosmosConnectionConstants.ContainerNameSetting) ?? throw new InvalidDataException();

            builder.Services.AddSingleton<IApiDataStore, CosmosDataStore>(sp => new CosmosDataStore(sp.GetRequiredService<ILogger<CosmosDataStore>>(), endpoint, readWriteKey, readOnlyKey, databaseId, containerId));
        }
    }
}
