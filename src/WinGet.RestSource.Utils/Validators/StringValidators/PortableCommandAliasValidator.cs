// -----------------------------------------------------------------------
// <copyright file="PortableCommandAliasValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// RelativeFilePath.
    /// </summary>
    public class PortableCommandAliasValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const uint Max = 40;
        private const uint Min = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortableCommandAliasValidator"/> class.
        /// </summary>
        public PortableCommandAliasValidator()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
            this.MinLength = Min;
        }
    }
}
