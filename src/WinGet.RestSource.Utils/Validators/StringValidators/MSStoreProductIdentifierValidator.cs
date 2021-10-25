// -----------------------------------------------------------------------
// <copyright file="MSStoreProductIdentifierValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// MSStoreProductIdentifierValidator.
    /// </summary>
    public class MSStoreProductIdentifierValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const string Pattern = "^[A-Za-z0-9]{12}$";

        /// <summary>
        /// Initializes a new instance of the <see cref="MSStoreProductIdentifierValidator"/> class.
        /// </summary>
        public MSStoreProductIdentifierValidator()
        {
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
        }
    }
}
