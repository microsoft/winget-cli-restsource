﻿// -----------------------------------------------------------------------
// <copyright file="AgreementLabelValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// AgreementLabelValidator.
    /// </summary>
    public class AgreementLabelValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Max = 100;
        private const uint Min = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgreementLabelValidator"/> class.
        /// </summary>
        public AgreementLabelValidator()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
            this.MinLength = Min;
        }
    }
}
