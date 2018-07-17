using System;

namespace EGrower.Infrastructure.DTO.SendedEmailMessage {
    public class SendedEmailMessageDto {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string TextHTMLBody { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}