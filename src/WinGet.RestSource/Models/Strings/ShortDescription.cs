// -----------------------------------------------------------------------
// <copyright file="ShortDescription.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// ShortDescription.
    /// </summary>
    public class ShortDescription : ApiString
    {
        private const uint Min = 3;
        private const uint Max = 256;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortDescription"/> class.
        /// </summary>
        public ShortDescription()
        {
            this.APIStringName = nameof(ShortDescription);
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
