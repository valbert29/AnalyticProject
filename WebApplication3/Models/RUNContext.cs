using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VSZANAL.Controllers;
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
            string adminRoleName = "admin";
            string userRoleName = "user";
            var adminLogin = "admin";
            var adminAvatar = "http://searchfoto.ru/img/xyygpKbDS18_Uy8xNLy3SS87P1Te0MI80KTXWK8hLBwA.jpg";

            var adminPeriod = new DateTime(9999, 12, 20);
            // добавляем роли
            
            //var sub1 = new Subscription { Id = 1, Name = "GetAverageValue" };
            //var sub2 = new Subscription { Id = 2, Name = "GetExpectedValue" };
            //var sub3 = new Subscription { Id = 3, Name = "GetDispersion" };
            //var sub4 = new Subscription { Id = 4, Name = "GetSquareDeviation" };
            //var adminSubs = new List<Subscription> { sub1, sub2, sub3, sub4 };

            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Login = adminLogin, Name = adminLogin,
                Avatar = adminAvatar, Password = adminLogin, RoleId = adminRole.Id };
            modelBuilder.Entity<User>()
                .HasMany(e => e.Subscriptions)
                .WithOne(x => x.User);
            var sub1 = new Subscription { Id = 1, Name = "GetAverageValue", Period = adminPeriod, UserId=adminUser.Id };
            var sub2 = new Subscription { Id = 2, Name = "GetExpectedValue", Period = adminPeriod, UserId = adminUser.Id };
            var sub3 = new Subscription { Id = 3, Name = "GetDispersion", Period = adminPeriod, UserId = adminUser.Id };
            var sub4 = new Subscription { Id = 4, Name = "GetSquareDeviation", Period = adminPeriod, UserId = adminUser.Id };
            var adminSubs = new List<Subscription> { new Subscription { Id = 1, Name = "GetAverageValue", Period = adminPeriod } };
            modelBuilder.Entity<User>()
                .HasMany(e => e.Subscriptions)
                .WithOne(x => x.User)
                .IsRequired();
            //adminUser.Subscriptions = new List<Subscription> { new Subscription { Id = 1, Name = "GetAverageValue", Period = adminPeriod, } };
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            modelBuilder.Entity<Subscription>().HasData(new Subscription[] { sub1, sub2, sub3, sub4 });

            //UserSubscriptions userSubscriptions1 = new UserSubscriptions
            //{
            //    UserId = 1,
            //    SubscriptionId = 1,
            //    User = adminUser,
            //    Subscription = sub1
            //};
            //UserSubscriptions userSubscriptions2 = new UserSubscriptions
            //{
            //    UserId = 1,
            //    User = adminUser,
            //    SubscriptionId = 2,
            //    Subscription = sub2
            //};
            //UserSubscriptions userSubscriptions3 = new UserSubscriptions
            //{
            //    UserId = 1,
            //    SubscriptionId = 3,
            //    User = adminUser,
            //    Subscription = sub3
            //};
            //UserSubscriptions userSubscriptions4 = new UserSubscriptions
            //{
            //    UserId = 1,
            //    SubscriptionId = 4,
            //    User = adminUser,
            //    Subscription = sub4
            //};
            //adminUser.UserSubscriptions = new List<UserSubscriptions>()
            //{
            //    userSubscriptions2,
            //    userSubscriptions1,
            //    userSubscriptions3,
            //    userSubscriptions4
            //};
            //sub1.UserSubscriptions = new List<UserSubscriptions>()
            //{
            //    userSubscriptions1,
            //    userSubscriptions2,
            //    userSubscriptions3,
            //    userSubscriptions4,
            //};

            //modelBuilder.Entity<UserSubscriptions>().HasData(userSubscriptions1, userSubscriptions2,
            //    userSubscriptions3, userSubscriptions4);

            //modelBuilder.Entity<UserSubscriptions>().HasKey(k => new { k.UserId, k.SubscriptionId });
            //modelBuilder.Entity<UserSubscriptions>().HasOne(x => x.User).WithMany(x => x.UserSubscriptions).HasForeignKey(l => l.UserId);
            //modelBuilder.Entity<UserSubscriptions>().HasOne(x => x.Subscription).WithMany(x => x.UserSubscriptions).HasForeignKey(l => l.SubscriptionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
