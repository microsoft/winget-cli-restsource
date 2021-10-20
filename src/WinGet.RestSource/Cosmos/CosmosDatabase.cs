// -----------------------------------------------------------------------
// <copyright file="CosmosDatabase.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Cosmos
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Linq;
    using Microsoft.Extensions.Logging;
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Exceptions;

    /// <summary>
    /// This class retrieves a database and sets it up if it does not exist.
    /// </summary>
    public class CosmosDatabase : ICosmosDatabase
    {
        private readonly string databaseId;
        private readonly string containerId;

        // Client and container used for database modification operations.
        private readonly CosmosClient readWriteClient;
        private readonly Container readWriteContainer;

        // Container use for read-only database operations.
        private readonly Container readOnlyContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabase"/> class.
        /// </summary>
        /// <param name="serviceEndpoint">Service Endpoint.</param>
        /// <param name="readWriteKey">Authorization Key with read-write permissions.</param>
        /// <param name="readOnlyKey">Authorization Key with read-only permissions.</param>
        /// <param name="databaseId">Database.</param>
        /// <param name="containerId">Database container.</param>
        public CosmosDatabase(string serviceEndpoint, string readWriteKey, string readOnlyKey, string databaseId, string containerId)
        {
            this.databaseId = databaseId;
            this.containerId = containerId;

            var readOnlyClient = new CosmosClient(serviceEndpoint, readOnlyKey);
            this.readOnlyContainer = readOnlyClient.GetContainer(databaseId, containerId);

            this.readWriteClient = new CosmosClient(serviceEndpoint, readWriteKey);
            this.readWriteContainer = this.readWriteClient.GetContainer(databaseId, containerId);
        }

        /// <inheritdoc />
        public async Task Add<T>(CosmosDocument<T> cosmosDocument)
            where T : class, ICosmosIdDocument
        {
            try
            {
                ItemResponse<T> resourceResponse = await this.readWriteContainer.CreateItemAsync(cosmosDocument.Document);
            }
            catch (CosmosException cosmosException)
            {
                throw new CosmosDatabaseException(cosmosException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }
        }

        /// <inheritdoc />
        public async Task Delete<T>(CosmosDocument<T> cosmosDocument)
            where T : class, ICosmosIdDocument
        {
            try
            {
                await this.readWriteContainer.DeleteItemAsync<T>(cosmosDocument.Id, new PartitionKey(cosmosDocument.PartitionKey));
            }
            catch (CosmosException cosmosException)
            {
                throw new CosmosDatabaseException(cosmosException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }
        }

        /// <inheritdoc />
        public async Task Upsert<T>(CosmosDocument<T> cosmosDocument)
            where T : class, ICosmosIdDocument
        {
            try
            {
                await this.readWriteContainer.UpsertItemAsync(cosmosDocument.Document);
            }
            catch (CosmosException cosmosException)
            {
                throw new CosmosDatabaseException(cosmosException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }
        }

        /// <inheritdoc />
        public async Task Update<T>(CosmosDocument<T> cosmosDocument)
            where T : class, ICosmosIdDocument
        {
            try
            {
                var requestOptions = new ItemRequestOptions { IfMatchEtag = cosmosDocument.Etag };
                await this.readWriteContainer.ReplaceItemAsync(cosmosDocument.Document, cosmosDocument.Id, requestOptions: requestOptions);
            }
            catch (CosmosException cosmosException)
            {
                throw new CosmosDatabaseException(cosmosException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }
        }

        /// <inheritdoc />
        public IQueryable<T> GetIQueryable<T>(QueryRequestOptions requestOptions = null, string continuationToken = null)
            where T : class
        {
            try
            {
                if (requestOptions == null)
                {
                    requestOptions = new QueryRequestOptions { ResponseContinuationTokenLimitInKb = CosmosConnectionConstants.ResponseContinuationTokenLimitInKb };
                }

                IQueryable<T> iQueryable = this.readOnlyContainer.GetItemLinqQueryable<T>(continuationToken: continuationToken, requestOptions: requestOptions);
                return iQueryable;
            }
            catch (CosmosException cosmosException)
            {
                throw new CosmosDatabaseException(cosmosException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }
        }

        /// <inheritdoc />
        public async Task<CosmosDocument<T>> GetByIdAndPartitionKey<T>(string id, string partitionKey)
            where T : class, ICosmosIdDocument
        {
            CosmosDocument<T> cosmosDocument = new CosmosDocument<T>();

            try
            {
                // Get the Resource Response
                ItemResponse<T> resourceResponse = await this.readOnlyContainer.ReadItemAsync<T>(id, new PartitionKey(partitionKey));

                // Process Response
                cosmosDocument.Etag = resourceResponse.ETag;
                cosmosDocument.Id = resourceResponse.Resource.Id;
                cosmosDocument.Document = resourceResponse.Resource;
            }
            catch (CosmosException cosmosException)
            {
                throw new CosmosDatabaseException(cosmosException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }

            // Return the model
            return cosmosDocument;
        }

        /// <inheritdoc />
        public async Task<ApiDataPage<T>> GetByDocumentQuery<T>(FeedIterator<T> documentQuery)
            where T : class
        {
            ApiDataPage<T> apiDataPage = new ApiDataPage<T>();

            try
            {
                FeedResponse<T> response = await documentQuery.ReadNextAsync();
                apiDataPage.ContinuationToken = response.ContinuationToken;
                apiDataPage.Items = response.ToList();
            }
            catch (CosmosException cosmosException)
            {
                throw new CosmosDatabaseException(cosmosException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }

            // Return the model
            return apiDataPage;
        }
    }
}
