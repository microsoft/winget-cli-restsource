// -----------------------------------------------------------------------
// <copyright file="InstallModes.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// InstallModes.
    /// </summary>
    public class InstallModes : ApiArray<Enum.InstallMode>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallModes"/> class.
        /// </summary>
        public InstallModes()
        {
            this.APIArrayName = nameof(InstallModes);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
