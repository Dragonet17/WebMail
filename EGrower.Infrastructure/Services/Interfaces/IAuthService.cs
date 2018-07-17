using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Services.Interfaces {
    public interface IAuthService {
        Task RegisterAsync (string email, string password, string name, string surname, string country);
        Task<User> LoginAsync (string email, string password);
    }
}