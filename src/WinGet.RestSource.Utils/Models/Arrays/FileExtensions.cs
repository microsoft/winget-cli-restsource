// -----------------------------------------------------------------------
// <copyright file="FileExtensions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// FileExtensions.
    /// </summary>
    public class FileExtensions : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 256;
        private static readonly Type Validator = typeof(FileExtensionsValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="FileExtensions"/> class.
        /// </summary>
        public FileExtensions()
        {
            this.APIArrayName = nameof(FileExtensions);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
            this.MemberValidator = Validator;
        }
    }
}
