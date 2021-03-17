// -----------------------------------------------------------------------
// <copyright file="Channel.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Channel.
    /// </summary>
    public class Channel : ApiString
    {
        private const bool Nullable = true;
        private const uint Min = 2;
        private const uint Max = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        public Channel()
        {
            this.APIStringName = nameof(Channel);
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
