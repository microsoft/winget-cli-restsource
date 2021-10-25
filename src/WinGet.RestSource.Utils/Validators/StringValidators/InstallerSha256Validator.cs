﻿// -----------------------------------------------------------------------
// <copyright file="InstallerSha256Validator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Validators.StringValidators
{
    /// <summary>
    /// InstallerSha256.
    /// </summary>
    public class InstallerSha256Validator : ApiStringValidator
    {
        private const bool Nullable = true;
        private const string Pattern = "^[A-Fa-f0-9]{64}$";

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerSha256Validator"/> class.
        /// </summary>
        public InstallerSha256Validator()
        {
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
        }
    }
}
