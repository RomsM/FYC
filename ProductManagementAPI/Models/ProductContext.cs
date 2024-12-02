using ProductManagementAPI.Models;
using Microsoft.EntityFrameworkCore;


public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

    public DbSet<Product> product { get; set; }
}
