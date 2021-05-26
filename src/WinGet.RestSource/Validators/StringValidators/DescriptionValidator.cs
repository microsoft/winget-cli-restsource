// -----------------------------------------------------------------------
// <copyright file="DescriptionValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// DescriptionValidator.
    /// </summary>
    public class DescriptionValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 3;
        private const uint Max = 10000;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionValidator"/> class.
        /// </summary>
        public DescriptionValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
