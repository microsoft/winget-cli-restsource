// -----------------------------------------------------------------------
// <copyright file="ApiConstants.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Constants
{
    using System;
    using Microsoft.WinGet.RestSource.Models.Arrays;

    /// <summary>
    /// API Constants.
    /// </summary>
    public class ApiConstants
    {
        /// <summary>
        /// This is the multiplication constant used by the hash code.
        /// </summary>
        public const int HashCodeConstant = 397;

        /// <summary>
        /// Source Identifier.
        /// </summary>
        public static readonly string SourceIdentifier = ApiConstants.ServerIdentifier;

        /// <summary>
        /// Server Supported Versions.
        /// </summary>
        public static readonly ApiVersions ServerSupportedVersions = new ApiVersions()
        {
            "1.0.0",
            "1.1.0",
        };

        /// <summary>
        /// Unsupported package match fields.
        /// Note: NormalizedPackageNameAndPublisher field is added here to demonstrate how this property can be used.
        /// </summary>
        public static readonly PackageMatchFields UnSupportedPackageMatchFields = new PackageMatchFields()
        {
            Enumerations.PackageMatchFields.NormalizedPackageNameAndPublisher,
        };

        /// <summary>
        /// Required package match fields.
        /// </summary>
        public static readonly PackageMatchFields RequiredPackageMatchFields = new PackageMatchFields();

        /// <summary>
        /// Unsupported query parameters.
        /// </summary>
        public static readonly QueryParameters UnsupportedQueryParameters = new QueryParameters();

        /// <summary>
        /// Required query paramters.
        /// </summary>
        public static readonly QueryParameters RequiredQueryParameters = new QueryParameters();

        /// <summary>
        /// Gets server Identifier.
        /// </summary>
        public static string ServerIdentifier => Environment.GetEnvironmentVariable("ServerIdentifier");
    }
}
