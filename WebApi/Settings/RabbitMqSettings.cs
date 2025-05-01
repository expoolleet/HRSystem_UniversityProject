namespace WebApi.Settings;

public class RabbitMqSettings
{
    public required string HostName { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required string VirtualHost { get; init; }
    public required string Exchange { get; init; }
    public required string QueueName { get; init; }
    public required string RoutingKey { get; init; }
    public int Port { get; init; }
}
