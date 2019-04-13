using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<User> Users { get; set; }
        public Role()
        {
            Users = new List<User>();
            Name = "User";
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        [NotMapped]
        public List<string> Subscription { get; set; }


        public virtual List<UserFile> Files { get; set; }
        public User()
        {
            Files = new List<UserFile>();
        }

        public int? RoleId { get; set; }
        public virtual Role Role { get; set; }
    }

    public class UserFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime Time { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }

    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int? Previous { get; set; }
    }

    public class Subscription
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
