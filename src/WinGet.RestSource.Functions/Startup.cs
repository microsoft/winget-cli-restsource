// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
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
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.Functions.Constants;

    /// <summary>
    /// Azure function startup class.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ICosmosDatabase>((s) => this.CreateCosmosDatabase());
            builder.Services.AddSingleton<IApiDataStore, CosmosDataStore>();
        }

        /// <summary>
        /// This instantiates a Cosmos DB from the Endpoint and Account Key.
        /// </summary>
        /// <returns>Cosmos Database.</returns>
        private CosmosDatabase CreateCosmosDatabase()
        {
            Uri endpoint = new Uri(
                Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosAccountEndpointSetting) ??
                throw new InvalidDataException());
            CosmosDatabase database = new CosmosDatabase(
                endpoint,
                Environment.GetEnvironmentVariable(CosmosConnectionConstants.CosmosAccountKeySetting),
                Constants.CosmosConnectionConstants.DatabaseName,
                Constants.CosmosConnectionConstants.CollectionName);

            return database;
        }
    }
}
