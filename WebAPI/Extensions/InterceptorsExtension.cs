using Infrastructure.Interceptors;

namespace WebApi.Extensions;

public static class InterceptorsExtension
{
    public static IServiceCollection AddScopedInterceptors(this IServiceCollection services)
    {
        services.AddScoped<IAuditHelper, AuditHelper>();
        services.AddScoped<AuditSaveChangesInterceptor>();
        return services;
    }
}