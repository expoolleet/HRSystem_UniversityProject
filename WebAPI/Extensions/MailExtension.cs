using Application.Mail;
using WebApi.Services;
using WebApi.Settings;

namespace WebApi.Extensions;

public static class MailExtension
{
    public static IServiceCollection AddMailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        services.AddTransient<IMailService, MailService>();
        return services;
    }
}