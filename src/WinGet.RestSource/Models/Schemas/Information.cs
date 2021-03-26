// -----------------------------------------------------------------------
// <copyright file="Information.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Strings;

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
            SourceIdentifier sourceIdentifier = new SourceIdentifier()
            {
                APIString = ApiConstants.ServerIdentifier,
            };
            this.SourceIdentifier = ApiConstants.SourceIdentifier;
            this.ServerSupportedVersions = ApiConstants.ServerSupportedVersions;
        }

        /// <summary>
        /// Gets sourceIdentifier.
        /// </summary>
        public SourceIdentifier SourceIdentifier { get; }

        /// <summary>
        /// Gets serverSupportedVersions.
        /// </summary>
        public ApiVersions ServerSupportedVersions { get; }
    }
}
