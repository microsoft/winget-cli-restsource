// -----------------------------------------------------------------------
// <copyright file="RebuildFactory.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Factories
{
    using Microsoft.WinGet.RestSource.Operations;

    /// <summary>
    /// Helps initialize the rebuilder.
    /// </summary>
    public class RebuildFactory
    {
        /// <summary>
        /// Factory method to initialize rebuild object.
        /// </summary>
        /// <returns>An instance of <see cref="Rebuild"/>.</returns>
        public static Rebuild InitializeRebuildInstance()
        {
            return new Rebuild();
        }
    }
}
