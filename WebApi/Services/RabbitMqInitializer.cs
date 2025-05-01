using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WebApi.Settings;

namespace WebApi.Services;

public class RabbitMqInitializer
{
    private readonly IOptions<RabbitMqSettings> _rabbitMqSettings;

    public RabbitMqInitializer(IOptions<RabbitMqSettings> rabbitMqSettings)
    {
        ArgumentNullException.ThrowIfNull(rabbitMqSettings);
        _rabbitMqSettings = rabbitMqSettings;
    }
    
    public async Task InitializeAsync(IChannel channel, CancellationToken cancellationToken = default)
    {
        await channel.ExchangeDeclareAsync(
            exchange: _rabbitMqSettings.Value.Exchange,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);
    }
}