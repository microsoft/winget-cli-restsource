// -----------------------------------------------------------------------
// <copyright file="Manifests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Models
{
    using System.Collections.Generic;
    using Microsoft.WinGet.Source.Models.DefaultLocale;
    using Microsoft.WinGet.Source.Models.Installer;
    using Microsoft.WinGet.Source.Models.Locale;
    using Microsoft.WinGet.Source.Models.Singleton;
    using Microsoft.WinGet.Source.Models.Version;

    /// <summary>
    /// This class wraps all the different kinds of manifests together into one place.
    /// </summary>
    public class Manifests
    {
        /// <summary>
        /// Gets or sets the default locale manifest.
        /// </summary>
        public DefaultLocaleManifest DefaultLocaleManifest { get; set; }

        /// <summary>
        /// Gets or sets the installer manifest.
        /// </summary>
        public InstallerManifest InstallerManifest { get; set; }

        /// <summary>
        /// Gets or sets the list of locale manifests.
        /// </summary>
        public List<LocaleManifest> LocaleManifests { get; set; } = new List<LocaleManifest>();

        /// <summary>
        /// Gets or sets the singleton manifest.
        /// </summary>
        public SingletonManifest SingletonManifest { get; set; }

        /// <summary>
        /// Gets or sets the version manifest.
        /// </summary>
        public VersionManifest VersionManifest { get; set; }

        /// <summary>
        /// Generates the proper file name for the given manifest.
        /// </summary>
        /// <typeparam name="T">Manifest type.</typeparam>
        /// <param name="manifest">Manifest object model.</param>
        /// <returns>File name string of manifest.</returns>
        public static string GetFileName<T>(T manifest)
        {
            return manifest switch
            {
                InstallerManifest installerManifest => $"{installerManifest.PackageIdentifier}.installer.yaml",
                VersionManifest versionManifest => $"{versionManifest.PackageIdentifier}.yaml",
                DefaultLocaleManifest defaultLocaleManifest => $"{defaultLocaleManifest.PackageIdentifier}.locale.{defaultLocaleManifest.PackageLocale}.yaml",
                LocaleManifest localeManifest => $"{localeManifest.PackageIdentifier}.locale.{localeManifest.PackageLocale}.yaml",
                SingletonManifest singletonManifest => $"{singletonManifest.PackageIdentifier}.yaml",
                _ => throw new System.ArgumentException(nameof(manifest)),
            };
        }
    }
}
