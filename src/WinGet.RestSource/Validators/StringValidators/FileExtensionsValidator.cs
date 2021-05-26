// -----------------------------------------------------------------------
// <copyright file="FileExtensionsValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// FileExtensionsValidator.
    /// </summary>
    public class FileExtensionsValidator : ApiStringValidator
    {
        private const string Pattern = "^[^\\\\/:\\*\\?\"<>\\|\\x01-\\x1f]+$";
        private const uint Max = 64;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileExtensionsValidator"/> class.
        /// </summary>
        public FileExtensionsValidator()
        {
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
