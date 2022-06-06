// -----------------------------------------------------------------------
// <copyright file="SqlVersion.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Helpers.Sql
{
    /// <summary>
    /// Sql version.
    /// </summary>
    internal class SqlVersion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlVersion"/> class.
        /// </summary>
        /// <param name="version">Version.</param>
        /// <param name="pathPart">Path part.</param>
        public SqlVersion(string version, string pathPart)
        {
            this.Version = version;
            this.PathPart = pathPart;
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Gets the path part.
        /// </summary>
        public string PathPart { get; private set; }
    }
}
