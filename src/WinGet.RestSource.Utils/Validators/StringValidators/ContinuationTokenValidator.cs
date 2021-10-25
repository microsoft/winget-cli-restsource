﻿// -----------------------------------------------------------------------
// <copyright file="ContinuationTokenValidator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// ContinuationTokenValidator.
    /// </summary>
    public class ContinuationTokenValidator : ApiStringValidator
    {
        private const bool Nullable = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuationTokenValidator"/> class.
        /// </summary>
        public ContinuationTokenValidator()
        {
            this.AllowNull = Nullable;
        }
    }
}
