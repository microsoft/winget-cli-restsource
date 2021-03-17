// -----------------------------------------------------------------------
// <copyright file="SignatureSha256.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// SignatureSha256.
    /// </summary>
    public class SignatureSha256 : ApiString
    {
        private const bool Nullable = true;
        private const string Pattern = "^[A-Fa-f0-9]{64}$";

        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureSha256"/> class.
        /// </summary>
        public SignatureSha256()
        {
            this.APIStringName = nameof(SignatureSha256);
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
        }
    }
}
