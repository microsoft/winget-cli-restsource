// -----------------------------------------------------------------------
// <copyright file="Information.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Schemas
{
    using System;
    using System.Linq;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Constants.Enumerations;
    using Microsoft.WinGet.RestSource.Utils.Models.Arrays;
    using Microsoft.WinGet.RestSource.Utils.Models.Objects;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// Server Information.
    /// </summary>
    public class Information
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Information"/> class.
        /// </summary>
        public Information()
        {
            if (string.IsNullOrEmpty(ApiConstants.ServerIdentifier))
            {
                throw new ArgumentNullException("SourceIdentifier environment variable is not configured and needs to be setup.");
            }

            this.SourceIdentifier = ApiConstants.ServerIdentifier;

            if (string.IsNullOrEmpty(ApiConstants.ServerAuthenticationType) ||
                AuthenticationType.None.Equals(ApiConstants.ServerAuthenticationType, StringComparison.OrdinalIgnoreCase))
            {
                this.ServerSupportedVersions = this.GetServerSupportedVersions();
            }
            else if (AuthenticationType.MicrosoftEntraId.Equals(ApiConstants.ServerAuthenticationType, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(ApiConstants.MicrosoftEntraIdResource))
                {
                    throw new ArgumentNullException("MicrosoftEntraIdResource environment variable is not configured and needs to be setup when authentication type is Microsoft Entra Id.");
                }

                this.ServerSupportedVersions = this.GetServerSupportedVersions(ApiConstants.MinVersionForMicrosoftEntraId);

                this.Authentication = new Authentication();
                this.Authentication.AuthenticationType = AuthenticationType.MicrosoftEntraId;
                this.Authentication.MicrosoftEntraIdAuthenticationInfo = new MicrosoftEntraIdAuthenticationInfo();
                this.Authentication.MicrosoftEntraIdAuthenticationInfo.Resource = ApiConstants.MicrosoftEntraIdResource;
                this.Authentication.MicrosoftEntraIdAuthenticationInfo.Scope = ApiConstants.MicrosoftEntraIdResourceScope;
            }
            else
            {
                throw new ArgumentException($"Authentication type not recognized: {ApiConstants.ServerAuthenticationType}");
            }

            this.UnsupportedPackageMatchFields = ApiConstants.UnsupportedPackageMatchFields;
            this.RequiredPackageMatchFields = ApiConstants.RequiredPackageMatchFields;
            this.UnsupportedQueryParameters = ApiConstants.UnsupportedQueryParameters;
            this.RequiredQueryParameters = ApiConstants.RequiredQueryParameters;
        }

        /// <summary>
        /// Gets sourceIdentifier.
        /// </summary>
        [SourceIdentifierValidator]
        public string SourceIdentifier { get; }

        /// <summary>
        /// Gets serverSupportedVersions.
        /// </summary>
        public ApiVersions ServerSupportedVersions { get; }

        /// <summary>
        /// Gets SourceAgreements.
        /// </summary>
        public SourceAgreementExtended SourceAgreements { get; }

        /// <summary>
        /// Gets UnsupportedPackageMatchFields.
        /// </summary>
        public Arrays.PackageMatchFields UnsupportedPackageMatchFields { get; }

        /// <summary>
        /// Gets RequiredPackageMatchFields.
        /// </summary>
        public Arrays.PackageMatchFields RequiredPackageMatchFields { get; }

        /// <summary>
        /// Gets UnsupportedQueryParameters.
        /// </summary>
        public QueryParameters UnsupportedQueryParameters { get; }

        /// <summary>
        /// Gets RequiredQueryParameters.
        /// </summary>
        public QueryParameters RequiredQueryParameters { get; }

        /// <summary>
        /// Gets Authentication.
        /// </summary>
        public Authentication Authentication { get; }

        private ApiVersions GetServerSupportedVersions(string minVersion = null)
        {
            var result = ApiConstants.ServerSupportedVersions;

            if (!string.IsNullOrEmpty(minVersion))
            {
                var targetMinVersion = new System.Version(minVersion);
                result = (ApiVersions)result.Where<string>(v => new System.Version(v) >= targetMinVersion);
            }

            return result;
        }
    }
}
