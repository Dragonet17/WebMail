using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Aggregates.Interfaces;
using EGrower.Infrastructure.Data;
using EGrower.Infrastructure.Factories.Interfaces;
using EGrower.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EGrower.Infrastructure.Aggregates {
    public class UserAggregate : IUserAggregate {
        private readonly EGrowerContext _context;
        private readonly IEmailMessageRepository _emailMessageRepository;
        private readonly ISendedEmailMessageRepository _sendedEmailMessageRepository;
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IEmailClientAggregate _emailClientAggregate;

        public UserAggregate (EGrowerContext context,
            IEmailMessageRepository emailMessageRepository,
            ISendedEmailMessageRepository sendedEmailMessageRepository,
            IEmailAccountRepository emailAccountRepository,
            IEmailClientAggregate emailClientAggregate) {
            _context = context;
            _emailMessageRepository = emailMessageRepository;
            _sendedEmailMessageRepository = sendedEmailMessageRepository;
            _emailAccountRepository = emailAccountRepository;
            _emailClientAggregate = emailClientAggregate;

        }

        public async Task<IEnumerable<string>> GetEmailAccountsContactsAsync (int userId) {
            List<string> contacts = new List<string> ();
            var contactsFromDeliveredEmails = await Task.FromResult (_context.EmailMessages
                .Include (a => a.EmailAccount)
                .ThenInclude (b => b.User)
                .Where (e => e.EmailAccount.User.Id == userId)
                .Select (a => a.From));
            contacts.AddRange (contactsFromDeliveredEmails);
            var contactsFromSendedEmails = await Task.FromResult (_context.SendedEmailMessages
                .Include (a => a.EmailAccount)
                .ThenInclude (b => b.User)
                .Where (e => e.EmailAccount.User.Id == userId)
                .Select (a => a.To));
            await contactsFromSendedEmails.ForEachAsync (a => {
                var splitedContactsFromSendedEmails = a.Split (',').ToList ();
                contacts.AddRange (splitedContactsFromSendedEmails);
            });
            return contacts.Distinct ();
        }

        public async Task GetNewEmailsByUserIdAsync (int userId) {
            var userEmailAccounts = await _emailAccountRepository.GetAllByUserIdWithUserAndImapAsync (userId);
            if (userEmailAccounts != null && userEmailAccounts.Count () > 0) {
                foreach (var emailAccount in userEmailAccounts) {
                    var emailAccountMessages = await _emailMessageRepository.GetAllForEmailAccountAsync (emailAccount.Id);
                    var emailAccountSendedMessages = await _sendedEmailMessageRepository.GetAllForEmailAccountAsync (emailAccount.Id);
                    if (emailAccountMessages.Count () == 0)
                        await _emailClientAggregate.AddEmailsFromEmailAccountToEmailMessagesAsync (emailAccount.Email, emailAccount.Password, emailAccount.Imap.Host, emailAccount.Imap.Port, DateTime.UtcNow.AddDays (-14));
                    if (emailAccountSendedMessages.Count () == 0)
                        await _emailClientAggregate.AddEmailsFromEmailAccountToEmailMessagesAsync (emailAccount.Email, emailAccount.Password, emailAccount.Imap.Host, emailAccount.Imap.Port, DateTime.UtcNow.AddDays (-14));
                    var lastDeliveredDate = emailAccountMessages.Select (x => x.DeliveredAt).FirstOrDefault ();
                    var lastSentDate = emailAccountSendedMessages.Select (x => x.SendedAt).FirstOrDefault ();
                    await _emailClientAggregate.GetNewEmailsFromEmailAccountAsync (emailAccount.Email, emailAccount.Password, emailAccount.Imap.Host, emailAccount.Imap.Port, lastDeliveredDate, lastSentDate);
                }
            }
        }

        public async Task GetNewEmailsForAllUsersAsync () {
            var usersIds = _context.Users.AsNoTracking ().Select (x => x.Id).AsEnumerable ();
            foreach (var userId in usersIds) {
                await GetNewEmailsByUserIdAsync (userId);
            }
        }
    }
}