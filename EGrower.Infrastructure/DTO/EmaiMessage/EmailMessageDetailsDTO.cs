using System;
using System.Collections.Generic;

namespace EGrower.Infrastructure.DTO.EmaiMessage {
    public class EmailMessageDetailsDTO {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DeliveredAt { get; set; }
        public string Subject { get; set; }
        public string TextHTMLBody { get; set; }
        public bool IsRead { get; set; }
        public bool HasAttachment { get; set; }
        // public ICollection<AtachmentDTO> Atachments { get; protected set; }
    }
}