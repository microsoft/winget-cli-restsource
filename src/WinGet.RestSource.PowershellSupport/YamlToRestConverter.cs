// -----------------------------------------------------------------------
// <copyright file="YamlToRestConverter.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.PowershellSupport
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.WinGet.RestSource.PowershellSupport.Helpers;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGetUtil.Helpers;
    using Microsoft.WinGetUtil.Models.V1;
    using Newtonsoft.Json;

    /// <summary>
    /// Supports converting yaml manifest to json string.
    /// Primarily supports powershell modules.
    /// </summary>
    public static class YamlToRestConverter
    {
        /// <summary>
        /// Processes a directory for yaml manifests and converts it into the rest json format.
        /// </summary>
        /// <param name="directory">direcoty to process. Should contain the manifests for a single app.</param>
        /// <param name="priorRestManifest">Prior json data to merge with.</param>
        /// <returns>A string of the rest source json.</returns>
        public static string AddManifestToPackageManifest(
            string directory,
            string priorRestManifest)
        {
            // Construct merged manifest
            var packageFiles = Directory.GetFiles(directory);
            string mergedManifestFilePath =
                        Path.Combine(
                            Path.GetTempPath(),
                            Path.GetRandomFileName() + ".yaml");
            if (packageFiles.Length > 1)
            {
                // Multi File manifest case
                // The winget client ValidateManifest function
                // merges the manifests into the merged manifest format.
                (bool succeeded, string response) = WinGetUtilWrapperManifest.ValidateManifest(
                    directory,
                    mergedManifestFilePath);

                if (!succeeded)
                {
                    throw new Exception("Unable to validate manifest");
                }
            }
            else
            {
                mergedManifestFilePath = packageFiles.First();
            }

            Manifest manifest = Manifest.CreateManifestFromPath(mergedManifestFilePath);

            // Convert the manifest into a rest manifestPost format and merge with any existing data.
            PackageManifest packageManifest = PackageManifestUtils.AddManifestToPackageManifest(
                manifest,
                priorRestManifest);

            return JsonConvert.SerializeObject(packageManifest);
        }
    }
}
