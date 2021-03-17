// -----------------------------------------------------------------------
// <copyright file="Commands.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Commands.
    /// </summary>
    public class Commands : ApiString
    {
        private const uint Min = 2;
        private const uint Max = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="Commands"/> class.
        /// </summary>
        public Commands()
        {
            this.APIStringName = nameof(Commands);
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
