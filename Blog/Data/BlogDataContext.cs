using Microsoft.EntityFrameworkCore;

namespace Blog.Data;

public class BlogDataContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=localhost,1433;" +
                                "Database=master;" +
                                "User ID=sa;" +
                                "Password=1q2w3e4r@#$;" +
                                "Trusted_Connection=False;" +
                                "TrustServerCertificate=True;");
}