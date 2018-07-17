using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace EGrower.Infrastructure.Commands.SendedEmail {
    public class CreateSendedEmail {
        public string From { get; set; }
        public ICollection<string> To { get; set; }
        public ICollection<string> Cc { get; set; }
        public ICollection<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string TextHTMLBody { get; set; }
        public ICollection<IFormFile> Attachments { get; set; }
    }
}