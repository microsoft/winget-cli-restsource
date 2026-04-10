// -----------------------------------------------------------------------
// <copyright file="ProtocolsValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// ProtocolsValidator.
    /// </summary>
    public class ProtocolsValidator : ApiStringValidator
    {
        private const uint Max = 2048;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolsValidator"/> class.
        /// </summary>
        public ProtocolsValidator()
        {
            this.MaxLength = Max;
        }
    }
}
