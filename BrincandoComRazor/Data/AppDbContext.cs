using BrincandoComRazor.Data.Mappings;
using BrincandoComRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace BrincandoComRazor.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
        => builder
            .UseSqlServer(
                "Server=localhost,1433;" +
                "Database=Blog;" +
                "User ID=sa;" +
                "Password=1q2w3e4r@#$;" +
                "Trusted_Connection=False;" +
                "TrustServerCertificate=True;");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new PostMap());
        modelBuilder.ApplyConfiguration(new CategoryMap());
    }
}