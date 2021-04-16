// -----------------------------------------------------------------------
// <copyright file="IApiDataStore.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.WinGet.RestSource.Models.Schemas;

    /// <summary>
    /// This provides an interface for IApiDataStore.
    /// </summary>
    public interface IApiDataStore
    {
        /// <summary>
        /// This will add a package to the data store.
        /// </summary>
        /// <param name="package">Package.</param>
        /// <returns>Task.</returns>
        Task AddPackage(Package package);

        /// <summary>
        /// This will remove a package based on the packageIdentifier.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <returns>Task.</returns>
        Task DeletePackage(string packageIdentifier);

        /// <summary>
        /// This will update a package based on the package identifier.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="package">Package.</param>
        /// <returns>Task.</returns>
        Task UpdatePackage(string packageIdentifier, Package package);

        /// <summary>
        /// This will retrieve a set of packages based on a package identifier and a continuation token.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="queryParameters">Query Parameters.</param>
        /// <returns>CosmosPage of Package Manifests.</returns>
        Task<ApiDataPage<Package>> GetPackages(string packageIdentifier, IQueryCollection queryParameters);

        /// <summary>
        /// This will add a version to a package based on the package identifier.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="version">Version.</param>
        /// <returns>Task.</returns>
        Task AddVersion(string packageIdentifier, Version version);

        /// <summary>
        /// This will delete a version based on the package identifier and package version.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <returns>Task.</returns>
        Task DeleteVersion(string packageIdentifier, string packageVersion);

        /// <summary>
        /// This updates a version associated with a package identifier and package version.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="version">Version.</param>
        /// <returns>Task.</returns>
        Task UpdateVersion(string packageIdentifier, string packageVersion, Version version);

        /// <summary>
        /// This will get a set of versions based on the package identifier, package version, and any continuation token.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="queryParameters">Query Parameters.</param>
        /// <returns>CosmosPage of Version.</returns>
        Task<ApiDataPage<Version>> GetVersions(string packageIdentifier, string packageVersion, IQueryCollection queryParameters);

        /// <summary>
        /// This will add an installer referencing a package identifier and a package version.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="installer">Installer.</param>
        /// <returns>Task.</returns>
        Task AddInstaller(string packageIdentifier, string packageVersion, Installer installer);

        /// <summary>
        /// This will delete an installer based on the package identifier, package version, and installer identifier.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="installerIdentifier">Installer Identifier.</param>
        /// <returns>Task.</returns>
        Task DeleteInstaller(string packageIdentifier, string packageVersion, string installerIdentifier);

        /// <summary>
        /// This updates an installer referencing a package identifier, package version, and an installer identifier.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="installerIdentifier">Installer Identifier.</param>
        /// <param name="installer">Installer.</param>
        /// <returns>Task.</returns>
        Task UpdateInstaller(string packageIdentifier, string packageVersion, string installerIdentifier, Installer installer);

        /// <summary>
        /// This will get a set of Installers based on the package identifier, package version, installer identifier, and any continuation token.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="installerIdentifier">Installer Identifier.</param>
        /// <param name="queryParameters">Query Parameters.</param>
        /// <returns>CosmosPage of Installer.</returns>
        Task<ApiDataPage<Installer>> GetInstallers(string packageIdentifier, string packageVersion, string installerIdentifier, IQueryCollection queryParameters);

        /// <summary>
        /// This will add a locale referencing a package identifier and a package version.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Task.</returns>
        Task AddLocale(string packageIdentifier, string packageVersion, Locale locale);

        /// <summary>
        /// This will delete a locale based on the package identifier, package version, and package locale.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="packageLocale">Package Locale.</param>
        /// <returns>Task.</returns>
        Task DeleteLocale(string packageIdentifier, string packageVersion, string packageLocale);

        /// <summary>
        /// /// This updates a locale referencing a package identifier, package version, and an installer identifier.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="packageLocale">Package Locale.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Task.</returns>
        Task UpdateLocale(string packageIdentifier, string packageVersion, string packageLocale, Locale locale);

        /// <summary>
        /// This will get a set of Installers based on the package identifier, package version, installer identifier, and any continuation token.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageVersion">Package Version.</param>
        /// <param name="packageLocale">Package Locale.</param>
        /// <param name="queryParameters">Query Parameters.</param>
        /// <returns>CosmosPage of Locale.</returns>
        Task<ApiDataPage<Locale>> GetLocales(string packageIdentifier, string packageVersion, string packageLocale, IQueryCollection queryParameters);

        /// <summary>
        /// Add a Package Manifest.
        /// </summary>
        /// <param name="packageManifest">Package Manifest.</param>
        /// <returns>Task.</returns>
        Task AddPackageManifest(PackageManifest packageManifest);

        /// <summary>
        /// Deletes a package manifest based on package identifier.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <returns>Task.</returns>
        Task DeletePackageManifest(string packageIdentifier);

        /// <summary>
        /// Update a package manifest based on package identifier.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="packageManifest">Package Manifest.</param>
        /// <returns>Task.</returns>
        Task UpdatePackageManifest(string packageIdentifier, PackageManifest packageManifest);

        /// <summary>
        /// Get a Package Manifest based on package identifier and query parameters.
        /// </summary>
        /// <param name="packageIdentifier">Package Identifier.</param>
        /// <param name="queryParameters">Query Parameters.</param>
        /// <returns>CosmosPage of Package Manifests.</returns>
        Task<ApiDataPage<PackageManifest>> GetPackageManifests(string packageIdentifier, IQueryCollection queryParameters);

        /// <summary>
        /// This will search for manifests based on a manifest search request and a set of query parameters.
        /// </summary>
        /// <param name="manifestSearchRequest">Manifest Search Request.</param>
        /// <param name="headerParameters">Header Parameters.</param>
        /// <param name="queryParameters">Query Parameters.</param>
        /// <returns>CosmosPage of ManifestSearchResponse.</returns>
        Task<ApiDataPage<ManifestSearchResponse>> SearchPackageManifests(ManifestSearchRequest manifestSearchRequest, Dictionary<string, string> headerParameters, IQueryCollection queryParameters);
    }
}
