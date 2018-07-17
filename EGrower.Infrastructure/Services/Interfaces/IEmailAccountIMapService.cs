using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO.EmailAccount;

namespace EGrower.Infrastructure.Services.Interfaces {
    public interface IEmailAccountIMapService {
        Task<bool> EmailAccountExistByEmailAsync (string email);
        Task<EmailAccountDTO> GetAsyncById (int id);
        Task<EmailAccountDTO> GetAsyncByEmail (string email);
        Task<IEnumerable<EmailAccountDTO>> BrowseAsync (string email = null);
        Task CreateAsync (User user, string email, string password, string SettingsHost, int SettingsPort);
        Task UpdateAsync (string email, string password);
        Task RemoveAsync (int id);
        Task DeleteAsync (int id);
    }
}