// -----------------------------------------------------------------------
// <copyright file="RemoveManifestVersionCommand.cs" company="Microsoft Corporation">
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
        VerbsCommon.Remove,
        Constants.ManifestVersionNoun,
        DefaultParameterSetName = Constants.SubscriptionNameSet)]
    public class RemoveManifestVersionCommand : BaseAzureTokenCommand
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
            this.MakeRequest();
        }

        private void MakeRequest()
        {
            string url = this.GetApiUrl() + $"/packages/{this.Id}/versions/{this.Version}";
            HttpResponseMessage message = this.GetHttpClient().DeleteAsync(url).Result;
            message.EnsureSuccessStatusCode();
        }
    }
}
