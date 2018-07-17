using System;
using System.Threading.Tasks;
using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Aggregates.Interfaces;
using EGrower.Infrastructure.DTO;
using EGrower.Infrastructure.DTO.EmaiMessage;
using EGrower.Infrastructure.Extension.ExtensionMethods;
using EGrower.Infrastructure.Factories.Interfaces;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;

namespace EGrower.Infrastructure.Services {
    public class UserService : IUserService {
        private readonly IUserRepository _userRepository;
        private readonly IUserEmailFactory _userEmailFactory;
        private readonly IEmailClientAggregate _emailClientAggregate;
        private readonly IMapper _mapper;

        public UserService (IUserRepository userRepository,
            IEmailClientAggregate emailClientAggregate,
            IUserEmailFactory userEmailFactory,
            IMapper mapper) {
            _userRepository = userRepository;
            _emailClientAggregate = emailClientAggregate;
            _userEmailFactory = userEmailFactory;
            _mapper = mapper;
        }
        public async Task<bool> UserExistAsync (int id) =>
            await _userRepository.GetAsync (id, true) != null;
        public async Task<bool> UserExistByEmailAsync (string email) =>
            await _userRepository.GetByEmailAsync (email, true) != null;

        public async Task<User> GetActiveByEmailAsync (string email) { // this method to the repo or use the automapper
            var user = await _userRepository.GetByEmailAsync (email, true);
            if (user == null || user.Deleted || !user.Activated)
                return null;
            return user;
        }

        public async Task<UserDTO> GetActiveWithEmailAccountsAsync (int id) {
            var activeUserWithEmailAccounts = await _userRepository.GetWithEmailAccountsAsync (id);
            if (activeUserWithEmailAccounts == null ||
                activeUserWithEmailAccounts.Deleted ||
                !activeUserWithEmailAccounts.Activated ||
                activeUserWithEmailAccounts.EmailAccounts.Count == 0
            )
                throw new Exception ("Active user with this id does not exist or he doest not have email account");
            return _mapper.Map<UserDTO> (activeUserWithEmailAccounts);
        }

        public async Task UpdatePasswordAsync (User user, string newPassword) {
            user.UpdatePassword (newPassword);
            await _userRepository.UpdateAsync (user);
        }
        public async Task ActivateAsync (Guid activationKey) {
            var user = await _userRepository.GetByActivationKeyAsync (activationKey);
            if (user == null) {
                throw new Exception ("Your activation key is incorrect");
            }
            user.Activate (user.UserActivation);
            await _userRepository.UpdateAsync (user);
        }

        public async Task UpdateAsync (int id, string name, string surname, string country) {
            var user = await _userRepository.GetAsync (id);
            user.Update (name, surname, country);
            await _userRepository.UpdateAsync (user);
        }

        public async Task DeleteAsync (int id) {
            var user = await _userRepository.GetAsync (id);
            await _userRepository.DeleteAsync (user);
        }

        public async Task RestorePasswordAsync (User user) {
            var token = Guid.NewGuid ();
            var userToPaswordRestor = await _userRepository.GetWithUserRestoringPasswordByAccountIdAsync (user.Id);
            if (userToPaswordRestor.UserRestoringPassword != null && userToPaswordRestor.Activated == true && userToPaswordRestor.Deleted == false)
                userToPaswordRestor.ChangeUserRestoringPassword (token);
            else
                userToPaswordRestor.AddUserRestoringPassword (new UserRestoringPassword (token));
            await _userEmailFactory.SendRecoveringPasswordEmailAsync (userToPaswordRestor, token);
            await _userRepository.UpdateAsync (userToPaswordRestor);
        }

        public async Task ChangePasswordByRestoringPassword (string userEmail, Guid token, string newPassword) {
            var user = await _userRepository.GetWithAccountRestoringPasswordByTokenAsync (token);
            if (user == null || user.UserRestoringPassword == null || user.UserRestoringPassword.Restored)
                throw new Exception ("Your token is incorrect");
            if (user.Email.ToLowerInvariant () != userEmail.ToLowerInvariant ())
                throw new Exception ("Invalid email address");
            await UpdatePasswordAsync (user, newPassword);
            user.UserRestoringPassword.PasswordRestoring ();
            await _userRepository.UpdateAsync (user);
        }
    }
}