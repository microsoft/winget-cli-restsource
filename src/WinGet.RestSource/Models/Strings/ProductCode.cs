// -----------------------------------------------------------------------
// <copyright file="ProductCode.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// ProductCode.
    /// </summary>
    public class ProductCode : ApiString
    {
        private const bool Nullable = true;
        private const uint Min = 1;
        private const uint Max = 255;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCode"/> class.
        /// </summary>
        public ProductCode()
        {
            this.APIStringName = nameof(ProductCode);
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
