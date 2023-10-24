// -----------------------------------------------------------------------
// <copyright file="GetManifestVersionCommand.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Commands
{
    using System.Management.Automation;
    using System.Net.Http;
    using Microsoft.WinGet.Source.Common;
    using Microsoft.WinGet.Source.Models;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// This command retrieves one version manifests of an application.
    /// </summary>
    [Cmdlet(
        VerbsCommon.Get,
        Constants.ManifestVersionNoun,
        DefaultParameterSetName = Constants.SubscriptionNameSet)]
    [OutputType(typeof(Manifests))]
    public class GetManifestVersionCommand : BaseAzureCommand
    {
        /// <summary>
        /// Gets or sets the ID of the package to retrieve the manifests for.
        /// </summary>
        [Alias("PackageId", "PackageIdentifier")]
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the version of the package to retrieve the manifests for.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string Version { get; set; }

        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            this.WriteObject(this.MakeRequest());
        }

        private Manifests MakeRequest()
        {
            string url = this.GetApiUrl() + $"/packages/{this.Id}/versions/{this.Version}";
            HttpResponseMessage message = this.GetHttpClient().GetAsync(url).Result;
            message.EnsureSuccessStatusCode();

            // The data we want is under a property in the response we get back.
            ManifestsPayloadVersionItem payload = JObject
                .Parse(message.Content.ReadAsStringAsync().Result)
                .Property("Data")
                .Value
                .ToObject<ManifestsPayloadVersionItem>();

            return RestConverter.CreateManifestsFromVersionItem(
                payload,
                this.Id);
        }
    }
}
