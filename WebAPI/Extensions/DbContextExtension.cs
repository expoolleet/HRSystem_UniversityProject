using Infrastructure.DbContexts;
using Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

public static class DbContextExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<MainDbContext>((serviceProvider, optionsBuilder) 
            => optionsBuilder.UseSqlite(connectionString,
                x => x.MigrationsAssembly("Infrastructure"))
                .AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>()));
        return services;
    }   
}