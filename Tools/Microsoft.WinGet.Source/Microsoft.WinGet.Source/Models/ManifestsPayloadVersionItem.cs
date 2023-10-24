// -----------------------------------------------------------------------
// <copyright file="ManifestsPayloadVersionItem.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Models
{
    /// <summary>
    /// The REST Source expects a <see cref="Manifests" /> object to be in a specific format.
    /// </summary>
    public class ManifestsPayloadVersionItem
    {
        /// <summary>
        /// Gets or sets the package version string.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("PackageVersion", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Required]
        public string PackageVersion { get; set; }

        /// <summary>
        /// Gets or sets the default locale manifest.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("DefaultLocale", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Required]
        public DefaultLocale.DefaultLocaleManifest DefaultLocale { get; set; }

        /// <summary>
        /// Gets or sets the list of installers.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("Installers", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.List<Installer.Installer> Installers { get; set; }

        /// <summary>
        /// Gets or sets the list of locale manifests.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("Locales", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.List<Locale.LocaleManifest> Locales { get; set; }
    }
}
