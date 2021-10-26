// -----------------------------------------------------------------------
// <copyright file="ProductCodeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// ProductCodeValidator.
    /// </summary>
    public class ProductCodeValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 1;
        private const uint Max = 255;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCodeValidator"/> class.
        /// </summary>
        public ProductCodeValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
