// -----------------------------------------------------------------------
// <copyright file="RestSourceTriggerFunctions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Helpers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Msix.Utils.Logger;
    using Microsoft.WindowsPackageManager.Rest.Diagnostics;
    using Microsoft.WindowsPackageManager.Rest.Utils;
    using Microsoft.WinGet.RestSource.Exceptions;
    using Microsoft.WinGet.RestSource.Interfaces;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Models;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Newtonsoft.Json;

    /// <summary>
    /// Trigger azure functions for rest source.
    /// Currently it supports
    ///     GET, PUT, POST and DELETE calls to package manifests endpoint.
    ///     GET, DELETE packages endpoint.
    ///     DELETE versions endpoint.
    /// </summary>
    public class RestSourceTriggerFunctions : IRestSourceTriggerFunction
    {
        private const string PackageManifests = "packageManifests";
        private const string Packages = "packages";
        private const string Versions = "versions";

        private readonly string azFuncPackageManifestsEndpoint;
        private readonly string azFuncPackageManifestsEndpointFormat;

        private readonly string azFuncPackagesEndpoint;
        private readonly string azFuncPackagesEndpointFormat;

        private readonly string azFuncVersionsEndpointFormat;
        private readonly string azFuncVersionsSpecificEndpointFormat;
        private readonly string azFuncHostKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestSourceTriggerFunctions"/> class.
        /// </summary>
        /// <param name="azFuncRestSourceEndpoint">Azure function Rest Source Endpoint.</param>
        /// <param name="azFuncHostKey">Azure function host key.</param>
        public RestSourceTriggerFunctions(
            string azFuncRestSourceEndpoint,
            string azFuncHostKey)
        {
            this.azFuncPackageManifestsEndpoint = $"{azFuncRestSourceEndpoint}{PackageManifests}";
            this.azFuncPackageManifestsEndpointFormat = this.azFuncPackageManifestsEndpoint + "/{0}";

            this.azFuncPackagesEndpoint = $"{azFuncRestSourceEndpoint}{Packages}";
            this.azFuncPackagesEndpointFormat = this.azFuncPackagesEndpoint + "/{0}";

            this.azFuncVersionsEndpointFormat = this.azFuncPackagesEndpointFormat + $"/{Versions}";
            this.azFuncVersionsSpecificEndpointFormat = this.azFuncVersionsEndpointFormat + "/{1}";

            this.azFuncHostKey = azFuncHostKey;
        }

        /// <inheritdoc />
        public async Task<PackageManifest> GetPackageManifestAsync(
            HttpClient httpClient,
            string packageIndetifier,
            LoggingContext loggingContext)
        {
            Logger.Info($"{loggingContext} Start Get PackageManifest {packageIndetifier}");

            var endpoint = string.Format(this.azFuncPackageManifestsEndpointFormat, packageIndetifier);
            var httpMethod = HttpMethod.Get;
            HttpResponseMessage responseMessage = await AzureFunctionHelper.TriggerAzureFunctionAsync(
                httpClient,
                httpMethod,
                endpoint,
                this.azFuncHostKey);

            Logger.Info($"{loggingContext} Get PackageManifest {packageIndetifier} response StatusCode '{responseMessage.StatusCode}'");

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RestSourceCallException(endpoint, httpMethod, responseMessage.StatusCode);
            }

            if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ApiResponse<PackageManifest>>(
                await responseMessage.Content.ReadAsStringAsync()).Data;
        }

        /// <inheritdoc />
        public async Task PostPackageManifestAsync(
            HttpClient httpClient,
            PackageManifest packageManifest,
            LoggingContext loggingContext)
        {
            string requestBody = JsonConvert.SerializeObject(packageManifest);
            Logger.Info($"{loggingContext} Start Post PackageManifest {packageManifest.PackageIdentifier} Body {requestBody}");

            string endpoint = this.azFuncPackageManifestsEndpoint;
            var httpMethod = HttpMethod.Post;
            HttpResponseMessage responseMessage = await AzureFunctionHelper.TriggerAzureFunctionAsync(
                httpClient,
                httpMethod,
                this.azFuncPackageManifestsEndpoint,
                this.azFuncHostKey,
                requestBody);

            Logger.Info($"{loggingContext} Post PackageManifest response {packageManifest.PackageIdentifier} StatusCode '{responseMessage.StatusCode}'");

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RestSourceCallException(endpoint, httpMethod, responseMessage.StatusCode);
            }
        }

        /// <inheritdoc />
        public async Task PutPackageManifestAsync(
            HttpClient httpClient,
            PackageManifest packageManifest,
            LoggingContext loggingContext)
        {
            string requestBody = JsonConvert.SerializeObject(packageManifest);
            Logger.Info($"{loggingContext} Start Put PackageManifest {packageManifest.PackageIdentifier} Body {requestBody}");

            string endpoint = string.Format(this.azFuncPackageManifestsEndpointFormat, packageManifest.PackageIdentifier);
            var httpMethod = HttpMethod.Put;
            HttpResponseMessage responseMessage = await AzureFunctionHelper.TriggerAzureFunctionAsync(
                httpClient,
                httpMethod,
                endpoint,
                this.azFuncHostKey,
                requestBody);

            Logger.Info($"{loggingContext} Put PackageManifest {packageManifest.PackageIdentifier} response StatusCode '{responseMessage.StatusCode}'");

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RestSourceCallException(endpoint, httpMethod, responseMessage.StatusCode);
            }
        }

        /// <inheritdoc />
        public async Task DeletePackageManifestAsync(
            HttpClient httpClient,
            string packageIdentifier,
            LoggingContext loggingContext)
        {
            Logger.Info($"{loggingContext} Start Delete PackageManifest {packageIdentifier}");

            string endpoint = string.Format(this.azFuncPackageManifestsEndpointFormat, packageIdentifier);
            var httpMethod = HttpMethod.Delete;
            HttpResponseMessage responseMessage = await AzureFunctionHelper.TriggerAzureFunctionAsync(
                httpClient,
                httpMethod,
                endpoint,
                this.azFuncHostKey);

            Logger.Info($"{loggingContext} Delete PackageManifest {packageIdentifier} response StatusCode '{responseMessage.StatusCode}'");

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RestSourceCallException(endpoint, httpMethod, responseMessage.StatusCode);
            }
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<Package>> GetAllPackagesAsync(
            HttpClient httpClient,
            LoggingContext loggingContext)
        {
            Logger.Info($"{loggingContext} Start Get All Packages");

            var packages = new List<Package>();

            string continuationToken = null;
            do
            {
                var pagedPackages = await this.GetPackagesAsync(
                    httpClient,
                    loggingContext,
                    continuationToken);

                // This can happen with a brand new source.
                if (pagedPackages == null)
                {
                    break;
                }

                packages.AddRange(pagedPackages.Data);
                continuationToken = pagedPackages.ContinuationToken;
            }
            while (continuationToken != null);

            return packages;
        }

        /// <inheritdoc />
        public async Task<ApiResponse<List<Package>>> GetPackagesAsync(
            HttpClient httpClient,
            LoggingContext loggingContext,
            string continuationToken = null)
        {
            Logger.Info($"{loggingContext} Start Get Packages ContinuationToken {continuationToken}");

            Dictionary<string, string> headers = null;
            if (!string.IsNullOrEmpty(continuationToken))
            {
                headers = new Dictionary<string, string>()
                {
                    { HeaderConstants.ContinuationToken, continuationToken },
                };
            }

            string endpoint = this.azFuncPackagesEndpoint;
            var httpMethod = HttpMethod.Get;
            HttpResponseMessage responseMessage = await AzureFunctionHelper.TriggerAzureFunctionAsync(
                httpClient,
                httpMethod,
                endpoint,
                this.azFuncHostKey,
                headers: headers);

            Logger.Info($"{loggingContext} Get Package response ContinuationToken {continuationToken} StatusCode '{responseMessage.StatusCode}'");

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RestSourceCallException(endpoint, httpMethod, responseMessage.StatusCode);
            }

            if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ApiResponse<List<Package>>>(
                await responseMessage.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task DeletePackageAsync(
            HttpClient httpClient,
            string packageIdentifier,
            LoggingContext loggingContext)
        {
            Logger.Info($"{loggingContext} Start Delete Package {packageIdentifier}");

            string endpoint = string.Format(this.azFuncPackagesEndpointFormat, packageIdentifier);
            var httpMethod = HttpMethod.Delete;
            HttpResponseMessage responseMessage = await AzureFunctionHelper.TriggerAzureFunctionAsync(
                httpClient,
                httpMethod,
                endpoint,
                this.azFuncHostKey);

            Logger.Info($"{loggingContext} Delete Package {packageIdentifier} response StatusCode '{responseMessage.StatusCode}'");

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RestSourceCallException(endpoint, httpMethod, responseMessage.StatusCode);
            }
        }

        /// <inheritdoc />
        public async Task DeleteVersionAsync(
            HttpClient httpClient,
            string packageIdentifier,
            string version,
            LoggingContext loggingContext)
        {
            Logger.Info($"{loggingContext} Start Delete Version {packageIdentifier} {version}");

            string endpoint = string.Format(this.azFuncVersionsSpecificEndpointFormat, packageIdentifier, version);
            var httpMethod = HttpMethod.Delete;
            HttpResponseMessage responseMessage = await AzureFunctionHelper.TriggerAzureFunctionAsync(
                httpClient,
                httpMethod,
                endpoint,
                this.azFuncHostKey);

            Logger.Info($"{loggingContext} Delete Version response {packageIdentifier} {version} response StatusCode '{responseMessage.StatusCode}'");

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RestSourceCallException(endpoint, httpMethod, responseMessage.StatusCode);
            }
        }
    }
}
