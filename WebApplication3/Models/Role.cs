using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VSZANAL.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<User> Users { get; set; }
        public Role()
        {
            Users = new List<User>();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        //public virtual ICollection<UserSubscriptions> UserSubscriptions { get; set; }

        public virtual List<Subscription> Subscriptions { get; set; }

        public virtual List<UserFile> Files { get; set; }
        public User()
        {
            Files = new List<UserFile>();
            Subscriptions = new List<Subscription>();
        }

        public int? RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class UserFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime Time { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int Previous { get; set; }
    }

    public enum SortState
    {
        NameAsc,    // по имени по возрастанию
        NameDesc,   // по имени по убыванию
        TimeAsc, // по времени по возрастанию
        TimeDesc,    // по времени по убыванию
    }

    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public int? Previous { get; set; }
    }

    //public class UserSubscriptions
    //{
    //    public int UserId { get; set; }
    //    public int SubscriptionId { get; set; }
    //    public virtual Subscription Subscription { get; set; }
    //    public virtual User User { get; set; }
    //}

    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Period { get; set; }
        //public int? UserId { get; set; }
        //public User User { get; set; }
        //public virtual ICollection<UserSubscriptions> UserSubscriptions { get; set; }
    }
}
