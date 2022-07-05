// -----------------------------------------------------------------------
// <copyright file="Information.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Schemas
{
    using Microsoft.WinGet.RestSource.Utils.Constants;
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
                throw new System.ArgumentNullException("SourceIdentifier environment variable is not configured and needs to be setup.");
            }

            this.SourceIdentifier = ApiConstants.ServerIdentifier;
            this.ServerSupportedVersions = ApiConstants.ServerSupportedVersions;

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
        public PackageMatchFields UnsupportedPackageMatchFields { get; }

        /// <summary>
        /// Gets RequiredPackageMatchFields.
        /// </summary>
        public PackageMatchFields RequiredPackageMatchFields { get; }

        /// <summary>
        /// Gets UnsupportedQueryParameters.
        /// </summary>
        public QueryParameters UnsupportedQueryParameters { get; }

        /// <summary>
        /// Gets RequiredQueryParameters.
        /// </summary>
        public QueryParameters RequiredQueryParameters { get; }
    }
}
