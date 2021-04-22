// -----------------------------------------------------------------------
// <copyright file="InstallerIdentifierValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// InstallerIdentifierValidatorValidator.
    /// </summary>
    public class InstallerIdentifierValidator : ApiStringValidator
    {
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerIdentifierValidator"/> class.
        /// </summary>
        public InstallerIdentifierValidator()
        {
            this.MaxLength = Max;
        }
    }
}
