// -----------------------------------------------------------------------
// <copyright file="MicrosoftEntraIdResourceScopeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// MicrosoftEntraIdResourceScopeValidator.
    /// </summary>
    public class MicrosoftEntraIdResourceScopeValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Max = 512;
        private const uint Min = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftEntraIdResourceScopeValidator"/> class.
        /// </summary>
        public MicrosoftEntraIdResourceScopeValidator()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
            this.MinLength = Min;
        }
    }
}
