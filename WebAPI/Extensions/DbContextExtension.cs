using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

public static class DbContextExtension
{
    public static IServiceCollection AddSqLiteContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<MainDbContext>(optionsBuilder 
            => optionsBuilder.UseSqlite(connectionString));
        return services;
    }
}