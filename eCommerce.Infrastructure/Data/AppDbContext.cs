using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;



namespace eCommerce.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
