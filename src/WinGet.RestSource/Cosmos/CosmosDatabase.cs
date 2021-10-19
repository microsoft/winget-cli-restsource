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
    using Microsoft.Azure.Cosmos.Fluent;
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Exceptions;

    /// <summary>
    /// This class retrieves a database and sets it up if it does not exist.
    /// </summary>
    public class CosmosDatabase : ICosmosDatabase
    {
        private readonly Container container;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabase"/> class.
        /// </summary>
        /// <param name="serviceEndpoint">Service Endpoint.</param>
        /// <param name="authKey">Authorization Key.</param>
        /// <param name="databaseId">Database.</param>
        /// <param name="containerId">Database container.</param>
        public CosmosDatabase(Uri serviceEndpoint, string authKey, string databaseId, string containerId)
        {
            CosmosClient client = new CosmosClientBuilder(serviceEndpoint.ToString(), authKey).Build();
            this.container = client.GetContainer(databaseId, containerId);
        }

        /// <inheritdoc />
        public async Task Add<T>(CosmosDocument<T> cosmosDocument)
            where T : class, ICosmosIdDocument
        {
            try
            {
                ItemResponse<T> resourceResponse = await this.container.CreateItemAsync(cosmosDocument.Document);
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
                await this.container.DeleteItemAsync<T>(cosmosDocument.Id, new PartitionKey(cosmosDocument.PartitionKey));
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
                await this.container.UpsertItemAsync(cosmosDocument.Document);
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
                await this.container.ReplaceItemAsync(cosmosDocument.Document, cosmosDocument.Id, requestOptions: requestOptions);
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

                IQueryable<T> iQueryable = this.container.GetItemLinqQueryable<T>(continuationToken: continuationToken, requestOptions: requestOptions);
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
                ItemResponse<T> resourceResponse = await this.container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));

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
