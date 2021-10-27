// -----------------------------------------------------------------------
// <copyright file="MatchType.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Constants.Enumerations
{
    /// <summary>
    /// Match Type Constants.
    /// </summary>
    public class MatchType
    {
        /// <summary>
        /// Exact.
        /// </summary>
        public const string Exact = "Exact";

        /// <summary>
        /// CaseInsensitive.
        /// </summary>
        public const string CaseInsensitive = "CaseInsensitive";

        /// <summary>
        /// StartsWith.
        /// </summary>
        public const string StartsWith = "StartsWith";

        /// <summary>
        /// Substring.
        /// </summary>
        public const string Substring = "Substring";

        /*********************************
         * These are currently unsupported
         *********************************
        /// <summary>
        /// Wildcard.
        /// </summary>
        public const string Wildcard = "Wildcard";

        /// <summary>
        /// Fuzzy.
        /// </summary>
        public const string Fuzzy = "Fuzzy";

        /// <summary>
        /// FuzzySubstring.
        /// </summary>
        public const string FuzzySubstring = "FuzzySubstring";
        */
    }
}
