using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Data;
using EGrower.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EGrower.Infrastructure.Repositories {
    public class SmtpRepository:ISmtpRepository {
        private readonly EGrowerContext _context;
        public SmtpRepository (EGrowerContext context) {
            _context = context;
        }

        public async Task<Smtp> GetAsyncByHost (string host) => await _context.Smtp.SingleOrDefaultAsync (x => x.Host.ToLowerInvariant () == host.ToLowerInvariant ());

        public async Task<Smtp> GetAsyncById (int id) => await _context.Smtp.SingleOrDefaultAsync (x => x.Id == id);

        public async Task<Smtp> GetAsyncByPort (int port) => await _context.Smtp.SingleOrDefaultAsync (x => x.Port == port);

        public async Task<Smtp> GetAsyncByEmailProvider (string emailProvider) => await _context.Smtp.SingleOrDefaultAsync (x => x.EmailProvider.ToLowerInvariant () == emailProvider.ToLowerInvariant ());

        public async Task<IEnumerable<Smtp>> BrowseAsync (string host = "") {
            var entities = _context.Smtp.AsEnumerable ();

            if (!string.IsNullOrWhiteSpace (host)) {
                entities = entities.Where (x => x.Host.ToLowerInvariant ().Contains (host.ToLowerInvariant ()));
            }

            return await Task.FromResult (entities);
        }

        public async Task UpdateAsync (Smtp smtp) {
            _context.Smtp.Update (smtp);
            await _context.SaveChangesAsync ();
        }

        public async Task DeleteAsync (Smtp smtp) {
            _context.Smtp.Remove (smtp);
            await _context.SaveChangesAsync ();
        }
    }
}