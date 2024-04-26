using Microsoft.EntityFrameworkCore;

namespace BrincandoComRazor.Data;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
        => builder
            .UseSqlServer(
                "Server=localhost,1433;" +
                "Database=blog;" +
                "User ID=sa;" +
                "Password=1q2w3e4r@#$;" +
                "Trusted_Connection=False;" +
                "TrustServerCertificate=True;");


}