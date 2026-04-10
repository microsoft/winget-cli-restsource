// -----------------------------------------------------------------------
// <copyright file="AgreementsIdentifierValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// AgreementIdentifierValidator.
    /// </summary>
    public class AgreementsIdentifierValidator : ApiStringValidator
    {
        private const bool Nullable = false;
        private const uint Max = 128;
        private const uint Min = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgreementsIdentifierValidator"/> class.
        /// </summary>
        public AgreementsIdentifierValidator()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
            this.MinLength = Min;
        }
    }
}
