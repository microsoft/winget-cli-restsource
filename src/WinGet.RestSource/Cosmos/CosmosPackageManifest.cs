// -----------------------------------------------------------------------
// <copyright file="CosmosPackageManifest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas
{
    using Microsoft.WinGet.RestSource.Cosmos;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Newtonsoft.Json;

    /// <summary>
    /// This is a manifest, which is an extension of the package core model, and the extended version model.
    /// </summary>
    public class CosmosPackageManifest : PackageManifest, ICosmosIdDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosPackageManifest"/> class.
        /// </summary>
        public CosmosPackageManifest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosPackageManifest"/> class.
        /// </summary>
        /// <param name="cosmosPackageManifest">manifest.</param>
        public CosmosPackageManifest(CosmosPackageManifest cosmosPackageManifest)
            : base(cosmosPackageManifest)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosPackageManifest"/> class.
        /// </summary>
        /// <param name="packageManifest">manifest.</param>
        public CosmosPackageManifest(PackageManifest packageManifest)
            : base(packageManifest)
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
        public PackageManifest ToManifest()
        {
            PackageManifest packageManifest = new PackageManifest(this);
            return packageManifest;
        }
    }
}
