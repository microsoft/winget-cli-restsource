// -----------------------------------------------------------------------
// <copyright file="AppsAndFeaturesEntryVersions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Utils.Models.Arrays
{
    using System;
    using Microsoft.WinGet.RestSource.Utils.Models.Core;
    using Microsoft.WinGet.RestSource.Utils.Validators.StringValidators;

    /// <summary>
    /// List of ARP entries.
    /// </summary>
    public class AppsAndFeaturesEntryVersions : ApiArray<string>
    {
        private const bool Nullable = true;
        private const bool Unique = false;
        private static readonly Type Validator = typeof(AppsAndFeaturesEntryVersionValidator);

        /// <summary>
        /// Initializes a new instance of the <see cref="AppsAndFeaturesEntryVersions"/> class.
        /// </summary>
        public AppsAndFeaturesEntryVersions()
        {
            this.APIArrayName = nameof(AppsAndFeaturesEntryVersions);
            this.AllowNull = Nullable;
            this.UniqueItems = Unique;
            this.MemberValidator = Validator;
        }
    }
}
