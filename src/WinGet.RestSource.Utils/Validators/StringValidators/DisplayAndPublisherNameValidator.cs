// -----------------------------------------------------------------------
// <copyright file="DisplayAndPublisherNameValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// DisplayAndPublisherNameValidator.
    /// </summary>
    public class DisplayAndPublisherNameValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Min = 1;
        private const uint Max = 256;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayAndPublisherNameValidator"/> class.
        /// </summary>
        public DisplayAndPublisherNameValidator()
        {
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
