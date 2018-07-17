using System.Threading.Tasks;
using EGrower.Infrastructure.Extension.Interfaces;
using EGrower.Infrastructure.Factories.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace EGrower.Infrastructure.Factories {
    public class EmailFactory : IEmailFactory {
        private readonly IEmailConfiguration _emailConfiguration;
        public EmailFactory (IEmailConfiguration emailConfiguration) {
            _emailConfiguration = emailConfiguration;
        }
        public async Task SendEmailAsync (MimeMessage mimeMessage) {
            using (var client = new SmtpClient ()) {
                await client.ConnectAsync (_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort);
                client.AuthenticationMechanisms.Remove ("XOAUTH2");
                await client.AuthenticateAsync (_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                await client.SendAsync (mimeMessage);
                await client.DisconnectAsync (true);
            }
        }
    }
}