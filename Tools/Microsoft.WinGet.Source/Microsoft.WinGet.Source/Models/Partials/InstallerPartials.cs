// -----------------------------------------------------------------------
// <copyright file="InstallerPartials.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable

namespace Microsoft.WinGet.Source.Models.Installer
{
    using System;
    using System.Collections.Generic;
    using Microsoft.WinGet.Source.Models.CustomValidation;
    using YamlDotNet.Serialization;

    /// <summary>
    /// Partial class that implements cloning functionality to the PackageDependencies class.
    /// </summary>
    public partial class PackageDependencies : ICloneable
    {
        /// <summary>
        /// Creates a new PackageDependencies object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new PackageDependencies object that is a copy of the current instance.</returns>
        public object Clone()
        {
            return new PackageDependencies { MinimumVersion = this.MinimumVersion, PackageIdentifier = this.PackageIdentifier };
        }
    }

    /// <summary>
    /// Partial InstallerManifest class for defining a string type ReleaseDateTimeField.
    /// Workaround for issue with model generating ReleaseDate with DateTimeOffset type.
    /// </summary>
    public partial class InstallerManifest
    {
        /// <summary>
        /// Gets or sets the Release Date time.
        /// </summary>
        [YamlMember(Alias = "ReleaseDate")]
        [DateTimeValidation]
        public string ReleaseDateTime { get; set; }
    }

    /// <summary>
    /// Partial Installer class for defining a string type ReleaseDateTime field.
    /// Workaround for issue with model generating ReleaseDate with DateTimeOffset type.
    /// </summary>
    public partial class Installer
    {
        /// <summary>
        /// Gets or sets the Release Date time.
        /// </summary>
        [YamlMember(Alias = "ReleaseDate")]
        [DateTimeValidation]
        public string ReleaseDateTime { get; set; }

        /// <summary>
        /// Gets or sets the unique installer identifier.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("InstallerIdentifier", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Required]
        public string InstallerIdentifier { get; set; } = System.Guid.NewGuid().ToString();
    }

    /// <summary>
    /// This partial class is a workaround to a known issue.
    /// NSwag does not handle the "oneof" keyword of the Markets field, resulting in missing model classes.
    /// Because of this, the Markets and Markets array classes do not get generated, therefore we need to define them here
    /// under the default NSwag generated name "Markets2".
    /// </summary>
    public partial class Markets2
    {
        /// <summary>
        /// Gets or sets the list of allowed installer target markets.
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(256)]
        public List<string> AllowedMarkets { get; set; }

        /// <summary>
        /// Gets or sets the list of excluded installer target markets.
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(256)]
        public List<string> ExcludedMarkets { get; set; }
    }
}
