using Application.Companies.Models.Response.Responses;
using Application.Companies.Repositories;
using Domain.Companies;
using Infrastructure.DbContext.Contexts;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _userDbContext;
    public UserRepository(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }
    
    public Task AddUser(User user, CancellationToken cancellationToken)
    {
        _userDbContext.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task<User?> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userDbContext.Users.FirstOrDefault(u => u.Id == id));
    }
    
    public Task<User?> FindUser(Guid id, Guid? companyId, string? title, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse> AuthUser(string login, string password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}