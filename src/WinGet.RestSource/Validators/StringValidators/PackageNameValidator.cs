// -----------------------------------------------------------------------
// <copyright file="PackageNameValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
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
        private const uint Max = 256;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageNameValidator"/> class.
        /// </summary>
        public PackageNameValidator()
        {
            this.AllowNull = true;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
