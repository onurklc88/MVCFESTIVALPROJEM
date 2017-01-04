using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FestivalWeb.Models
{
    public class DataContext : DbContext
    {
        static DataContext()
        {
            Database.SetInitializer<DataContext>(null);
        }

        public DataContext()
            : base("Name=DataContext")
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Fests> Fests { get; set; }
        public DbSet<UserGoing> UserGoing { get; set; }
        public DbSet<UserComments> UserComments { get; set; }
    }
}