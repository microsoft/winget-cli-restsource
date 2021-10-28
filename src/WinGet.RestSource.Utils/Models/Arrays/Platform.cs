// -----------------------------------------------------------------------
// <copyright file="Platform.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators;

    /// <summary>
    /// Platform.
    /// </summary>
    public class Platform : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 2;
        private const uint Min = 1;
        private static readonly Type Validator = typeof(PlatformValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="Platform"/> class.
        /// </summary>
        public Platform()
        {
            this.APIArrayName = nameof(Platform);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
            this.MinItems = Min;
            this.MemberValidator = Validator;
        }
    }
}
