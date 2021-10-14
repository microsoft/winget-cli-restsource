// -----------------------------------------------------------------------
// <copyright file="PublisherValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// PublisherValidator.
    /// </summary>
    public class PublisherValidator : ApiStringValidator
    {
        private const uint Min = 2;
        private const uint Max = 256;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublisherValidator"/> class.
        /// </summary>
        public PublisherValidator()
        {
            this.AllowNull = true;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
