// -----------------------------------------------------------------------
// <copyright file="Tag.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Tag.
    /// </summary>
    public class Tag : ApiString
    {
        private const bool Nullable = true;
        private const string Pattern = "^\\S+$";
        private const uint Max = 32;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        public Tag()
        {
            this.APIStringName = nameof(Tag);
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
