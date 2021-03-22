// -----------------------------------------------------------------------
// <copyright file="PackageFamilyName.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Strings
{
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// PackageFamilyName.
    /// </summary>
    public class PackageFamilyName : ApiString
    {
        private const bool Nullable = true;
        private const string Pattern = "^[A-Za-z0-9][-\\.A-Za-z0-9]+_[A-Za-z0-9]{13}$";
        private const uint Max = 255;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageFamilyName"/> class.
        /// </summary>
        public PackageFamilyName()
        {
            this.APIStringName = nameof(PackageFamilyName);
            this.AllowNull = Nullable;
            this.MatchPattern = Pattern;
            this.MaxLength = Max;
        }
    }
}
