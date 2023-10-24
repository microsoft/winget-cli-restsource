// -----------------------------------------------------------------------
// <copyright file="BaseManifestsCommand.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Common
{
    using System.Management.Automation;
    using Microsoft.WinGet.Source.Models;

    /// <summary>
    /// This is the base command for all commands that need a
    /// manifests object to execute.
    /// </summary>
    public class BaseManifestsCommand : BaseAzureTokenCommand, IDynamicParameters
    {
        private ManifestsDynamicParameters context;
        private string path;

        /// <summary>
        /// Gets or sets the path to the directory containing manifest files.
        /// </summary>
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true)]
        public string Path
        {
            get => this.path;
            set
            {
                this.path = System.IO.Path.IsPathRooted(value)
                    ? value
                    : System.IO.Path.Combine(
                        this.SessionState.Path.CurrentFileSystemLocation.Path,
                        value);
            }
        }

        /// <inheritdoc />
        public object GetDynamicParameters()
        {
            if (this.Path is null)
            {
                this.context = new ManifestsDynamicParameters();
                return this.context;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the <see cref="Manifests" /> object to operate with by,
        ///     1. forwarding the manually supplied parameter, or by
        ///     2. deserializing files on the filesystem.
        /// </summary>
        /// <returns>A <see cref="Manifests" /> object.</returns>
        protected Manifests GetManifests()
        {
            return (this.Path != null)
                ? Serialization.DeserializeFromManifestsDirectory(this.Path)
                : this.context.Manifests;
        }

        private class ManifestsDynamicParameters
        {
            /// <summary>
            /// Gets or sets the <see cref="Manifests" /> object to work with.
            /// </summary>
            [Parameter(
                Mandatory = false,
                Position = 0,
                ValueFromPipeline = true,
                ValueFromPipelineByPropertyName = true)]
            public Manifests Manifests { get; set; }
        }
    }
}
