using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Data;
using EGrower.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EGrower.Infrastructure.Repositories {
    public class SendedEmailMessageRepository : ISendedEmailMessageRepository {
        EGrowerContext _context;
        protected SendedEmailMessageRepository () { }

        public SendedEmailMessageRepository (EGrowerContext context) {
            _context = context;
        }

        public async Task<SendedEmailMessage> GetAsync (int id) =>
            await _context.SendedEmailMessages.SingleOrDefaultAsync (x => x.Id == id);

        public async Task<IEnumerable<SendedEmailMessage>> GetAllByUserIdAsync (int userId, bool IsNoTracking = false) {
            if (IsNoTracking)
                return await Task.FromResult (_context.SendedEmailMessages.Include (x => x.EmailAccount).ThenInclude (x => x.User).AsNoTracking ().Where (x => x.EmailAccount.User.Id == userId).OrderBy (x => x.SendedAt).AsEnumerable ());
            return await Task.FromResult (_context.SendedEmailMessages.Include (x => x.EmailAccount).ThenInclude (x => x.User).Where (x => x.EmailAccount.User.Id == userId).OrderBy (x => x.SendedAt).AsEnumerable ());
        }
        public async Task<IEnumerable<SendedEmailMessage>> GetAllForEmailAccountAsync (int emailAccountId) {
            return await Task.FromResult (_context.SendedEmailMessages.Include (e => e.EmailAccount).AsNoTracking ().Where (e => e.EmailAccount.Id == emailAccountId).OrderByDescending (a => a.SendedAt).AsEnumerable ());
        }
        public async Task<SendedEmailMessage> GetWithAttachmentsByUserIdAsync (int userId, int id) =>
            await _context.SendedEmailMessages.Include (a => a.SendedAtachments).Include (a => a.EmailAccount).ThenInclude (a => a.User).AsNoTracking ().SingleOrDefaultAsync (x => x.Id == id && x.EmailAccount.User.Id == userId);
        public async Task AddAsync (SendedEmailMessage sendedEmailMessage) {
            await _context.SendedEmailMessages.AddAsync (sendedEmailMessage);
            await _context.SaveChangesAsync ();
        }

        public async Task AddRangeAsync (ICollection<SendedEmailMessage> sendedEmailMessage) {
            _context.SendedEmailMessages.AddRange (sendedEmailMessage);
            await _context.SaveChangesAsync ();
        }
        public async Task UpdateRangeAsync (ICollection<SendedEmailMessage> sendedEmailMessage) {
            _context.SendedEmailMessages.UpdateRange (sendedEmailMessage);
            await _context.SaveChangesAsync ();
        }
        public async Task UpdateAsync (SendedEmailMessage sendedEmailMessage) {
            _context.SendedEmailMessages.Update (sendedEmailMessage);
            await _context.SaveChangesAsync ();
        }

        public async Task DeleteAsync (SendedEmailMessage sendedEmailMessage) {
            _context.SendedEmailMessages.Remove (sendedEmailMessage);
            await _context.SaveChangesAsync ();
        }
    }
}