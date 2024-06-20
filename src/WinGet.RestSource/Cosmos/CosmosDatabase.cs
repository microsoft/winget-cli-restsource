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
    using global::Azure.Identity;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Linq;
    using Microsoft.WinGet.RestSource.Interfaces;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants;
    using Microsoft.WinGet.RestSource.Utils.Exceptions;
    using Newtonsoft.Json;

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

            if (!string.IsNullOrEmpty(readWriteKey) && !string.IsNullOrEmpty(readOnlyKey))
            {
                var readOnlyClient = new CosmosClient(serviceEndpoint, readOnlyKey);
                this.readOnlyContainer = readOnlyClient.GetContainer(databaseId, containerId);

                this.readWriteClient = new CosmosClient(serviceEndpoint, readWriteKey);
                this.readWriteContainer = this.readWriteClient.GetContainer(databaseId, containerId);
            }
            else
            {
                // The azure function will have read and write roles. It doesn't make a lot of sense
                // to have to user managed identity just to split it if the same resource have both roles.
                this.readWriteClient = new CosmosClient(serviceEndpoint, new DefaultAzureCredential());
                this.readOnlyContainer = this.readWriteContainer = this.readWriteClient.GetContainer(databaseId, containerId);
            }
        }

        /// <inheritdoc />
        public async Task CreateContainer(int? throughput = null)
        {
            var database = await this.readWriteClient.CreateDatabaseIfNotExistsAsync(this.databaseId);
            await database.Database.CreateContainerIfNotExistsAsync(this.containerId, "/id", throughput);
        }

        /// <inheritdoc />
        public async Task DeleteContainer()
        {
            await this.readWriteContainer.DeleteContainerAsync();
        }

        /// <inheritdoc />
        public async Task<int> Count<T>()
            where T : class
        {
            int count = await this.readOnlyContainer.GetItemLinqQueryable<T>().CountAsync();
            return count;
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
                await this.readWriteContainer.ReplaceItemAsync(cosmosDocument.Document, cosmosDocument.Id, new PartitionKey(cosmosDocument.Id), requestOptions: requestOptions);
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
        public IOrderedQueryable<T> GetIQueryable<T>(QueryRequestOptions requestOptions = null, string continuationToken = null)
            where T : class
        {
            try
            {
                if (requestOptions == null)
                {
                    requestOptions = new QueryRequestOptions { ResponseContinuationTokenLimitInKb = CosmosConnectionConstants.ResponseContinuationTokenLimitInKb };
                }

                IOrderedQueryable<T> iQueryable = this.readOnlyContainer.GetItemLinqQueryable<T>(true, continuationToken: continuationToken, requestOptions: requestOptions);
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
                apiDataPage.RequestCharge = response.RequestCharge;
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

        /// <inheritdoc />
        public async Task<ApiDataPage<T>> GetByDocumentQuery<T>(IQueryable<T> documentQuery, QueryRequestOptions feedOptions, string continuationToken)
            where T : class
        {
            ApiDataPage<T> apiDataPage = new ApiDataPage<T>();

            try
            {
                string sql = JsonConvert.DeserializeObject<IQueryableSql>(documentQuery.ToString()).Sql;
                FeedIterator<T> query = this.readOnlyContainer.GetItemQueryIterator<T>(sql, continuationToken, feedOptions);
                FeedResponse<T> response = await query.ReadNextAsync();
                apiDataPage.RequestCharge = response.RequestCharge;
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

        private class IQueryableSql
        {
            [JsonProperty(PropertyName = "query")]
            public string Sql { get; set; }
        }
    }
}
