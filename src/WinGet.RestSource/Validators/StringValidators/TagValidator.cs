// -----------------------------------------------------------------------
// <copyright file="TagValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// TagValidator.
    /// </summary>
    public class TagValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 1;
        private const uint Max = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagValidator"/> class.
        /// </summary>
        public TagValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
