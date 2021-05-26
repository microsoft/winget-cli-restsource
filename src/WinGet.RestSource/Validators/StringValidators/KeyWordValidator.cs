// -----------------------------------------------------------------------
// <copyright file="KeyWordValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// KeyWordValidator.
    /// </summary>
    public class KeyWordValidator : ApiStringValidator
    {
        private const uint Max = 255;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyWordValidator"/> class.
        /// </summary>
        public KeyWordValidator()
        {
            this.MaxLength = Max;
        }
    }
}
