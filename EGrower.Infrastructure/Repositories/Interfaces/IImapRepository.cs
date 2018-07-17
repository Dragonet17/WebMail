using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Repositories.Interfaces {
    public interface IImapRepository {
        Task<Imap> GetAsyncById (int id);
        Task<Imap> GetAsyncByPort (int port);
        Task<Imap> GetAsyncByHost (string host);
        Task<Imap> GetAsyncByEmailProvider (string emailProvider);
        Task<IEnumerable<Imap>> BrowseAsync (string host = "");
        Task UpdateAsync (Imap imap);
        Task DeleteAsync (Imap imap);
    }
}