using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Repositories.Interfaces {
    public interface IUserRepository {
        Task AddAsync (User user);
        Task<User> GetAsync (int id, bool IfNoTracking = false);
        Task<User> GetWithEmailAccountAndImapAsync (int id);
        Task<IEnumerable<User>> GetAllWithEmailAccountAndImapAsync ();
        Task<User> GetWithEmailAccountsAsync (int id);
        Task<User> GetByEmailAsync (string email, bool IfNoTracking = false);
        Task<User> GetByActivationKeyAsync (Guid activationKey);
        Task<User> GetWithUserRestoringPasswordByAccountIdAsync (int id);
        Task<User> GetWithAccountRestoringPasswordByTokenAsync (Guid token);
        Task<IEnumerable<User>> BrowseAsync (string email = null);
        Task UpdateAsync (User user);
        Task DeleteAsync (User user);
    }
}