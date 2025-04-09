using WebApi.Authorizations;

namespace WebApi.Extensions;

public static class AuthorizationExtension
{
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicies.RequireAdminRole, 
                policy => policy.RequireRole(AppRoles.Admin));
            options.AddPolicy(AuthPolicies.RequireManagerRole,
                policy => policy.RequireRole(AppRoles.Manager));
            options.AddPolicy(AuthPolicies.RequireAdminOrManagerRole,
                policy => policy.RequireRole(AppRoles.Admin, AppRoles.Manager));
            options.AddPolicy(AuthPolicies.RequireRecruiterRole,
                policy => policy.RequireRole(AppRoles.Recruiter));
        });
        return services;
    }
}