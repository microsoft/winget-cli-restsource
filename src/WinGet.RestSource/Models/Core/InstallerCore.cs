// -----------------------------------------------------------------------
// <copyright file="InstallerCore.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Core
{
    using Newtonsoft.Json;

    /// <summary>
    /// This is the Core Installer model.
    /// </summary>
    public class InstallerCore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerCore"/> class.
        /// </summary>
        public InstallerCore()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerCore"/> class.
        /// </summary>
        /// <param name="installerCore">Installer Core.</param>
        public InstallerCore(InstallerCore installerCore)
        {
            this.Arch = installerCore.Arch;
            this.Url = installerCore.Url;
            this.Sha256 = installerCore.Sha256;
        }

        /// <summary>
        /// Gets or sets SHA256.
        /// </summary>
        [JsonProperty("sha256")]
        public string Sha256 { get; set; }

        /// <summary>
        /// Gets or sets installer architecture.
        /// </summary>
        [JsonProperty("arch")]
        public string Arch { get; set; }

        /// <summary>
        /// Gets or sets URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        // TODO: Finalize schema.
    }
}