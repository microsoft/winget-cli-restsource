// -----------------------------------------------------------------------
// <copyright file="DependenciesValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// Dependencies.
    /// </summary>
    public class DependenciesValidator : ApiStringValidator
    {
        private const uint Min = 1;
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependenciesValidator"/> class.
        /// </summary>
        public DependenciesValidator()
        {
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
