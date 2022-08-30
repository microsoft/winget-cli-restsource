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
        /// AzFuncRestSourceEndpoint environmental variable name.
        /// </summary>
        public const string AzFuncRestSourceEndpointEnvName = "AzFuncRestSourceEndpoint";

        /// <summary>
        /// CertificateAuthenticationRequiredName environmental variable name.
        /// </summary>
        public const string CertificateAuthenticationRequiredEnvName = "CertificateAuthenticationRequired";

        /// <summary>
        /// CertificateAuthenticationSelfSigned environmental variable name.
        /// </summary>
        public const string CertificateAuthenticationSelfSignedEnvName = "CertificateAuthenticationSelfSigned";

        /// <summary>
        /// CertificateAuthenticationSubjectName environmental variable name.
        /// </summary>
        public const string CertificateAuthenticationSubjectNameEnvName = "CertificateAuthenticationSubjectName";

        /// <summary>
        /// FunctionHostKey environmental variable name.
        /// </summary>
        public const string FunctionHostKeyEnvName = "FunctionHostKey";

        /// <summary>
        /// ManifestCacheEndpoint environmental variable name.
        /// </summary>
        public const string ManifestCacheEndpointEnvName = "ManifestCacheEndpoint";

        /// <summary>
        /// ServerIdentifier environmental variable name.
        /// </summary>
        public const string ServerIdentifierEnvName = "ServerIdentifier";

        /// <summary>
        /// Server Supported Versions.
        /// </summary>
        public static readonly ApiVersions ServerSupportedVersions = new ApiVersions()
        {
            "1.0.0",
            "1.1.0",
            "1.4.0",
        };

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
        /// Gets Rest source endpoint.
        /// </summary>
        public static string AzFuncRestSourceEndpoint => Environment.GetEnvironmentVariable(AzFuncRestSourceEndpointEnvName);

        /// <summary>
        /// Gets whether certificate authentication is enabled.
        /// </summary>
        public static string CertificateAuthenticationRequiredEnv => Environment.GetEnvironmentVariable(CertificateAuthenticationRequiredEnvName);

        /// <summary>
        /// Gets a value indicating whether certificate authentication is enabled.
        /// </summary>
        public static bool CertificateAuthenticationRequired => bool.Parse(ApiConstants.CertificateAuthenticationRequiredEnv);

        /// <summary>
        /// Gets whether self signed certificates are allowed.
        /// </summary>
        public static string CertificateAuthenticationSelfSignedEnv => Environment.GetEnvironmentVariable(CertificateAuthenticationSelfSignedEnvName);

        /// <summary>
        /// Gets a value indicating whether self signed certificates are allowed as the root/intermediate certificate.
        /// </summary>
        public static bool CertificateAuthenticationSelfSigned => bool.Parse(ApiConstants.CertificateAuthenticationSelfSignedEnv);

        /// <summary>
        /// Gets the expected root/intermediate cert subject name.
        /// </summary>
        public static string CertificateAuthenticationSubjectNameEnv => Environment.GetEnvironmentVariable(CertificateAuthenticationSubjectNameEnvName);

        /// <summary>
        /// Gets Subject name of the root/intermediate certificate.
        /// </summary>
        public static string CertificateAuthenticationSubjectName => ApiConstants.CertificateAuthenticationSubjectNameEnv;

        /// <summary>
        /// Gets Functions host key.
        /// </summary>
        public static string AzureFunctionHostKey => Environment.GetEnvironmentVariable(FunctionHostKeyEnvName);

        /// <summary>
        /// Gets manifest cache endpoint.
        /// </summary>
        public static string ManifestCacheEndpoint => Environment.GetEnvironmentVariable(ManifestCacheEndpointEnvName);

        /// <summary>
        /// Gets server Identifier.
        /// </summary>
        public static string ServerIdentifier => Environment.GetEnvironmentVariable(ServerIdentifierEnvName);
    }
}
