using System;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Factories.Interfaces;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;

namespace EGrower.Infrastructure.Services {
    public class AuthService : IAuthService {
        private readonly IUserRepository _userRepository;
        private readonly IUserEmailFactory _userEmailFactory;

        public AuthService (IUserRepository userRepository,
            IUserEmailFactory userEmailFactory) {
            _userEmailFactory = userEmailFactory;
            _userRepository = userRepository;
        }
        public async Task<User> LoginAsync (string email, string password) {
            var user = await _userRepository.GetByEmailAsync (email, true);
            if (user == null || !user.Activated || user.Deleted)
                return null;
            if (!VerifyPasswordHash (password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }
        public async Task RegisterAsync (string email, string password, string name, string surname, string country) {
            var user = new User (email, password, name, surname, country);
            var activationKey = Guid.NewGuid ();
            user.AddUserActivation (new UserActivation (activationKey));
            await _userRepository.AddAsync (user);
            await _userEmailFactory.SendActivationEmailAsync (user, activationKey);
        }
        private bool VerifyPasswordHash (string password, byte[] passwordHash, byte[] passwordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512 (passwordSalt)) {
                var computedHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }
    }
}