﻿// -----------------------------------------------------------------------
// <copyright file="ApiVersionValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// ApiVersionValidator.
    /// </summary>
    public class ApiVersionValidator : ApiStringValidator
    {
        private const string Pattern = "^([0-9]+\\.){0,3}(\\*|[0-9]+)$";
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiVersionValidator"/> class.
        /// </summary>
        public ApiVersionValidator()
        {
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
