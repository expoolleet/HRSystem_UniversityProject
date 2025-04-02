using Domain.Companies;

namespace Application.Companies.Repositories;

public interface IRoleRepository
{
    Task Create(
        Role role,
        CancellationToken cancellationToken);
    
    Task<Role> Get(
        Guid id, 
        CancellationToken cancellationToken);
    
    Task<Role> Get(
        string name, 
        CancellationToken cancellationToken);
}