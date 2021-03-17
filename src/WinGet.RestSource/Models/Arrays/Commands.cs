// -----------------------------------------------------------------------
// <copyright file="Commands.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Commands.
    /// </summary>
    public class Commands : ApiArray<Strings.Commands>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="Commands"/> class.
        /// </summary>
        public Commands()
        {
            this.APIArrayName = nameof(Commands);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
