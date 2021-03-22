// -----------------------------------------------------------------------
// <copyright file="SourceIdentifier.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// SourceIdentifier.
    /// </summary>
    public class SourceIdentifier : ApiString
    {
        private const uint Min = 3;
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceIdentifier"/> class.
        /// </summary>
        public SourceIdentifier()
        {
            this.APIStringName = nameof(SourceIdentifier);
            this.MinLength = Min;
            this.MaxLength = Max;
        }
    }
}
