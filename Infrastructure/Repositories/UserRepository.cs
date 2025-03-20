using Application.Companies.Repositories;
using Domain.Companies;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MainDbContext _dbContext;
    public UserRepository(MainDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }
    
    public async Task Add(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<User> Get(Guid userId, CancellationToken cancellationToken)
    {
       var user = await _dbContext.Users.FindAsync(userId, cancellationToken);
       return user!;
    }

    public async Task<User> Get(string login, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(login, cancellationToken);
        return user!;
    }
}