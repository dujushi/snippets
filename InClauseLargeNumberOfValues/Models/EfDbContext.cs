using System.Data.Entity;

namespace Demo.Models
{
    public class EfDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
    }
}