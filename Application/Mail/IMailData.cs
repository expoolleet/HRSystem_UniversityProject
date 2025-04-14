namespace Application.Mail;

public interface IMailService
{
    Task SendMail(MailData mailData);
}