using System.Collections.Immutable;
using BlazingShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazingShop.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}