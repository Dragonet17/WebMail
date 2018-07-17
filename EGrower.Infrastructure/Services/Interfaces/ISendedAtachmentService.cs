using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO.SendedEmailMessage;

namespace EGrower.Infrastructure.Services.Interfaces {
    public interface ISendedAtachmentService {
        Task CreateAsync (string name, string contentType, byte[] Data);
        Task<SendedAtachmentDto> GetAsync (int sendedEmailMessageId);
        Task<IEnumerable<SendedAtachmentDto>> BrowseAsync (int sendedEmailMessageId);
        Task UpdateAsync (int sendedEmailMessageId);
        Task DeleteAsync (int sendedEmailMessageId);
    }
}