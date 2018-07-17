using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using Microsoft.AspNetCore.Http;
using MimeKit;

namespace EGrower.Infrastructure.Factories.Interfaces {
    public interface IEmailClientFactory {
        Task SendAsync (string email, string password, string smtpServerPath, int smtpPort, string name, ICollection<string> to, ICollection<string> cc, ICollection<string> bcc, string subject, string textHtmlBody, ICollection<SendedAtachment> attachments = null);
        Task SendAsync (string email, string password, string smtpServerPath, int smtpPort, MimeMessage message);
        Task<MimeMessage> GetMessageByUniqeIdAsync (string serverPath, int port, string email, string password, uint uniqueId);
        Task<MimeMessage> GetDeliveredMessageBySubjectAndDateAsync (string serverPath, int port, string email, string password, string subject, DateTime deliveredDate);
        Task<MimeMessage> GetSentMessageBySubjectAndDateAsync (string serverPath, int port, string email, string password, string subject, DateTime sentDate);
        Task<IEnumerable<MimeMessage>> GetInboxEmailsAsync (string serverPath, int port, string email, string password, DateTime dekiveredAfter);
        Task<IEnumerable<MimeMessage>> GetSentEmailsAsync (string serverPath, int port, string email, string password, DateTime sentAfter);
        Task DeleteMessageAsync (DateTime date);
    }
}