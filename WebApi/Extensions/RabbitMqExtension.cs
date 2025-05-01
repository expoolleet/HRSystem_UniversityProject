using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WebApi.Abstractions.Services;
using WebApi.BackgroundServices;
using WebApi.Services;
using WebApi.Settings;

namespace WebApi.Extensions;

public static class RabbitMqExtension
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
        services.AddSingleton<IConnectionFactory>(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<RabbitMqSettings>> ();
            return new ConnectionFactory
            {
                HostName = opt.Value.HostName,
                UserName = opt.Value.UserName,
                Password = opt.Value.Password,
                Port = opt.Value.Port,
            };
        });
        services.AddSingleton<IConnection>(sp =>
        {
            var factory = sp.GetRequiredService<IConnectionFactory>();
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });
        services.AddSingleton<IChannel>(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            return connection.CreateChannelAsync().GetAwaiter().GetResult();
        });
        services.AddSingleton<RabbitMqInitializer>();
        services.AddSingleton<IMessageProducer, RabbitMqProducer>();
        services.AddHostedService<RabbitMqConsumer>();
        return services;
    }
}