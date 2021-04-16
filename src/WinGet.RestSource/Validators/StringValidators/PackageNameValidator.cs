// -----------------------------------------------------------------------
// <copyright file="PackageNameValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// PackageNameValidator.
    /// </summary>
    public class PackageNameValidator : ApiStringValidator
    {
        private const uint Min = 2;
        private const uint Max = 64;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageNameValidator"/> class.
        /// </summary>
        public PackageNameValidator()
        {
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
