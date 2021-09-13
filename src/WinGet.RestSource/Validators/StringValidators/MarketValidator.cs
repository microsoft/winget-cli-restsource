// -----------------------------------------------------------------------
// <copyright file="MarketValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Validators.StringValidators
{
    /// <summary>
    /// MarketValidator.
    /// </summary>
    public class MarketValidator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const string Pattern = "^[A-Z]{2}$";

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketValidator"/> class.
        /// </summary>
        public MarketValidator()
        {
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
        }
    }
}
