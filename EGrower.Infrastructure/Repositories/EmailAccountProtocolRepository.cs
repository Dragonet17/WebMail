using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Data;
using EGrower.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EGrower.Infrastructure.Repositories {
    public class EmailAccountProtocolRepository<T> : IEmailAccountProtocolRepository<T> where T : EmailAccountProtocol {
        private readonly EGrowerContext _context;
        private DbSet<T> _entities;

        public EmailAccountProtocolRepository () { }

        public EmailAccountProtocolRepository (EGrowerContext context) {
            _context = context;
            _entities = context.Set<T> ();
        }

        public async Task<T> GetAsyncByHost (string host) => await _entities.SingleOrDefaultAsync (x => x.Host.ToLowerInvariant () == host.ToLowerInvariant ());

        public async Task<T> GetAsyncById (int id) => await _entities.SingleOrDefaultAsync (x => x.Id == id);

        public async Task<T> GetAsyncByPort (int port) => await _entities.SingleOrDefaultAsync (x => x.Port == port);

        public async Task<T> GetAsyncByEmailProvider (string emailProvider) => await _entities.SingleOrDefaultAsync (x => x.EmailProvider.ToLowerInvariant () == emailProvider.ToLowerInvariant ());

        public async Task<IEnumerable<T>> BrowseAsync (string host = "") {
            var entities = _entities.AsEnumerable ();

            if (!string.IsNullOrWhiteSpace (host)) {
                entities = entities.Where (x => x.Host.ToLowerInvariant ().Contains (host.ToLowerInvariant ()));
            }

            return await Task.FromResult (entities);
        }

        public async Task UpdateAsync (T t) {
            await Task.CompletedTask;
        }

        public async Task DeleteAsync (T T) {
            _entities.Remove (T);
            await _context.SaveChangesAsync ();
        }
    }
}