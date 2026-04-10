// -----------------------------------------------------------------------
// <copyright file="InstallationMetadataFiles.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Utils.Models.Core;

    /// <summary>
    /// InstallationMetadataFiles.
    /// </summary>
    public class InstallationMetadataFiles : ApiArray<Objects.InstallationMetadataFile>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 2048;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationMetadataFiles"/> class.
        /// </summary>
        public InstallationMetadataFiles()
        {
            this.APIArrayName = nameof(InstallationMetadataFiles);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
