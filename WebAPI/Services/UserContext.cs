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
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User.Identity?.IsAuthenticated != true)
        {
            return Guid.Empty;
        }
        var name = httpContext.User.Identity.Name;
        var user = await _userRepository.Get(name!, cancellationToken);
        return user.Id;
    }
}