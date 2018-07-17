using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO.SendedEmailMessage;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;

namespace EGrower.Infrastructure.Services {
    public class SendedAtachmentService : ISendedAtachmentService {
        private readonly ISendedAtachmentRepository _sendedAtachmentRepository;
        private readonly IMapper _mapper;

        public SendedAtachmentService (ISendedAtachmentRepository sendedAtachmentRepository, IMapper mapper) {
            _sendedAtachmentRepository = sendedAtachmentRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SendedAtachmentDto>> BrowseAsync (int sendedEmailMessageId) {
            var sendedAtachments = await _sendedAtachmentRepository.BrowseByEmailMessageIdAsync (sendedEmailMessageId);
            return _mapper.Map<IEnumerable<SendedAtachmentDto>> (sendedAtachments);
        }

        public async Task CreateAsync (string name, string contentType, byte[] data) {
            var sendedAtachment = new SendedAtachment (name, contentType, data);
            await _sendedAtachmentRepository.AddAsync (sendedAtachment);
        }

        public async Task DeleteAsync (int Id) {
            var sendedAtachment = await _sendedAtachmentRepository.GetAsync (Id);
            await _sendedAtachmentRepository.DeleteAsync (sendedAtachment);
        }

        public async Task<SendedAtachmentDto> GetAsync (int Id) {
            var sendedAtachment = await _sendedAtachmentRepository.GetAsync (Id);
            return _mapper.Map<SendedAtachmentDto> (sendedAtachment);
        }

        public Task UpdateAsync (int sendedEmailMessageId) {
            throw new NotImplementedException ();
        }
    }
}