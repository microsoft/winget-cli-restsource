// -----------------------------------------------------------------------
// <copyright file="SqlReader.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Helpers.Sql
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Data.Sqlite;

    /// <summary>
    /// Helper class to read sql database.
    /// </summary>
    internal class SqlReader : IDisposable
    {
        private readonly SqliteConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlReader"/> class.
        /// </summary>
        /// <param name="dbPath">Database path.</param>
        public SqlReader(string dbPath)
        {
            this.connection = new SqliteConnection($"Data Source={dbPath}");
            this.connection.Open();
        }

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">Bool value indicating if Dispose is being run.</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.connection?.Dispose();
            }
        }

        /// <summary>
        /// Gets a list of packages in the database.
        /// </summary>
        /// <returns>List of packages.</returns>
        public IReadOnlyList<SqlPackage> GetPackages()
        {
            var packages = new List<SqlPackage>();

            // Select name and pathpart.
            var getPackagescommand = this.connection.CreateCommand();
            getPackagescommand.CommandText = "SELECT rowid, id FROM ids";

            using var reader = getPackagescommand.ExecuteReader();
            while (reader.Read())
            {
                string rowId = reader.GetString(0);
                string id = reader.GetString(1);
                var versions = this.GetVersions(rowId);

                packages.Add(new SqlPackage(id, versions));
            }

            return packages;
        }

        private IReadOnlyList<SqlVersion> GetVersions(string idsRowid)
        {
            var versions = new List<SqlVersion>();

            // Get the version and pathpart rowids.
            var getVersionAndPathPartIdsCommand = this.connection.CreateCommand();
            getVersionAndPathPartIdsCommand.CommandText = "SELECT version, pathpart FROM manifest where id = $idsRowid";
            getVersionAndPathPartIdsCommand.Parameters.AddWithValue("$idsRowid", idsRowid);

            using var reader = getVersionAndPathPartIdsCommand.ExecuteReader();
            while (reader.Read())
            {
                string versionsRowId = reader.GetString(0);
                string version = this.GetVersion(versionsRowId);

                string pathPathRowId = reader.GetString(1);

                // Get the path parts. This will result in something like manifests\a\a\b\a.b.yaml
                string path = this.ReadPathPartFromId(pathPathRowId);

                versions.Add(new SqlVersion(version, path));
            }

            return versions;
        }

        private string GetVersion(string versionsRowId)
        {
            // Get the string version.
            var versionCommand = this.connection.CreateCommand();
            versionCommand.CommandText = @"SELECT version FROM versions where rowid = $versionsRowId";
            versionCommand.Parameters.AddWithValue("$versionsRowId", versionsRowId);

            using var versionCommandReader = versionCommand.ExecuteReader();

            // There should only be 1 match for a row id.
            if (versionCommandReader.Read())
            {
                return versionCommandReader.GetString(0);
            }

            throw new ArgumentException();
        }

        private string ReadPathPartFromId(string pathPathRowId)
        {
            // Get path for that pathrowid
            var pathPartCommand = this.connection.CreateCommand();
            pathPartCommand.CommandText = @"SELECT parent, pathpart FROM pathparts where rowid = $pathPathRowId";
            pathPartCommand.Parameters.AddWithValue("$pathPathRowId", pathPathRowId);

            // There should only be 1 match for a row id.
            // We just grab the first as we wouldn't know what to do with additional matches anyhow.
            using var reader = pathPartCommand.ExecuteReader();
            if (reader.Read())
            {
                var parent = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                var pathPart = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);

                return (string.IsNullOrWhiteSpace(parent) ? string.Empty : this.ReadPathPartFromId(parent) + @"\") + pathPart;
            }

            return string.Empty;
        }
    }
}
