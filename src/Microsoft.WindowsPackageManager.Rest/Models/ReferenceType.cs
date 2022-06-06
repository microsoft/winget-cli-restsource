// -----------------------------------------------------------------------
// <copyright file="ReferenceType.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WindowsPackageManager.Rest.Models
{
    /// <summary>
    /// Type of reference provided.
    /// </summary>
    public enum ReferenceType
    {
        /// <summary>
        /// The reference is being added.
        /// </summary>
        Add,

        /// <summary>
        /// The reference is being modified.
        /// </summary>
        Modify,

        /// <summary>
        /// The reference is being deleted.
        /// </summary>
        Delete,
    }
}
