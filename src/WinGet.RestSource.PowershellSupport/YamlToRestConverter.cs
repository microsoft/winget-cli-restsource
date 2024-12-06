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
    using Microsoft.WinGet.RestSource.Utils;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using Microsoft.WinGetUtil.Api;
    using Microsoft.WinGetUtil.Common;
    using Microsoft.WinGetUtil.Models.V1;
    using Newtonsoft.Json;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    /// <summary>
    /// Supports converting yaml manifest to json string.
    /// Primarily supports powershell modules.
    /// </summary>
    public static class YamlToRestConverter
    {
        /// <summary>
        /// Processes a directory for yaml manifests and converts it into the rest json format.
        /// </summary>
        /// <param name="path">Manifest file or directory to process. Should contain manifests for a single app.</param>
        /// <param name="priorRestManifest">Prior json data to merge with.</param>
        /// <returns>A string of the rest source json.</returns>
        public static string AddManifestToPackageManifest(
            string path,
            string priorRestManifest)
        {
            string manifestPath = string.Empty;

            if (File.Exists(path))
            {
                manifestPath = path;
            }
            else if (Directory.Exists(path))
            {
                var packageFiles = Directory.GetFiles(path);

                if (packageFiles.Length > 1)
                {
                    manifestPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".yaml");

                    // Create merged manifest from multifile manifests.
                    var factory = new WinGetFactory();
                    using var manifestResult = factory.CreateManifest(path, manifestPath, WinGetCreateManifestOption.NoValidation);

                    if (!manifestResult.IsValid)
                    {
                        throw new ArgumentException("Unable to create merged manifest from multifile manifests.");
                    }
                }
                else
                {
                    manifestPath = packageFiles.First();
                }
            }
            else
            {
                throw new ArgumentException("Input manifest path not found.");
            }

            Manifest manifest = Manifest.CreateManifestFromPath(manifestPath);

            // Convert the manifest into a rest manifestPost format and merge with any existing data.
            PackageManifest packageManifest = PackageManifestUtils.AddManifestToPackageManifest(
                manifest,
                string.IsNullOrWhiteSpace(priorRestManifest) ? null : Parser.StringParser<PackageManifest>(priorRestManifest));

            return JsonConvert.SerializeObject(
                packageManifest,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                });
        }
    }
}
