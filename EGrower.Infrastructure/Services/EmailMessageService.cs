using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Aggregates.Interfaces;
using EGrower.Infrastructure.DTO.EmaiMessage;
using EGrower.Infrastructure.Extension.ExtensionMethods;
using EGrower.Infrastructure.Extension.Zip;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EGrower.Infrastructure.Services {
    public class EmailMessageService : IEmailMessageService {
        private readonly IEmailMessageRepository _emailMessageRepository;
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailClientAggregate _emailClientAggregate;
        private readonly IMapper _mapper;

        public EmailMessageService (IEmailMessageRepository emailMessageRepository,
            IEmailAccountRepository emailAccountRepository,
            IEmailAccountService emailAccountService,
            IUserService userService,
            IUserRepository userRepository,
            IEmailClientAggregate emailClientAggregate,
            IMapper mapper) {
            _emailMessageRepository = emailMessageRepository;
            _emailAccountRepository = emailAccountRepository;
            _emailAccountService = emailAccountService;
            _userService = userService;
            _userRepository = userRepository;
            _emailClientAggregate = emailClientAggregate;
            _mapper = mapper;
        }

        // public async Task<EmailMessageDto> GetByUserIdAsync (int userId, int emailId) {
        //     // var emailMessage = await _emailMessageRepository.GetByUserIdAsync (userId, emailId);
        //     // var message 
        //     // return _mapper.Map<EmailMessageDto> (emailMessages);
        //     var accountData = await _emailAccountRepository.GetByUserIdAndByEmailWithSmtpAsync(userId,).
        //     var emailData = await _emailAccountService.
        //     var sa = await _emailClientAggregate.
        // }
        public async Task<IEnumerable<EmailMessageDto>> GetAllForEmailAccountAsync (int emailAccountId) {
            if (await _emailAccountService.EmailAccountExistAsync (emailAccountId))
                throw new Exception ("Email account does not exist.");
            var emailMessages = await _emailMessageRepository.GetAllForEmailAccountAsync (emailAccountId);
            return _mapper.Map<IEnumerable<EmailMessageDto>> (emailMessages);
        }

        public async Task<IEnumerable<EmailMessageDto>> GetAllForUserAsync (int userId) {
            var emailMessages = await _emailMessageRepository.GetAllForUserAsync (userId);
            // if (emailMessages == null || emailMessages.Count () == 0) {
            //     throw new Exception ("This user does not have email messages.");
            // }
            var undeletedEmailMessages = emailMessages.Where (a => !a.Deleted).OrderByDescending (x => x.DeliveredAt);
            return _mapper.Map<IEnumerable<EmailMessageDto>> (undeletedEmailMessages);
        }
        public async Task<EmailMessageDetailsDTO> GetAsync (int userId, int emailId) {

            var emailMessage = await _emailMessageRepository.GetCheckSeenOrFailAsync (userId, emailId);
            return _mapper.Map<EmailMessageDetailsDTO> (emailMessage);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public async Task<EmailMessageDetailsDTO> GetForUserAsync (int userId, int emailId) {
            var emailMessage = await _emailMessageRepository.GetByUserIdAsync (userId, emailId);
            if (emailMessage == null)
                throw new Exception ("This user has not email message with this id.");
            if (!emailMessage.IsRead) {
                emailMessage.MarkAsRead ();
                await _emailMessageRepository.UpdateAsync (emailMessage);
            }
            return _mapper.Map<EmailMessageDetailsDTO> (emailMessage);
        }

        public async Task<EmailMessageDetailsDTO> GetDeletedForUserAsync (int userId, int emailId) {
            var emailMessage = await _emailMessageRepository.GetDeletedByUserIdAsync (userId, emailId);
            if (emailMessage == null)
                throw new Exception ("This user has not email message with this id");
            if (!emailMessage.Deleted)
                throw new Exception ("Email with this  id is undeleted.");
            if (!emailMessage.IsRead) {
                emailMessage.MarkAsRead ();
                await _emailMessageRepository.UpdateAsync (emailMessage);
            }
            return _mapper.Map<EmailMessageDetailsDTO> (emailMessage);
        }

        public async Task<EmailMessageDetailsDTO> GetUndeletedForUserAsync (int userId, int emailId) {
            var emailMessage = await _emailMessageRepository.GetUndeletedByUserIdAsync (userId, emailId);
            if (emailMessage == null)
                throw new Exception ("This user has not email message with this id");
            if (emailMessage.Deleted)
                throw new Exception ("Email with this id id deleted.");
            if (!emailMessage.IsRead) {
                emailMessage.MarkAsRead ();
                await _emailMessageRepository.UpdateAsync (emailMessage);
            }
            return _mapper.Map<EmailMessageDetailsDTO> (emailMessage);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUserMessageAsync (int id, int userId) {
            var messageToDelete = await _emailMessageRepository.GetUndeletedByUserIdAsync (userId, id);
            if (messageToDelete == null)
                throw new Exception ("This user has not email message with this id to delete");
            messageToDelete.Delete ();
            await _emailMessageRepository.UpdateAsync (messageToDelete);
        }

        public async Task RestoreMessageFromTrashAsync (int id, int userId) {
            var messageToRestore = await _emailMessageRepository.GetDeletedByUserIdAsync (userId, id);
            if (messageToRestore == null)
                throw new Exception ("This user has not email message with this id to restore from trash");
            messageToRestore.Restore ();
            await _emailMessageRepository.UpdateAsync (messageToRestore);
        }

        // public async Task GetForAllNewEmailMessagesAsync () {
        //     var users = await _userRepository.GetAllWithEmailAccountAndImapAsync ();
        //     if (users != null || users.Count () != 0)
        //         throw new Exception ("This user does not exist or does not have email accounts.");
        //     foreach (var user in users) {
        //         if (user != null || user.EmailAccounts != null || user.EmailAccounts.Count () != 0) {
        //             foreach (var emailAccount in user.EmailAccounts) {
        //                 await _emailClientAggregate.GetNewEmailsFromEmailAccountAsync (emailAccount, emailAccount.Imap.Host, emailAccount.Imap.Port);
        //             }
        //         }
        //     }
        // }

        public async Task<EmailMessageDetailsDTO> GetForEmailAccountAsync (int emailAccountId, int emailId) {
            if (await _emailAccountService.EmailAccountExistAsync (emailAccountId))
                throw new Exception ("Email account does not exist.");
            var emailMessages = await _emailMessageRepository.GetAllForEmailAccountAsync (emailAccountId);
            if (emailMessages == null || emailMessages.Count () == 0) {
                throw new Exception ("This email account does not have email messages.");
            }
            var emailMessage = emailMessages.SingleOrDefault (a => a.Id == emailId);
            return _mapper.Map<EmailMessageDetailsDTO> (emailMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public async Task<MemoryStream> GetUserAttachmentsAsync (int userId, int emailId) {
            var emailMessage = await _emailMessageRepository.GetWithAttachmentsByUserIdAsync (emailId, userId);
            if (emailMessage == null)
                throw new Exception ("Message with this id is empty or doest not exist.");
            if (emailMessage.Atachments.Count == 0)
                throw new Exception ("Message with this id does not have attachments.");
            var zipStream = await Zip.GetZipStream (emailMessage.Atachments);
            if (zipStream == null)
                throw new Exception ("Lack of attachments to download.");
            return zipStream;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="emailAccountId"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public async Task<MemoryStream> GetAttachmentsForEmailAccountAsync (int emailAccountId, int emailId) {
            if (await _emailAccountService.EmailAccountExistAsync (emailAccountId))
                throw new Exception ("Email account does not exist.");
            var emailMessages = await _emailMessageRepository.GetAllForEmailAccountAsync (emailAccountId);
            if (emailMessages == null || emailMessages.Count () == 0)
                throw new Exception ("This email account does not have email massages.");
            var emailMessage = emailMessages.SingleOrDefault (a => a.Id == emailId);
            return await Zip.GetZipStream (emailMessage.Atachments);
        }

        // public async Task GetNewEmailMessagesAsync (int userId) {
        //     var user = await _userRepository.GetWithEmailAccountAndImapAsync (userId);
        //     if (user == null || user.EmailAccounts == null || user.EmailAccounts.Count () == 0)
        //         throw new Exception ("This user does not exist or does not have email accounts.");
        //     foreach (var emailAccount in user.EmailAccounts) {
        //         await _emailClientAggregate.GetNewEmailsFromEmailAccountAsync (emailAccount, emailAccount.Imap.Host, emailAccount.Imap.Port);
        //     }
        // }

        public async Task<IEnumerable<EmailMessageDto>> GetAllDeletedForUserAsync (int userId) {
            var deletedEmailMessages = await _emailMessageRepository.GetAllDeletedForUserAsync (userId);
            // if (deletedEmailMessages == null || deletedEmailMessages.Count () == 0)
            //     throw new Exception ("User with this id does not have meesages to remove.");
            return _mapper.Map<IEnumerable<EmailMessageDto>> (deletedEmailMessages);
        }

        public async Task<IEnumerable<EmailMessageDto>> GetAllUnDeletedForUserAsync (int userId) {
            var undeletedEmailMessages = await _emailMessageRepository.GetAllUndeletedMessagesForUserAsync (userId);
            // if (undeletedEmailMessages == null || undeletedEmailMessages.Count () == 0)
            //     throw new Exception ("User with this id does not have received emails.");
            return _mapper.Map<IEnumerable<EmailMessageDto>> (undeletedEmailMessages);
        }

        public async Task MarkAsReadAsync (int userId, int emailId) {
            var emailMessage = await _emailMessageRepository.GetByUserIdAsync (userId, emailId);
            if (emailMessage == null)
                throw new Exception ("Email message with this id does not exist");
            if (emailMessage.IsRead)
                throw new Exception ("Email message with this id is already mark as read");
            emailMessage.MarkAsRead ();
            await _emailMessageRepository.UpdateAsync (emailMessage);
        }
        public async Task MarkAsUnreadAsync (int userId, int emailId) {
            var emailMessage = await _emailMessageRepository.GetByUserIdAsync (userId, emailId);
            if (emailMessage == null)
                throw new Exception ("Email message with this id does not exist");
            if (!emailMessage.IsRead)
                throw new Exception ("Email message with this id is already mark as unread");
            emailMessage.MarkAsUnread ();
            await _emailMessageRepository.UpdateAsync (emailMessage);
        }

        public async Task RemoveDeletedUserMessageAsync (int id, int userId) {
            var deletedMessage = await _emailMessageRepository.GetDeletedByUserIdAsync (userId, id);
            if (deletedMessage == null)
                throw new Exception ("User with this id does not have meesages to remove.");
            await _emailMessageRepository.RemoveAsync (deletedMessage);
        }

        public async Task ReplyToAsync (int id, int userId, string answer, bool replyToAll, ICollection<IFormFile> attachments = null, ICollection<string> cc = null, ICollection<string> bcc = null) {
            var messageWithEmailAccountAndEmailProviders = await _emailMessageRepository.GetByUserIdWithEmailAccountAndEmailProvidersAsync (userId, id);
            if (messageWithEmailAccountAndEmailProviders == null)
                throw new Exception ("This user does not have message with this id.");
            if (messageWithEmailAccountAndEmailProviders.EmailAccount == null ||
                messageWithEmailAccountAndEmailProviders.EmailAccount.Imap == null ||
                messageWithEmailAccountAndEmailProviders.EmailAccount.Smtp == null)
                throw new Exception ("This user does not have email account or email providers.");
            var messageFromEmailAccount = await _emailClientAggregate.GetDeliveredMessageBySubjectAndDateAsync (messageWithEmailAccountAndEmailProviders.EmailAccount.Imap.Host, messageWithEmailAccountAndEmailProviders.EmailAccount.Imap.Port, messageWithEmailAccountAndEmailProviders.EmailAccount.Email, messageWithEmailAccountAndEmailProviders.EmailAccount.Password, messageWithEmailAccountAndEmailProviders.Subject, messageWithEmailAccountAndEmailProviders.DeliveredAt);
            await _emailClientAggregate.ReplyToAsync (messageWithEmailAccountAndEmailProviders.EmailAccount.Smtp.Host, messageWithEmailAccountAndEmailProviders.EmailAccount.Smtp.Port, messageWithEmailAccountAndEmailProviders.EmailAccount.Email, messageWithEmailAccountAndEmailProviders.EmailAccount.Password, messageFromEmailAccount, replyToAll, answer, attachments, cc, bcc);
        }
    }
}