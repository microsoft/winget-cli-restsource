// -----------------------------------------------------------------------
// <copyright file="PackageFamilyNames.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// PackageFamilyNames.
    /// </summary>
    public class PackageFamilyNames : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private static readonly Type Validator = typeof(PackageFamilyNameValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageFamilyNames"/> class.
        /// </summary>
        public PackageFamilyNames()
        {
            this.APIArrayName = nameof(PackageFamilyNames);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MemberValidator = Validator;
        }
    }
}
