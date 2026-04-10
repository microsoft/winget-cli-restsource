// -----------------------------------------------------------------------
// <copyright file="AddDllSearchDirectory.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.PowershellSupport
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Supports adding custom directories to native dll search path.
    /// </summary>
    public static class AddDllSearchDirectory
    {
        // LOAD_LIBRARY_SEARCH_DEFAULT_DIRS
        private const uint DefaultSearchDirectoryFlags = 0x00001000;

        static AddDllSearchDirectory()
        {
            SetDefaultDllDirectories(DefaultSearchDirectoryFlags);
        }

        /// <summary>
        /// Adds a directory to native dll search path.
        /// </summary>
        /// <param name="directory">The directory to be added.</param>
        public static void AddDirectory(string directory)
        {
            AddDllDirectory(directory);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int AddDllDirectory(string directory);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDefaultDllDirectories(uint directoryFlags);
    }
}
