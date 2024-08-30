using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MVCLearn.DTOs;
using MVCLearn.Interfaces.Messages;
using MVCLearn.Models;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using MimeKit;
using MimeKit.Text;

namespace MVCLearn.Services.Messages
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SMTPSettings _smtpSettings;

        public EmailSenderService(IOptions<SMTPSettings> smtpSettings)
        {
                this._smtpSettings = smtpSettings.Value;
        }
        public async Task SendEmailAsync(MessageDto messageDto)
        {
            try
            {
                var mail = new MimeMessage();
                mail.From.Add(MailboxAddress.Parse(_smtpSettings.Username));
                mail.To.Add(MailboxAddress.Parse(messageDto.To));
                mail.Subject = messageDto.Title;
                mail.Body = new TextPart(TextFormat.Html)
                {
                    Text = messageDto.Body
                };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);

            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Elektron pochta yuborishda xatolik yuz berdi.", ex);
            }
        }
    }
}
