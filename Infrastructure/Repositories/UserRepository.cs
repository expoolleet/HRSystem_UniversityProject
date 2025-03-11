using Domain.Companies;

namespace Infrastructure.Repositories;

class UserRepository : IUserRepository
{
    private List<User> _users = new List<User>();
    private int _userCounter = 0;
    
    public async Task<User> GetUser(Guid? id)
    {
        ArgumentNullException.ThrowIfNull(id);

        return _users.FirstOrDefault(u => u.Id == id) ??
               throw new InvalidOperationException("User with given ID not found");
    }

    public int UserCounter => _userCounter;

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return _users;
    }
}
