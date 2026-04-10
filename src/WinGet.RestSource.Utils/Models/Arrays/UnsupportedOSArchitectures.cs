// -----------------------------------------------------------------------
// <copyright file="UnsupportedOSArchitectures.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators;

    /// <summary>
    /// UnsupportedOSArchitectures.
    /// </summary>
    public class UnsupportedOSArchitectures : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private static readonly Type Validator = typeof(UnsupportedOSArchitecturesValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedOSArchitectures"/> class.
        /// </summary>
        public UnsupportedOSArchitectures()
        {
            this.APIArrayName = nameof(UnsupportedOSArchitectures);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MemberValidator = Validator;
        }
    }
}
