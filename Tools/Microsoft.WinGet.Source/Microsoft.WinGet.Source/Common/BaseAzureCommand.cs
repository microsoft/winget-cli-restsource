// -----------------------------------------------------------------------
// <copyright file="BaseAzureCommand.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Common
{
    using System;
    using System.Management.Automation;
    using System.Net.Http;
    using Azure.Identity;
    using Azure.ResourceManager;
    using Azure.ResourceManager.AppService;
    using Azure.ResourceManager.Resources;

    /// <summary>
    /// This is the base class for all commands that need to access Azure; namely,
    /// to obtain the base URL for the REST API & possibly an authentication token
    /// if the user wants to perform privileged actions.
    /// </summary>
    public class BaseAzureCommand : BaseSourceCommand
    {
        private WebSiteResource webSiteResource;

        /// <summary>
        /// Gets or sets the base URL for the REST API, if the user would rather
        /// supply one manually than have it automatically resolved through Azure.
        /// </summary>
        [Alias("BaseUrl")]
        [Parameter(
            Mandatory = false,
            ParameterSetName = Constants.ManualSet,
            ValueFromPipelineByPropertyName = true)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the ID for the Azure subscription that contains the
        /// Azure function REST source; this way, we can resolve the base URL
        /// automatically.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ParameterSetName = Constants.SubscriptionIdSet,
            ValueFromPipelineByPropertyName = true)]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Azure subscription that contains the
        /// Azure function REST source; this way, we can resolve the base URL
        /// automatically.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ParameterSetName = Constants.SubscriptionNameSet,
            ValueFromPipelineByPropertyName = true)]
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the Azure function REST source; this
        /// is used to resolve the base URL automatically, and possibly
        /// the authentication token.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ParameterSetName = Constants.SubscriptionIdSet,
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            Mandatory = true,
            ParameterSetName = Constants.SubscriptionNameSet,
            ValueFromPipelineByPropertyName = true)]
        public string FunctionName { get; set; }

        /// <summary>
        /// Gets the <see cref="WebSiteResource" /> that the Azure parameters are
        /// referring to.
        /// </summary>
        protected WebSiteResource WebSiteResource
        {
            get
            {
                if (this.webSiteResource is null)
                {
                    this.webSiteResource = this.GetWebSiteResource();
                }

                return this.webSiteResource;
            }
        }

        /// <summary>
        /// Creates the <see cref="ArmClient" /> used to communicate with Azure.
        /// </summary>
        /// <returns>An <see cref="ArmClient" /> object.</returns>
        protected virtual ArmClient GetArmClient()
        {
            DefaultAzureCredential credential = new DefaultAzureCredential(true);
            return new ArmClient(credential);
        }

        /// <summary>
        /// Retrieves the <see cref="SubscriptionResource" /> that contains the Azure function.
        /// </summary>
        /// <returns>A <see cref="SubscriptionResource" /> object.</returns>
        protected SubscriptionResource GetSubscriptionResource()
        {
            if (this.SubscriptionId != null)
            {
                SubscriptionResource resource = this.GetArmClient().GetSubscriptions().Get(this.SubscriptionId);
                if (resource != null)
                {
                    return resource;
                }
            }
            else if (this.SubscriptionName != null)
            {
                foreach (SubscriptionResource resource in this.GetArmClient().GetSubscriptions())
                {
                    if (resource.HasData && resource.Data.DisplayName.Equals(
                        this.SubscriptionName,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        // We've found a subscription with the desired name!
                        return resource;
                    }
                }
            }

            // We couldn't find the subscription with the given ID or name!
            throw new ArgumentException(Utilities.ResourceManager.GetString("ArgumentExceptionSubscriptionNotFound"));
        }

        /// <summary>
        /// Retrieves the <see cref="HttpClient" /> that will be used to
        /// communicate with the REST API; sets the base URL internally.
        /// </summary>
        /// <returns>A <see cref="HttpClient" /> object.</returns>
        protected virtual HttpClient GetHttpClient()
        {
            return new HttpClient()
            {
            };
        }

        /// <summary>
        /// Retrieves the URL for the REST API by,
        ///     1. forwarding the manually provided URL, or by
        ///     2. resolving the function through the Azure API.
        /// </summary>
        /// <returns>The URL for the REST API.</returns>
        /// <exception cref="RuntimeException">An error occurred while talking with Azure.</exception>
        protected string GetApiUrl()
        {
            if (this.ParameterSetName == Constants.ManualSet)
            {
                return this.Url;
            }
            else
            {
                return this.WebSiteResource.HasData
                    ? $"https://{this.WebSiteResource.Data.DefaultHostName}/api"
                    : throw new RuntimeException(
                        Utilities.ResourceManager.GetString("RuntimeExceptionWebSiteHasDataFalse"));
            }
        }

        /// <summary>
        /// Finds an Azure function with the given name & returns it.
        /// </summary>
        /// <returns>A <see cref="WebSiteResource" /> object.</returns>
        /// <exception cref="ArgumentException">There was no Azure function with the given name.</exception>
        private WebSiteResource GetWebSiteResource()
        {
            if (this.FunctionName != null)
            {
                foreach (WebSiteResource resource in this.GetSubscriptionResource().GetWebSites())
                {
                    if (resource.HasData && resource.Data.Name.Equals(
                        this.FunctionName,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        // We've found a function with the desired name!
                        return resource;
                    }
                }
            }

            // We didn't find a function with the given name; it probably doesn't exist!
            throw new ArgumentException(string.Format(
                Utilities.ResourceManager.GetString("ArgumentExceptionFunctionNotFound"),
                this.FunctionName));
        }
    }
}
