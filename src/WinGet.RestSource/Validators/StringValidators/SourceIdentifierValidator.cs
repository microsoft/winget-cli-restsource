// -----------------------------------------------------------------------
// <copyright file="SourceIdentifierValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// SourceIdentifierValidator.
    /// </summary>
    public class SourceIdentifierValidator : ApiStringValidator
    {
        private const uint Min = 3;
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceIdentifierValidator"/> class.
        /// </summary>
        public SourceIdentifierValidator()
        {
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
