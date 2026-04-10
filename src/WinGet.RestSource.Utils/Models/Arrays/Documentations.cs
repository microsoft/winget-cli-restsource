// -----------------------------------------------------------------------
// <copyright file="Documentations.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Utils.Models.Core;

    /// <summary>
    /// Documentations.
    /// </summary>
    public class Documentations : ApiArray<Objects.Documentation>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 256;

        /// <summary>
        /// Initializes a new instance of the <see cref="Documentations"/> class.
        /// </summary>
        public Documentations()
        {
            this.APIArrayName = nameof(Documentations);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
