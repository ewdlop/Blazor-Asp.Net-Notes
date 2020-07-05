using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazorServerApp.Models.API.RPG;

namespace BlazorServerApp.Data
{
    public class BlazorServerAppContext : DbContext
    {
        public BlazorServerAppContext (DbContextOptions<BlazorServerAppContext> options)
            : base(options)
        {
        }

        public DbSet<BlazorServerApp.Models.API.RPG.Monster> Monster { get; set; }
    }
}
