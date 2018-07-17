using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EGrower.Infrastructure.DTO.EmailAccount;

namespace EGrower.Infrastructure.Services.Interfaces
{
    public interface IIMapService
    {
        Task<IMapDTO> GetAsyncById(int id);
        Task<IMapDTO> GetAsyncByHost(string host);
        Task<IMapDTO> GetAsyncByPort(int port);
        Task<IMapDTO> GetAsyncByEmailProvider(string emailProvider);
        Task<IEnumerable<IMapDTO>> BrowseAsync(string host = null);
        Task UpdateAsync(string host, int port, string emailProvider);
        Task DeleteAsync(int id);
    }
}
