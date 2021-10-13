// -----------------------------------------------------------------------
// <copyright file="LocaleValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// LocaleValidator.
    /// </summary>
    public class LocaleValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Max = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocaleValidator"/> class.
        /// </summary>
        public LocaleValidator()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
        }
    }
}
