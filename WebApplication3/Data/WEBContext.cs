using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace VSZANAL.Models
{
    public class WEBContext : DbContext
    {
        public WEBContext (DbContextOptions<WEBContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplication3.Models.UserFile> UserFile { get; set; }
    }
}
