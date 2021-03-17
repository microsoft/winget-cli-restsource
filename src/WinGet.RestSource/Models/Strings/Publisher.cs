// -----------------------------------------------------------------------
// <copyright file="Publisher.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Publisher.
    /// </summary>
    public class Publisher : ApiString
    {
        private const uint Min = 2;
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="Publisher"/> class.
        /// </summary>
        public Publisher()
        {
            this.APIStringName = nameof(Publisher);
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
