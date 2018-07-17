using System;
using System.Collections.Generic;

namespace EGrower.Core.Domains {
    public class SendedEmailMessage {
        public int Id { get; private set; }
        public string From { get; private set; }
        public string To { get; private set; }
        public DateTime SendedAt { get; private set; }
        public DateTime LastActivity { get; private set; }
        public string Subject { get; private set; }
        public string TextHTMLBody { get; private set; }
        public bool HasAttachment { get; set; }
        public bool Deleted { get; private set; }
        public ICollection<SendedAtachment> SendedAtachments { get; private set; }
        public EmailAccount EmailAccount { get; private set; }

        protected SendedEmailMessage () { }

        public SendedEmailMessage (string from, ICollection<string> to, string subject, string textBody, EmailAccount emailAccount) {
            From = from;
            SetTo (to);
            SendedAt = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
            Subject = subject;
            TextHTMLBody = textBody;
            HasAttachment = false;
            Deleted = false;
            EmailAccount = emailAccount;
        }
        public SendedEmailMessage (string from, ICollection<string> to, DateTime sendedAt, string subject, string textBody) {
            From = from;
            SetTo (to);
            SendedAt = sendedAt.ToLocalTime ();
            LastActivity = DateTime.UtcNow;
            Subject = subject;
            TextHTMLBody = textBody;
            HasAttachment = false;
            Deleted = false;
        }
        public SendedEmailMessage (string from, string to, DateTime sendedAt, string subject, string textBody, bool hasAttachment) {
            From = from;
            To = to;
            SendedAt = sendedAt.ToLocalTime ();
            LastActivity = DateTime.UtcNow;
            Subject = subject;
            TextHTMLBody = textBody;
            HasAttachment = hasAttachment;
            Deleted = false;
        }
        public void AddEmailAccount (EmailAccount emailAccount) {
            if (emailAccount != null) {
                EmailAccount = emailAccount;
            }
        }
        public void AddSendedAtachments (ICollection<SendedAtachment> sendedAtachments) {
            if (sendedAtachments != null && sendedAtachments.Count > 0) {
                SendedAtachments = sendedAtachments;
                HasAttachment = true;
            }
        }

        private void SetTo (ICollection<string> emails) {
            To = String.Join (',', emails);
        }

    }
}