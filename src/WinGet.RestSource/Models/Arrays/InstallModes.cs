// -----------------------------------------------------------------------
// <copyright file="InstallModes.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators.EnumValidators;

    /// <summary>
    /// InstallModes.
    /// </summary>
    public class InstallModes : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 3;
        private static readonly Type Validator = typeof(InstallModeValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallModes"/> class.
        /// </summary>
        public InstallModes()
        {
            this.APIArrayName = nameof(InstallModes);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
            this.MemberValidator = Validator;
        }
    }
}
