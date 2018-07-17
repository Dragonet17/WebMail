using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Data;
using EGrower.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EGrower.Infrastructure.Repositories {
    public class UserRepository : IUserRepository {
        private readonly EGrowerContext _context;
        public UserRepository (EGrowerContext context) {
            _context = context;
        }
        public async Task AddAsync (User user) {
            await _context.Users.AddAsync (user);
            await _context.SaveChangesAsync ();
        }
        public async Task<User> GetAsync (int id, bool IfNoTracking = false) {
            if (IfNoTracking)
                return await _context.Users.AsNoTracking ().SingleOrDefaultAsync (c => c.Id == id);
            return await _context.Users.SingleOrDefaultAsync (c => c.Id == id);
        }
        public async Task<User> GetByEmailAsync (string email, bool IfNoTracking = false) {
            if (IfNoTracking)
                return await _context.Users.AsNoTracking ().SingleOrDefaultAsync (c => c.Email.ToLowerInvariant () == email.ToLowerInvariant ());
            return await _context.Users.SingleOrDefaultAsync (c => c.Email.ToLowerInvariant () == email.ToLowerInvariant ());
        }

        public async Task<User> GetWithEmailAccountAndImapAsync (int id) =>
            await _context.Users.Include (x => x.EmailAccounts).ThenInclude (x => x.Imap).AsNoTracking ().SingleOrDefaultAsync (c => c.Id == id);

        public async Task<IEnumerable<User>> GetAllWithEmailAccountAndImapAsync () =>
            await Task.FromResult (_context.Users.Include (x => x.EmailAccounts).ThenInclude (x => x.Imap).AsNoTracking ().AsEnumerable ());

        public async Task<User> GetWithEmailAccountsAsync (int id) =>
            await _context.Users.Include (x => x.EmailAccounts).AsNoTracking ().SingleOrDefaultAsync (c => c.Id == id);
        public async Task<User> GetByActivationKeyAsync (Guid activationKey) =>
            await _context.Users
            .Include (x => x.UserActivation)
            .SingleOrDefaultAsync (x => x.UserActivation.ActivationKey == activationKey && x.UserActivation.Inactive == false);
        public async Task<User> GetWithUserRestoringPasswordByAccountIdAsync (int id) =>
            await _context.Users.Include (x => x.UserRestoringPassword).SingleOrDefaultAsync (x => x.Id == id);

        public async Task<User> GetWithAccountRestoringPasswordByTokenAsync (Guid token) =>
            await _context.Users.Include (x => x.UserRestoringPassword).SingleOrDefaultAsync (x => x.UserRestoringPassword.Token == token);

        public async Task<IEnumerable<User>> BrowseAsync (string email = null) {
            var users = _context.Users.AsNoTracking ().AsEnumerable ();
            if (string.IsNullOrEmpty (email)) {
                users = users.Where (u => u.Email.ToLowerInvariant ().Contains (email.ToLowerInvariant ()));
            }
            return await Task.FromResult (users);
        }
        public async Task UpdateAsync (User user) {
            _context.Users.Update (user);
            await _context.SaveChangesAsync ();
        }
        public async Task DeleteAsync (User user) {
            _context.Users.Remove (user);
            await _context.SaveChangesAsync ();
        }
        public async Task Data (int id) {
            var user = await _context.Users.Where (u => u.Id == id).Select (aa => aa.EmailAccounts).SingleOrDefaultAsync ();
            await _context.SaveChangesAsync ();
        }
    }
}