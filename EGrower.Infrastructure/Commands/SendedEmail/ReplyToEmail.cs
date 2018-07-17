using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace EGrower.Infrastructure.Commands.SendedEmail {
    public class ReplyToEmail {
        // public int EmailMessageId { get; set; }
        public ICollection<string> Cc { get; set; }
        public ICollection<string> Bcc { get; set; }
        public string TextHTMLBody { get; set; }
        public bool ReplyToAll { get; set; }
        public ICollection<IFormFile> Attachments { get; set; }
    }
}