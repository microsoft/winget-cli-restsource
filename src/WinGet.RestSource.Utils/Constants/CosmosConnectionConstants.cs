// -----------------------------------------------------------------------
// <copyright file="CosmosConnectionConstants.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Constants
{
    /// <summary>
    /// This contains the constants for connecting to Cosmos.
    /// </summary>
    public class CosmosConnectionConstants
    {
        /// <summary>
        /// Cosmos Collection Name.
        /// </summary>
        public const string CollectionNameSetting = "CosmosCollection";

        /// <summary>
        /// Cosmos Database Name setting.
        /// </summary>
        public const string DatabaseNameSetting = "CosmosDatabase";

        /// <summary>
        /// This is the Cosmos Account Endpoint setting.
        /// </summary>
        public const string CosmosAccountEndpointSetting = "CosmosAccountEndpoint";

        /// <summary>
        /// This is the Cosmos Account Key setting.
        /// </summary>
        public const string CosmosAccountKeySetting = "CosmosAccountKey";

        /// <summary>
        /// This is the maximum continuation token from cosmos db in kb.
        /// </summary>
        public const int ResponseContinuationTokenLimitInKb = 3;
    }
}
