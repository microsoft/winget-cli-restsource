// -----------------------------------------------------------------------
// <copyright file="SearchResponseTestHelper.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit.Abstractions;

    /// <summary>
    /// Search Response test helper.
    /// </summary>
    public class SearchResponseTestHelper : IXunitSerializable
    {
        /// <summary>
        /// Gets or sets the package identifier.
        /// </summary>
        public string PackageIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the package publisher.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets the expected search result versions.
        /// </summary>
        public string[] ExpectedVersions { get; set; }

        /// <inheritdoc/>
        public void Deserialize(IXunitSerializationInfo info)
        {
            this.PackageIdentifier = info.GetValue<string>(nameof(this.PackageIdentifier));
            this.Publisher = info.GetValue<string>(nameof(this.Publisher));
            this.ExpectedVersions = info.GetValue<string[]>(nameof(this.ExpectedVersions));
        }

        /// <inheritdoc/>
        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(this.PackageIdentifier), this.PackageIdentifier, typeof(string));
            info.AddValue(nameof(this.Publisher), this.Publisher, typeof(string));
            info.AddValue(nameof(this.ExpectedVersions), this.ExpectedVersions, typeof(string[]));
        }
    }
}
