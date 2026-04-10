// -----------------------------------------------------------------------
// <copyright file="IRestSourceTriggerFunction.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Interfaces
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;

    /// <summary>
    /// Provides an interface to wrap azure function manually triggered calls.
    /// </summary>
    public interface IRestSourceTriggerFunction
    {
        /// <summary>
        /// Performs PackageManifest Get request.
        /// </summary>
        /// <param name="httpClient">Http Client.</param>
        /// <param name="packageIndetifier">Package indetifier.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<PackageManifest> GetPackageManifestAsync(
            HttpClient httpClient,
            string packageIndetifier,
            string azFuncHostKey,
            LoggingContext loggingContext);

        /// <summary>
        /// Performs PackageManifest Post request.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="packageManifest">Package Manifest.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task PostPackageManifestAsync(
            HttpClient httpClient,
            PackageManifest packageManifest,
            string azFuncHostKey,
            LoggingContext loggingContext);

        /// <summary>
        /// Performs PackageManifest Put request.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="packageManifest">Package Manifest.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task PutPackageManifestAsync(
            HttpClient httpClient,
            PackageManifest packageManifest,
            string azFuncHostKey,
            LoggingContext loggingContext);

        /// <summary>
        /// Performs PackageManifest Delete request.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeletePackageManifestAsync(
            HttpClient httpClient,
            string packageIdentifier,
            string azFuncHostKey,
            LoggingContext loggingContext);

        /// <summary>
        /// Get all packages.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>List of packages.</returns>
        Task<IReadOnlyList<Package>> GetAllPackagesAsync(
            HttpClient httpClient,
            string azFuncHostKey,
            LoggingContext loggingContext);

        /// <summary>
        /// Gets paged packages.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">LoggingContext.</param>
        /// <param name="continuationToken">Optional continuation token.</param>
        /// <returns>List of packages.</returns>
        Task<ApiResponse<List<Package>>> GetPackagesAsync(
            HttpClient httpClient,
            string azFuncHostKey,
            LoggingContext loggingContext,
            string continuationToken = null);

        /// <summary>
        /// Deletes a package.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="packageIdentifier">Package indentifier.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeletePackageAsync(
            HttpClient httpClient,
            string packageIdentifier,
            string azFuncHostKey,
            LoggingContext loggingContext);

        /// <summary>
        /// Delete a version.
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="packageIdentifier">Package identifier.</param>
        /// <param name="version">Version.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        /// <param name="loggingContext">LoggingContext.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeleteVersionAsync(
            HttpClient httpClient,
            string packageIdentifier,
            string version,
            string azFuncHostKey,
            LoggingContext loggingContext);
    }
}
