using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TagDistributor.DAL
{
    public class TDContext : DbContext
    {
        public TDContext() : base("TDContext") { }

        public DbSet<Models.TagDistribut> TagDistributs { get; set; }
        public DbSet<Models.TagsLog> TagsLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
    
}