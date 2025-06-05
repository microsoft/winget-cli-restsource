// -----------------------------------------------------------------------
// <copyright file="Capabilities.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// Capabilities.
    /// </summary>
    public class Capabilities : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 1000;
        private static readonly Type Validator = typeof(CapabilitiesValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="Capabilities"/> class.
        /// </summary>
        public Capabilities()
        {
            this.APIArrayName = nameof(Capabilities);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
            this.MemberValidator = Validator;
        }
    }
}
