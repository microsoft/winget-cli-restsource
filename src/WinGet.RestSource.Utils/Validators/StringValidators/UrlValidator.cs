// -----------------------------------------------------------------------
// <copyright file="UrlValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// UrlValidator.
    /// </summary>
    public class UrlValidator : ApiStringValidator
    {
        private const string Pattern = "^([Hh][Tt][Tt][Pp][Ss]?)://";
        private const uint Max = 2048;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlValidator"/> class.
        /// </summary>
        public UrlValidator()
        {
            this.AllowNull = this.Nullable;
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the value could be Nullable.
        /// </summary>
        public bool Nullable { get; set; } = true;
    }
}
