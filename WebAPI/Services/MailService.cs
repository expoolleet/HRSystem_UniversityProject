using Application.Mail;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using WebApi.Settings;
using IMailService = Application.Mail.IMailService;

namespace WebApi.Services;

public class MailService : IMailService
{
    private readonly IOptions<MailSettings> _mailSettings;

    public MailService(IOptions<MailSettings> mailSettings)
    {
        ArgumentNullException.ThrowIfNull(mailSettings);
        _mailSettings = mailSettings;
    }
    
    public async Task SendMail(MailData mailData)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_mailSettings.Value.Name, _mailSettings.Value.EmailId));
            emailMessage.To.Add(new MailboxAddress(mailData.EmailToName, mailData.EmailToId));
            emailMessage.Subject = mailData.EmailSubject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = mailData.EmailBody;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var mailClient = new SmtpClient();
            await mailClient.ConnectAsync(_mailSettings.Value.Host, _mailSettings.Value.Port);
            await mailClient.AuthenticateAsync(_mailSettings.Value.UserName, _mailSettings.Value.Password);
            await mailClient.SendAsync(emailMessage);
            await mailClient.DisconnectAsync(true);
        }
        catch(Exception ex)
        {
            throw new ApplicationException($"Could not send email: {ex.Message}");
        }
    }
}