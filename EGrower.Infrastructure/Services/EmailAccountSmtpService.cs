using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO.EmailAccount;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;

namespace EGrower.Infrastructure.Services {
    public class EmailAccountSmtpService : IEmailAccountSmtpService {
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IEmailAccountProtocolRepository<Smtp> _settingsRepository; //IMAP or SMTP
        private readonly IMapper _mapper;

        public EmailAccountSmtpService () { }

        public EmailAccountSmtpService (IEmailAccountRepository emailAccountRepository,
            IEmailAccountProtocolRepository<Smtp> settingsRepository,
            IMapper mapper) {
            _emailAccountRepository = emailAccountRepository;
            _settingsRepository = settingsRepository;
            _mapper = mapper;
        }

        public async Task<EmailAccountDTO> GetAsyncByEmail (string email) {
            var emailAccount = await _emailAccountRepository.GetByEmailAsync (email);

            return _mapper.Map<EmailAccountDTO> (emailAccount);
        }

        public async Task<EmailAccountDTO> GetAsyncById (int id) {
            var emailAccount = await _emailAccountRepository.GetByIdAsync (id);

            return _mapper.Map<EmailAccountDTO> (emailAccount);
        }

        public async Task<IEnumerable<EmailAccountDTO>> BrowseAsync (string email = null) {
            var emailAccount = await _emailAccountRepository.BrowseAsync (email);

            return _mapper.Map<IEnumerable<EmailAccountDTO>> (emailAccount);
        }

        public async Task CreateAsync (string email, string password, string SettingsHost, int SettingsPort) {
            var emailAccount = await _emailAccountRepository.GetByEmailAsync (email);
            var settings = await _settingsRepository.GetAsyncByPort (SettingsPort);

            if (emailAccount != null) {
                throw new Exception ($"Client with E-Mail: '{email}' already exsist.");
            }
            if (settings == null) {
                settings = new Smtp (SettingsPort, SettingsHost);
            }
            emailAccount = new EmailAccount (email, password);
            await _emailAccountRepository.AddAsync (emailAccount);
        }

        public async Task UpdateAsync (string email, string password) {
            var emailAccount = await _emailAccountRepository.GetByEmailAsync (email);

            emailAccount.Update (email);
            emailAccount.UpdatePassword (password);

            await _emailAccountRepository.UpdateAsync (emailAccount);
        }

        public async Task RemoveAsync (int id) {
            var emailAccount = await _emailAccountRepository.GetByIdAsync (id);
            emailAccount.Delete ();
        }

        public async Task DeleteAsync (int id) {
            var emailAccount = await _emailAccountRepository.GetByIdAsync (id);
            await _emailAccountRepository.DeleteAsync (emailAccount);
        }
    }
}