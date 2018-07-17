using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO.EmaiMessage;
using Microsoft.AspNetCore.Http;

namespace EGrower.Infrastructure.Services.Interfaces {
    public interface IEmailMessageService {
        Task<EmailMessageDetailsDTO> GetAsync (int userId, int emailId);
        Task<EmailMessageDetailsDTO> GetForUserAsync (int userId, int emailId);
        Task<EmailMessageDetailsDTO> GetDeletedForUserAsync (int userId, int emailId);
        Task<EmailMessageDetailsDTO> GetUndeletedForUserAsync (int userId, int emailId);
        Task<EmailMessageDetailsDTO> GetForEmailAccountAsync (int emailAccountId, int emailId);
        Task<IEnumerable<EmailMessageDto>> GetAllForUserAsync (int userId);
        Task<IEnumerable<EmailMessageDto>> GetAllForEmailAccountAsync (int emailAccountId);
        Task<MemoryStream> GetUserAttachmentsAsync (int userId, int emailId);
        Task<MemoryStream> GetAttachmentsForEmailAccountAsync (int emailAccountId, int emailId);
        Task MarkAsReadAsync (int userId, int emailId);
        Task MarkAsUnreadAsync (int userId, int emailId);
        // Task GetNewEmailMessagesAsync (int userId);
        // Task GetForAllNewEmailMessagesAsync ();
        Task<IEnumerable<EmailMessageDto>> GetAllDeletedForUserAsync (int userId);
        Task<IEnumerable<EmailMessageDto>> GetAllUnDeletedForUserAsync (int userId);
        Task ReplyToAsync (int id, int userId, string answer, bool replyToAll, ICollection<IFormFile> attachments = null, ICollection<string> cc = null, ICollection<string> bcc = null);
        Task DeleteUserMessageAsync (int id, int userId);
        Task RestoreMessageFromTrashAsync (int id, int userId);
        Task RemoveDeletedUserMessageAsync (int id, int userId);
    }
}