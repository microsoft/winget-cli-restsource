// -----------------------------------------------------------------------
// <copyright file="SignatureSha256Validator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// SignatureSha256Validator.
    /// </summary>
    public class SignatureSha256Validator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const string Pattern = "^[A-Fa-f0-9]{64}$";

        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureSha256Validator"/> class.
        /// </summary>
        public SignatureSha256Validator()
        {
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
        }
    }
}
