// -----------------------------------------------------------------------
// <copyright file="MatchTypeValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators
{
    using System.Collections.Generic;
    using Microsoft.WinGet.RestSource.Utils.Constants.Enumerations;

    /// <summary>
    /// MatchTypeValidatorSchema.
    /// </summary>
    public class MatchTypeValidator : ApiEnumValidator
    {
        private const bool Nullable = true;
        private List<string> enumList = new List<string>
        {
            MatchType.Exact,
            MatchType.CaseInsensitive,
            MatchType.StartsWith,
            MatchType.Substring,
            MatchType.Wildcard,
            MatchType.Fuzzy,
            MatchType.FuzzySubstring,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchTypeValidator"/> class.
        /// </summary>
        public MatchTypeValidator()
        {
            this.AllowNull = Nullable;
            this.Values = this.enumList;
        }
    }
}
