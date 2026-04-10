// -----------------------------------------------------------------------
// <copyright file="IUpdate.cs" company="Microsoft Corporation">
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
    /// Update interface.
    /// </summary>
    public interface IUpdate
    {
        /// <summary>
        /// Process an update request.
        /// </summary>
        /// <param name="httpClient">Http client.</param>
        /// <param name="operationId">Operation id.</param>
        /// <param name="commit">Commit.</param>
        /// <param name="sasManifestUrl">SAS manifest url.</param>
        /// <param name="referenceType">Reference type.</param>
        /// <param name="restSourceTriggerFunction">RestSourceTriggerFunction.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ProcessUpdateRequestAsync(
            HttpClient httpClient,
            string operationId,
            string commit,
            string sasManifestUrl,
            ReferenceType referenceType,
            IRestSourceTriggerFunction restSourceTriggerFunction,
            string azFuncHostKey,
            LoggingContext loggingContext);
    }
}
