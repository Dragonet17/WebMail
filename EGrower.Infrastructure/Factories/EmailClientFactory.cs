using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Factories.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using MimeKit;

namespace EGrower.Infrastructure.Factories {
    public class EmailClientFactory : IEmailClientFactory {
        public async Task<IEnumerable<MimeMessage>> GetInboxEmailsAsync (string serverPath, int port, string email, string password, DateTime deliveredAfter) {
            using (var client = new ImapClient ()) {
                await client.ConnectAsync (serverPath, port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync (email, password);
                await client.Inbox.OpenAsync (FolderAccess.ReadOnly);
                var orderBy = new [] { OrderBy.ReverseArrival };
                var uids = await client.Inbox.SearchAsync (SearchQuery.DeliveredAfter (deliveredAfter).And (SearchQuery.NotAnswered));
                List<MimeMessage> messages = new List<MimeMessage> ();
                foreach (var uid in uids) {
                    var message = await client.Inbox.GetMessageAsync (uid);
                    messages.Add (message);
                }
                await client.DisconnectAsync (true);
                return messages;
            }
        }

        public async Task<IEnumerable<MimeMessage>> GetSentEmailsAsync (string serverPath, int port, string email, string password, DateTime sentAfter) {
            using (var client = new ImapClient ()) {
                await client.ConnectAsync (serverPath, port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync (email, password);
                var folders = await client.GetFoldersAsync (client.PersonalNamespaces[0], StatusItems.None, false);
                var sentFolder = await GetSentFolder (folders);
                if (sentFolder == null)
                    throw new Exception ("No access to the folder with sended emails.");
                await sentFolder.OpenAsync (FolderAccess.ReadOnly);
                var uids = await sentFolder.SearchAsync (SearchQuery.SentAfter (sentAfter));
                List<MimeMessage> messages = new List<MimeMessage> ();
                foreach (var uid in uids.Reverse ()) {
                    var message = await sentFolder.GetMessageAsync (uid);
                    messages.Add (message);
                }
                await client.DisconnectAsync (true);
                return messages;
            }
        }

        private async Task<IMailFolder> GetSentFolder (IList<IMailFolder> folders) {
            string[] CommonSentFolderNames = {
                "Sent",
                "Wysłan",
                "Wyslan",
                "Sent Items",
                "Sent Mail",
                "Sended",
            };
            IMailFolder sentFolder = null;
            foreach (var name in CommonSentFolderNames) {
                sentFolder = await Task.FromResult (folders.SingleOrDefault (a => a.FullName.ToLowerInvariant ().Contains (name.ToLowerInvariant ())));
                if (sentFolder != null)
                    return sentFolder;
            }
            return null;
        }
        public async Task DeleteMessageAsync (DateTime date) {
            using (var client = new ImapClient ()) {
                // await client.ConnectAsync (ServerPath, Port, SecureSocketOptions.SslOnConnect);
                // await client.AuthenticateAsync (Email, Password);
                await client.Inbox.OpenAsync (FolderAccess.ReadWrite);
                var uids = await client.Inbox.SearchAsync (SearchQuery.DeliveredOn (date));
                await client.Inbox.SetFlagsAsync (uids[0], MessageFlags.Deleted, false).ConfigureAwait (false);
                await client.DisconnectAsync (true);
            }
        }

        public async Task SendAsync (string email, string password, string smtpServerPath, int smtpPort, string name, ICollection<string> to, ICollection<string> cc, ICollection<string> bcc, string subject, string textHtmlBody, ICollection<SendedAtachment> attachments = null) {
            var message = new MimeMessage ();
            message.From.Add (new MailboxAddress (name, email));
            foreach (var emailTo in to) {
                message.To.Add (new MailboxAddress (emailTo, emailTo));
            }
            if (cc != null && cc.Count > 0) {
                foreach (var emailCc in cc) {
                    message.Cc.Add (new MailboxAddress (emailCc));
                }
            }
            if (bcc != null && bcc.Count > 0) {
                foreach (var emailBcc in bcc) {
                    message.Bcc.Add (new MailboxAddress (emailBcc));
                }
            }
            message.Subject = subject;
            var builder = new BodyBuilder ();
            builder.HtmlBody = textHtmlBody;
            if (attachments != null && attachments.Count > 0)
                foreach (var attachment in attachments) {
                    builder.Attachments.Add (attachment.Name, attachment.Data, ContentType.Parse (attachment.ContentType));
                }
            message.Body = builder.ToMessageBody ();
            using (var client = new SmtpClient ()) {
                await client.ConnectAsync (smtpServerPath, smtpPort);
                client.AuthenticationMechanisms.Remove ("XOAUTH2");
                await client.AuthenticateAsync (email, password).ConfigureAwait (false);
                await client.SendAsync (message).ConfigureAwait (false);
                await client.DisconnectAsync (true).ConfigureAwait (false);
            }
        }
        public async Task<MimeMessage> GetMessageByUniqeIdAsync (string serverPath, int port, string email, string password, uint uniqueId) {
            using (var client = new ImapClient ()) {
                await client.ConnectAsync (serverPath, port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync (email, password);
                await client.Inbox.OpenAsync (FolderAccess.ReadOnly);
                var uids = new List<UniqueId> {
                    new UniqueId (uniqueId)
                };
                var uid = new UniqueId (uniqueId);
                uids.Add (uid);
                var uidFromEmail = await client.Inbox.SearchAsync (SearchQuery.Uids (uids));
                MimeMessage message = await client.Inbox.GetMessageAsync (uidFromEmail[1]);
                await client.DisconnectAsync (true);
                return message;
            }
        }

        public async Task<MimeMessage> GetDeliveredMessageBySubjectAndDateAsync (string serverPath, int port, string email, string password, string subject, DateTime deliveredDate) {
            using (var client = new ImapClient ()) {
                await client.ConnectAsync (serverPath, port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync (email, password);
                await client.Inbox.OpenAsync (FolderAccess.ReadOnly);
                var uidsFromEmail = await client.Inbox.SearchAsync (SearchQuery.SubjectContains (subject).And (SearchQuery.DeliveredOn (deliveredDate)));
                // trzeba tutaj zrobic filtrowanie po dacie
                if (uidsFromEmail == null || uidsFromEmail.Count == 0)
                    return null;
                var uidFromEmail = uidsFromEmail.FirstOrDefault ();
                var message = await client.Inbox.GetMessageAsync (uidFromEmail);
                await client.DisconnectAsync (true);
                return message;
            }
        }

        public async Task<MimeMessage> GetSentMessageBySubjectAndDateAsync (string serverPath, int port, string email, string password, string subject, DateTime sentDate) {
            using (var client = new ImapClient ()) {
                await client.ConnectAsync (serverPath, port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync (email, password);
                await client.Inbox.OpenAsync (FolderAccess.ReadOnly);
                var uidsFromEmail = await client.Inbox.SearchAsync (SearchQuery.SubjectContains (subject).And (SearchQuery.SentOn (sentDate)));
                if (uidsFromEmail == null || uidsFromEmail.Count == 0)
                    return null;
                var uidFromEmail = uidsFromEmail.FirstOrDefault ();
                var message = await client.Inbox.GetMessageAsync (uidFromEmail);
                await client.DisconnectAsync (true);
                return message;
            }
        }

        public async Task SendAsync (string email, string password, string smtpServerPath, int smtpPort, MimeMessage message) {
            using (var client = new SmtpClient ()) {
                await client.ConnectAsync (smtpServerPath, smtpPort);
                client.AuthenticationMechanisms.Remove ("XOAUTH2");
                await client.AuthenticateAsync (email, password).ConfigureAwait (false);
                message.Sender = new MailboxAddress (email);
                await client.SendAsync (message).ConfigureAwait (false);
                await client.DisconnectAsync (true).ConfigureAwait (false);
            }
        }
    }
}