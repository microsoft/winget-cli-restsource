// -----------------------------------------------------------------------
// <copyright file="ProductCodes.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// ProductCodes.
    /// </summary>
    public class ProductCodes : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private static readonly Type Validator = typeof(ProductCodeValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCodes"/> class.
        /// </summary>
        public ProductCodes()
        {
            this.APIArrayName = nameof(ProductCodes);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MemberValidator = Validator;
        }
    }
}
