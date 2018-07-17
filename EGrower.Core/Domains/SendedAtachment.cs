using System;

namespace EGrower.Core.Domains {
    public class SendedAtachment {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ContentType { get; private set; }
        public byte[] Data { get; private set; }
        public DateTime AddedAt { get; private set; }
        public int SendedEmailMessageId { get; private set; }
        public SendedEmailMessage SendedEmailMessage { get; private set; }
        protected SendedAtachment () { }
        public SendedAtachment (string name, string contentType, byte[] data) {
            Name = name;
            ContentType = contentType;
            Data = data;
            AddedAt = DateTime.UtcNow;
        }
    }
}