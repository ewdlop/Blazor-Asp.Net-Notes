using BlazorServerApp.Models.EF.NautralKey;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Data
{
    public class ShopDbContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }
    }
}
