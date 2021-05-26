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
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Microsoft.WinGet.RestSource.Common;
    using Microsoft.WinGet.RestSource.Constants;
    using Microsoft.WinGet.RestSource.Exceptions;

    /// <summary>
    /// This class retrieves a database, and sets it up if it does not exist.
    /// </summary>
    public class CosmosDatabase : ICosmosDatabase
    {
        private readonly DocumentClient client;
        private readonly string database;
        private readonly string collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabase"/> class.
        /// </summary>
        /// <param name="serviceEndpoint">Service Endpoint.</param>
        /// <param name="authKey">Authorization Key.</param>
        /// <param name="database">Database.</param>
        /// <param name="collection">Database collection.</param>
        public CosmosDatabase(Uri serviceEndpoint, string authKey, string database, string collection)
        {
            this.database = database;
            this.collection = collection;
            this.client = new DocumentClient(serviceEndpoint, authKey);
        }

        /// <inheritdoc />
        public async Task Add<T>(CosmosDocument<T> cosmosDocument)
            where T : class
        {
            try
            {
                Uri documentCollectionUri = UriFactory.CreateDocumentCollectionUri(this.database, this.collection);
                ResourceResponse<Document> resourceResponse =
                    await this.client.CreateDocumentAsync(documentCollectionUri, cosmosDocument.Document);
            }
            catch (DocumentClientException documentClientException)
            {
                throw new CosmosDatabaseException(documentClientException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }
        }

        /// <inheritdoc />
        public async Task Delete<T>(CosmosDocument<T> cosmosDocument)
            where T : class
        {
            try
            {
                Uri documentUri = UriFactory.CreateDocumentUri(this.database, this.collection, cosmosDocument.Id);
                await this.client.DeleteDocumentAsync(
                    documentUri,
                    new RequestOptions
                    {
                        PartitionKey = new PartitionKey(cosmosDocument.PartitionKey),
                    });
            }
            catch (DocumentClientException documentClientException)
            {
                throw new CosmosDatabaseException(documentClientException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }
        }

        /// <inheritdoc />
        public async Task Upsert<T>(CosmosDocument<T> cosmosDocument)
            where T : class
        {
            try
            {
                Uri documentCollectionUri = UriFactory.CreateDocumentCollectionUri(this.database, this.collection);
                await this.client.UpsertDocumentAsync(documentCollectionUri, cosmosDocument.Document);
            }
            catch (DocumentClientException documentClientException)
            {
                throw new CosmosDatabaseException(documentClientException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }
        }

        /// <inheritdoc />
        public async Task Update<T>(CosmosDocument<T> cosmosDocument)
            where T : class
        {
            try
            {
                var requestOptions = new RequestOptions
                {
                    AccessCondition = new AccessCondition
                    {
                        Condition = cosmosDocument.Etag,
                        Type = AccessConditionType.IfMatch,
                    },
                };

                Uri documentUri = UriFactory.CreateDocumentUri(this.database, this.collection, cosmosDocument.Id);
                await this.client.ReplaceDocumentAsync(documentUri, cosmosDocument.Document, requestOptions);
            }
            catch (DocumentClientException documentClientException)
            {
                throw new CosmosDatabaseException(documentClientException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }
        }

        /// <inheritdoc />
        public IQueryable<T> GetIQueryable<T>(FeedOptions feedOptions = null)
            where T : class
        {
            try
            {
                if (feedOptions == null)
                {
                    feedOptions = new FeedOptions { ResponseContinuationTokenLimitInKb = CosmosConnectionConstants.ResponseContinuationTokenLimitInKb };
                }

                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(this.database, this.collection);
                IQueryable<T> iQueryable = this.client.CreateDocumentQuery<T>(collectionUri, feedOptions);
                return iQueryable;
            }
            catch (DocumentClientException documentClientException)
            {
                throw new CosmosDatabaseException(documentClientException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }
        }

        /// <inheritdoc />
        public async Task<CosmosDocument<T>> GetByIdAndPartitionKey<T>(string id, string partitionKey)
            where T : class
        {
            CosmosDocument<T> cosmosDocument = new CosmosDocument<T>();

            try
            {
                // Get the Resource Response
                Uri documentUri = UriFactory.CreateDocumentUri(this.database, this.collection, id);
                RequestOptions requestOptions = new RequestOptions { PartitionKey = new PartitionKey(partitionKey) };
                ResourceResponse<Document> resourceResponse =
                    await this.client.ReadDocumentAsync(documentUri, requestOptions);

                // Process Response
                Document document = resourceResponse.Resource;
                cosmosDocument.Etag = document.ETag;
                cosmosDocument.Id = document.Id;
                cosmosDocument.Document = (T)(dynamic)document;
            }
            catch (DocumentClientException documentClientException)
            {
                throw new CosmosDatabaseException(documentClientException);
            }
            catch (Exception exception)
            {
                throw new DefaultException(exception);
            }

            // Return the model
            return cosmosDocument;
        }

        /// <inheritdoc />
        public async Task<ApiDataPage<T>> GetByDocumentQuery<T>(IDocumentQuery<T> documentQuery)
            where T : class
        {
            ApiDataPage<T> apiDataPage = new ApiDataPage<T>();

            try
            {
                FeedResponse<T> response = await documentQuery.ExecuteNextAsync<T>();
                apiDataPage.ContinuationToken = response.ResponseContinuation;
                apiDataPage.Items = response.ToList<T>();
            }
            catch (DocumentClientException documentClientException)
            {
                throw new CosmosDatabaseException(documentClientException);
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
