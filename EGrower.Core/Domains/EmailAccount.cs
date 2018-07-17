using System;
using System.Collections.Generic;
using System.Text;

namespace EGrower.Core.Domains {
    public class EmailAccount {
        public int Id { get; set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool Deleted { get; private set; }
        public User User { get; private set; }
        public Imap Imap { get; private set; }
        public Smtp Smtp { get; private set; }
        public ICollection<EmailMessage> EmailMessages { get; private set; }
        public ICollection<SendedEmailMessage> SendedEmailMessages { get; private set; }

        protected EmailAccount () { }

        public EmailAccount (string email, string password) {
            Email = email;
            Password = password;
            // CreatePasswordHash (password);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Deleted = false;
        }
        public void Delete () {
            Deleted = true;
        }

        public void Restore () {
            if (Deleted == true) {
                Deleted = false;
                UpdatedAt = DateTime.UtcNow;
            }
        }
        public void Update (string email) {
            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdatePassword (string newPassword) {
            CreatePasswordHash (newPassword);
            UpdatedAt = DateTime.UtcNow;
        }
        public void AddUser (User user) {
            User = user;
        }
        public void AddImapSettings (Imap imap) {
            Imap = imap;
        }
        public void AddSmtpSettings (Smtp smtp) {
            Smtp = smtp;
        }
        public void AddEmailMessages (ICollection<EmailMessage> emailMessages) {
            if (emailMessages == null)
                throw new Exception ("Email Account does not consist of an email message.");
            EmailMessages = emailMessages;
        }
        private void CreatePasswordHash (string password) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512 ()) {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
            }
        }
    }
}