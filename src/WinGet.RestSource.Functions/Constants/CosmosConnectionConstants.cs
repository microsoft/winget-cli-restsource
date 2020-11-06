// -----------------------------------------------------------------------
// <copyright file="CosmosConnectionConstants.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions.Constants
{
    /// <summary>
    /// This contains the constants for connecting to Cosmos.
    /// </summary>
    internal class CosmosConnectionConstants
    {
        /// <summary>
        /// Cosmos Database Name.
        /// </summary>
        public const string DatabaseName = "WinGet";

        /// <summary>
        /// Cosmos Collection Name.
        /// </summary>
        public const string CollectionName = "Manifests";

        /// <summary>
        /// This is the connection string setting for the Document DB.
        /// This is the environment variable set in settings for the cosmos db connection string.
        /// </summary>
        public const string ConnectionStringSetting = "CosmosDBConnection";

        /// <summary>
        /// This is the Cosmos Account Endpoint.
        /// </summary>
        public const string CosmosAccountEndpointSetting = "CosmosAccountEndpoint";

        /// <summary>
        /// This is the Cosmos Account Key.
        /// </summary>
        public const string CosmosAccountKeySetting = "CosmosAccountKey";
    }
}