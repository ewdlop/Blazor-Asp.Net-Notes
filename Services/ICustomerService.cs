using BlazorServerApp.Models.EF.NautralKey;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public interface ICustomerService
    {
        public Task AddUserAsync(string firstName, string lastName, string emailAddress);
        public IEnumerable<Customer> GetCustmersWithFirstNameEqualTo(string firstName);
    }
}
