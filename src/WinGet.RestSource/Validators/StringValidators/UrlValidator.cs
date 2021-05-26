// -----------------------------------------------------------------------
// <copyright file="UrlValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// UrlValidator.
    /// </summary>
    public class UrlValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const string Pattern = "^([Hh][Tt][Tt][Pp][Ss]?)://";
        private const uint Max = 2048;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlValidator"/> class.
        /// </summary>
        public UrlValidator()
        {
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
