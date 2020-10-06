// -----------------------------------------------------------------------
// <copyright file="PackageCore.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Core
{
    using Newtonsoft.Json;

    /// <summary>
    /// This is the packageCore package model.
    /// </summary>
    public class PackageCore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageCore"/> class.
        /// </summary>
        public PackageCore()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageCore"/> class.
        /// </summary>
        /// <param name="packageCore">Package Core.</param>
        public PackageCore(PackageCore packageCore)
        {
            this.Id = packageCore.Id;
            this.DefaultLocale = packageCore.DefaultLocale;
        }

        /// <summary>
        /// Gets or sets package id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets default locale.
        /// </summary>
        [JsonProperty("defaultLocale")]
        public string DefaultLocale { get; set; }
    }
}