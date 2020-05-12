using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public interface ICosmosDbService<T>
    {
        Task<IEnumerable<T>> GetItemsAsync(string query);
        Task<IEnumerable<T>> GetItemsAsync(int i);
        Task<T> GetItemAsync(string id);
        Task AddItemAsync(T item);
        Task UpdateItemAsync(string id, T item);
        Task DeleteItemAsync(string id);
    }
}
