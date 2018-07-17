using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Repositories.Interfaces {
    public interface IAtachmentRepository {
        Task AddAsync (Atachment atachment);
        Task<Atachment> GetAsync (int id);
        Task<IEnumerable<Atachment>> BrowseByEmailMessageIdAsync (int emailMessageId);
        Task UpdateAsync (Atachment atachment);
        Task DeleteAsync (Atachment atachment);
    }
}