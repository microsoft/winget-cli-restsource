// -----------------------------------------------------------------------
// <copyright file="IStorageCleanup.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.IntegrationTest.Common
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface to setup storage.
    /// </summary>
    public interface IStorageCleanup
    {
        /// <summary>
        /// Sets up the storage state.
        /// </summary>
        /// <returns>An object of type <see cref="Task"/> representing an asynchronous operation.</returns>
        Task CleanupAsync();
    }
}