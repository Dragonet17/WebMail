using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EGrower.Core.Domains;
using EGrower.Infrastructure.DTO.EmailAccount;

namespace EGrower.Infrastructure.Extension.AutoMapper {
    public class AutoMapperConfig {
        public static IMapper Initialize () => new MapperConfiguration (cfg => {
                cfg.CreateMap<EmailAccount, EmailAccountDTO> ();
                cfg.CreateMap<Imap, IMapDTO> ();
                cfg.CreateMap<Smtp, SmtpDTO> ();
            })
            .CreateMapper ();
    }
}