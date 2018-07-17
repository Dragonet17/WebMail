using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Aggregates.Interfaces;
using EGrower.Infrastructure.DTO.EmailAccount;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;
using EGrower.Infrastructure.Validators.EmailAccount;

namespace EGrower.Infrastructure.Services {
    public class EmailAccountService : IEmailAccountService {
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImapRepository _imapRepository;
        private readonly ISmtpRepository _smtpRepository;
        private readonly IMapper _mapper;
        private readonly IEmailClientAggregate _emailClientAggregate;

        public EmailAccountService (IEmailAccountRepository emailAccountRepository,
            IUserRepository userRepository,
            IEmailClientAggregate emailClientAggregate,
            IImapRepository imapRepository,
            ISmtpRepository smtpRepository,
            IMapper mapper) {
            _emailAccountRepository = emailAccountRepository;
            _userRepository = userRepository;
            _imapRepository = imapRepository;
            _smtpRepository = smtpRepository;
            _mapper = mapper;
            _emailClientAggregate = emailClientAggregate;
        }
        public async Task<bool> EmailAccountExistAsync (int id) =>
            await _emailAccountRepository.GetByIdAsync (id, true) != null;
        public async Task<bool> ExistsByEmailAndUserIdAsync (int userId, string email) =>
            await _emailAccountRepository.GetByUserIdAndEmailAsync (userId, email, true) != null;

        public async Task<bool> EmailAccountExistByEmailAsync (string email) =>
            await _emailAccountRepository.GetByEmailAsync (email, true) != null;
        public async Task CreateAsync (int userId, string email, string password, string imapHost, int imapPort, string smtpHost, int smtpPort) {
            if (!await EmailAccountValuesValidator.EmailAccountUsingImapIsValid (imapHost, imapPort, email, password))
                throw new Exception ("Invalid imap email account's credentials or check your email settings");
            if (!await EmailAccountValuesValidator.EmailAccountUsingSmtpIsValid (smtpHost, smtpPort))
                throw new Exception ("Invalid smtp email account's credentials or check your email settings");
            var emailAccount = new EmailAccount (email, password);
            var user = await _userRepository.GetAsync (userId);
            emailAccount.AddUser (user);
            var imap = await _imapRepository.GetAsyncByPort (imapPort);
            if (imap != null)
                emailAccount.AddImapSettings (imap);
            else {
                emailAccount.AddImapSettings (new Imap (imapPort, imapHost));
            }
            var smtp = await _smtpRepository.GetAsyncByPort (smtpPort);
            if (smtp != null)
                emailAccount.AddSmtpSettings (smtp);
            else {
                emailAccount.AddSmtpSettings (new Smtp (smtpPort, smtpHost));
            }
            try {
                await _emailClientAggregate.GetEmailsFromEmailAccountAsync (emailAccount, imapHost, imapPort);
                // await _emailAccountRepository.AddAsync (emailAccount);
            } catch {
                throw new Exception ("Something went wrong during adding email account's datas");
            }
        }

        public async Task<IEnumerable<EmailAccountDTO>> GetAllWIthUserByUserIdAsync (int userId) {
            var userEmailEccounts = await _emailAccountRepository.GetAllWithUserByUserIdAsync (userId);
            if (userEmailEccounts == null) {
                throw new Exception ("User with this id does not have email accounts");
            }
            return _mapper.Map<IEnumerable<EmailAccountDTO>> (userEmailEccounts);
        }

        public async Task DeleteAsync (int userId, int id) {
            var emailAccountToDelete = await _emailAccountRepository.GetByUserIdlAsync (userId, id);
            if (emailAccountToDelete == null)
                throw new Exception ("Email account with this id does not exist.");
            await _emailAccountRepository.DeleteAsync (emailAccountToDelete);
        }
    }
}