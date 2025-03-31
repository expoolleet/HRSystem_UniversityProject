using Application.Companies.Models.Response.Responses;
using Domain.Companies;

namespace Application.Companies.Repositories;

public interface IUserRepository
{
    Task Create(
        User user,
        CancellationToken cancellationToken);
    
    Task<User> Get(
        Guid userId,
        CancellationToken cancellationToken);

    Task<User> Get(
        string login,
        CancellationToken cancellationToken);
}