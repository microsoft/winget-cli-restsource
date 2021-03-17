// -----------------------------------------------------------------------
// <copyright file="Description.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// Description.
    /// </summary>
    public class Description : ApiString
    {
        private const bool Nullable = true;
        private const uint Min = 3;
        private const uint Max = 10000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Description"/> class.
        /// </summary>
        public Description()
        {
            this.APIStringName = nameof(Description);
            this.AllowNull = Nullable;
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
