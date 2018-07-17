using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Aggregates.Interfaces;
using EGrower.Infrastructure.DTO.SendedEmailMessage;
using EGrower.Infrastructure.Extension.Zip;
using EGrower.Infrastructure.Factories.Interfaces;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EGrower.Infrastructure.Services {
    public class SendedEmailMessageService : ISendedEmailMessageService {
        private readonly ISendedEmailMessageRepository _sendedEmailMessageRepository;
        private readonly IEmailAccountRepository _emailAccountRepository;
        private readonly IEmailClientFactory _emailClientFactory;
        private readonly IEmailClientAggregate _emailClientAggregate;
        private readonly IMapper _mapper;

        public SendedEmailMessageService (
            ISendedEmailMessageRepository sendedEmailMessageRepository,
            IEmailAccountRepository emailAccountRepository,
            IEmailClientFactory emailClientFactory,
            IEmailClientAggregate emailClientAggregate,
            IMapper mapper) {
            _sendedEmailMessageRepository = sendedEmailMessageRepository;
            _emailAccountRepository = emailAccountRepository;
            _emailClientFactory = emailClientFactory;
            _emailClientAggregate = emailClientAggregate;
            _mapper = mapper;
        }

        public async Task SendAsync (int userId, string from, ICollection<string> to, ICollection<string> cc, ICollection<string> bcc, string subject, string textHtmlBody, ICollection<IFormFile> attachments = null) {
            var emailAccountWithSmtpSettings = await _emailAccountRepository.GetByUserIdAndByEmailWithSmtpAsync (userId, from, false);
            if (emailAccountWithSmtpSettings == null || emailAccountWithSmtpSettings.Smtp == null)
                throw new Exception ("Email account with this ids does not exist.");
            var sendedEmailMessage = new SendedEmailMessage (from, to, subject, textHtmlBody, emailAccountWithSmtpSettings);
            ICollection<SendedAtachment> sendedAttachments = null;
            if (attachments != null && attachments.Count > 0) {
                sendedAttachments = await ConvertFormFilesToSendedAttachments (attachments);
                sendedEmailMessage.AddSendedAtachments (sendedAttachments);
            }
            await _emailClientFactory.SendAsync (emailAccountWithSmtpSettings.Email, emailAccountWithSmtpSettings.Password, emailAccountWithSmtpSettings.Smtp.Host, emailAccountWithSmtpSettings.Smtp.Port, emailAccountWithSmtpSettings.User.Name, to, cc, bcc, subject, textHtmlBody, sendedAttachments);
            await _sendedEmailMessageRepository.AddAsync (sendedEmailMessage);
        }

        public async Task<MemoryStream> GetUserSendedAttachmentsAsync (int userId, int emailId) {
            var sendedEmailMessage = await _sendedEmailMessageRepository.GetWithAttachmentsByUserIdAsync (userId, emailId);
            if (sendedEmailMessage == null)
                throw new Exception ("Message with this id is empty or doest not exist.");
            if (sendedEmailMessage.SendedAtachments.Count == 0)
                throw new Exception ("Message with this id does not have attachments.");
            var zipStream = await Zip.GetZipStream (sendedEmailMessage.SendedAtachments);
            if (zipStream == null)
                throw new Exception ("Lack of attachments to download.");
            return zipStream;
        }

        public async Task<SendedEmailMessageDto> GetAsync (int id) {
            var sendedEmailMessage = await _sendedEmailMessageRepository.GetAsync (id);
            return _mapper.Map<SendedEmailMessageDto> (sendedEmailMessage);
        }
        public async Task<IEnumerable<SendedEmailMessageDto>> GetAllByUserIdAsync (int userId) {
            var sendedEmails = await _sendedEmailMessageRepository.GetAllByUserIdAsync (userId);
            return _mapper.Map<IEnumerable<SendedEmailMessageDto>> (sendedEmails);
        }
        public Task<IEnumerable<SendedEmailMessageDto>> BrowseAllAsync (int id) {
            throw new NotImplementedException ();
        }
        public async Task DeleteAsync (int id) {
            var sendedEmailMessage = await _sendedEmailMessageRepository.GetAsync (id);
            await _sendedEmailMessageRepository.DeleteAsync (sendedEmailMessage);
        }
        private async Task<ICollection<SendedAtachment>> ConvertFormFilesToSendedAttachments (ICollection<IFormFile> attachments) {
            HashSet<SendedAtachment> sendedAtachments = new HashSet<SendedAtachment> ();
            foreach (var attachment in attachments) {
                if (attachment.Length > 0) {
                    using (var ms = new MemoryStream ()) {
                        await attachment.CopyToAsync (ms);
                        var attachmentBytes = ms.ToArray ();
                        var name = attachment.Name;
                        if (name == null)
                            name = attachment.FileName;
                        var sendedAttachment = new SendedAtachment (name, attachment.ContentType, attachmentBytes);
                        sendedAtachments.Add (sendedAttachment);
                    }
                }
            }
            return await Task.FromResult (sendedAtachments);
        }
    }
}