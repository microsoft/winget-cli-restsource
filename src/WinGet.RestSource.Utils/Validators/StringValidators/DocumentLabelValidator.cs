// -----------------------------------------------------------------------
// <copyright file="DocumentLabelValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// DocumentLabel.
    /// </summary>
    public class DocumentLabelValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 1;
        private const uint Max = 256;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentLabelValidator"/> class.
        /// </summary>
        public DocumentLabelValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
