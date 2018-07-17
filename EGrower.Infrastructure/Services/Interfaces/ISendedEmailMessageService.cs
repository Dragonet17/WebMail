using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO.SendedEmailMessage;
using Microsoft.AspNetCore.Http;

namespace EGrower.Infrastructure.Services.Interfaces {
    public interface ISendedEmailMessageService {
        Task SendAsync (int userId, string from, ICollection<string> to, ICollection<string> cc, ICollection<string> bcc, string subject, string textHtmlBody, ICollection<IFormFile> attachments = null);
        Task<MemoryStream> GetUserSendedAttachmentsAsync (int userId, int emailId);
        Task<SendedEmailMessageDto> GetAsync (int id);
        Task<IEnumerable<SendedEmailMessageDto>> BrowseAllAsync (int id);
        Task<IEnumerable<SendedEmailMessageDto>> GetAllByUserIdAsync (int userId);
        // Task UpdateAsync (int id);
        Task DeleteAsync (int id);
    }
}