using Ecom.core.DTO;
using Ecom.core.services;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration configuration;

        public EmailServices(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmail(EmailDTO emailDTO)
        {
            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("My Ecom", configuration["EmailSetting:From"]));
            message.Subject = emailDTO.Subject;
            message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content
            };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    // CHANGED: Auto بيشتغل مع بورت 465 (SSL) وبورت 587 (StartTLS)
                    await smtp.ConnectAsync(
                        configuration["EmailSetting:Smtp"],
                        int.Parse(configuration["EmailSetting:Port"]),
                        SecureSocketOptions.Auto
                    );

                    await smtp.AuthenticateAsync(
                        configuration["EmailSetting:Username"],
                        configuration["EmailSetting:Password"]
                    );

                    await smtp.SendAsync(message);
                }
                finally
                {
                    // CHANGED: ما نعملش Disconnect غير لو فعلًا متوصل
                    if (smtp.IsConnected)
                    {
                        await smtp.DisconnectAsync(true);
                    }
                }
            }
        }
    }
}