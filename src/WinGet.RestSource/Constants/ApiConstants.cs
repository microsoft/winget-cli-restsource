// -----------------------------------------------------------------------
// <copyright file="ApiConstants.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Constants
{
    using System;
    using Microsoft.WinGet.RestSource.Models.Arrays;
    using Microsoft.WinGet.RestSource.Models.Strings;

    /// <summary>
    /// API Constants.
    /// </summary>
    public class ApiConstants
    {
        /// <summary>
        /// This is the multiplaction constant used by the hash code..
        /// </summary>
        public const int HashCodeConstant = 397;

        /// <summary>
        /// Source Identifier.
        /// </summary>
        public static readonly SourceIdentifier SourceIdentifier = new SourceIdentifier()
        {
            APIString = ApiConstants.ServerIdentifier,
        };

        /// <summary>
        /// Server Supported Versions.
        /// </summary>
        public static readonly ApiVersions ServerSupportedVersions = new ApiVersions()
        {
            new ApiVersion()
            {
                APIString = "0.2.0",
            },
        };

        /// <summary>
        /// Gets server Identifier.
        /// </summary>
        public static string ServerIdentifier => Environment.GetEnvironmentVariable("ServerIdentifier");
    }
}
