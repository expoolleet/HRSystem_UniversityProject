using Infrastructure.DbContexts;

namespace WebApi.Seedings;

public interface ISeeding
{
    static abstract void SeedDatabase(MainDbContext context);
}