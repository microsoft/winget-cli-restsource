// -----------------------------------------------------------------------
// <copyright file="CosmosSearch.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.ExtendedSchemas
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Models.Schemas;

    /// <summary>
    /// CosmosSearchResponse.
    /// </summary>
    public class CosmosSearch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosSearch"/> class.
        /// CosmosSearch.
        /// </summary>
        public CosmosSearch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosSearch"/> class.
        /// </summary>
        /// <param name="packageManifest">Package manifest.</param>
        public CosmosSearch(PackageManifest packageManifest)
        {
            this.PackageIdentifier = packageManifest.PackageIdentifier;
            foreach (VersionExtended version in packageManifest.Versions)
            {
                this.Versions.Add(new SearchVersion { PackageVersion = version.PackageVersion, Channel = version.Channel });
            }
        }

        /// <summary>
        /// Gets or sets PackageIdentifier.
        /// </summary>
        public string PackageIdentifier { get; set; }

        /// <summary>
        /// Gets or sets PackageIdentifier.
        /// </summary>
        public List<SearchVersion> Versions { get; set; }

        /// <summary>
        /// Versions.
        /// </summary>
        public struct SearchVersion
        {
            /// <summary>
            /// Gets or sets package Version.
            /// </summary>
            public string PackageVersion { get; set; }

            /// <summary>
            /// Gets or sets channel.
            /// </summary>
            public string Channel { get; set; }
        }
    }
}
