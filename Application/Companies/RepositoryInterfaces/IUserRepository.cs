using Application.Companies.Models.Response.Responses;
using Domain.Companies;

namespace Application.Companies.RepositoryInterfaces;

public interface IUserRepository
{
    Task AddUser(
        User user,
        CancellationToken cancellationToken);
    
    Task<User?> GetUserById(
        Guid id, 
        CancellationToken cancellationToken);

    Task<UserResponse> AuthUser(
        string login, 
        string password, 
        CancellationToken cancellationToken);

    Task<User?> FindUser(
        Guid id,
        Guid? companyId,
        string? title,
        CancellationToken cancellationToken);
}