// -----------------------------------------------------------------------
// <copyright file="CommandsValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// CommandsValidator.
    /// </summary>
    public class CommandsValidator : ApiStringValidator
    {
        private const uint Min = 1;
        private const uint Max = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsValidator"/> class.
        /// </summary>
        public CommandsValidator()
        {
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
