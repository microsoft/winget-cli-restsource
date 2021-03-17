// -----------------------------------------------------------------------
// <copyright file="Capabilities.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Capabilities.
    /// </summary>
    public class Capabilities : ApiString
    {
        private const uint Min = 1;
        private const uint Max = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="Capabilities"/> class.
        /// </summary>
        public Capabilities()
        {
            this.APIStringName = nameof(Capabilities);
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
