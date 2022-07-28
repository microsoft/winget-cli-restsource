// -----------------------------------------------------------------------
// <copyright file="GetManifestCommand.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Commands
{
    using System.Collections.Generic;
    using System.IO;
    using System.Management.Automation;
    using System.Net.Http;
    using Microsoft.WinGet.Source.Common;
    using Microsoft.WinGet.Source.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// This command retrieves all version manifests of an application.
    /// </summary>
    [Cmdlet(
        VerbsCommon.Get,
        Constants.ManifestNoun,
        DefaultParameterSetName = Constants.SubscriptionNameSet)]
    [OutputType(typeof(Manifests))]
    public class GetManifestCommand : BaseAzureCommand
    {
        private string path;

        /// <summary>
        /// Gets or sets the ID of the package to retrieve the manifests for.
        /// </summary>
        [Alias("PackageId", "PackageIdentifier")]
        [Parameter(
            Mandatory = true,
            ParameterSetName = Constants.ManualSet,
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            Mandatory = true,
            ParameterSetName = Constants.SubscriptionIdSet,
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            Mandatory = true,
            ParameterSetName = Constants.SubscriptionNameSet,
            ValueFromPipelineByPropertyName = true)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a path to a directory containing manifest files; if
        /// the user desires to simply retrieve a manifests object from the
        /// filesystem, perhaps to be sent to a REST source via another command.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ParameterSetName = Constants.PathSet,
            ValueFromPipelineByPropertyName = true)]
        public string Path
        {
            get
            {
                if (this.path is null)
                {
                    this.path = this.SessionState.Path.CurrentFileSystemLocation.Path;
                }

                return this.path;
            }

            set
            {
                this.path = System.IO.Path.IsPathRooted(value)
                    ? value
                    : System.IO.Path.Combine(this.SessionState.Path.CurrentFileSystemLocation.Path, value);
            }
        }

        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if (this.ParameterSetName == Constants.PathSet)
            {
                // We need to deserialize all of the manifest files in the given directory.
                this.WriteObject(Serialization.DeserializeFromManifestsDirectory(this.Path));
            }
            else
            {
                foreach (var item in this.MakeRequest())
                {
                    // We can output multiple objects since an application might
                    // have more than one version.
                    this.WriteObject(item);
                }
            }
        }

        private IEnumerable<Manifests> MakeRequest()
        {
            string url = this.GetApiUrl() + $"/packageManifests/{this.Id}";
            HttpResponseMessage message = this.GetHttpClient().GetAsync(url).Result;
            message.EnsureSuccessStatusCode();

            // The data we want is under a property in the response we get back.
            ManifestsPayload payload = JObject
                .Parse(message.Content.ReadAsStringAsync().Result)
                .Property("Data")
                .Value
                .ToObject<ManifestsPayload>();

            foreach (ManifestsPayloadVersionItem version in payload.Versions)
            {
                yield return RestConverter.CreateManifestsFromVersionItem(
                    version,
                    payload.PackageIdentifier);
            }
        }
    }
}
