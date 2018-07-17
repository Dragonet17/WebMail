using System;

namespace EGrower.Core.Domains {
    public class Atachment {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ContentType { get; private set; }
        public byte[] Data { get; private set; }
        public DateTime AddedAt { get; private set; }
        public int EmailMessageId { get; private set; }
        public EmailMessage EmailMessage { get; private set; }

        protected Atachment () { }
        public Atachment (string name, string contentType, byte[] data) {
            Name = name;
            Data = data;
            ContentType = contentType;
            AddedAt = DateTime.UtcNow;
        }
    }
}