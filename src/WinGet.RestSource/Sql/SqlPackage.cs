// -----------------------------------------------------------------------
// <copyright file="SqlPackage.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Sql
{
    using System.Collections.Generic;

    /// <summary>
    /// Sql package table.
    /// </summary>
    internal class SqlPackage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlPackage"/> class.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="versions">Versions.</param>
        public SqlPackage(string id, IReadOnlyList<SqlVersion> versions)
        {
            this.Id = id;
            this.Versions = versions;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public IReadOnlyList<SqlVersion> Versions { get; private set; }
    }
}
