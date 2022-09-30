using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ServiceBusAPI.Models;

namespace ServiceBusAPI.Services;

public class NotificationService : INotificationService
{
    private readonly ILogger _logger;
    private readonly MailSettings _mailSettings;
    public NotificationService(ILogger<NotificationService> logger, IOptions<MailSettings> mailSettings)
    {
        _logger = logger;
        _mailSettings = mailSettings.Value;
    }

    public async Task SendNotficaiton(Notification notification)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        email.To.Add(MailboxAddress.Parse(notification.ToEmail));
        email.Subject = notification.Subject;
        var builder = new BodyBuilder();

        builder.HtmlBody = notification.Body;
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();

        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);
        _logger.LogInformation($"Notfication sent on email {notification.ToEmail}");

        smtp.Disconnect(true);
        smtp.Dispose();

    }
}