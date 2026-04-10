// -----------------------------------------------------------------------
// <copyright file="Icons.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Utils.Models.Core;

    /// <summary>
    /// Icons.
    /// </summary>
    public class Icons : ApiArray<Objects.Icon>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 1024;

        /// <summary>
        /// Initializes a new instance of the <see cref="Icons"/> class.
        /// </summary>
        public Icons()
        {
            this.APIArrayName = nameof(Icons);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
