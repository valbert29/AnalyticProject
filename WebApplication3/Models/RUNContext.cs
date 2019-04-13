using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class RUNContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserFile> Files { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public RUNContext(DbContextOptions<RUNContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role[]
                {
                new Role { ID=1, Name="User",},
                new Role { ID=2, Name="Admin"},
                new Role { ID=3, Name="VIP"}
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
