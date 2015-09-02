using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ljsflooring.Models;

namespace ljsflooring.Data
{
    public class LjsflooringContext : DbContext
    {
        public LjsflooringContext()
            : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(
              new MigrateDatabaseToLatestVersion<LjsflooringContext, LjsflooringMigrationsConfiguration>()
              );
        }

        public DbSet<Listing> Listing { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}