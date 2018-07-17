using System;
using System.Collections.Generic;
using System.Text;

namespace EGrower.Infrastructure.Commands.EmailAccount {
    public class CreateEmailAccount {
        public string Email { get; set; }
        public string Password { get; set; }
        public int ImapPort { get; set; }
        public string ImapHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpHost { get; set; }
    }
}