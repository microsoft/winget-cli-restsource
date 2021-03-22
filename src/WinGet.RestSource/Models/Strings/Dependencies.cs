// -----------------------------------------------------------------------
// <copyright file="Dependencies.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Dependencies.
    /// </summary>
    public class Dependencies : ApiString
    {
        private const uint Min = 1;
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dependencies"/> class.
        /// </summary>
        public Dependencies()
        {
            this.APIStringName = nameof(Dependencies);
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
