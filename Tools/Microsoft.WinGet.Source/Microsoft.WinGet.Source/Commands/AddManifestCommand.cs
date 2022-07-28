// -----------------------------------------------------------------------
// <copyright file="AddManifestCommand.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Commands
{
    using System.Management.Automation;
    using System.Net.Http;
    using Microsoft.WinGet.Source.Common;

    /// <summary>
    /// This command adds a manifest to a REST source.
    /// </summary>
    [Cmdlet(
        VerbsCommon.Add,
        Constants.ManifestNoun,
        DefaultParameterSetName = Constants.ManifestNoun)]
    public class AddManifestCommand : BaseManifestsCommand
    {
        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            this.MakeRequest();
        }

        private void MakeRequest()
        {
            string url = this.GetApiUrl() + $"/packageManifests/";
            HttpResponseMessage message = this.GetHttpClient().PostAsync(url, this.MakePayload()).Result;
            message.EnsureSuccessStatusCode();
        }

        private HttpContent MakePayload()
        {
            return new StringContent(
                RestConverter.CreateManifestsPayload(this.GetManifests()).ToJson(),
                System.Text.Encoding.UTF8,
                "application/json");
        }
    }
}