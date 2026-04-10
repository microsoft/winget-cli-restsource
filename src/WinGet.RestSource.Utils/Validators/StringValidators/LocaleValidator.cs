// -----------------------------------------------------------------------
// <copyright file="LocaleValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// LocaleValidator.
    /// </summary>
    public class LocaleValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const string Pattern = "^([a-zA-Z]{2,3}|[iI]-[a-zA-Z]+|[xX]-[a-zA-Z]{1,8})(-[a-zA-Z]{1,8})*$";
        private const uint Max = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocaleValidator"/> class.
        /// </summary>
        public LocaleValidator()
        {
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
