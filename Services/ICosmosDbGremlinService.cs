using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public interface ICosmosDbGremlinService
    {
        public Task SubmitQuery(string test);
    }
}