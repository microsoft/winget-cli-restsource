// -----------------------------------------------------------------------
// <copyright file="ICosmosDatabase.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Cosmos
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos;
    using Microsoft.WinGet.RestSource.Utils.Common;

    /// <summary>
    /// This provides an interface for CosmosDatabase.
    /// </summary>
    public interface ICosmosDatabase
    {
        /// <summary>
        /// This will add a new document.
        /// This will fail if a document already exists that corresponds to the same ID.
        /// </summary>
        /// <param name="cosmosDocument">Cosmos Document.</param>
        /// <typeparam name="T">Document Type.</typeparam>
        /// <returns>Finalized Document.</returns>
        Task Add<T>(CosmosDocument<T> cosmosDocument)
            where T : class, ICosmosIdDocument;

        /// <summary>
        /// This will delete a document by ID and Partition Key.
        /// </summary>
        /// <param name="cosmosDocument">Cosmos Document.</param>
        /// <typeparam name="T">Document Type.</typeparam>
        /// <returns>Document.</returns>
        Task Delete<T>(CosmosDocument<T> cosmosDocument)
            where T : class, ICosmosIdDocument;

        /// <summary>
        /// This will add a new document or update it if the document already exists.
        /// </summary>
        /// <param name="cosmosDocument">Cosmos Document.</param>
        /// <typeparam name="T">Document Type.</typeparam>
        /// <returns>Finalized Document.</returns>
        Task Upsert<T>(CosmosDocument<T> cosmosDocument)
            where T : class, ICosmosIdDocument;

        /// <summary>
        /// This will add update a document.
        /// This will fail if a document does not exists.
        /// </summary>
        /// <param name="cosmosDocument">Cosmos Document.</param>
        /// <typeparam name="T">Document Type.</typeparam>
        /// <returns>Finalized Document.</returns>
        Task Update<T>(CosmosDocument<T> cosmosDocument)
            where T : class, ICosmosIdDocument;

        /// <summary>
        /// This will return an IQueryable for building out document queries.
        /// </summary>
        /// <param name="feedOptions">Feed Options.</param>
        /// <param name="continuationToken">(Optional) The continuation token in the Azure Cosmos DB service.</param>
        /// <typeparam name="T">Document Type.</typeparam>
        /// <returns>IQueryable.</returns>
        IQueryable<T> GetIQueryable<T>(QueryRequestOptions feedOptions = null, string continuationToken = null)
            where T : class;

        /// <summary>
        /// This will retrieve a document by ID and Partition Key.
        /// </summary>
        /// <param name="id">Document ID.</param>
        /// <param name="partitionKey">Partition Key.</param>
        /// <typeparam name="T">Document Type.</typeparam>
        /// <returns>Document.</returns>
        Task<CosmosDocument<T>> GetByIdAndPartitionKey<T>(string id, string partitionKey)
            where T : class, ICosmosIdDocument;

        /// <summary>
        /// This will retrieve a document by document query.
        /// </summary>
        /// <param name="documentQuery">Document Query.</param>
        /// <typeparam name="T">Document Type.</typeparam>
        /// <returns>Document.</returns>
        Task<ApiDataPage<T>> GetByDocumentQuery<T>(FeedIterator<T> documentQuery)
            where T : class;
    }
}
