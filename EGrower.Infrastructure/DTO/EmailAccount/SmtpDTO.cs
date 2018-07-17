using System;
using System.Collections.Generic;
using System.Text;

namespace EGrower.Infrastructure.DTO.EmailAccount
{
    public class SmtpDTO
    {
        public int Id { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public string EmailProvider { get; set; }
    }
}
