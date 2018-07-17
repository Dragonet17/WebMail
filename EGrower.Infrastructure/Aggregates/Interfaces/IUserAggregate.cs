using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Aggregates.Interfaces {
    public interface IUserAggregate {
        Task<IEnumerable<string>> GetEmailAccountsContactsAsync (int userId);
        Task GetNewEmailsByUserIdAsync (int userId);
        Task GetNewEmailsForAllUsersAsync ();
    }
}