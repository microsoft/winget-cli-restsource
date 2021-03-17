// -----------------------------------------------------------------------
// <copyright file="Switch.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Switch.
    /// </summary>
    public class Switch : ApiString
    {
        private const bool Nullable = true;
        private const uint Min = 1;
        private const uint Max = 512;

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch"/> class.
        /// </summary>
        public Switch()
        {
            this.APIStringName = nameof(Switch);
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
