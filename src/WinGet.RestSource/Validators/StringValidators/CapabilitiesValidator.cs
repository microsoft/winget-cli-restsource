// -----------------------------------------------------------------------
// <copyright file="CapabilitiesValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// CapabilitiesValidator.
    /// </summary>
    public class CapabilitiesValidator : ApiStringValidator
    {
        private const uint Min = 1;
        private const uint Max = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilitiesValidator"/> class.
        /// </summary>
        public CapabilitiesValidator()
        {
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
