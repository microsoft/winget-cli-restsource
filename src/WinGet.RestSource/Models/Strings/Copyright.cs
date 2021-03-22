// -----------------------------------------------------------------------
// <copyright file="Copyright.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Copyright.
    /// </summary>
    public class Copyright : ApiString
    {
        private const bool Nullable = true;
        private const uint Min = 3;
        private const uint Max = 512;

        /// <summary>
        /// Initializes a new instance of the <see cref="Copyright"/> class.
        /// </summary>
        public Copyright()
        {
            this.APIStringName = nameof(Copyright);
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
