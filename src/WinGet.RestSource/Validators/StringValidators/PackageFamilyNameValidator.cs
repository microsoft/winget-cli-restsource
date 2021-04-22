// -----------------------------------------------------------------------
// <copyright file="PackageFamilyNameValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// PackageFamilyNameValidator.
    /// </summary>
    public class PackageFamilyNameValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const string Pattern = "^[A-Za-z0-9][-\\.A-Za-z0-9]+_[A-Za-z0-9]{13}$";
        private const uint Max = 255;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageFamilyNameValidator"/> class.
        /// </summary>
        public PackageFamilyNameValidator()
        {
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
