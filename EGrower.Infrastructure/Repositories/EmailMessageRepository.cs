using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Data;
using EGrower.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EGrower.Infrastructure.Repositories {
    public class EmailMessageRepository : IEmailMessageRepository {
        EGrowerContext _context;
        public EmailMessageRepository (EGrowerContext context) {
            _context = context;
        }

        public async Task<EmailMessage> GetAsync (int id) =>
            await Task.FromResult (_context.EmailMessages.SingleOrDefault (x => x.Id == id));
        public async Task<EmailMessage> GetByUserIdAsync (int userId, int id) =>
            await _context.EmailMessages.Include (e => e.EmailAccount).ThenInclude (u => u.User).AsNoTracking ().SingleOrDefaultAsync (x => x.EmailAccount.User.Id == userId && x.Id == id);

        public async Task<EmailMessage> GetDeletedByUserIdAsync (int userId, int id) =>
            await _context.EmailMessages.Include (e => e.EmailAccount).ThenInclude (u => u.User).AsNoTracking ().SingleOrDefaultAsync (x => x.EmailAccount.User.Id == userId && x.Id == id && x.Deleted == true);

        public async Task<EmailMessage> GetUndeletedByUserIdAsync (int userId, int id) =>
            await _context.EmailMessages.Include (e => e.EmailAccount).ThenInclude (u => u.User).AsNoTracking ().SingleOrDefaultAsync (x => x.EmailAccount.User.Id == userId && x.Id == id && x.Deleted == false);

        public async Task<EmailMessage> GetWithAttachmentsByUserIdAsync (int id, int userId) =>
            await _context.EmailMessages.Include (a => a.Atachments).Include (a => a.EmailAccount).ThenInclude (a => a.User).AsNoTracking ().SingleOrDefaultAsync (x => x.Id == id && x.EmailAccount.User.Id == userId);
        public async Task<EmailMessage> GetLastByEmailAccountIdAsync (int accountId) {
            return await _context.EmailMessages.Include (a => a.EmailAccount).AsNoTracking ().LastOrDefaultAsync (x => x.EmailAccount.Id == accountId);
        }

        public async Task<EmailMessage> GetByUserIdWithEmailAccountAndEmailProvidersAsync (int userId, int id) =>
            await _context.EmailMessages.Include (e => e.EmailAccount).ThenInclude (k => k.Imap).Include (e => e.EmailAccount).ThenInclude (k => k.Smtp).Include (a => a.EmailAccount).ThenInclude (u => u.User).AsNoTracking ().SingleOrDefaultAsync (x => x.EmailAccount.User.Id == userId && x.Id == id);

        public async Task AddAsync (EmailMessage emailMessage) {
            await _context.EmailMessages.AddAsync (emailMessage);
            await _context.SaveChangesAsync ();
        }

        public async Task AddRangeAsync (ICollection<EmailMessage> emailMessage) {
            await _context.EmailMessages.AddRangeAsync (emailMessage);
            await _context.SaveChangesAsync ();
        }

        public async Task<IEnumerable<EmailMessage>> GetAllForUserAsync (int userId) {
            return await Task.FromResult (_context.EmailMessages.Include (e => e.EmailAccount).ThenInclude (u => u.User).AsNoTracking ().Where (e => e.EmailAccount.User.Id == userId).OrderByDescending (a => a.DeliveredAt).AsEnumerable ());
        }

        public async Task<IEnumerable<EmailMessage>> GetAllForEmailAccountAsync (int emailAccountId) {
            return await Task.FromResult (_context.EmailMessages.Include (e => e.EmailAccount).AsNoTracking ().Where (e => e.EmailAccount.Id == emailAccountId).OrderByDescending (a => a.DeliveredAt).AsEnumerable ());
        }

        public async Task<IEnumerable<EmailMessage>> GetAllDeletedForUserAsync (int userId) {
            return await Task.FromResult (_context.EmailMessages.Include (e => e.EmailAccount).ThenInclude (u => u.User).AsNoTracking ().Where (e => e.EmailAccount.User.Id == userId && e.Deleted == true).OrderByDescending (a => a.DeliveredAt).AsEnumerable ());
        }

        public async Task<IEnumerable<EmailMessage>> GetAllUndeletedMessagesForUserAsync (int userId) {
            return await Task.FromResult (_context.EmailMessages.Include (e => e.EmailAccount).ThenInclude (u => u.User).AsNoTracking ().Where (e => e.EmailAccount.User.Id == userId && e.Deleted == false).OrderByDescending (a => a.DeliveredAt).AsEnumerable ());
        }
        public async Task UpdateRangeAsync (ICollection<EmailMessage> emailMessage) {
            _context.EmailMessages.UpdateRange (emailMessage);
            await _context.SaveChangesAsync ();
        }

        public async Task UpdateAsync (EmailMessage emailMessage) {
            _context.EmailMessages.Update (emailMessage);
            await _context.SaveChangesAsync ();
        }

        public async Task DeleteAsync (EmailMessage emailMessage) {
            _context.EmailMessages.Update (emailMessage);
            await _context.SaveChangesAsync ();
        }
        public async Task RemoveAsync (EmailMessage emailMessage) {
            _context.EmailMessages.Remove (emailMessage);
            await _context.SaveChangesAsync ();
        }
    }
}