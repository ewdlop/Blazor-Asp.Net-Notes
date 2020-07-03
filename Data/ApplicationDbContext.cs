using BlazorServerApp.Areas.Identity.Data;
using BlazorServerApp.Models.API.RPG;
using BlazorServerApp.Models.EF.NautralKey;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            
            builder.Entity<Customer>().HasKey(c => new { c.FirstName, c.LastName, c.EmailAddress });
            builder.Entity<Membership>().HasKey(m => m.Id);
            builder.Entity<Membership>().Property(m => m.Type).HasConversion<string>().HasDefaultValue(MemberShipType.Free);
            builder.Entity<CustomerMembership>().HasKey(c => new { c.FirstName, c.LastName, c.EmailAddress, c.MembershipId });
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

            builder.Entity<Monster>().Property(m => m.Type).HasConversion<string>().HasDefaultValue(MonsterType.Creature);
            builder.Entity<Monster>().Property(m => m.Rarity).HasConversion<string>().HasDefaultValue(MonsteRarity.Normal);
            builder.Entity<Monster>().Property(m => m.AttackType).HasConversion<string>().HasDefaultValue(AttackType.Melee);
            builder.Entity<MonsterResidency>()
                .HasOne(s => s.Monster )
                .WithMany(m => m.SpawnLocations)
                .HasForeignKey(s => s.MonsterId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MonsterResidency>()
                .HasOne(s => s.Location)
                .WithMany(m => m.Monsters)
                .HasForeignKey(s => s.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<CustomerMembership> CustomeMemberShips { get; set; }
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<MonsterResidency> MonsterResidencies { get; set; }

        //public DbSet<PetOwner> PetOwners { get;set;}
        //public DbSet<Pet> Pets { get; set; }
        //public DbSet<PetCompanionShip> PetCompanionShips { get; set; }
    }
}
