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
    public class EmailAccountRepository : IEmailAccountRepository {
        private readonly EGrowerContext _context;

        public EmailAccountRepository (EGrowerContext context) {
            _context = context;
        }

        public async Task<EmailAccount> GetByIdAsync (int id, bool IfNoTracking = false) {
            if (IfNoTracking)
                return await _context.EmailAccounts.AsNoTracking ().SingleOrDefaultAsync (x => x.Id == id);
            return await _context.EmailAccounts.SingleOrDefaultAsync (x => x.Id == id);
        }

        public async Task<EmailAccount> GetByEmailAsync (string email, bool IfNoTracking = false) {
            if (IfNoTracking)
                return await _context.EmailAccounts.AsNoTracking ().SingleOrDefaultAsync (x => x.Email.ToLowerInvariant () == email.ToLowerInvariant ());
            return await _context.EmailAccounts.SingleOrDefaultAsync (x => x.Email.ToLowerInvariant () == email.ToLowerInvariant ());
        }

        public async Task<EmailAccount> GetByUserIdlAsync (int userId, int id, bool IfNoTracking = false) {
            if (IfNoTracking)
                return await _context.EmailAccounts.Include (a => a.User).AsNoTracking ().SingleOrDefaultAsync (x => x.User.Id == userId && x.Id == id);
            return await _context.EmailAccounts.Include (a => a.User).SingleOrDefaultAsync (x => x.User.Id == userId && x.Id == id);
        }
        public async Task<EmailAccount> GetByUserIdAndEmailAsync (int userId, string email, bool IfNoTracking = false) {
            if (IfNoTracking)
                return await _context.EmailAccounts.Include (a => a.User).AsNoTracking ().SingleOrDefaultAsync (x => x.Email.ToLowerInvariant () == email.ToLowerInvariant () && x.User.Id == userId);
            return await _context.EmailAccounts.Include (a => a.User).SingleOrDefaultAsync (x => x.Email.ToLowerInvariant () == email.ToLowerInvariant () && x.User.Id == userId);
        }
        public async Task<EmailAccount> GetByUserIdAndByEmailWithImapAsync (int userId, string email, bool IfNoTracking = false) {
            if (IfNoTracking)
                return await _context.EmailAccounts.Include (a => a.User).Include (a => a.Imap).AsNoTracking ().SingleOrDefaultAsync (x => x.Email.ToLowerInvariant () == email.ToLowerInvariant ());
            return await _context.EmailAccounts.Include (a => a.User).Include (a => a.Imap).SingleOrDefaultAsync (x => x.Email.ToLowerInvariant () == email.ToLowerInvariant ());
        }
        public async Task<EmailAccount> GetByUserIdAndByEmailWithSmtpAsync (int userId, string email, bool IfNoTracking = false) {
            if (IfNoTracking)
                return await _context.EmailAccounts.Include (a => a.User).Include (a => a.Smtp).AsNoTracking ().SingleOrDefaultAsync (x => x.Email.ToLowerInvariant () == email.ToLowerInvariant ());
            return await _context.EmailAccounts.Include (a => a.User).Include (a => a.Smtp).SingleOrDefaultAsync (x => x.Email.ToLowerInvariant () == email.ToLowerInvariant ());
        }
        public async Task<IEnumerable<EmailAccount>> GetAllByUserIdWithUserAndImapAsync (int userId, bool IfNoTracking = false) {
            if (IfNoTracking)
                return await Task.FromResult (_context.EmailAccounts.Include (a => a.User).Include (a => a.Imap).AsNoTracking ().Where (x => x.User.Id == userId).AsEnumerable ());;
            return await Task.FromResult (_context.EmailAccounts.Include (a => a.User).Include (a => a.Imap).Where (x => x.User.Id == userId).AsEnumerable ());
        }
        public async Task<IEnumerable<EmailAccount>> GetAllWithUserByUserIdAsync (int userId) =>
            await Task.FromResult (_context.EmailAccounts.Include (x => x.User).AsNoTracking ().Where (x => x.User.Id == userId).AsEnumerable ());
        public async Task<IEnumerable<EmailAccount>> BrowseAsync (string email = "") {
            var emailAccount = _context.EmailAccounts.AsNoTracking ().AsEnumerable ();

            if (!string.IsNullOrWhiteSpace (email)) {
                emailAccount = emailAccount.Where (x => x.Email.ToLowerInvariant ().Contains (email.ToLowerInvariant ()));
            }

            return await Task.FromResult (emailAccount);
        }

        public async Task AddAsync (EmailAccount emailAccount) {
            await _context.EmailAccounts.AddAsync (emailAccount);
            await _context.SaveChangesAsync ();
        }

        public async Task UpdateAsync (EmailAccount emailAccount) {
            await Task.CompletedTask;
        }

        public async Task DeleteAsync (EmailAccount emailAccount) {
            var emailAccountToRemove = await _context.EmailAccounts
                .Include (e => e.EmailMessages).ThenInclude (x => x.Atachments)
                .Include (e => e.SendedEmailMessages).ThenInclude (x => x.SendedAtachments)
                .SingleOrDefaultAsync (x => x.Id == emailAccount.Id);
            _context.EmailMessages.RemoveRange (emailAccountToRemove.EmailMessages);
            _context.SendedEmailMessages.RemoveRange (emailAccountToRemove.SendedEmailMessages);
            _context.EmailAccounts.Remove (emailAccountToRemove);
            await _context.SaveChangesAsync ();
        }
    }
}