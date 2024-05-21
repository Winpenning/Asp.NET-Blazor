using ItemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemApi.Data;

public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions<AppDataContext> options):base(options)
        {}

    public DbSet<Item> Items {get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}