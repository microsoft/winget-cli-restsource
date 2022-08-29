// -----------------------------------------------------------------------
// <copyright file="NestedInstallerFiles.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Utils.Models.Core;

    /// <summary>
    /// NestedInstallerFiles.
    /// </summary>
    public class NestedInstallerFiles : ApiArray<Objects.NestedInstallerFile>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 1024;

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedInstallerFiles"/> class.
        /// </summary>
        public NestedInstallerFiles()
        {
            this.APIArrayName = nameof(NestedInstallerFiles);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
