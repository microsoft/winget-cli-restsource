// -----------------------------------------------------------------------
// <copyright file="InstallationMetadataFileRelativeFilePathValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// RelativeFilePath.
    /// </summary>
    public class InstallationMetadataFileRelativeFilePathValidator : ApiStringValidator
    {
        private const bool Nullable = false;
        private const uint Max = 512;
        private const uint Min = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationMetadataFileRelativeFilePathValidator"/> class.
        /// </summary>
        public InstallationMetadataFileRelativeFilePathValidator()
        {
            this.AllowNull = Nullable;
            this.MaxLength = Max;
            this.MinLength = Min;
        }
    }
}
