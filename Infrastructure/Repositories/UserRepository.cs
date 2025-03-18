using Application.Companies.Models.Response.Responses;
using Application.Companies.Repositories;
using Domain.Companies;
using Infrastructure.DbContext.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _userDbContext;
    public UserRepository(UserDbContext userDbContext)
    {
        ArgumentNullException.ThrowIfNull(userDbContext);
        _userDbContext = userDbContext;
    }
    
    public async Task Add(User user, CancellationToken cancellationToken)
    {
        await _userDbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<User> Get(Guid userId, CancellationToken cancellationToken)
    {
       var user = await _userDbContext.Users.FindAsync(userId, cancellationToken);
       return user!;
    }

    public async Task<User> Get(string login, CancellationToken cancellationToken)
    {
        var user = await _userDbContext.Users.FindAsync(login, cancellationToken);
        return user!;
    }
}