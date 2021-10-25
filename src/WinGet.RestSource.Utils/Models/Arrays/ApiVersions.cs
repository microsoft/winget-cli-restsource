﻿// -----------------------------------------------------------------------
// <copyright file="ApiVersions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// ApiVersions.
    /// </summary>
    public class ApiVersions : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private static readonly Type Validator = typeof(ApiVersionValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiVersions"/> class.
        /// </summary>
        public ApiVersions()
        {
            this.APIArrayName = nameof(ApiVersions);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MemberValidator = Validator;
        }
    }
}
