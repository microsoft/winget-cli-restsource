// -----------------------------------------------------------------------
// <copyright file="BaseAzureTokenCommand.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Common
{
    using System.Management.Automation;
    using System.Net.Http;
    using Azure.ResourceManager.AppService.Models;

    /// <summary>
    /// This is the base class for all commands that both
    /// need to talk with Azure & need an authentication token
    /// to perform privileged actions in the REST API.
    /// </summary>
    public class BaseAzureTokenCommand : BaseAzureCommand
    {
        /// <summary>
        /// Gets or sets the authentication token for the REST API, if the user would
        /// rather provide one manually than have it resolved via Azure.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ParameterSetName = Constants.ManualSet,
            ValueFromPipelineByPropertyName = true)]
        public string Token { get; set; }

        /// <inheritdoc />
        protected override HttpClient GetHttpClient()
        {
            HttpClient client = base.GetHttpClient();
            client.DefaultRequestHeaders.Add("x-functions-key", this.GetToken());
            return client;
        }

        /// <summary>
        /// Retrieves the authentication token or host key by,
        ///     1. forwarding the token from a manually provided parameter, or by
        ///     2. querying the Azure function for a key.
        /// </summary>
        /// <returns>An authentication token.</returns>
        private string GetToken()
        {
            if (this.ParameterSetName == Constants.ManualSet)
            {
                return this.Token;
            }
            else
            {
                // A token was not supplied so we have to get it from Azure!
                HostKeys keys = this.WebSiteResource.GetHostKeys();
                keys.FunctionKeys.TryGetValue("default", out string hostKey);
                return hostKey ?? throw new RuntimeException(
                    Utilities.ResourceManager.GetString("RuntimeExceptionTokenNotFound"));
            }
        }
    }
}
