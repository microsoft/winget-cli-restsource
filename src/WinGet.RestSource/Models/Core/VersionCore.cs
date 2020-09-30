// -----------------------------------------------------------------------
// <copyright file="VersionCore.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Core
{
    using Newtonsoft.Json;

    /// <summary>
    /// This is the core version model.
    /// </summary>
    public class VersionCore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionCore"/> class.
        /// </summary>
        public VersionCore()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionCore"/> class.
        /// </summary>
        /// <param name="versionCore">Version Core Model.</param>
        public VersionCore(VersionCore versionCore)
        {
            this.Version = versionCore.Version;
            this.Name = versionCore.Name;
            this.Publisher = versionCore.Publisher;
        }

        /// <summary>
        /// Gets or sets version.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets publisher.
        /// </summary>
        [JsonProperty("publisher")]
        public string Publisher { get; set; }
    }
}