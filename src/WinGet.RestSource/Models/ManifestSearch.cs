// -----------------------------------------------------------------------
// <copyright file="ManifestSearch.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Manifest Search Schema.
    /// </summary>
    public class ManifestSearch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearch"/> class.
        /// </summary>
        public ManifestSearch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestSearch"/> class.
        /// </summary>
        /// <param name="manifestSearch">Manifest Search.</param>
        public ManifestSearch(ManifestSearch manifestSearch)
        {
            this.ContinuationToken = manifestSearch.ContinuationToken;
            this.MaximumResults = manifestSearch.MaximumResults;
            this.FetchAllManifests = manifestSearch.FetchAllManifests;
        }

        /// <summary>
        /// Gets or sets continuation token.
        /// </summary>
        [JsonProperty("continuationToken")]
        public string ContinuationToken { get; set; }

        /// <summary>
        /// Gets or sets maximum results.
        /// </summary>
        [JsonProperty("maximumResults")]
        public int MaximumResults { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch all manifests.
        /// </summary>
        [JsonProperty("fetchAllManifests")]
        public bool FetchAllManifests { get; set; }
    }
}