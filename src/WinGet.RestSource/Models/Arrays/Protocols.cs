// -----------------------------------------------------------------------
// <copyright file="Protocols.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Validators.StringValidators;

    /// <summary>
    /// Protocols.
    /// </summary>
    public class Protocols : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 16;
        private static readonly Type Validator = typeof(ProtocolsValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="Protocols"/> class.
        /// </summary>
        public Protocols()
        {
            this.APIArrayName = nameof(Protocols);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
            this.MemberValidator = Validator;
        }
    }
}
