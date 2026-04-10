// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Functions.Geneva
{
    /// <summary>
    /// Geneva NameSpaces.
    /// </summary>
    public enum MetricNamespaces
    {
        /// <summary>
        /// Geneva Metrics namespace.
        /// </summary>
        MsPkgMgrProdLogs,
    }

    /// <summary>
    /// Geneva Metrics.
    /// </summary>
    public enum ErrorMetrics
    {
        /// <summary>
        /// Unhandled exception updating the DB.
        /// </summary>
        DatabaseUpdateError,

        /// <summary>
        /// Unhandled exception getting data from the DB.
        /// </summary>
        DatabaseGetError,

        /// <summary>
        /// Unhandled exception retrieving server information.
        /// </summary>
        ServerInformationError,

        /// <summary>
        /// Error handling rebuild request.
        /// </summary>
        SourceRebuildError,

        /// <summary>
        /// Error handling update request.
        /// </summary>
        SourceUpdateError,
    }
}