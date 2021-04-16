// -----------------------------------------------------------------------
// <copyright file="PublisherValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
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
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublisherValidator"/> class.
        /// </summary>
        public PublisherValidator()
        {
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
