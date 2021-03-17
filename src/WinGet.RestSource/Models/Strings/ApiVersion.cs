// -----------------------------------------------------------------------
// <copyright file="ApiVersion.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// ApiVersion.
    /// </summary>
    public class ApiVersion : ApiString
    {
        private const string Pattern = "^([0-9]+\\.){0,3}(\\*|[0-9]+)$";
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiVersion"/> class.
        /// </summary>
        public ApiVersion()
        {
            this.APIStringName = nameof(ApiVersion);
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
