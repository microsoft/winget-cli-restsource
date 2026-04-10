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
        /// ServerAuthenticationType environmental variable name.
        /// </summary>
        public const string ServerAuthenticationTypeEnvName = "ServerAuthenticationType";

        /// <summary>
        /// MicrosoftEntraIdResource environmental variable name.
        /// </summary>
        public const string MicrosoftEntraIdResourceEnvName = "MicrosoftEntraIdResource";

        /// <summary>
        /// MicrosoftEntraIdResourceScope environmental variable name.
        /// </summary>
        public const string MicrosoftEntraIdResourceScopeEnvName = "MicrosoftEntraIdResourceScope";

        /// <summary>
        /// Server Supported Versions.
        /// </summary>
        public static readonly ApiVersions ServerSupportedVersions = new ApiVersions()
        {
            "1.0.0",
            "1.1.0",
            "1.4.0",
            "1.5.0",
            "1.6.0",
            "1.7.0",
            "1.9.0",
            "1.10.0",
        };

        /// <summary>
        /// Min version for Microsoft Entra Id support.
        /// </summary>
        public static readonly string MinVersionForMicrosoftEntraId = "1.7.0";

        /// <summary>
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
        /// Gets Functions host key.
        /// </summary>
        public static string AzureFunctionHostKey => Environment.GetEnvironmentVariable(FunctionHostKeyEnvName);

        /// <summary>
        /// Gets manifest cache endpoint.
        /// </summary>
        public static string ManifestCacheEndpoint => Environment.GetEnvironmentVariable(ManifestCacheEndpointEnvName);

        /// <summary>
        /// Gets server identifier.
        /// </summary>
        public static string ServerIdentifier => Environment.GetEnvironmentVariable(ServerIdentifierEnvName);

        /// <summary>
        /// Gets Microsoft Entra Id resource.
        /// </summary>
        public static string ServerAuthenticationType => Environment.GetEnvironmentVariable(ServerAuthenticationTypeEnvName);

        /// <summary>
        /// Gets Microsoft Entra Id resource.
        /// </summary>
        public static string MicrosoftEntraIdResource => Environment.GetEnvironmentVariable(MicrosoftEntraIdResourceEnvName);

        /// <summary>
        /// Gets optional Microsoft Entra Id resource scope.
        /// </summary>
        public static string MicrosoftEntraIdResourceScope => Environment.GetEnvironmentVariable(MicrosoftEntraIdResourceScopeEnvName);
    }
}
