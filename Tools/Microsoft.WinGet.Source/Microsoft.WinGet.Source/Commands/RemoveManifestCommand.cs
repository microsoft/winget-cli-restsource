// -----------------------------------------------------------------------
// <copyright file="RemoveManifestCommand.cs" company="Microsoft Corporation">
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
    /// This command removes all versions of a given manifest from
    ///  a REST source.
    /// </summary>
    [Cmdlet(
        VerbsCommon.Remove,
        Constants.ManifestNoun,
        DefaultParameterSetName = Constants.SubscriptionNameSet)]
    public class RemoveManifestCommand : BaseAzureTokenCommand
    {
        /// <summary>
        /// Gets or sets the ID of the package to retrieve the manifests for.
        /// </summary>
        [Alias("PackageId", "PackageIdentifier")]
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string Id { get; set; }

        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            this.MakeRequest();
        }

        private void MakeRequest()
        {
            string url = this.GetApiUrl() + $"/packageManifests/{this.Id}";
            HttpResponseMessage message = this.GetHttpClient().DeleteAsync(url).Result;
            message.EnsureSuccessStatusCode();
        }
    }
}
