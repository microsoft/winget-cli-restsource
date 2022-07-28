// -----------------------------------------------------------------------
// <copyright file="RestConverter.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Common
{
    using System.Collections.Generic;
    using Microsoft.WinGet.Source.Models;

    /// <summary>
    /// Converts manifest objects to and from REST JSON format.
    /// </summary>
    internal static class RestConverter
    {
        /// <summary>
        /// Creates a payload for a REST source from a manifests object.
        /// </summary>
        /// <param name="manifests">Manifests object.</param>
        /// <returns>Payload object.</returns>
        public static ManifestsPayload CreateManifestsPayload(Manifests manifests)
        {
            return new ManifestsPayload()
            {
                PackageIdentifier = manifests.VersionManifest.PackageIdentifier,
                Versions = new List<ManifestsPayloadVersionItem>
                {
                    new ManifestsPayloadVersionItem()
                    {
                        DefaultLocale = manifests.DefaultLocaleManifest,
                        Installers = manifests.InstallerManifest.Installers,
                        PackageVersion = manifests.VersionManifest.PackageVersion,
                    },
                },
            };
        }

        /// <summary>
        /// Creates a manifests object from a REST source response.
        /// </summary>
        /// <param name="item">Version item object from REST source.</param>
        /// <param name="packageIdentifier">Identifier of package.</param>
        /// <returns>Manifests object.</returns>
        public static Manifests CreateManifestsFromVersionItem(ManifestsPayloadVersionItem item, string packageIdentifier)
        {
            Manifests manifests = new()
            {
                DefaultLocaleManifest = item.DefaultLocale,
                VersionManifest = new Models.Version.VersionManifest()
                {
                    PackageIdentifier = packageIdentifier,
                    PackageVersion = item.PackageVersion,
                    DefaultLocale = item.DefaultLocale.PackageLocale,
                },
                InstallerManifest = new Models.Installer.InstallerManifest()
                {
                    PackageIdentifier = packageIdentifier,
                    PackageVersion = item.PackageVersion,
                    Installers = item.Installers,
                },
                LocaleManifests = item.Locales,
            };

            // These are properties that are left out of place in the response we get back.
            manifests.DefaultLocaleManifest.PackageIdentifier = packageIdentifier;
            manifests.DefaultLocaleManifest.PackageVersion = item.PackageVersion;

            return manifests;
        }
    }
}
