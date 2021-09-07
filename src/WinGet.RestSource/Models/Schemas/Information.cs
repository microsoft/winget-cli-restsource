// -----------------------------------------------------------------------
// <copyright file="Information.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Objects;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

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
            this.SourceIdentifier = string.IsNullOrEmpty(ApiConstants.SourceIdentifier) ? "TestSource" : ApiConstants.SourceIdentifier;
            this.ServerSupportedVersions = ApiConstants.ServerSupportedVersions;

            // Examples of how Unsupported and Required Package match fields and query parameters can be used.
            this.UnsupportedPackageMatchFields = ApiConstants.UnSupportedPackageMatchFields;
            this.RequiredPackageMatchFields = ApiConstants.RequiredPackageMatchFields;
            this.UnsupportedQueryParameters = ApiConstants.UnsupportedQueryParameters;
            this.RequiredQueryParameters = ApiConstants.RequiredQueryParameters;

            // Example of a Soruce Agreement.
            this.SourceAgreements = new SourceAgreementExtended()
            {
                AgreementsIdentifier = "Hi Agreement",
                Agreements = new Agreements(),
            };
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
