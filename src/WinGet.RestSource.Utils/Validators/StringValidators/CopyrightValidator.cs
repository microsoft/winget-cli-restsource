// -----------------------------------------------------------------------
// <copyright file="CopyrightValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// CopyrightValidator.
    /// </summary>
    public class CopyrightValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 3;
        private const uint Max = 512;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyrightValidator"/> class.
        /// </summary>
        public CopyrightValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
