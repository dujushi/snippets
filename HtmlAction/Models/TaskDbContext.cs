using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HtmlAction.Models
{
    public class TaskDbContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
    }
}