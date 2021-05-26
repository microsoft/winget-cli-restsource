// -----------------------------------------------------------------------
// <copyright file="SwitchValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// SwitchValidator.
    /// </summary>
    public class SwitchValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 1;
        private const uint Max = 512;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchValidator"/> class.
        /// </summary>
        public SwitchValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
