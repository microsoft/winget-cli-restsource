// -----------------------------------------------------------------------
// <copyright file="PackageMatchFields.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Utils.Models.Core;

    /// <summary>
    /// PackageMatchFields.
    /// </summary>
    public class PackageMatchFields : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageMatchFields"/> class.
        /// </summary>
        public PackageMatchFields()
        {
            this.APIArrayName = nameof(PackageMatchFields);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
        }
    }
}
