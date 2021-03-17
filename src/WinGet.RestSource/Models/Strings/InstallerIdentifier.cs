// -----------------------------------------------------------------------
// <copyright file="InstallerIdentifier.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// InstallerIdentifier.
    /// </summary>
    public class InstallerIdentifier : ApiString
    {
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerIdentifier"/> class.
        /// </summary>
        public InstallerIdentifier()
        {
            this.APIStringName = nameof(InstallerIdentifier);
            this.MaxLength = Max;
        }
    }
}
