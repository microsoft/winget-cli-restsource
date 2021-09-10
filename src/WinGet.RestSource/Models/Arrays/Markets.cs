// -----------------------------------------------------------------------
// <copyright file="Markets.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// Markets.
    /// </summary>
    public class Markets : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const int Max = 256;
        private static readonly Type Validator = typeof(MarketValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="Markets"/> class.
        /// </summary>
        public Markets()
        {
            this.APIArrayName = nameof(Markets);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
            this.MemberValidator = Validator;
        }
    }
}
