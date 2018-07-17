using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Repositories.Interfaces {
    public interface IEmailAccountRepository {
        Task<EmailAccount> GetByIdAsync (int id, bool IfNoTracking = false);
        Task<EmailAccount> GetByEmailAsync (string email, bool IfNoTracking = false);
        Task<EmailAccount> GetByUserIdlAsync (int userId, int id, bool IfNoTracking = false);
        Task<EmailAccount> GetByUserIdAndEmailAsync (int userId, string email, bool IfNoTracking = false);
        Task<EmailAccount> GetByUserIdAndByEmailWithImapAsync (int userId, string email, bool IfNoTracking = false);
        Task<EmailAccount> GetByUserIdAndByEmailWithSmtpAsync (int userId, string email, bool IfNoTracking = false);
        Task<IEnumerable<EmailAccount>> GetAllWithUserByUserIdAsync (int userId);
        Task<IEnumerable<EmailAccount>> GetAllByUserIdWithUserAndImapAsync (int userId, bool IfNoTracking = false);
        Task<IEnumerable<EmailAccount>> BrowseAsync (string email = "");
        Task AddAsync (EmailAccount emailAccount);
        Task UpdateAsync (EmailAccount emailAccount);
        Task DeleteAsync (EmailAccount emailAccount);
    }
}