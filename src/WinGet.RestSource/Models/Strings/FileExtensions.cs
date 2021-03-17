// -----------------------------------------------------------------------
// <copyright file="FileExtensions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// FileExtensions.
    /// </summary>
    public class FileExtensions : ApiString
    {
        private const string Pattern = "^[^\\\\/:\\*\\?\"<>\\|\\x01-\\x1f]+$";
        private const uint Max = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileExtensions"/> class.
        /// </summary>
        public FileExtensions()
        {
            this.APIStringName = nameof(FileExtensions);
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
