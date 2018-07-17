using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Data;
using EGrower.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EGrower.Infrastructure.Repositories {
    public class ImapRepository : IImapRepository {
        private readonly EGrowerContext _context;
        public ImapRepository (EGrowerContext context) {
            _context = context;
        }

        public async Task<Imap> GetAsyncByHost (string host) => await _context.Imaps.SingleOrDefaultAsync (x => x.Host.ToLowerInvariant () == host.ToLowerInvariant ());

        public async Task<Imap> GetAsyncById (int id) => await _context.Imaps.SingleOrDefaultAsync (x => x.Id == id);

        public async Task<Imap> GetAsyncByPort (int port) => await _context.Imaps.SingleOrDefaultAsync (x => x.Port == port);

        public async Task<Imap> GetAsyncByEmailProvider (string emailProvider) => await _context.Imaps.SingleOrDefaultAsync (x => x.EmailProvider.ToLowerInvariant () == emailProvider.ToLowerInvariant ());

        public async Task<IEnumerable<Imap>> BrowseAsync (string host = "") {
            var entities = _context.Imaps.AsEnumerable ();

            if (!string.IsNullOrWhiteSpace (host)) {
                entities = entities.Where (x => x.Host.ToLowerInvariant ().Contains (host.ToLowerInvariant ()));
            }

            return await Task.FromResult (entities);
        }

        public async Task UpdateAsync (Imap imap) {
            _context.Imaps.Update (imap);
            await _context.SaveChangesAsync ();
        }

        public async Task DeleteAsync (Imap imap) {
            _context.Imaps.Remove (imap);
            await _context.SaveChangesAsync ();
        }
    }
}