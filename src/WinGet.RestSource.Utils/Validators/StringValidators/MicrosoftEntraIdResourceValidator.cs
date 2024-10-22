// -----------------------------------------------------------------------
// <copyright file="MicrosoftEntraIdResourceValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// MicrosoftEntraIdResourceValidator.
    /// </summary>
    public class MicrosoftEntraIdResourceValidator : ApiStringValidator
    {
        private const bool Nullable = false;
        private const uint Max = 512;
        private const uint Min = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftEntraIdResourceValidator"/> class.
        /// </summary>
        public MicrosoftEntraIdResourceValidator()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
            this.MinLength = Min;
        }
    }
}
