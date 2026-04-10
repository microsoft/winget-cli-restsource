// -----------------------------------------------------------------------
// <copyright file="Versions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Schemas
{
    using Microsoft.WinGet.RestSource.Utils.Models.Core;

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
