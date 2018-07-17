using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EGrower.Infrastructure.DTO.EmailAccount;

namespace EGrower.Infrastructure.Services.Interfaces
{
    public interface ISmtpService
    {
        Task<SmtpDTO> GetAsyncById(int id);
        Task<SmtpDTO> GetAsyncByHost(string host);
        Task<SmtpDTO> GetAsyncByPort(int port);
        Task<SmtpDTO> GetAsyncByEmailProvider(string emailProvider);
        Task<IEnumerable<SmtpDTO>> BrowseAsync(string host = null);
        Task UpdateAsync(string host, int port, string emailProvider);
        Task DeleteAsync(int id);
    }
}
