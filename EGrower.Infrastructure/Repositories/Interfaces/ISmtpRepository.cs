using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Repositories.Interfaces {
    public interface ISmtpRepository {

        Task<Smtp> GetAsyncById (int id);
        Task<Smtp> GetAsyncByPort (int port);
        Task<Smtp> GetAsyncByHost (string host);
        Task<Smtp> GetAsyncByEmailProvider (string emailProvider);
        Task<IEnumerable<Smtp>> BrowseAsync (string host = "");
        Task UpdateAsync (Smtp smtp);
        Task DeleteAsync (Smtp smtp);
    }
}