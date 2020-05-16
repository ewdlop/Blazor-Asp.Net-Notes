using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public interface ICosmosDbService<T>
    {
        public Task AddItemAsync(T item);
        public Task DeleteItemAsync(string id);
        public Task<T> GetItemAsync(string id);
        public Task<IEnumerable<T>> GetItemsAsync(int i);
        public Task<IEnumerable<T>> GetItemsAsync(string query);
        public Task UpdateItemAsync(string id, T item);

    }
}