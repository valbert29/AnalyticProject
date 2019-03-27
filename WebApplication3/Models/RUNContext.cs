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
        public RUNContext(DbContextOptions<RUNContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
