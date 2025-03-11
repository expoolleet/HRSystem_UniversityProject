using Domain.Companies;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User> GetUser(Guid? id);
    Task<IEnumerable<User>> GetAllUsers();
}