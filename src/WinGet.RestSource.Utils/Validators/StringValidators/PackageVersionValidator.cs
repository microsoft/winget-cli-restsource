﻿// -----------------------------------------------------------------------
// <copyright file="PackageVersionValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// PackageVersionValidator.
    /// </summary>
    public class PackageVersionValidator : ApiStringValidator
    {
        private const string Pattern = "^[^\\\\/:\\*\\?\"<>\\|\\x01-\\x1f]+$";
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageVersionValidator"/> class.
        /// </summary>
        public PackageVersionValidator()
        {
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
