using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSZANAL.Models;

namespace VSZANAL
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
            /*modelBuilder.Entity<User>().HasData(
                new User[]
                {
                    new User{Id=99, Avatar="http://www.lets-develop.com/wp-content/themes/olivias_theme/images/custom-avatar-admin.jpg",
                    Login="Admin", Password="Admin", Name="ADMINISTRATOR_SERVERA", RoleId=3, Subscriptions=new List<Subscription>{new Subscription {Id=1, Name="GetAverageValue", Price=50},
                    new Subscription {Id=2, Name="GetExpectedValue", Price=100},
                    new Subscription {Id=3, Name="GetDispersion", Price=150},
                    new Subscription {Id=4, Name="GetSquareDeviation", Price=100}}}
                });
            modelBuilder.Entity<Subscription>().HasData(
                new Subscription[]
                {
                    new Subscription {Id=1, Name="GetAverageValue", Price=50},
                    new Subscription {Id=2, Name="GetExpectedValue", Price=100},
                    new Subscription {Id=3, Name="GetDispersion", Price=150},
                    new Subscription {Id=4, Name="GetSquareDeviation", Price=100}
                });*/
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
