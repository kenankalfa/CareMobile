using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using CareMobile.API.Configuration;

namespace CareMobile.Azure.DocumentDB
{
    public class DocumentDBRepository<T> : IDocumentDBRepository<T> where T :  class
    {
        private IConfigurationSettings _settings;

        private string DatabaseId;
        private string CollectionId;
        private DocumentClient client;
        
        public DocumentDBRepository(IConfigurationSettings settings)
        {
            _settings = settings;
            Init();
        }

        private void Init()
        {
            DatabaseId = _settings.AzureDocumentDbDatabaseId;
            CollectionId = typeof(T).Name.ToLower() + "_collection";

            client = new DocumentClient(new Uri(_settings.AzureDocumentDbEndPoint), _settings.AzureDocumentDbAuthKey);
            CreateDatabaseIfNotExistsAsync();
            CreateCollectionIfNotExistsAsync();
        }

        protected void CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                // await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
                client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId)).Wait();
            }
            catch (Exception e)
            {
                if (e.InnerException is DocumentClientException)
                {
                    var innerException = e.InnerException as DocumentClientException;
                    if (innerException.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        // await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                        client.CreateDatabaseAsync(new Database { Id = DatabaseId }).Wait();
                    }
                }
                
            }
        }

        protected void CreateCollectionIfNotExistsAsync()
        {
            try
            {
                // await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
                client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId)).Wait();
            }
            catch (Exception e)
            {
                if (e.InnerException is DocumentClientException)
                {
                    var innerException = e.InnerException as DocumentClientException;
                    if (innerException.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        //await client.CreateDocumentCollectionAsync(
                        //    UriFactory.CreateDatabaseUri(DatabaseId),
                        //    new DocumentCollection { Id = CollectionId },
                        //    new RequestOptions { OfferThroughput = 1000 });

                        client.CreateDocumentCollectionAsync(
                            UriFactory.CreateDatabaseUri(DatabaseId),
                            new DocumentCollection { Id = CollectionId },
                            new RequestOptions { OfferThroughput = 1000 }).Wait();
                    }
                }
                
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId))
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<Document> CreateItemAsync(T item)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }

        public async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        }

    }
}
