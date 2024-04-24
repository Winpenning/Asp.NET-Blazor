using Instuments_API.Data.Mappings;
using Instuments_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Instuments_API.Data;

public class InstrumentDataContext : DbContext
{
    public DbSet<Seller> sellers { get; set; }
    public DbSet<Item> items { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder Builder)
    {
        Builder.UseSqlServer
            ("Server=localhost,1433;" +
             "Database=Instrument;" +
             "User ID=sa;" +
             "Password=1q2w3e4r@#$;" +
             "Trusted_Connection=False;" +
             "TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SellerMap());
        modelBuilder.ApplyConfiguration(new ItemMap());
    }
}