// -----------------------------------------------------------------------
// <copyright file="AuthorValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// AuthorValidator.
    /// </summary>
    public class AuthorValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 2;
        private const uint Max = 256;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorValidator"/> class.
        /// </summary>
        public AuthorValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
