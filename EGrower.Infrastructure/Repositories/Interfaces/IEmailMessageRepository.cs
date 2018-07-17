using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Repositories.Interfaces {
    public interface IEmailMessageRepository {
        Task AddAsync (EmailMessage emailMessage);
        Task AddRangeAsync (ICollection<EmailMessage> emailMessage);
        Task<EmailMessage> GetAsync (int id);
        Task<EmailMessage> GetByUserIdAsync (int userId, int id);
        Task<EmailMessage> GetByUserIdWithEmailAccountAndEmailProvidersAsync (int userId, int id);
        Task<EmailMessage> GetDeletedByUserIdAsync (int userId, int id);
        Task<EmailMessage> GetUndeletedByUserIdAsync (int userId, int id);
        Task<EmailMessage> GetLastByEmailAccountIdAsync (int accountId);
        Task<IEnumerable<EmailMessage>> GetAllForUserAsync (int userId);
        Task<IEnumerable<EmailMessage>> GetAllUndeletedMessagesForUserAsync (int userId);
        Task<IEnumerable<EmailMessage>> GetAllForEmailAccountAsync (int emailAccountId);
        Task<EmailMessage> GetWithAttachmentsByUserIdAsync (int id, int userId);
        Task<IEnumerable<EmailMessage>> GetAllDeletedForUserAsync (int userId);
        Task UpdateAsync (EmailMessage emailMessage);
        Task UpdateRangeAsync (ICollection<EmailMessage> emailMessage);
        Task DeleteAsync (EmailMessage emailMessage);
        Task RemoveAsync (EmailMessage emailMessage);
    }
}