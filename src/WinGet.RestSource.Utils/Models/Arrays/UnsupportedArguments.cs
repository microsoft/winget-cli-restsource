// -----------------------------------------------------------------------
// <copyright file="UnsupportedArguments.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.EnumValidators;

    /// <summary>
    /// UnsupportedArguments.
    /// </summary>
    public class UnsupportedArguments : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private static readonly Type Validator = typeof(UnsupportedArgumentValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedArguments"/> class.
        /// </summary>
        public UnsupportedArguments()
        {
            this.APIArrayName = nameof(UnsupportedArguments);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MemberValidator = Validator;
        }
    }
}
