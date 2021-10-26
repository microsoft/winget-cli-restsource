// -----------------------------------------------------------------------
// <copyright file="PackageIdentifierValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// PackageIdentifierValidator.
    /// </summary>
    public class PackageIdentifierValidator : ApiStringValidator
    {
        private const string Pattern = "^[^\\.\\s\\\\/:\\*\\?\"<>\\|\\x01-\\x1f]{1,32}(\\.[^\\.\\s\\\\/:\\*\\?\"<>\\|\\x01-\\x1f]{1,32}){1,3}$";
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageIdentifierValidator"/> class.
        /// </summary>
        public PackageIdentifierValidator()
        {
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
