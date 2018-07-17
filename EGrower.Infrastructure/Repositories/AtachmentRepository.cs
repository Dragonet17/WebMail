using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Data;
using EGrower.Infrastructure.Repositories.Interfaces;

namespace EGrower.Infrastructure.Repositories {
    public class AtachmentRepository : IAtachmentRepository {
        EGrowerContext _context;

        protected AtachmentRepository () { }

        public AtachmentRepository (EGrowerContext context) {
            _context = context;
        }

        public async Task<Atachment> GetAsync (int id)
            => await Task.FromResult (_context.Atachments.SingleOrDefault (x => x.Id == id));

        public async Task<IEnumerable<Atachment>> BrowseByEmailMessageIdAsync (int emailMessageId) {
            var atachments = _context.Atachments.AsEnumerable ();
            if (emailMessageId > 0) {
                atachments = atachments.Where (x => x.EmailMessageId == emailMessageId);
            }
            return await Task.FromResult (atachments);
        }

        public async Task AddAsync (Atachment atachment) {
            await _context.Atachments.AddAsync (atachment);
            await _context.SaveChangesAsync ();
        }

        public async Task UpdateAsync (Atachment atachment) {
            _context.Atachments.Update (atachment);
            await _context.SaveChangesAsync ();

        }

        public async Task DeleteAsync (Atachment atachment) {
            _context.Atachments.Remove (atachment);
            await _context.SaveChangesAsync ();
        }
        
    }
}