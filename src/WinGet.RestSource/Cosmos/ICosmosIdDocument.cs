// -----------------------------------------------------------------------
// <copyright file="ICosmosIdDocument.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Cosmos
{
    /// <summary>
    /// This provides an interface for a Cosmos document which has the Id property.
    /// </summary>
    public interface ICosmosIdDocument
    {
        /// <summary>
        /// Gets id.
        /// </summary>
        public string Id { get; }
    }
}
