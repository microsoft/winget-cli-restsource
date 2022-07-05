// -----------------------------------------------------------------------
// <copyright file="ApiConstants.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Constants
{
    using System;
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;

    /// <summary>
    /// API Constants.
    /// </summary>
    public class ApiConstants
    {
        /// <summary>
        /// Source Identifier.
        /// </summary>
        public static readonly string SourceIdentifier = ApiConstants.ServerIdentifier;

        /// <summary>
        /// Whether certificate authentication is enabled.
        /// </summary>
        public static readonly bool CertificateAuthenticationRequired = bool.Parse(ApiConstants.CertificateAuthenticationRequiredEnv);

        /// <summary>
        /// Subject name of the root/intermediate certificate.
        /// </summary>
        public static readonly string CertificateAuthenticationSubjectName = ApiConstants.CertificateAuthenticationSubjectNameEnv;

        /// <summary>
        /// Whether self signed certificates are allowed as the root/intermediate certificate.
        /// </summary>
        public static readonly bool CertificateAuthenticationSelfSigned = bool.Parse(ApiConstants.CertificateAuthenticationSelfSignedEnv);

        /// <summary>
        /// Server Supported Versions.
        /// </summary>
        public static readonly ApiVersions ServerSupportedVersions = new ApiVersions()
        {
            "1.0.0",
            "1.1.0",
        };

        /// <summary>
        /// Gets manifest cache endpoint.
        /// </summary>
        public static readonly string ManifestCacheEndpoint = Environment.GetEnvironmentVariable("ManifestCacheEndpoint");

        /// <summary>
        /// Functions host key.
        /// </summary>
        public static readonly string AzureFunctionHostKey = Environment.GetEnvironmentVariable("FunctionHostKey");

        /// <summary>
        /// Rest source endpoint.
        /// </summary>
        public static readonly string AzFuncRestSourceEndpoint = Environment.GetEnvironmentVariable("AzFuncRestSourceEndpoint");

        /// <summary>
        /// Server Supported Versions.
        /// Unsupported package match fields.
        /// TODO: NormalizedPackageNameAndPublisher field support is currently not implemented.
        /// GitHub Issue: https://github.com/microsoft/winget-cli-restsource/issues/59.
        /// </summary>
        public static readonly PackageMatchFields UnsupportedPackageMatchFields = new PackageMatchFields()
        {
            // TODO: Currently the winget client sends this field up despite us reporting it as unsupported, so for compatibility
            // we don't currently return it as unsupported. Above issue needs to be resolved.

            // Enumerations.PackageMatchFields.NormalizedPackageNameAndPublisher,
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

        /// <summary>
        /// Gets whether certificate authentication is enabled.
        /// </summary>
        public static string CertificateAuthenticationRequiredEnv => Environment.GetEnvironmentVariable("CertificateAuthenticationRequired");

        /// <summary>
        /// Gets the expected root/intermediate cert subject name.
        /// </summary>
        public static string CertificateAuthenticationSubjectNameEnv => Environment.GetEnvironmentVariable("CertificateAuthenticationSubjectName");

        /// <summary>
        /// Gets whether self signed certificates are allowed.
        /// </summary>
        public static string CertificateAuthenticationSelfSignedEnv => Environment.GetEnvironmentVariable("CertificateAuthenticationSelfSigned");
    }
}
