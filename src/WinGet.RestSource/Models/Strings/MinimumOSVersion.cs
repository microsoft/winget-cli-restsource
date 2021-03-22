// -----------------------------------------------------------------------
// <copyright file="MinimumOSVersion.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// MinimumOSVersion.
    /// </summary>
    public class MinimumOSVersion : ApiString
    {
        private const bool Nullable = true;
        private const string Pattern = "^(0|[1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])(\\.(0|[1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])){0,3}$";

        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumOSVersion"/> class.
        /// </summary>
        public MinimumOSVersion()
        {
            this.APIStringName = nameof(MinimumOSVersion);
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
        }
    }
}
