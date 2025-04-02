using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.DbContexts;

public class MainDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
{
    public MainDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();
        optionsBuilder.UseSqlite("Data Source=app.db");
        return new MainDbContext(optionsBuilder.Options);
    }
}