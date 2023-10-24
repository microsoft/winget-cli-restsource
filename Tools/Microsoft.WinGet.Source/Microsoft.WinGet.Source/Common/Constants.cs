// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Common
{
    /// <summary>
    /// Configurable constants for this assembly.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// Base URL and authentication token supplied.
        /// </summary>
        public const string ManualSet = "ManualSet";

        /// <summary>
        /// An ID for an Azure subscription was supplied.
        /// </summary>
        public const string SubscriptionIdSet = "SubscriptionIdSet";

        /// <summary>
        /// A name for an Azure subscription was supplied.
        /// </summary>
        public const string SubscriptionNameSet = "SubscriptionNameSet";

        /// <summary>
        /// A path to a directory containing manifest files was given.
        /// </summary>
        public const string PathSet = "PathSet";

        /// <summary>
        /// This is the noun for an application manifest; changing it here
        /// will change the invocation for all of the related commands.
        /// </summary>
        public const string ManifestNoun = "WinGetManifest";

        /// <summary>
        /// This is the noun for a version of an application manifest; changing it
        /// here will change the invocation for all of the related commands.
        /// </summary>
        public const string ManifestVersionNoun = "WinGetManifestVersion";
    }
}
