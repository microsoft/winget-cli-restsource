// -----------------------------------------------------------------------
// <copyright file="PackageDependencyVersionValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// PackageVersionValidator.
    /// </summary>
    public class PackageDependencyVersionValidator : ApiStringValidator
    {
        private const string Pattern = "^[^\\\\/:\\*\\?\"<>\\|\\x01-\\x1f]+$";
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageDependencyVersionValidator"/> class.
        /// </summary>
        public PackageDependencyVersionValidator()
        {
            this.AllowNull = true;
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
