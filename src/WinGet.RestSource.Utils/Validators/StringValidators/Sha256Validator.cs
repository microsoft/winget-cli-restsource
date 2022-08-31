// -----------------------------------------------------------------------
// <copyright file="Sha256Validator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// SignatureSha256Validator.
    /// </summary>
    public class Sha256Validator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const string Pattern = "^[A-Fa-f0-9]{64}$";

        /// <summary>
        /// Initializes a new instance of the <see cref="Sha256Validator"/> class.
        /// </summary>
        public Sha256Validator()
        {
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
        }
    }
}
