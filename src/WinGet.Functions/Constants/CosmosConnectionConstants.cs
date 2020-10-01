// -----------------------------------------------------------------------
// <copyright file="CosmosConnectionConstants.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Functions.Constants
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
    }
}