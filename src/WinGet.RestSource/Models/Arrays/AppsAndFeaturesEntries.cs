// -----------------------------------------------------------------------
// <copyright file="AppsAndFeaturesEntries.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Models.Core;

    /// <summary>
    /// List of ARP entries.
    /// </summary>
    public class AppsAndFeaturesEntries : ApiArray<Objects.AppsAndFeatures>
    {
        private const bool Nullable = true;
        private const bool Unique = true;
        private const uint Max = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppsAndFeaturesEntries"/> class.
        /// </summary>
        public AppsAndFeaturesEntries()
        {
            this.APIArrayName = nameof(AppsAndFeaturesEntries);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MaxItems = Max;
        }
    }
}
