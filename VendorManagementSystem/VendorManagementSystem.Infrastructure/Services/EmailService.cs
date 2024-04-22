using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IServices;

namespace VendorManagementSystem.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettingsDTO _emailSettings;

        public EmailService(IOptions<EmailSettingsDTO> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public void SendLoginEmail(EmailDetailsDTO emailDetails)
        {
            MimeMessage message = new MimeMessage();
            Console.WriteLine(emailDetails.ToName);
            message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.From));
            message.To.Add(new MailboxAddress(emailDetails.ToName, emailDetails.ToAddress));

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = emailDetails.Body;

            message.Subject = emailDetails.Subject;
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            client.Connect(_emailSettings.SmtpClient, _emailSettings.Port, false);
            client.Authenticate(_emailSettings.AuthEmail, _emailSettings.AuthKey);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
