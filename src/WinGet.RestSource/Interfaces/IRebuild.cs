// -----------------------------------------------------------------------
// <copyright file="IRebuild.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WindowsPackageManager.Rest.Models;

    /// <summary>
    /// Rebuild interface.
    /// </summary>
    public interface IRebuild
    {
        /// <summary>
        /// Processes a rebuild request.
        /// </summary>
        /// <param name="httpClient">The function's http client.</param>
        /// <param name="operationId">Operation id.</param>
        /// <param name="sasReference">SAS reference to SQLite file.</param>
        /// <param name="type">type of operation performed on the SQLite file.</param>
        /// <param name="restSourceTriggerFunction">IRestSourceTriggerFunction.</param>
        /// <param name="manifestCacheEndpoint">Manifest cache endpoint.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ProcessRebuildRequestAsync(
            HttpClient httpClient,
            string operationId,
            string sasReference,
            ReferenceType type,
            IRestSourceTriggerFunction restSourceTriggerFunction,
            string manifestCacheEndpoint,
            LoggingContext loggingContext);
    }
}
