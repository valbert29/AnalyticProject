using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VSZANAL.Models
{
    public class WEBContext : DbContext
    {
        public WEBContext (DbContextOptions<WEBContext> options)
            : base(options)
        {
        }

        public DbSet<VSZANAL.Models.UserFile> UserFile { get; set; }
    }
}
