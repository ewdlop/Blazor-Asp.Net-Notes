using System;
using System.Collections.Generic;
using System.Text;
using BlazorServerApp.Areas.Identity.Data;
using BlazorServerApp.Models.EF.NautralKey;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>().HasKey(c => new { c.FirstName, c.LastName, c.EmailAddress });
            builder.Entity<Membership>().HasKey(m => m.Id);
            builder.Entity<Membership>()
                .Property(m => m.Type)
                .HasConversion<string>();

            builder.Entity<CustomerMembership>()
                .HasKey(c => new { c.FirstName, c.LastName, c.EmailAddress, c.MembershipId });

            builder.Entity<CustomerMembership>()
                .HasOne(c => c.Customer)
                .WithMany(c => c.CustomerMemberShips)
                .HasForeignKey(c => new { c.FirstName, c.LastName, c.EmailAddress })
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CustomerMembership>()
                .HasOne(c => c.Membership)
                .WithMany(m => m.OwnedByMembers)
                .HasForeignKey(c => new { c.MembershipId })
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Membership> Membership { get; set; }
        public DbSet<CustomerMembership> CustomeMemberShip { get; set; }

    }
}
