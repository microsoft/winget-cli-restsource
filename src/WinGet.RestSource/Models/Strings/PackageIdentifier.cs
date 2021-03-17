// -----------------------------------------------------------------------
// <copyright file="PackageIdentifier.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// PackageIdentifier.
    /// </summary>
    public class PackageIdentifier : ApiString
    {
        private const string Pattern = "^[^\\.\\s\\\\/:\\*\\?\"<>\\|\\x01-\\x1f]{1,32}(\\.[^\\.\\s\\\\/:\\*\\?\"<>\\|\\x01-\\x1f]{1,32}){1,3}$";
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageIdentifier"/> class.
        /// </summary>
        public PackageIdentifier()
        {
            this.APIStringName = nameof(PackageIdentifier);
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
