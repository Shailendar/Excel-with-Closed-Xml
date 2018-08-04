using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ExcelPractice.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ExcelPractice.Models
{
    public class ExcelDBContext : DbContext
    {
        public ExcelDBContext() :
            base("DefaultConnection") { }

        public DbSet<Company> Company { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}