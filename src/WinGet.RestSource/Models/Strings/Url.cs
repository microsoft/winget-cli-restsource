// -----------------------------------------------------------------------
// <copyright file="Url.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Url.
    /// </summary>
    public class Url : ApiString
    {
        private const bool Nullable = true;
        private const string Pattern = "^([Hh][Tt][Tt][Pp][Ss]?)://";
        private const uint Max = 2000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Url"/> class.
        /// </summary>
        public Url()
        {
            this.APIStringName = nameof(Url);
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
