namespace Microsoft.WinGet.RestSource.Models
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// This is a manifest, which is an extension of the package core model, and the extended version model.
    /// </summary>
    public class Manifest : PackageCore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Manifest"/> class.
        /// </summary>
        public Manifest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Manifest"/> class.
        /// </summary>
        /// <param name="package">Package Core.</param>
        public Manifest(PackageCore package)
        {
            this.DefaultLocale = package.DefaultLocale;
            this.Id = package.Id;
            this.Versions = null;
        }

        /// <summary>
        /// Gets or sets versions.
        /// Setting order of 1 while keeping everything else default adds this to the end of the inherited class.
        /// </summary>
        [JsonProperty("versions", Order = 1)]
        public List<VersionExtended> Versions { get; set; }
    }
}