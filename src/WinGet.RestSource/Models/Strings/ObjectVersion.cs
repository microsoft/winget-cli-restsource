// -----------------------------------------------------------------------
// <copyright file="ObjectVersion.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// ObjectVersion.
    /// </summary>
    public class ObjectVersion : ApiString
    {
        private const string Pattern = "^([0-9]+\\.){0,3}(\\*|[0-9]+)$";
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectVersion"/> class.
        /// </summary>
        public ObjectVersion()
        {
            this.APIStringName = nameof(ObjectVersion);
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
