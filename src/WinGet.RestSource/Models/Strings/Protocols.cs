// -----------------------------------------------------------------------
// <copyright file="Protocols.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Protocols.
    /// </summary>
    public class Protocols : ApiString
    {
        private const string Pattern = "^[a-z][-a-z0-9\\.\\+]*$";
        private const uint Max = 39;

        /// <summary>
        /// Initializes a new instance of the <see cref="Protocols"/> class.
        /// </summary>
        public Protocols()
        {
            this.APIStringName = nameof(Protocols);
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
