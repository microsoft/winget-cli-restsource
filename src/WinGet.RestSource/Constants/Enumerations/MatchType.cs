// -----------------------------------------------------------------------
// <copyright file="MatchType.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Constants.Enumerations
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
    }
}
