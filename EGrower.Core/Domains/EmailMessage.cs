using System;
using System.Collections.Generic;

namespace EGrower.Core.Domains {
    public class EmailMessage {
        public int Id { get; private set; }
        public string From { get; private set; }
        public string To { get; private set; }
        public DateTime DeliveredAt { get; private set; }
        public DateTime AddedAt { get; private set; }
        public DateTime LastActivity { get; private set; }
        public string Subject { get; private set; }
        public string TextHTMLBody { get; private set; }
        public bool HasAttachment { get; set; }
        public bool IsRead { get; private set; }
        public bool Deleted { get; private set; }
        public ICollection<Atachment> Atachments { get; protected set; }
        public EmailAccount EmailAccount { get; private set; }

        protected EmailMessage () { }

        public EmailMessage (string from, string to, DateTimeOffset deliveredAt, string subject, string textBody, bool hasAttachment) {
            From = from;
            To = to;
            DeliveredAt = deliveredAt.LocalDateTime;
            AddedAt = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
            Subject = subject;
            HasAttachment = hasAttachment;
            Deleted = false;
            TextHTMLBody = textBody;
            IsRead = false;
        }
        public void Delete () {
            if (!Deleted) {
                Deleted = true;
                LastActivity = DateTime.UtcNow;
            }

        }
        public void Restore () {
            if (Deleted) {
                Deleted = false;
                LastActivity = DateTime.UtcNow;
            }
        }
        public void MarkAsRead () {
            if (!IsRead) {
                IsRead = true;
                LastActivity = DateTime.UtcNow;
            }
        }
        public void MarkAsUnread () {
            if (IsRead) {
                IsRead = false;
                LastActivity = DateTime.UtcNow;
            }
        }
        public void AddAtachments (ICollection<Atachment> attachments) {
            if (attachments == null || attachments.Count == 0)
                throw new Exception ("List of attachments can not be empty.");
            Atachments = attachments;
        }
        public void AddEmailAccount (EmailAccount emailAccount) {
            if (emailAccount == null)
                throw new Exception ("Email account can not be empty.");
            EmailAccount = emailAccount;
        }
    }
}