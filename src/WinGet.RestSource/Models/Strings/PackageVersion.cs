// -----------------------------------------------------------------------
// <copyright file="PackageVersion.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// PackageVersion.
    /// </summary>
    public class PackageVersion : ApiString
    {
        private const string Pattern = "^[^\\\\/:\\*\\?\"<>\\|\\x01-\\x1f]+$";
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageVersion"/> class.
        /// </summary>
        public PackageVersion()
        {
            this.APIStringName = nameof(PackageVersion);
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
