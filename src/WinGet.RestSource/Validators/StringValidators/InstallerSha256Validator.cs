// -----------------------------------------------------------------------
// <copyright file="InstallerSha256Validator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// InstallerSha256.
    /// </summary>
    public class InstallerSha256Validator : ApiStringValidator
    {
        private const string Pattern = "^[A-Fa-f0-9]{64}$";

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerSha256Validator"/> class.
        /// </summary>
        public InstallerSha256Validator()
        {
            this.MatchPattern = Pattern;
        }
    }
}
