using System.Security.Claims;
using Application.Contexts;

namespace WebApi.Services;

public class RoleContext : IRoleContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RoleContext(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
    }
    public string? GetUserRoleName(Guid userId, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        return httpContext?.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
    }
}