using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Aggregates.Interfaces;
using EGrower.Infrastructure.DTO.EmailAccount;
using EGrower.Infrastructure.Factories;
using EGrower.Infrastructure.Factories.Interfaces;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;
using EGrower.Infrastructure.Validators.EmailAccount;

namespace EGrower.Infrastructure.Services {
    public class EmailAccountIMapService : IEmailAccountIMapService {
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IImapRepository _imapRepository;
        private readonly IMapper _mapper;
        private readonly IEmailClientAggregate _emailClientAggregate;

        public EmailAccountIMapService (IEmailAccountRepository emailAccountRepository,
            IEmailClientAggregate emailClientAggregate,
            IImapRepository imapRepository,
            IMapper mapper,
            IEmailClientAggregate iEmailClientAggregate) {
            _emailAccountRepository = emailAccountRepository;
            _imapRepository = imapRepository;
            _mapper = mapper;
            _emailClientAggregate = emailClientAggregate;
        }

        public async Task<bool> EmailAccountExistByEmailAsync (string email) =>
            await _emailAccountRepository.GetByEmailAsync (email) != null;
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

        public async Task CreateAsync (User user, string email, string password, string settingsHost, int settingsPort) {
            if (!await EmailAccountValuesValidator.EmailAccountUsingImapIsValid (settingsHost, settingsPort, email, password))
                throw new Exception ("Invalid imap email account's credentials");
            var emailAccount = new EmailAccount (email, password);
            emailAccount.AddUser (user);
            var imap = await _imapRepository.GetAsyncByPort (settingsPort);
            if (imap != null)
                emailAccount.AddImapSettings (imap);
            else {
                emailAccount.AddImapSettings (new Imap (settingsPort, settingsHost));
            }
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