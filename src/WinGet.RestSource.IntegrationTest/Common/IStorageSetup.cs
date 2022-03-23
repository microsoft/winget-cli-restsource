// -----------------------------------------------------------------------
// <copyright file="IStorageSetup.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common
{
    using System.Threading.Tasks;
    using Microsoft.WinGet.RestSource.IntegrationTest.Common.Fixtures;

    /// <summary>
    /// Interface to setup storage.
    /// </summary>
    public interface IStorageSetup
    {
        /// <summary>
        /// Sets up the storage state.
        /// </summary>
        /// <param name="fixture">An object of type <see cref="IntegrationTestFixture"/>.</param>
        /// <returns>An object of type <see cref="Task"/> representing an asynchronous operation.</returns>
        Task SetupAsync(IntegrationTestFixture fixture);
    }
}
