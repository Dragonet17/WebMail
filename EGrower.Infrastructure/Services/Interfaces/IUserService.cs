using System;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO;
using EGrower.Infrastructure.DTO.EmaiMessage;

namespace EGrower.Infrastructure.Services.Interfaces {
    public interface IUserService {
        Task<bool> UserExistAsync (int id);
        Task<bool> UserExistByEmailAsync (string email);
        Task<User> GetActiveByEmailAsync (string email);
        Task<UserDTO> GetActiveWithEmailAccountsAsync (int id);
        Task ActivateAsync (Guid activationKey);
        Task UpdateAsync (int id, string name, string surname, string country);
        Task RestorePasswordAsync (User user);
        Task ChangePasswordByRestoringPassword (string userEmail, Guid token, string newPassword);
        Task UpdatePasswordAsync (User admin, string newPassword);
        Task DeleteAsync (int id);
    }
}