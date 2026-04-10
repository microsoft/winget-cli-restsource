// -----------------------------------------------------------------------
// <copyright file="DefaultInstallLocationValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// DefaultInstallLocationValidator.
    /// </summary>
    public class DefaultInstallLocationValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Max = 2048;
        private const uint Min = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultInstallLocationValidator"/> class.
        /// </summary>
        public DefaultInstallLocationValidator()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
            this.MinLength = Min;
        }
    }
}
