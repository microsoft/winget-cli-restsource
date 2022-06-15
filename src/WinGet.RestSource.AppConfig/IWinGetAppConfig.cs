// -----------------------------------------------------------------------
// <copyright file="IWinGetAppConfig.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.AppConfig
{
    using System.Threading.Tasks;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;

    /// <summary>
    /// WinGet App config Interface.
    /// </summary>
    public interface IWinGetAppConfig
    {
        /// <summary>
        /// Is Feature flag enabled.
        /// </summary>
        /// <param name="flag">Flag.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>True if feature is enabled.</returns>
        Task<bool> IsEnabledAsync(FeatureFlag flag, LoggingContext loggingContext);

        /// <summary>
        /// Is Feature Flag Enabled Async.
        /// </summary>
        /// <param name="flag">Flag.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>True if feature is enabled.</returns>
        bool IsEnabled(FeatureFlag flag, LoggingContext loggingContext);
    }
}
