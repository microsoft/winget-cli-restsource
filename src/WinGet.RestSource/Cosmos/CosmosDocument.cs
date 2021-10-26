// -----------------------------------------------------------------------
// <copyright file="CosmosDocument.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Cosmos
{
    /// <summary>
    /// This represents a cosmos document.
    /// It contains the document, associated data (such as e-tag), as well as error information.
    /// </summary>
    /// <typeparam name="T">This is the document type for cosmos.</typeparam>
    public class CosmosDocument<T>
        where T : ICosmosIdDocument
    {
        /// <summary>
        /// Gets or sets the Cosmos Document.
        /// </summary>
        public T Document { get; set; }

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets partition Key.
        /// </summary>
        public string PartitionKey => this.Id;

        /// <summary>
        /// Gets or sets the etag for a document.
        /// </summary>
        public string Etag { get; set; }
    }
}
