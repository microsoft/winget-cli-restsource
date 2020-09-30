// -----------------------------------------------------------------------
// <copyright file="PackageCore.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Models.Core
{
    using Newtonsoft.Json;

    /// <summary>
    /// This is the core package model.
    /// </summary>
    public class PackageCore
    {
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