using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Data;
using EGrower.Infrastructure.Repositories.Interfaces;

namespace EGrower.Infrastructure.Repositories
{
    public class SendedAtachmentRepository : ISendedAtachmentRepository
    {
        EGrowerContext _context;

        public SendedAtachmentRepository(EGrowerContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SendedAtachment sendedAtachment)
        {
            await _context.AddAsync(sendedAtachment);
            await _context.SaveChangesAsync();
        }

        public async Task<SendedAtachment> GetAsync(int id)
            => await Task.FromResult(_context.SendedAtachments.SingleOrDefault(x => x.Id == id));

        public async Task<IEnumerable<SendedAtachment>> BrowseByEmailMessageIdAsync(int sendedEmailMessageId)
        {
            var sendedAtachments = _context.SendedAtachments.AsEnumerable();
            if (sendedEmailMessageId > 0) {
                sendedAtachments = sendedAtachments.Where (x => x.SendedEmailMessageId == sendedEmailMessageId);
            }
            return await Task.FromResult (sendedAtachments);
        }

        public async Task UpdateAsync(SendedAtachment sendedAtachment)
        {
            _context.SendedAtachments.Update (sendedAtachment);
            await _context.SaveChangesAsync ();
        }

        public async Task DeleteAsync(SendedAtachment sendedAtachment)
        {
            _context.SendedAtachments.Remove (sendedAtachment);
            await _context.SaveChangesAsync ();
        }
    }
}