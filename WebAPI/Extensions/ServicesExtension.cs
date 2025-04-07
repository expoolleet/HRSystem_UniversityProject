using Application.Companies.Models.Services;
using Application.Contexts;
using WebApi.Services;

namespace WebApi.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddScopedServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IRoleContext, RoleContext>();
        return services;
    }
}