using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace ModelBinding.Models
{
    public class EfDbContext : DbContext
    {
        public DbSet<Chore> Chores { get; set; }
    }
}