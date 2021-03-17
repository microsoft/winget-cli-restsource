// -----------------------------------------------------------------------
// <copyright file="Protocols.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Protocols.
    /// </summary>
    public class Protocols : ApiArray<Strings.Protocols>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="Protocols"/> class.
        /// </summary>
        public Protocols()
        {
            this.APIArrayName = nameof(Protocols);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
