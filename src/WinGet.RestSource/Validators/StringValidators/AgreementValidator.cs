// -----------------------------------------------------------------------
// <copyright file="AgreementValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// AgreementValidator.
    /// </summary>
    public class AgreementValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Max = 10000;
        private const uint Min = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgreementValidator"/> class.
        /// </summary>
        public AgreementValidator()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
            this.MinLength = Min;
        }
    }
}
