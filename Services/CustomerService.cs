using BlazorServerApp.Data;
using BlazorServerApp.Models.EF.NautralKey;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ShopDbContext _context;
        public CustomerService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(string firstName, string lastName,string emailAddress) {
            _context.Customers.Add(new Customer {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress
            });
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Customer> GetCustmersWithFirstNameEqualTo(string firstName) {
            return _context.Customers.Where(c => c.FirstName.Equals(firstName)).ToList();
        }
    }
}
