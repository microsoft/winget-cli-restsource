// -----------------------------------------------------------------------
// <copyright file="License.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// License.
    /// </summary>
    public class License : ApiString
    {
        private const uint Min = 3;
        private const uint Max = 512;

        /// <summary>
        /// Initializes a new instance of the <see cref="License"/> class.
        /// </summary>
        public License()
        {
            this.APIStringName = nameof(License);
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
