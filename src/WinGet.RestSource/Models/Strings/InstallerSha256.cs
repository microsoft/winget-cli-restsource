// -----------------------------------------------------------------------
// <copyright file="InstallerSha256.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// InstallerSha256.
    /// </summary>
    public class InstallerSha256 : ApiString
    {
        private const string Pattern = "^[A-Fa-f0-9]{64}$";

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerSha256"/> class.
        /// </summary>
        public InstallerSha256()
        {
            this.APIStringName = nameof(InstallerSha256);
            this.MatchPattern = Pattern;
        }
    }
}
