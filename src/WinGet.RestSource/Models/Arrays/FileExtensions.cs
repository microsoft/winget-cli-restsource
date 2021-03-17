// -----------------------------------------------------------------------
// <copyright file="FileExtensions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// FileExtensions.
    /// </summary>
    public class FileExtensions : ApiArray<Strings.FileExtensions>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 256;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileExtensions"/> class.
        /// </summary>
        public FileExtensions()
        {
            this.APIArrayName = nameof(FileExtensions);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
