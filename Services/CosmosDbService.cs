using BlazorServerApp.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public class CosmosDbService<T> : ICosmosDbService<T>
    {
        private readonly Container _container;

        public CosmosDbService(CosmosClient dbClient,string databaseName,string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }
        public async Task AddItemAsync(T item)
        {
            if (item.GetType() == typeof(MarvelCharactersResult))
                await _container.CreateItemAsync(item, new PartitionKey(((MarvelCharactersResult)(object)item).api_id));
        }

        public Task DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetItemsAsync(int i)
        {
            var queryDefinition = new QueryDefinition($"select * from c order by c.id OFFSET {(i-1)*10} LIMIT 10");
            var query = _container.GetItemQueryIterator<T>(queryDefinition) ;
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
                return results;
            }

            return results;
        }
        public Task<IEnumerable<T>> GetItemsAsync(string query)
        {
            throw new NotImplementedException();
        }

        public Task UpdateItemAsync(string id, T item)
        {
            throw new NotImplementedException();
        }
    }
}
