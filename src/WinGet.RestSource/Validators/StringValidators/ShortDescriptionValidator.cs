// -----------------------------------------------------------------------
// <copyright file="ShortDescriptionValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// ShortDescriptionValidator.
    /// </summary>
    public class ShortDescriptionValidator : ApiStringValidator
    {
        private const uint Min = 3;
        private const uint Max = 256;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortDescriptionValidator"/> class.
        /// </summary>
        public ShortDescriptionValidator()
        {
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
