using GT.SiteCheck.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.Entity
{
    public class ScDbContext : DbContext
    {
        public ScDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SiteStatus> Status { get; set; }
        public DbSet<Nodes> Nodes { get; set; }
    }
}
