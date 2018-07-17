using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Repositories.Interfaces
{
    public interface ISendedAtachmentRepository
    {
        Task AddAsync (SendedAtachment sendedAtachment);
        Task<SendedAtachment> GetAsync (int id);
        Task<IEnumerable<SendedAtachment>> BrowseByEmailMessageIdAsync (int sendedEmailMessageId);
        Task UpdateAsync (SendedAtachment sendedAtachment);
        Task DeleteAsync (SendedAtachment sendedAtachment);
    }
}