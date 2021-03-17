// -----------------------------------------------------------------------
// <copyright file="Versions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Schemas
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Versions.
    /// </summary>
    public class Versions : ApiArray<Schemas.Version>
    {
        private const bool Nullable = true;
        private const bool Unique = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Versions"/> class.
        /// </summary>
        public Versions()
        {
            this.APIArrayName = nameof(Versions);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
        }
    }
}
