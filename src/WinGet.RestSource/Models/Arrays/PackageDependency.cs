// -----------------------------------------------------------------------
// <copyright file="PackageDependency.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// PackageDependency.
    /// </summary>
    public class PackageDependency : ApiArray<Objects.PackageDependency>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageDependency"/> class.
        /// </summary>
        public PackageDependency()
        {
            this.APIArrayName = nameof(PackageDependency);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
