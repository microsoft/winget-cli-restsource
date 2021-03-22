// -----------------------------------------------------------------------
// <copyright file="PackageName.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// PackageName.
    /// </summary>
    public class PackageName : ApiString
    {
        private const uint Min = 2;
        private const uint Max = 64;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageName"/> class.
        /// </summary>
        public PackageName()
        {
            this.APIStringName = nameof(PackageName);
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
