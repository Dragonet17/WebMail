using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Repositories.Interfaces {
    public interface IEmailAccountProtocolRepository<T> where T : EmailAccountProtocol {
        Task<T> GetAsyncById (int id);
        Task<T> GetAsyncByPort (int port);
        Task<T> GetAsyncByHost (string host);
        Task<T> GetAsyncByEmailProvider (string emailProvider);
        Task<IEnumerable<T>> BrowseAsync (string host = "");
        Task UpdateAsync (T T);
        Task DeleteAsync (T T);
    }
}