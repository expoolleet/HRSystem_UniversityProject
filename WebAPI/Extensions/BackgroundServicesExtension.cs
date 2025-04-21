using WebApi.BackgroundServices;

namespace WebApi.Extensions;

public static class BackgroundServicesExtension
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<OutboxProcessor>();
        return services;
    }
}