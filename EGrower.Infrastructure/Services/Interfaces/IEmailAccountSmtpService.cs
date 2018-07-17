using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Infrastructure.DTO.EmailAccount;

namespace EGrower.Infrastructure.Services.Interfaces
{
    public interface IEmailAccountSmtpService
    {
         Task<EmailAccountDTO> GetAsyncById(int id);
        Task<EmailAccountDTO> GetAsyncByEmail(string email);
        Task<IEnumerable<EmailAccountDTO>> BrowseAsync(string email = null);
        Task CreateAsync(string email, string password, string SettingsHost, int SettingsPort);
        Task UpdateAsync(string email, string password);
        Task RemoveAsync(int id);
        Task DeleteAsync(int id);
    }
}