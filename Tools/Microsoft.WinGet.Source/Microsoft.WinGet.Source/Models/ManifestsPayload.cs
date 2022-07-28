// -----------------------------------------------------------------------
// <copyright file="ManifestsPayload.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Models
{
    /// <summary>
    /// The REST Source expects a <see cref="Manifests" /> object to be in a specific format.
    /// </summary>
    public class ManifestsPayload
    {
        /// <summary>
        /// Gets or sets the package identifier.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("PackageIdentifier", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Required]
        public string PackageIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the list of version items.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("Versions", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.List<ManifestsPayloadVersionItem> Versions { get; set; }
    }
}
