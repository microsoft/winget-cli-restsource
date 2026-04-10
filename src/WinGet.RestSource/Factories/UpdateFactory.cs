// -----------------------------------------------------------------------
// <copyright file="UpdateFactory.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Factories
{
    using Microsoft.WinGet.RestSource.Operations;

    /// <summary>
    /// Helps initialize the updater.
    /// </summary>
    public class UpdateFactory
    {
        /// <summary>
        /// Factory method to initialize update object.
        /// </summary>
        /// <returns>An instance of <see cref="Update"/>.</returns>
        public static Update InitializeUpdateInstance()
        {
            return new Update();
        }
    }
}
