using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO.EmaiMessage;
using Microsoft.AspNetCore.Http;
using MimeKit;

namespace EGrower.Infrastructure.Aggregates.Interfaces {
    public interface IEmailClientAggregate {
        Task AddEmailsFromEmailAccountToEmailMessagesAsync (string email, string password, string imapHost, int imapPort, DateTime serchAfterDeliveredDate);
        Task AddEmailsFromEmailAccountToSendeEmailMessagesAsync (string email, string password, string imapHost, int imapPort, DateTime serchAfterSentDate);
        Task<MimeMessage> GetDeliveredMessageBySubjectAndDateAsync (string serverPath, int port, string email, string password, string subject, DateTime date);
        Task<MimeMessage> GetSentMessageBySubjectAndDateAsync (string serverPath, int port, string email, string password, string subject, DateTime date);
        Task ReplyToAsync (string serverPath, int port, string email, string password, MimeMessage messageToReply, bool replyToAll, string answer, ICollection<IFormFile> attachments = null, ICollection<string> cc = null, ICollection<string> bcc = null);
        Task<EmailMessage> GetByUniqeIdAsync (EmailAccount emailAccount, string imapHost, int imapPort, int uniqId);
        Task GetEmailsFromEmailAccountAsync (EmailAccount emailAccount, string impaHost, int imapPort);
        Task GetNewEmailsFromEmailAccountAsync (string email, string password, string imapHost, int imapPort, DateTime lastDeliveredDate, DateTime lastSentDate);
        // Task<EmailMessageDetailsDTO> GetEmailByEmailID (int emailId);
        // Task<EmailMessageDetailsDTO> GetEmailsByUserID (int userId);
        Task DeleteEmailByEmailID (int emailId);
    }
}