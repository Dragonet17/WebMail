using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Aggregates.Interfaces;
using EGrower.Infrastructure.DTO.EmaiMessage;
using EGrower.Infrastructure.Factories.Interfaces;
using EGrower.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using MimeKit;

namespace EGrower.Infrastructure.Aggregates {
    public class EmailClientAggregate : IEmailClientAggregate {
        private readonly IEmailMessageRepository _emailMessageRepository;
        private readonly ISendedEmailMessageRepository _sendedEmailMessageRepository;
        private readonly IEmailClientFactory _emailClientFactory;
        private readonly IEmailAccountRepository _emailAccountRepository;
        public EmailClientAggregate (IEmailMessageRepository emailMessageRepository,
            ISendedEmailMessageRepository sendedEmailMessageRepository,
            IEmailClientFactory emailClientFactory,
            IEmailAccountRepository emailAccountRepository) {
            _emailMessageRepository = emailMessageRepository;
            _sendedEmailMessageRepository = sendedEmailMessageRepository;
            _emailAccountRepository = emailAccountRepository;
            _emailClientFactory = emailClientFactory;

        }
        public async Task AddEmailsFromEmailAccountToEmailMessagesAsync (string email, string password, string imapHost, int imapPort, DateTime serchAfterDeliveredDate) {
            var emailAccount = await _emailAccountRepository.GetByEmailAsync (email);
            var deliveredEmails = await _emailClientFactory.GetInboxEmailsAsync (imapHost, imapPort, email, password, serchAfterDeliveredDate);
            var afterGivenDateEmails = deliveredEmails.Where (x => x.Date.LocalDateTime > serchAfterDeliveredDate);

            if (afterGivenDateEmails.Count () > 0) {
                ICollection<EmailMessage> deliveredMessages = new List<EmailMessage> ();
                foreach (var item in afterGivenDateEmails) {
                    EmailMessage emailMessage;
                    if (item.Attachments != null && item.Attachments.Count () > 0) {
                        emailMessage = new EmailMessage (item.From.ToString (), item.To.ToString (),
                            item.Date, item.Subject, item.HtmlBody, true);
                        ICollection<Atachment> atachments = GetAttachmentsFromMmimeMessage (item);
                        emailMessage.AddAtachments (atachments);
                    } else {
                        emailMessage = new EmailMessage (item.From.ToString (), item.To.ToString (),
                            item.Date, item.Subject, item.HtmlBody, false);
                    }
                    emailMessage.AddEmailAccount (emailAccount);
                    deliveredMessages.Add (emailMessage);
                }
                await _emailMessageRepository.AddRangeAsync (deliveredMessages);
            }
        }

        public async Task AddEmailsFromEmailAccountToSendeEmailMessagesAsync (string email, string password, string imapHost, int imapPort, DateTime serchAfterSentDate) {
            var emailAccount = await _emailAccountRepository.GetByEmailAsync (email);
            var sentEmails = await _emailClientFactory.GetSentEmailsAsync (imapHost, imapPort, email, password, serchAfterSentDate);
            var afterGivenDateEmails = sentEmails.Where (x => x.Date.Date > serchAfterSentDate.Date);
            if (afterGivenDateEmails.Count () > 0) {
                ICollection<SendedEmailMessage> sentMessages = new List<SendedEmailMessage> ();
                foreach (var item in sentEmails) {
                    SendedEmailMessage sendedEmailMessage;
                    if (item.Attachments != null && item.Attachments.Count () > 0) {
                        ICollection<SendedAtachment> sendedAttachments = GetSendedAttachmentsFromMmimeMessage (item);
                        sendedEmailMessage = new SendedEmailMessage (item.From.ToString (), item.To.ToString (),
                            item.Date.LocalDateTime, item.Subject, item.HtmlBody, true);
                        sendedEmailMessage.AddSendedAtachments (sendedAttachments);
                    } else {
                        sendedEmailMessage = new SendedEmailMessage (item.From.ToString (), item.To.ToString (),
                            item.Date.LocalDateTime, item.Subject, item.HtmlBody, false);
                    }
                    sendedEmailMessage.AddEmailAccount (emailAccount);
                    sentMessages.Add (sendedEmailMessage);
                }
                await _sendedEmailMessageRepository.UpdateRangeAsync (sentMessages);
            }
        }
        public async Task GetNewEmailsFromEmailAccountAsync (string email, string password, string imapHost, int imapPort, DateTime lastDeliveredDate, DateTime lastSentDate) {
            await AddEmailsFromEmailAccountToEmailMessagesAsync (email, password, imapHost, imapPort, lastDeliveredDate);
            await AddEmailsFromEmailAccountToSendeEmailMessagesAsync (email, password, imapHost, imapPort, lastSentDate);
        }

        public async Task GetEmailsFromEmailAccountAsync (EmailAccount emailAccount, string impaHost, int imapPort) {
            var deliveredEmails = await _emailClientFactory.GetInboxEmailsAsync (impaHost, imapPort, emailAccount.Email, emailAccount.Password, DateTime.UtcNow.AddDays (-14));
            ICollection<EmailMessage> deliveredMessages = new List<EmailMessage> ();
            foreach (var item in deliveredEmails) {
                EmailMessage emailMessage;
                if (item.Attachments != null && item.Attachments.Count () > 0) {
                    emailMessage = new EmailMessage (item.From.ToString (), item.To.ToString (),
                        item.Date.UtcDateTime, item.Subject, item.HtmlBody, true);
                    ICollection<Atachment> atachments = GetAttachmentsFromMmimeMessage (item);
                    emailMessage.AddAtachments (atachments);
                } else {
                    emailMessage = new EmailMessage (item.From.ToString (), item.To.ToString (),
                        item.Date.UtcDateTime, item.Subject, item.HtmlBody, false);
                }
                emailMessage.AddEmailAccount (emailAccount);
                deliveredMessages.Add (emailMessage);
            }
            await _emailMessageRepository.AddRangeAsync (deliveredMessages);

            var sentEmails = await _emailClientFactory.GetSentEmailsAsync (impaHost, imapPort, emailAccount.Email, emailAccount.Password, DateTime.UtcNow.AddDays (-14));
            ICollection<SendedEmailMessage> sentMessages = new List<SendedEmailMessage> ();
            foreach (var item in sentEmails) {
                SendedEmailMessage sendedEmailMessage;
                if (item.Attachments != null && item.Attachments.Count () > 0) {
                    ICollection<SendedAtachment> sendedAttachments = GetSendedAttachmentsFromMmimeMessage (item);
                    sendedEmailMessage = new SendedEmailMessage (item.From.ToString (), item.To.ToString (),
                        item.Date.UtcDateTime, item.Subject, item.HtmlBody, true);
                    sendedEmailMessage.AddSendedAtachments (sendedAttachments);
                } else {
                    sendedEmailMessage = new SendedEmailMessage (item.From.ToString (), item.To.ToString (),
                        item.Date.UtcDateTime, item.Subject, item.HtmlBody, false);
                }
                sendedEmailMessage.AddEmailAccount (emailAccount);
                sentMessages.Add (sendedEmailMessage);
            }
            await _sendedEmailMessageRepository.AddRangeAsync (sentMessages);
        }

        private ICollection<Atachment> GetAttachmentsFromMmimeMessage (MimeMessage mime) {
            List<Atachment> atachments = new List<Atachment> ();
            foreach (var atachment in mime.Attachments) {
                string name = atachment.ContentType.Name;
                if (name == null)
                    name = atachment.ContentDisposition.FileName;
                byte[] data = ConvertToByteArray (atachment);
                string contentType = atachment.ContentType.ToString ();
                atachments.Add (new Atachment (name, contentType, data));
            }
            return atachments;
        }

        private ICollection<SendedAtachment> GetSendedAttachmentsFromMmimeMessage (MimeMessage mime) {
            List<SendedAtachment> atachments = new List<SendedAtachment> ();
            foreach (var atachment in mime.Attachments) {
                string name = atachment.ContentType.Name;
                if (name == null)
                    name = atachment.ContentDisposition.FileName;
                byte[] data = ConvertToByteArray (atachment);
                string contentType = atachment.ContentType.ToString ();
                atachments.Add (new SendedAtachment (name, contentType, data));
            }
            return atachments;
        }

        private byte[] ConvertToByteArray (MimeEntity attachment) {

            byte[] bytes;
            using (var memory = new MemoryStream ()) {
                if (attachment is MimePart)
                    ((MimePart) attachment).Content.DecodeTo (memory);
                else
                    ((MessagePart) attachment).Message.WriteTo (memory);

                bytes = memory.ToArray ();
            }
            return bytes;
        }

        public async Task DeleteEmailByEmailID (int emailId) {
            var email = await _emailMessageRepository.GetAsync (emailId);
            await _emailClientFactory.DeleteMessageAsync (email.DeliveredAt);
            await _emailMessageRepository.DeleteAsync (email);
        }

        public async Task<EmailMessage> GetByUniqeIdAsync (EmailAccount emailAccount, string imapHost, int imapPort, int uniqId) {
            var lastMessages = await _emailClientFactory.GetInboxEmailsAsync (imapHost, imapPort, emailAccount.Email, emailAccount.Password, DateTime.UtcNow.AddDays (-1));
            ICollection<EmailMessage> deliveredMessages = new List<EmailMessage> ();
            var message = await _emailClientFactory.GetMessageByUniqeIdAsync (imapHost, imapPort, emailAccount.Email, emailAccount.Password, uint.Parse (lastMessages.First ().MessageId));
            return new EmailMessage (message.From.ToString (), message.To.ToString (), message.Date, message.Subject, message.HtmlBody, false);
        }

        public async Task<MimeMessage> GetDeliveredMessageBySubjectAndDateAsync (string host, int port, string email, string password, string subject, DateTime date) {
            var messageFromEmailAccount = await _emailClientFactory.GetDeliveredMessageBySubjectAndDateAsync (host, port, email, password, subject, date);
            if (messageFromEmailAccount == null)
                throw new Exception ("This email account does not have emial with this date.");
            if (DateTime.Compare (messageFromEmailAccount.Date.LocalDateTime, date) != 0)
                throw new Exception ("This email account does not have emial with this date.");
            return messageFromEmailAccount;
        }

        public async Task<MimeMessage> GetSentMessageBySubjectAndDateAsync (string host, int port, string email, string password, string subject, DateTime date) {
            var messageFromEmailAccount = await _emailClientFactory.GetSentMessageBySubjectAndDateAsync (host, port, email, password, subject, date);
            if (messageFromEmailAccount == null)
                throw new Exception ("This email account does not have emial with this date.");
            if (messageFromEmailAccount.Date == date)
                throw new Exception ("This email account does not have emial with this date.");
            return messageFromEmailAccount;
        }

        public async Task ReplyToAsync (string serverPath, int port, string email, string password, MimeMessage messageToReply, bool replyToAll, string answer, ICollection<IFormFile> attachments = null, ICollection<string> cc = null, ICollection<string> bcc = null) {
            var sendedAtachments = await ConvertFormFilesToSendedAttachments (attachments);
            var messageToSend = await ReplyTo (messageToReply, email, true, answer, sendedAtachments, cc, bcc);
            await _emailClientFactory.SendAsync (email, password, serverPath, port, messageToSend);
        }
        private async Task<MimeMessage> ReplyTo (MimeMessage message, string from, bool replyToAll, string answer, ICollection<SendedAtachment> attachments = null, ICollection<string> cc = null, ICollection<string> bcc = null) {
            var reply = new MimeMessage ();
            message.From.Add (new MailboxAddress (from));

            // reply to the sender of the message
            if (message.ReplyTo.Count > 0) {
                reply.To.AddRange (message.ReplyTo);
            } else if (message.From.Count > 0) {
                reply.To.AddRange (message.From);
            } else if (message.Sender != null) {
                reply.To.Add (message.Sender);
            }

            if (replyToAll) {
                // include all of the other original recipients - TODO: remove ourselves from these lists
                reply.To.AddRange (message.To);
                reply.Cc.AddRange (message.Cc);
            }
            if (cc != null && cc.Count > 0 && !replyToAll) {
                foreach (var emailCc in cc) {
                    message.Cc.Add (new MailboxAddress (emailCc));
                }
            }
            if (bcc != null && bcc.Count > 0) {
                foreach (var emailCc in cc) {
                    message.Bcc.Add (new MailboxAddress (emailCc));
                }
            }
            // set the reply subject
            if (!message.Subject.StartsWith ("Re:", StringComparison.OrdinalIgnoreCase))
                reply.Subject = "Re:" + message.Subject;
            else
                reply.Subject = message.Subject;

            // construct the In-Reply-To and References headers
            if (!string.IsNullOrEmpty (message.MessageId)) {
                reply.InReplyTo = message.MessageId;
                foreach (var id in message.References)
                    reply.References.Add (id);
                reply.References.Add (message.MessageId);
            }
            var bodyBuilder = new BodyBuilder ();
            if (attachments != null && attachments.Count > 0)
                foreach (var attachment in attachments)
                    bodyBuilder.Attachments.Add (attachment.Name, attachment.Data, ContentType.Parse (attachment.ContentType));
            using (var quoted = new StringWriter ()) {
                var sender = message.Sender ?? message.From.Mailboxes.FirstOrDefault ();
                var month = message.Date.Month < 10 ? "0" + message.Date.Month.ToString () : message.Date.Month.ToString ();
                quoted.WriteLine ("On <b>{0}</b> {1}.{2}.{3}, {4} wrote: <br/>", message.Date.TimeOfDay, message.Date.Day, month, message.Date.Year, !string.IsNullOrEmpty (sender.Name) ? sender.Name : sender.Address);
                var messageContent = message.TextBody == null ? message.HtmlBody : message.TextBody;
                using (var reader = new StringReader (messageContent)) {
                    string line;

                    while ((line = reader.ReadLine ()) != null) {
                        quoted.Write ("> ");
                        quoted.WriteLine (line);
                    }
                }
                bodyBuilder.HtmlBody = answer + "<br/><br/>" + quoted.ToString ();
                // reply.Body = new TextPart ("html") {
                //     Text = answer + "<br/><br/>" + quoted.ToString ()
                // };
                reply.Body = bodyBuilder.ToMessageBody ();
            }

            return await Task.FromResult (reply);
        }
        private async Task<ICollection<SendedAtachment>> ConvertFormFilesToSendedAttachments (ICollection<IFormFile> attachments) {
            if (attachments == null || attachments.Count == 0)
                return null;
            HashSet<SendedAtachment> sendedAtachments = new HashSet<SendedAtachment> ();
            foreach (var attachment in attachments) {
                if (attachment.Length > 0) {
                    using (var ms = new MemoryStream ()) {
                        await attachment.CopyToAsync (ms);
                        var attachmentBytes = ms.ToArray ();
                        var name = attachment.Name;
                        if (name == null)
                            name = attachment.FileName;
                        var sendedAttachment = new SendedAtachment (name, attachment.ContentType, attachmentBytes);
                        sendedAtachments.Add (sendedAttachment);
                    }
                }
            }
            return await Task.FromResult (sendedAtachments);
        }
    }
}