// -----------------------------------------------------------------------
// <copyright file="BaseTestData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common
{
    using System.Collections;
    using System.Collections.Generic;

    /// <inheritdoc />
    public abstract class BaseTestData : IEnumerable<object[]>
    {
        /// <summary>
        /// Gets or sets data.
        /// </summary>
        protected IEnumerable<object[]> Data { get; set; }

        /// <summary>
        /// Get Enumerator.
        /// </summary>
        /// <returns>IEnumerator object[].</returns>
        public IEnumerator<object[]> GetEnumerator()
        {
            return this.Data.GetEnumerator();
        }

        /// <summary>
        /// IEnumerator.
        /// </summary>
        /// <returns>IEnumerator IEnumerable.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
