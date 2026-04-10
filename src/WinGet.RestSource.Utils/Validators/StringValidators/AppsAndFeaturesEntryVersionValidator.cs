// -----------------------------------------------------------------------
// <copyright file="AppsAndFeaturesEntryVersionValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// DisplayVersionValidator.
    /// </summary>
    public class AppsAndFeaturesEntryVersionValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 1;
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppsAndFeaturesEntryVersionValidator"/> class.
        /// </summary>
        public AppsAndFeaturesEntryVersionValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
