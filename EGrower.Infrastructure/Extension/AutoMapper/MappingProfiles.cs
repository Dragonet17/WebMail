using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO;
using EGrower.Infrastructure.DTO.EmailAccount;
using EGrower.Infrastructure.DTO.EmaiMessage;
using EGrower.Infrastructure.DTO.SendedEmailMessage;

namespace EGrower.Infrastructure.Extension.AutoMapper {
    public class MappingProfiles : Profile {
        public MappingProfiles () {
            CreateMap<User, UserDTO> ().ReverseMap ();
            CreateMap<SendedEmailMessage, SendedEmailMessageDto> ().ReverseMap ();
            CreateMap<EmailMessage, EmailMessageDetailsDTO> ().ReverseMap ();
            CreateMap<EmailMessage, EmailMessageDto> ().ReverseMap ();
            CreateMap<SendedAtachment, SendedAtachmentDto> ().ReverseMap ();
            CreateMap<User, UserDTO> ().ReverseMap ();
            CreateMap<EmailAccount, EmailAccountDTO> ().ReverseMap ();
            CreateMap<Imap, IMapDTO> ().ReverseMap ();
            CreateMap<Smtp, SmtpDTO> ().ReverseMap ();
        }
    }

}