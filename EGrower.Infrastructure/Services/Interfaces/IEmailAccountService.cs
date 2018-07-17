using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO.EmailAccount;

namespace EGrower.Infrastructure.Services.Interfaces {
    public interface IEmailAccountService {
        Task<bool> EmailAccountExistAsync (int id);
        Task<bool> ExistsByEmailAndUserIdAsync (int userId, string email);
        Task CreateAsync (int userId, string email, string password, string imapHost, int imapPort, string smtpHost, int smtpPort);
        Task<bool> EmailAccountExistByEmailAsync (string email);
        Task<IEnumerable<EmailAccountDTO>> GetAllWIthUserByUserIdAsync (int userId);
        Task DeleteAsync (int userId, int id);
    }
}