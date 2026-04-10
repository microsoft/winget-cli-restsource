// -----------------------------------------------------------------------
// <copyright file="HttpClientExtensions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Extensions
{
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WindowsPackageManager.Rest.Utils;

    /// <summary>
    /// Extension methods for HttpClient.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// This function wraps downloading files using http client.
        /// </summary>
        /// <param name="httpClient">Http Client.</param>
        /// <param name="uri">uri to download.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <param name="downloadPath">Optional download path. If null or empty creates a temporary path.</param>
        /// <returns>Path where the file was saved.</returns>
        public static async Task<string> DownloadFileAsync(
            this HttpClient httpClient,
            string uri,
            LoggingContext loggingContext,
            string downloadPath = null)
        {
            string save = string.IsNullOrWhiteSpace(downloadPath) ?
                Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
                : downloadPath;

            using var stream = await httpClient.GetStreamWithRetryAsync(uri, nameof(DownloadFileAsync), loggingContext);
            using var fileStream = new FileStream(save, FileMode.CreateNew);
            await stream.CopyToAsync(fileStream);

            return save;
        }

        /// <summary>
        /// Calls HttpClient.GetStringAsync with retries..
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="requestUri">Request uri.</param>
        /// <param name="caller">Caller. Used for logging.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>Downloaded string.</returns>
        public static async Task<string> GetStringWithRetryAsync(
            this HttpClient httpClient,
            string requestUri,
            string caller,
            LoggingContext loggingContext)
        {
            return await RetryHelper.RunAndRetryWithExceptionAsync<string, HttpRequestException>(
                async () =>
                {
                    return await httpClient.GetStringAsync(requestUri);
                },
                caller,
                new HttpRequestException(),
                loggingContext);
        }

        /// <summary>
        /// Calls HttpClient.GetStreamAsync with retries..
        /// </summary>
        /// <param name="httpClient">HttpClient.</param>
        /// <param name="requestUri">Request uri.</param>
        /// <param name="caller">Caller. Used for logging.</param>
        /// <param name="loggingContext">Logging context.</param>
        /// <returns>Downloaded stream.</returns>
        public static async Task<Stream> GetStreamWithRetryAsync(
            this HttpClient httpClient,
            string requestUri,
            string caller,
            LoggingContext loggingContext)
        {
            return await RetryHelper.RunAndRetryWithExceptionAsync<Stream, HttpRequestException>(
                async () =>
                {
                    return await httpClient.GetStreamAsync(requestUri);
                },
                caller,
                new HttpRequestException(),
                loggingContext);
        }
    }
}
