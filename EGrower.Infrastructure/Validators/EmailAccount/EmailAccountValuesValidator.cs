using System;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace EGrower.Infrastructure.Validators.EmailAccount {
    public static class EmailAccountValuesValidator {
        public static async Task<bool> EmailAccountUsingImapIsValid (string host, int port, string email, string password) {
            try {
                using (var client = new ImapClient ()) {
                    await client.ConnectAsync (host, port, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync (email, password);
                }
            } catch (Exception e) {
                var errmsg = e.Message;
                return false;
            }
            return true;
        }
        public static async Task<bool> EmailAccountUsingSmtpIsValid (string smtpHost, int smtpPort) {
            try {
                using (var client = new SmtpClient ()) {
                    await client.ConnectAsync (smtpHost, smtpPort);
                }
            } catch {
                return false;
            }
            return true;
        }
    }
}