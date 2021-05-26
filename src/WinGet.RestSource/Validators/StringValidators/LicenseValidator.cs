// -----------------------------------------------------------------------
// <copyright file="LicenseValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// LicenseValidator.
    /// </summary>
    public class LicenseValidator : ApiStringValidator
    {
        private const uint Min = 3;
        private const uint Max = 512;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseValidator"/> class.
        /// </summary>
        public LicenseValidator()
        {
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
