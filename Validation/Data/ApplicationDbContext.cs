using Microsoft.EntityFrameworkCore;
using Validation.Models;

namespace Validation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
        //}
    }
}
