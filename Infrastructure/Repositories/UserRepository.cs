using Application.Companies.Repositories;
using Domain.Companies;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MainDbContext _dbContext;
    public UserRepository(MainDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }
    
    public async Task Create(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<User> Get(Guid userId, CancellationToken cancellationToken)
    {
       var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
       return user!;
    }

    public async Task<User> Get(string login, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == login, cancellationToken);
        return user!;
    }
}