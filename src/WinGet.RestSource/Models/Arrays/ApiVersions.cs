// -----------------------------------------------------------------------
// <copyright file="ApiVersions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using Microsoft.WinGet.RestSource.Models.Core;
    using Microsoft.WinGet.RestSource.Models.Strings;

    /// <summary>
    /// ApiVersions.
    /// </summary>
    public class ApiVersions : ApiArray<ApiVersion>
    {
        private const bool Nullable = true;
        private const bool Unique = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiVersions"/> class.
        /// </summary>
        public ApiVersions()
        {
            this.APIArrayName = nameof(ApiVersion);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
        }
    }
}
