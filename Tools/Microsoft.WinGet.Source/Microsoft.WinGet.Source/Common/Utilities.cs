// -----------------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Common
{
    using System.Resources;

    /// <summary>
    /// This class contains various helper methods for the <see cref="Source" /> namespace.
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Gets the instance of the resource manager that can be used to access localized & configurable strings.
        /// </summary>
        public static ResourceManager ResourceManager
        {
            get
            {
                return new ResourceManager(typeof(Properties.Resources));
            }
        }
    }
}
