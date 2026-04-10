// -----------------------------------------------------------------------
// <copyright file="ReleaseNotesValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// ReleaseNotesValidator.
    /// </summary>
    public class ReleaseNotesValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Max = 10000;
        private const uint Min = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseNotesValidator"/> class.
        /// </summary>
        public ReleaseNotesValidator()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
            this.MinLength = Min;
        }
    }
}
