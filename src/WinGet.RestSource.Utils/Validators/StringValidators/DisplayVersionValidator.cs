// -----------------------------------------------------------------------
// <copyright file="DisplayVersionValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// DisplayVersionValidator.
    /// </summary>
    public class DisplayVersionValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 1;
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayVersionValidator"/> class.
        /// </summary>
        public DisplayVersionValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
