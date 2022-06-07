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
        /// Cosmos Container Name.
        /// </summary>
        public const string ContainerNameSetting = "CosmosContainer";

        /// <summary>
        /// Cosmos Database Name setting.
        /// </summary>
        public const string DatabaseNameSetting = "CosmosDatabase";

        /// <summary>
        /// This is the Cosmos Account Endpoint setting.
        /// </summary>
        public const string CosmosAccountEndpointSetting = "CosmosAccountEndpoint";

        /// <summary>
        /// This is the Cosmos read-write key setting.
        /// </summary>
        public const string CosmosReadWriteKeySetting = "CosmosReadWriteKey";

        /// <summary>
        /// This is the Cosmos read-only key setting.
        /// </summary>
        public const string CosmosReadOnlyKeySetting = "CosmosReadOnlyKey";

        /// <summary>
        /// This is the maximum continuation token from cosmos db in kb.
        /// </summary>
        public const int ResponseContinuationTokenLimitInKb = 3;
    }
}
