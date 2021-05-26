// -----------------------------------------------------------------------
// <copyright file="CustomSwitchValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// CustomSwitchValidator.
    /// </summary>
    public class CustomSwitchValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 1;
        private const uint Max = 2048;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSwitchValidator"/> class.
        /// </summary>
        public CustomSwitchValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
