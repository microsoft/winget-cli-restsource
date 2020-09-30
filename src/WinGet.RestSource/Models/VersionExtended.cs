// -----------------------------------------------------------------------
// <copyright file="VersionExtended.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// This extends the core version model by nesting the installers model.
    /// </summary>
    public class VersionExtended : VersionCore
    {
        /// <summary>
        /// Gets or sets the set of installers for a version.
        /// Setting order of 1 while keeping everything else default adds this to the end of the inherited class.
        /// </summary>
        [JsonProperty("installers", Order = 1)]
        public List<InstallerCore> Installers { get; set; }

        /// <summary>
        /// This returns the VersionCore representation of this object.
        /// </summary>
        /// <returns>Version Core Model.</returns>
        public VersionCore GetVersionCore()
        {
            return new VersionCore(this);
        }
    }
}