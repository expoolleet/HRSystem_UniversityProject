using Application.Companies.Repositories;
using Application.Context;

namespace WebApi.Services;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    public UserContext(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(userRepository);
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }
    public async Task<Guid> GetUserId(CancellationToken cancellationToken)
    {
        if (!(_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false))
        {
            return Guid.Empty;
        }
        var name = _httpContextAccessor.HttpContext.User.Identity.Name;
        var user = await _userRepository.Get(name!, cancellationToken);
        return user.Id;
    }
}