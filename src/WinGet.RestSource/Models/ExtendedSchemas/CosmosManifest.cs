// -----------------------------------------------------------------------
// <copyright file="CosmosManifest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.ExtendedSchemas
{
    using Microsoft.WinGet.RestSource.Models.Schemas;
    using Newtonsoft.Json;

    /// <summary>
    /// This is a manifest, which is an extension of the package core model, and the extended version model.
    /// </summary>
    public class CosmosManifest : Manifest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosManifest"/> class.
        /// </summary>
        public CosmosManifest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosManifest"/> class.
        /// </summary>
        /// <param name="cosmosManifest">manifest.</param>
        public CosmosManifest(CosmosManifest cosmosManifest)
            : base(cosmosManifest)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosManifest"/> class.
        /// </summary>
        /// <param name="manifest">manifest.</param>
        public CosmosManifest(Manifest manifest)
            : base(manifest)
        {
        }

        /// <summary>
        /// Gets ID.
        /// </summary>
        [JsonProperty("id", Order = 1)]
        public string Id => this.PackageIdentifier.ToString();

        /// <summary>
        /// Converts to a Package Core.
        /// </summary>
        /// <returns>Package Core.</returns>
        public Manifest ToManifest()
        {
            Manifest manifest = new Manifest(this);
            return manifest;
        }
    }
}
