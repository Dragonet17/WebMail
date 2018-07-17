using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Repositories.Interfaces {
    public interface ISendedEmailMessageRepository {
        Task AddAsync (SendedEmailMessage sendedEmailMessage);
        Task AddRangeAsync (ICollection<SendedEmailMessage> sendedEmailMessage);
        Task<SendedEmailMessage> GetAsync (int id);
        Task<IEnumerable<SendedEmailMessage>> GetAllByUserIdAsync (int userId, bool IsNoTracking = false);
        Task<IEnumerable<SendedEmailMessage>> GetAllForEmailAccountAsync (int emailAccountId);
        Task<SendedEmailMessage> GetWithAttachmentsByUserIdAsync (int userId, int id);
        // Task<SendedEmailMessage> GetWithUserByEmailAsync (int userId, string email);
        Task UpdateRangeAsync (ICollection<SendedEmailMessage> sendedEmailMessage);
        Task UpdateAsync (SendedEmailMessage sendedEmailMessage);
        Task DeleteAsync (SendedEmailMessage sendedEmailMessage);
    }
}