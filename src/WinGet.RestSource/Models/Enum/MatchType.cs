// -----------------------------------------------------------------------
// <copyright file="MatchType.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Enum
{
    /// <summary>
    /// Match Type Schema.
    /// </summary>
    public enum MatchType
    {
        /// <summary>
        /// Exact Match Type.
        /// </summary>
        Exact,

        /// <summary>
        /// Case Insensitive Match Type.
        /// </summary>
        CaseInsensitive,

        /// <summary>
        /// Starts With Match Type.
        /// </summary>
        StartsWith,

        /// <summary>
        /// Sub String Match Type.
        /// </summary>
        Substring,
    }
}